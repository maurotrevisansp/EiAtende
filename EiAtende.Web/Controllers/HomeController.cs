using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EiAtende;
using EiAtende.Entity;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace EiAtende.Web.Controllers
{
    public class HomeController : Controller
    {
        private EiAtendeContexto db = new EiAtendeContexto();
        private Helpers.clsEnviarEmail _enviarEmail = new Helpers.clsEnviarEmail();
        CultureInfo culture = new CultureInfo("pt-BR");

        // GET: PortalChamados
        [HttpGet]
        public ActionResult Index(PortalUsuario portalUsuario, string submit, ViewModels.VwChamados model, string AtenderEmpID, string DeUsrID, 
            string ParaUsrID, string TipoChamadoID, string AtividadeChamadoID, string Status, string atenderEmpresa,
            string idChamadoHist, string empresa, string tipochamado, string atividadechamado, string Historico,
            string idchamado, string StatusPesq, string StatusHist, string deStatus, string Descricao, string dtAdiar, string paraStatus,
            string filtroempresa, string filtrotipochamado, string filtroatividadechamado, string filtroidchamado, string filtroStatusPesq, 
            string filtroempresa1, string filtrotipochamado1, string filtroatividadechamado1, string filtroidchamado1, string filtroStatusPesq1)
        {
            if (Request.QueryString["idDoChamado"] != null)
            {
                idchamado = Request.QueryString["idDoChamado"];
                submit = "PesquisarGeral";
            }
            TempData["msg"] = "Lista de Chamados";

            var portalChamados = db.PortalChamados.Include(p => p.PortalAtividadeChamados).Include(p => p.PortalTipoChamados);
            ViewModels.VwChamados _vwChamados = new ViewModels.VwChamados();
            PortalAtividadeChamados _Atividades = new PortalAtividadeChamados();

            if (Session["LoginUsuario"] == null)
            {
                return RedirectToAction("", "Login");
            }

            if (submit == "Excell")
            {
                _vwChamados = PesquisaGeral(empresa, tipochamado, atividadechamado, idchamado, StatusPesq);
                this.ExportChamadosToExcel(_vwChamados);
                TempData["msg"] = "Arquivo Excell Gerado com Sucesso";
                _vwChamados = PesquisaGeral(empresa, tipochamado, atividadechamado, idchamado, StatusPesq);
                return View(_vwChamados);
            }

            if (submit == "PesquisarGeral")
            {
                _vwChamados = PesquisaGeral(empresa, tipochamado, atividadechamado, idchamado, StatusPesq);
                TempData["titulo"] = "Chamados";
                TempData["titulo1"] = "Cadastrados";
                TempData["msg"] = "Pesquisa Efetuada com Sucesso";

                return View(_vwChamados);

            }

            var _PortalUsuario = db.PortalUsuario.Find(Convert.ToInt32(Session["IdUsuario"]));
            _PortalUsuario.PortalUsuarioGrupo = db.PortalUsuarioGrupo.Find(_PortalUsuario.UsrGrpID);

            if (submit == "SalvarChamado")
            {
                PortalChamados _PortalChamados = new PortalChamados();
                _Atividades = db.PortalAtividadeChamados.Find(Convert.ToInt32(AtividadeChamadoID));

                _PortalChamados.AtenderEmpID = Convert.ToInt32(atenderEmpresa);
                _PortalChamados.AtividadeChamadoID = Convert.ToInt32(AtividadeChamadoID);
                _PortalChamados.Avaliacao = string.Empty;
                _PortalChamados.ChamadoConhecimento = string.Empty;
                _PortalChamados.ChamadoDtAbertura = DateTime.Now;
                _PortalChamados.ChamadoDtPrevista = DateTime.Now.AddDays(_Atividades.PrevisaoDias);
                _PortalChamados.ChamadoDtPrevista = _PortalChamados.ChamadoDtPrevista.AddHours(_Atividades.PrevisaoHoras);
                _PortalChamados.ChamadoHistorico = model.PortalChamado.ChamadoHistorico;
                if (model.PortalChamado.ChamadoTitulo == string.Empty)
                {
                    model.PortalChamado.ChamadoTitulo = "Titulo";
                }

                _PortalChamados.ChamadoTitulo = model.PortalChamado.ChamadoTitulo;
                DeUsrID = Session["IdUsuario"].ToString();
                _PortalChamados.DeUsrID = Convert.ToInt32(DeUsrID);
                if (ParaUsrID == null)
                {
                    ParaUsrID = "0";
                }
                _PortalChamados.ParaUsrID = Convert.ToInt32(ParaUsrID);
                _PortalChamados.TipoChamadoID = Convert.ToInt32(TipoChamadoID);
                _PortalChamados.Status = "Postado";

                db.PortalChamados.Add(_PortalChamados);
                db.SaveChanges();
                
                TempData["msg"] = "Chamado Incluido com Sucesso";

                
                _vwChamados = PesquisaGeral(filtroempresa, filtrotipochamado, filtroatividadechamado, filtroidchamado, filtroStatusPesq);
                return View(_vwChamados);


            }
            if (submit == "NovoHistorico")
            {
                PortalChamadosHistorico _PortalChamadosHistorico = new PortalChamadosHistorico();
                _PortalChamadosHistorico.ChamadoID = Convert.ToInt32(idChamadoHist);
                _PortalChamadosHistorico.Descricao = Descricao;
                _PortalChamadosHistorico.DeStatus = deStatus;
                _PortalChamadosHistorico.ParaStatus = paraStatus;
                _PortalChamadosHistorico.DtIncl = DateTime.Now;
                if (dtAdiar != string.Empty)
                {
                    _PortalChamadosHistorico.DtAdiar = Convert.ToDateTime(dtAdiar, culture);
                }
                db.PortalChamadosHistorico.Add(_PortalChamadosHistorico);
                db.SaveChanges();
                PortalChamados _portalChamados = db.PortalChamados.Find(Convert.ToInt32(idChamadoHist));
                _portalChamados.Status = paraStatus;
                if (StatusHist == "Finalizado")
                {
                    _portalChamados.ChamadoDtTermino = DateTime.Now;
                }
                if (dtAdiar != string.Empty)
                {
                    _portalChamados.ChamadoDtAdiada = Convert.ToDateTime(dtAdiar, culture);
                }
                db.Entry(_portalChamados).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "Histórico para o Chamado " + idChamadoHist +  " Incluido com Sucesso";
                _vwChamados = PesquisaGeral(filtroempresa1, filtrotipochamado1, filtroatividadechamado1, filtroidchamado1, filtroStatusPesq1);
                return View(_vwChamados);

            }

            _vwChamados = PesquisaGeral(filtroempresa1, filtrotipochamado1, filtroatividadechamado1, filtroidchamado1, filtroStatusPesq1);
            return View(_vwChamados);

        }

        private ViewModels.VwChamados PesquisaGeral(string empresa, string tipochamado, string atividadechamado, string idchamado, string StatusPesq)
        {

            var portalChamados = db.PortalChamados.Include(p => p.PortalAtividadeChamados).Include(p => p.PortalTipoChamados);
            ViewModels.VwChamados _vwChamados = new ViewModels.VwChamados();
            IList<PortalEmpresa> _PortalEmpresas = new List<PortalEmpresa>();
            PortalEmpresa _PortalEmpresa = new PortalEmpresa();
            IList<PortalUsuario> _PortalUsuariosDasEmpresas = new List<PortalUsuario>();

            var _PortalUsuario = db.PortalUsuario.Find(Convert.ToInt32(Session["IdUsuario"]));
            _PortalUsuario.PortalUsuarioGrupo = db.PortalUsuarioGrupo.Find(_PortalUsuario.UsrGrpID);
            _PortalEmpresa = db.PortalEmpresa.Find(_PortalUsuario.UsrEmpID);

            if (_PortalEmpresa.EmpID == 0)
            {
                _PortalEmpresas = db.PortalEmpresa.ToList().Where(e => e.EmpID.Equals(0) || e.EmpEmpID.Equals(0)).ToList();
            }
            if (_PortalEmpresa.EmpID != 0 && _PortalEmpresa.EmpEmpID == 0)
            {
                _PortalEmpresas = db.PortalEmpresa.ToList().Where(e => e.EmpID.Equals(_PortalEmpresa.EmpID) || e.EmpEmpID.Equals(_PortalEmpresa.EmpID)).ToList();
            }
            if (_PortalEmpresa.EmpID != 0 && _PortalEmpresa.EmpEmpID != 0)
            {
                _PortalEmpresas = db.PortalEmpresa.ToList().Where(e => e.EmpID.Equals(_PortalEmpresa.EmpID)).ToList();
            }


            if (empresa == null)
            {
                empresa = "99999";
                tipochamado = "0";
                atividadechamado = "0";
                if (idchamado == null)
                {
                    idchamado = "0";
                }
                StatusPesq = "Todos Status";
            }
            var IdUsuario = Convert.ToInt32((Session["IdUsuario"]));
            var Gestor = Session["UsuarioGestor"].ToString();

            ViewBag.filtroEmpresa = empresa;
            ViewBag.filtroTipos = tipochamado;
            ViewBag.filtroAtividade = atividadechamado;
            ViewBag.filtroStatus = StatusPesq;
            ViewBag.filtroTitulo = string.Empty;
            if (idchamado == string.Empty)
            {
                idchamado = "0";
            }
            ViewBag.filtroIdChamado = idchamado;

            if (Gestor == "True")
            {
                _vwChamados.PortalChamados = portalChamados.ToList()
                    .OrderByDescending(e => e.ChamadoID).ToList();
            }
            else
            {
                _vwChamados.PortalChamados = portalChamados.ToList()
                    .Where(e => e.DeUsrID == IdUsuario || e.ParaUsrID == IdUsuario)
                    .OrderByDescending(e => e.ChamadoID).ToList();
            }

            foreach (var item in _vwChamados.PortalChamados)
            {
                item.PortalChamadosHistorico = db.PortalChamadosHistorico.ToList().Where(e => e.ChamadoID.Equals(item.ChamadoID)).ToList();
                item.ChamadoAnexos = db.ChamadoAnexos.ToList().Where(e => e.ChamadoID.Equals(item.ChamadoID)).ToList();
            }
            if (Convert.ToInt32(idchamado) != 0)
            {
                _vwChamados.PortalChamados = _vwChamados.PortalChamados.Where(e => e.ChamadoID.Equals(Convert.ToInt32(idchamado))).ToList();
            }
            if (empresa != "99999")
            {
                _vwChamados.PortalChamados = _vwChamados.PortalChamados.Where(e => e.AtenderEmpID.Equals(Convert.ToInt32(empresa))).ToList();
            }
            if (tipochamado != "0")
            {
                _vwChamados.PortalChamados = _vwChamados.PortalChamados.Where(e => e.TipoChamadoID.Equals(Convert.ToInt32(tipochamado))).ToList();
            }
            if (atividadechamado != "0")
            {
                _vwChamados.PortalChamados = _vwChamados.PortalChamados.Where(e => e.AtividadeChamadoID.Equals(Convert.ToInt32(atividadechamado))).ToList();
            }

            if (StatusPesq != "Todos Status")
            {
                List<PortalChamados> novalista = new List<PortalChamados>();
                foreach (var item in _vwChamados.PortalChamados)
                {
                    if (item.Status == StatusPesq)
                    {
                        novalista.Add(item);
                    }
                }
                _vwChamados.PortalChamados = novalista;
            }
            else
            {
                List<PortalChamados> novalista = new List<PortalChamados>();
                foreach (var item in _vwChamados.PortalChamados)
                {
                    if (item.Status != "Finalizado")
                    {
                        novalista.Add(item);
                    }
                }
                _vwChamados.PortalChamados = novalista;

            }
            var usrdasempresas = db.PortalUsuario.ToList();
            bool pertence = false;
            foreach (var item in usrdasempresas)
            {
                foreach (var item2 in _PortalEmpresas)
                {
                    if (item2.EmpID == item.UsrEmpID)
                    {
                        pertence = true;
                    }
                }
                if (pertence)
                {
                    _PortalUsuariosDasEmpresas.Add(item);
                }
                pertence = false;
            }
           
            _vwChamados.PortalTipoChamados = db.PortalTipoChamados.ToList();

            _vwChamados.PortalEmpresa = _PortalEmpresas;
            _vwChamados.PortalAtividadeChamados = db.PortalAtividadeChamados.ToList();
            _vwChamados.DeUsuario = db.PortalUsuario.ToList();
            _vwChamados.ParaUsuario = db.PortalUsuario.ToList();
            _vwChamados.AtenderEmpresa = _PortalEmpresas;
            ViewBag.DeUsrID = new SelectList(db.PortalUsuario.ToList().Where(e => e.UsrID == Convert.ToInt32(Session["IdUsuario"])), "UsrID", "UsrNome");
            ViewBag.ParaUsrID = new SelectList(_PortalUsuariosDasEmpresas, "UsrID", "UsrNome");
            ViewBag.AtenderEmpID = new SelectList(_PortalEmpresas, "EmpID", "EmpNomeFantasia");
            ViewBag.TipoChamadoID = new SelectList(db.PortalTipoChamados, "TipoChamadoID", "TipoChamadoNome");
            ViewBag.AtividadeChamadoID = new SelectList(db.PortalAtividadeChamados, "AtividadeChamadoID", "AtividadeChamadoNome");

            return _vwChamados;
        }

        [HttpGet]
        public ActionResult EditarChamados(int? id, string Acao)
        {

            var portalChamados = db.PortalChamados.Include(p => p.PortalAtividadeChamados).Include(p => p.PortalTipoChamados);
            PortalChamados _vwChamado = new PortalChamados();

            IList<PortalEmpresa> _PortalEmpresas = new List<PortalEmpresa>();
            PortalEmpresa _PortalEmpresa = new PortalEmpresa();
            IList<PortalUsuario> _PortalUsuariosDasEmpresas = new List<PortalUsuario>();

            var _PortalUsuario = db.PortalUsuario.Find(Convert.ToInt32(Session["IdUsuario"]));
            _PortalUsuario.PortalUsuarioGrupo = db.PortalUsuarioGrupo.Find(_PortalUsuario.UsrGrpID);
            _PortalEmpresa = db.PortalEmpresa.Find(_PortalUsuario.UsrEmpID);

            if (_PortalEmpresa.EmpID == 0)
            {
                _PortalEmpresas = db.PortalEmpresa.ToList().Where(e => e.EmpID.Equals(0) || e.EmpEmpID.Equals(0)).ToList();
            }
            if (_PortalEmpresa.EmpID != 0 && _PortalEmpresa.EmpEmpID == 0)
            {
                _PortalEmpresas = db.PortalEmpresa.ToList().Where(e => e.EmpEmpID.Equals(_PortalEmpresa.EmpID)).ToList();
            }
            if (_PortalEmpresa.EmpID != 0 && _PortalEmpresa.EmpEmpID != 0)
            {
                _PortalEmpresas = db.PortalEmpresa.ToList().Where(e => e.EmpID.Equals(_PortalEmpresa.EmpID)).ToList();
            }

            var usrdasempresas = db.PortalUsuario.ToList();
            bool pertence = false;
            foreach (var item in usrdasempresas)
            {
                foreach (var item2 in _PortalEmpresas)
                {
                    if (item2.EmpID == item.UsrEmpID)
                    {
                        pertence = true;
                    }
                }
                if (pertence)
                {
                    _PortalUsuariosDasEmpresas.Add(item);
                }
                pertence = false;
            }

            if (id != null)
            {
                if (Session["UsrGrpTratarChamado"].ToString() == "True")
                {
                    if (Acao == "Editar" || Acao == "Adiar" || Acao == "Finalizar")
                    {
                        _vwChamado = portalChamados.ToList().Where(e => e.ChamadoID.Equals(Convert.ToInt32(id))).First();
                        _vwChamado.ChamadoAnexos = db.ChamadoAnexos.ToList().Where(e => e.ChamadoID.Equals(Convert.ToInt32(id))).ToList();
                        ViewBag.DeUsrID = new SelectList(db.PortalUsuario.ToList().Where(e => e.UsrID == Convert.ToInt32(Session["IdUsuario"])), "UsrID", "UsrNome");
                        ViewBag.ParaUsrID = new SelectList(_PortalUsuariosDasEmpresas, "UsrID", "UsrNome", _vwChamado.ParaUsrID);
                        ViewBag.AtenderEmpID = new SelectList(_PortalEmpresas, "EmpID", "EmpNomeFantasia");
                        ViewBag.TipoChamadoID = new SelectList(db.PortalTipoChamados, "TipoChamadoID", "TipoChamadoNome", _vwChamado.TipoChamadoID);
                        ViewBag.AtividadeChamadoID = new SelectList(db.PortalAtividadeChamados, "AtividadeChamadoID", "AtividadeChamadoNome", _vwChamado.AtividadeChamadoID);
                        TempData["titulo"] = "Chamados";
                        TempData["titulo1"] = "Cadastrados";

                        TempData["Acao"] = "Editar";
                        return View(_vwChamado);
                    }
                    if (Acao == "Excluir")
                    {
                        PortalChamados portalChamado = db.PortalChamados.Find(id);
                        db.PortalChamados.Remove(portalChamado);
                        db.SaveChanges();
                    }
                }
                else
                {
                    TempData["msg"] = "Usuário sem permissão para Tratar Chamados";
                    return RedirectToAction("Index", "Home", (PortalUsuario)Session["PortalUsuarioessao"]);
                }

            }
            TempData["titulo"] = "Chamados";
            TempData["titulo1"] = "Cadastrados";
            if (Session["UsrGrpAbrirChamado"].ToString() == "True")
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["msg"] = "Usuário sem permissão de Abrir Chamados";
            return RedirectToAction("Index", "Home", (PortalUsuario)Session["PortalUsuarioessao"]);



        }

        [HttpPost]
        public ActionResult EditarChamados(FormCollection _FormCollection, PortalChamados portalChamados, string Status)
        {
            if (ModelState.IsValid)
            {
                portalChamados.Status = Status;
                if (Status == "Finalizado")
                {
                    portalChamados.ChamadoDtTermino = DateTime.Now;
                }
                portalChamados.AtenderEmpID = Convert.ToInt32(_FormCollection["atenderEmpresa"].ToString());
                db.Entry(portalChamados).State = EntityState.Modified;
                db.SaveChanges();
            }
            TempData["titulo"] = "Chamados";
            TempData["titulo1"] = "Cadastrados";

            return RedirectToAction("Index", "Home");

        }


        public void ExportChamadosToExcel(ViewModels.VwChamados _vwChamados)
        {


            _vwChamados.PortalTipoChamados = db.PortalTipoChamados.ToList();
            _vwChamados.PortalEmpresa = db.PortalEmpresa.ToList();
            _vwChamados.PortalAtividadeChamados = db.PortalAtividadeChamados.ToList();
            _vwChamados.DeUsuario = db.PortalUsuario.ToList();
            _vwChamados.ParaUsuario = db.PortalUsuario.ToList();
            _vwChamados.AtenderEmpresa = db.PortalEmpresa.ToList();

            System.Text.Encoding encoding = System.Text.Encoding.Unicode;
            StringWriter sw = new StringWriter();
            string linha = string.Empty;
            linha = "NR.CHAMADO;ATENDER EMPRESA;TITULO;DE USUÁRIO;PARA USUÁRIO;DT.ABERTURA;DT.PREVISÃO;STATUS";
            sw.WriteLine(linha);

            foreach (var item in _vwChamados.PortalChamados)
            {
                linha = string.Empty;
                linha += item.ChamadoID + ";";
                foreach (var itemAt in _vwChamados.AtenderEmpresa)
                {
                    if (itemAt.EmpID == item.AtenderEmpID)
                    {
                        linha += itemAt.EmpNomeFantasia + ";";
                    }
                }
                linha += item.ChamadoTitulo + ";";
                foreach (var itemDe in _vwChamados.DeUsuario)
                {
                    if (itemDe.UsrID == item.DeUsrID)
                    {
                        linha += itemDe.UsrNome + ";";
                    }
                }
                foreach (var itemPara in _vwChamados.ParaUsuario)
                {
                    if (itemPara.UsrID == item.ParaUsrID)
                    {
                        linha += itemPara.UsrNome + ";";
                    }
                }
                linha += item.ChamadoDtAbertura.ToShortDateString() + ";";
                linha += item.ChamadoDtPrevista.ToShortDateString() + ";";
                linha += item.Status;

                sw.WriteLine(linha);
                linha = ";Descrição;Status;Dt.Inclusão;Dt.Adiada";
                sw.WriteLine(linha);
                foreach (var item2 in item.PortalChamadosHistorico)
                {
                    linha = ";" + item2.Descricao + ";" + item2.ParaStatus + ";" + item2.DtIncl + ";" + item2.DtAdiar + ";";

                    sw.WriteLine(linha);

                }
            }
            sw.Close();

            Response.ClearContent();
            Response.ContentEncoding = encoding;
            Response.AddHeader("content-disposition", "attachment; filename=Chamados_" + System.Web.HttpContext.Current.Session["LoginUsuario"].ToString() + ".csv");
            Response.ContentType = "application/excel";
            Response.Write(sw);
            Response.End();

        }

        [HttpGet]
        public ActionResult Anexos(int id)
        {
            string patch = string.Empty;
            ViewModels.VwImagens _VwImagens = new ViewModels.VwImagens();
            var _portalChamados = db.PortalChamados.Find(id);
            patch = Server.MapPath("/Anexos/" + _portalChamados.ChamadoTitulo);

            if (!Directory.Exists(patch))
            {
                Directory.CreateDirectory(patch);
            }

            string[] ArquivosImg = Directory.GetFiles(patch, "*.*", SearchOption.AllDirectories);
            string[] ArquivosNome = new string[ArquivosImg.Count()];
            string[] ArquivosNomeArquivo = new string[ArquivosImg.Count()];
            string[] ArquivosDataUpload = new string[ArquivosImg.Count()];
            int i = 0;
            foreach (string fileName in ArquivosImg)
            {
                FileInfo infoArquivo = new FileInfo(fileName);
                ArquivosNome[i] = "/" + infoArquivo.Directory.Name + "/" + infoArquivo.Name;
                ArquivosNomeArquivo[i] = infoArquivo.Name;
                ArquivosDataUpload[i] = infoArquivo.CreationTime.Date.ToShortDateString();
                i++;
            }

            _VwImagens.Imagens = ArquivosNome;
            _VwImagens.NomeArquivo = ArquivosNomeArquivo;
            _VwImagens.DataUpload = ArquivosDataUpload;
            _VwImagens.ChamadoID = id;
            _VwImagens.ChamadoTitulo = _portalChamados.ChamadoTitulo;

            TempData["titulo"] = "Anexos do Chamado";
            TempData["titulo1"] = _portalChamados.ChamadoTitulo;

            return View(_VwImagens);
        }

        [HttpPost]
        public ActionResult Anexos(string submit, FormCollection formCollection)
        {

            switch (submit)
            {
                case "submitTable":
                    break;

                case "ENVIAR A IMAGEM":
                    this.ImportarImagens(formCollection);
                    break;

                default:
                    break;
            }
            return RedirectToAction("Index", "Home");
        }

        public void ImportarImagens(FormCollection formCollection)
        {
            string caminhoArquivo = string.Empty;

            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["submitFile"];
                System.Text.Encoding encoding = System.Text.Encoding.Unicode;

                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    caminhoArquivo = Server.MapPath("/Anexos/" + formCollection["ChamadoTitulo"] + "/" + file.FileName);
                    file.SaveAs(caminhoArquivo);
                }
            }
        }


        public ActionResult Index_old(PortalUsuario portalUsuario)
        {
            
            if (portalUsuario.UsrID == 0)
            {
                return RedirectToAction("", "Login");
            }
            var _PortalUsuario = db.PortalUsuario.Find(Convert.ToInt32(Session["IdUsuario"]));
            _PortalUsuario.PortalUsuarioGrupo = db.PortalUsuarioGrupo.Find(_PortalUsuario.UsrGrpID);
            System.Web.HttpContext.Current.Session["PortalUsuarioessao"] = _PortalUsuario;
            System.Web.HttpContext.Current.Session["LoginUsuario"] = _PortalUsuario.UsrEmail.ToString();
            System.Web.HttpContext.Current.Session["IdUsuario"] = _PortalUsuario.UsrID.ToString();
            System.Web.HttpContext.Current.Session["NomeUsuario"] = _PortalUsuario.UsrNome.ToString();
            System.Web.HttpContext.Current.Session["UsuarioGestor"] = _PortalUsuario.PortalUsuarioGrupo.UsrGrpTotalAcessoChamado.ToString();
            System.Web.HttpContext.Current.Session["UsrGrpAbrirChamado"] = _PortalUsuario.PortalUsuarioGrupo.UsrGrpAbrirChamado.ToString();
            System.Web.HttpContext.Current.Session["UsrGrpAdiarChamado"] = _PortalUsuario.PortalUsuarioGrupo.UsrGrpAdiarChamado.ToString();
            System.Web.HttpContext.Current.Session["UsrGrpFecharChamado"] = _PortalUsuario.PortalUsuarioGrupo.UsrGrpFecharChamado.ToString();
            System.Web.HttpContext.Current.Session["UsrGrpTratarChamado"] = _PortalUsuario.PortalUsuarioGrupo.UsrGrpTratarChamado.ToString();
            System.Web.HttpContext.Current.Session["UsrEmpID"] = _PortalUsuario.UsrEmpID.ToString();
            System.Web.HttpContext.Current.Session["UsuarioAdmin"] = _PortalUsuario.UsrAdmin.ToString();

            return RedirectToAction("Index","Home");
        }

        public void LogOff()
        {
            System.Web.HttpContext.Current.Session.RemoveAll();
            Response.Redirect("http://homologportal.eiadvanced.com.br/Login?ReturnUrl=%2f");
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult ContaUsuario()
        {
            if (System.Web.HttpContext.Current.Session["LoginUsuario"] == null || System.Web.HttpContext.Current.Session["LoginUsuario"].ToString() == string.Empty)
            {
                return RedirectToAction("Index", "Login");
            }

            if (TempData["idUsuario"] == null)
            {
                TempData["idUsuario"] = System.Web.HttpContext.Current.Session["IdUsuario"];
            }

            IEnumerable<PortalUsuario> lUsuario = db.PortalUsuario.ToList();
            lUsuario = db.PortalUsuario.ToList().Where(e => e.UsrID.Equals(Convert.ToInt32(TempData["idUsuario"])));

            IEnumerable<PortalUsuario> lUsuarios = db.PortalUsuario.ToList();
            if (Session["UsuarioGestor"].ToString() == "True")
            {
                TempData["Acao"] = "Editar";
                var _Emp = db.PortalEmpresa.Find(lUsuario.First().UsrEmpID);
                ViewModels.VwUsuarioUsuarios _vw_Usuario_Usuarios = new ViewModels.VwUsuarioUsuarios();
                IEnumerable<PortalUsuarioGrupo> lGrupos = db.PortalUsuarioGrupo.ToList();
                _vw_Usuario_Usuarios._Usuario = lUsuario.First();
                _vw_Usuario_Usuarios._Usuarios = lUsuarios.ToList();
                _vw_Usuario_Usuarios._UsuarioGrupo = lGrupos.ToList();
                _vw_Usuario_Usuarios._PortalEmpresa = _Emp;
                TempData["titulo"] = "Manutenção de ";
                TempData["titulo1"] = "Usuários";

                return View(_vw_Usuario_Usuarios);
            }
            TempData["msg"] = "´Somente Usuário Gestor tem Acesso a Contas de Usuário";
            return RedirectToAction("Index", "Home", lUsuario.First());

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ContaUsuario(ViewModels.VwUsuarioUsuarios model, string submit, string senha_atual, string senha_nova, string senha_nova2, string selecUsuarios, string selecGrupoUsuario, FormCollection formCollection)
        {

            TempData["AbaDefault"] = "1";
            TempData["titulo"] = "Manutenção de ";
            TempData["titulo1"] = "Usuários";


            switch (submit)
            {
                case "AtualizarPerfil":
                    var usrGrupo = db.PortalUsuarioGrupo.Find(Convert.ToInt32(selecGrupoUsuario));
                    if (formCollection["UsrGrpAbrirChamado"] == "on") { usrGrupo.UsrGrpAbrirChamado = true; } else { usrGrupo.UsrGrpAbrirChamado = false; }
                    if (formCollection["UsrGrpAdiarChamado"] == "on") { usrGrupo.UsrGrpAdiarChamado = true; } else { usrGrupo.UsrGrpAdiarChamado = false; }
                    if (formCollection["UsrGrpAtivo"] == "on") { usrGrupo.UsrGrpAtivo = true; } else { usrGrupo.UsrGrpAtivo = false; }
                    if (formCollection["UsrGrpFecharChamado"] == "on") { usrGrupo.UsrGrpFecharChamado = true; } else { usrGrupo.UsrGrpFecharChamado = false; }
                    if (formCollection["UsrGrpTotalAcessoChamado"] == "on") { usrGrupo.UsrGrpTotalAcessoChamado = true; } else { usrGrupo.UsrGrpTotalAcessoChamado = false; }
                    if (formCollection["UsrGrpTratarChamado"] == "on") { usrGrupo.UsrGrpTratarChamado = true; } else { usrGrupo.UsrGrpTratarChamado = false; }
                    if (formCollection["UsrGrpVerChamado"] == "on") { usrGrupo.UsrGrpVerChamado = true; } else { usrGrupo.UsrGrpVerChamado = false; }
                    
                    db.Entry(usrGrupo).State = EntityState.Modified;
                    db.SaveChanges();

                    var usr = db.PortalUsuario.Find(Convert.ToInt32(selecUsuarios));
                    usr.UsrGrpID = Convert.ToInt32(selecGrupoUsuario);
                    usr.UsrNome = model._Usuario.UsrNome;

                    db.Entry(usr).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["idUsuario"] = selecUsuarios;
                    return RedirectToAction("ContaUsuario");

                case "NovoUsuario":
                    return RedirectToAction("Registrar", "Login", new { Acao = "NovoUsuario" });

                case "submitPageUsuario":
                    TempData["idUsuario"] = selecUsuarios;
                    return RedirectToAction("ContaUsuario");

                case "SalvarNovaSenha":
                    if (senha_nova != senha_nova2)
                    {
                        TempData["msg"] = "Senhas não conferem !";
                        return RedirectToAction("Index", "Home");
                    }
                    if (senha_atual != System.Web.HttpContext.Current.Session["UsuarioSenha"].ToString())
                    {
                        TempData["msg"] = "Senha Atual não Confere !";
                        return RedirectToAction("Index", "Home");
                    }

                    IEnumerable<PortalUsuario> lUsuarios = db.PortalUsuario.ToList().Where(e => e.UsrEmail.Equals(System.Web.HttpContext.Current.Session["LoginUsuario"]));
                    lUsuarios.First().UsrSenha = Helpers.StringChiper.Encrypt(senha_nova, string.Empty);
                    db.Entry(lUsuarios.First()).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["msg"] = "Senha alterada com Sucesso !";
                    TempData["idUsuario"] = selecUsuarios;
                    return RedirectToAction("ContaUsuario");
            }
            TempData["idUsuario"] = selecUsuarios;
            return RedirectToAction("ContaUsuario");

        }


    }
}