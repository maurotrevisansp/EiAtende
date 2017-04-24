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
using System.Globalization;
using System.IO;

namespace EiAtende.Controllers
{
    public class PortalChamadosController : Controller
    {
        private EiAtendeContexto db = new EiAtendeContexto();
        CultureInfo culture = new CultureInfo("pt-BR");


        // GET: PortalChamados
        [HttpGet]
        public ActionResult Index(FormCollection _Formcollection, string submit, ViewModels.VwChamados model, string AtenderEmpID, string DeUsrID, string ParaUsrID, string TipoChamadoID, string AtividadeChamadoID, string Status)
        {
            
            if (System.Web.HttpContext.Current.Session["NomeUsuario"] == null || 
                System.Web.HttpContext.Current.Session["NomeUsuario"].ToString() == string.Empty)
            {
                return RedirectToAction("", "Login");
            }

            if (submit == "SalvarChamado")
            {
                PortalChamados _PortalChamados = new PortalChamados();

                _PortalChamados.AtenderEmpID = Convert.ToInt32(AtenderEmpID);
                _PortalChamados.AtividadeChamadoID = Convert.ToInt32(AtividadeChamadoID);
                _PortalChamados.Avaliacao = string.Empty;
                _PortalChamados.ChamadoConhecimento = string.Empty;
                _PortalChamados.ChamadoDtAbertura = DateTime.Now;
                _PortalChamados.ChamadoDtPrevista = DateTime.Now.AddDays(10);
                _PortalChamados.ChamadoHistorico = model.PortalChamado.ChamadoHistorico;
                if (model.PortalChamado.ChamadoTitulo == string.Empty)
                {
                    model.PortalChamado.ChamadoTitulo = "Titulo";
                }
                _PortalChamados.ChamadoTitulo = model.PortalChamado.ChamadoTitulo;
                _PortalChamados.DeUsrID = Convert.ToInt32(DeUsrID);
                _PortalChamados.ParaUsrID = Convert.ToInt32(ParaUsrID);
                _PortalChamados.TipoChamadoID = Convert.ToInt32(TipoChamadoID);
                _PortalChamados.Status = Status;

                db.PortalChamados.Add(_PortalChamados);
                db.SaveChanges();

            }
            var portalChamados = db.PortalChamados.Include(p => p.PortalAtividadeChamados).Include(p => p.PortalTipoChamados);

            ViewModels.VwChamados _vwChamados = new ViewModels.VwChamados();
            var IdUsuario = Convert.ToInt32((Session["IdUsuario"]));
            var Gestor = Session["UsuarioGestor"].ToString();
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
            }
            _vwChamados.PortalTipoChamados = db.PortalTipoChamados.ToList();
            _vwChamados.PortalEmpresa = db.PortalEmpresa.ToList();
            _vwChamados.PortalAtividadeChamados = db.PortalAtividadeChamados.ToList();
            _vwChamados.DeUsuario = db.PortalUsuario.ToList();
            _vwChamados.ParaUsuario = db.PortalUsuario.ToList();
            _vwChamados.AtenderEmpresa = db.PortalEmpresa.ToList();
            ViewBag.DeUsrID = new SelectList(db.PortalUsuario.ToList().Where(e => e.UsrID == Convert.ToInt32(Session["IdUsuario"])), "UsrID", "UsrNome");
            ViewBag.ParaUsrID = new SelectList(db.PortalUsuario, "UsrID", "UsrNome");
            if (Session["UsuarioGestor"].ToString() == "True")
            {
                ViewBag.AtenderEmpID = new SelectList(db.PortalEmpresa, "EmpID", "EmpNomeFantasia");
            }
            else
            {
                ViewBag.AtenderEmpID = new SelectList(db.PortalEmpresa.ToList().Where(e => e.EmpID == Convert.ToInt32(Session["UsrEmpID"])), "EmpID", "EmpNomeFantasia");
            }

            ViewBag.TipoChamadoID = new SelectList(db.PortalTipoChamados, "TipoChamadoID", "TipoChamadoNome");
            ViewBag.AtividadeChamadoID = new SelectList(db.PortalAtividadeChamados, "AtividadeChamadoID", "AtividadeChamadoNome");

            TempData["titulo"] = "Chamados";
            TempData["titulo1"] = "Cadastrados";
            ViewBag.filtroEmpresa = "99999";
            ViewBag.filtroTipos = 0;
            ViewBag.filtroAtividade = 0;
            ViewBag.filtroStatus = "Todos Status";
            ViewBag.filtroTitulo = string.Empty;
            ViewBag.filtroIdChamado = 0;

            return View(_vwChamados);

        }

        [HttpPost]
        public ActionResult Index(FormCollection _Formcollection, string empresa, string tipochamado, string atividadechamado, string submit, string idchamado, string StatusPesq)
        {

            if (submit == "Excell")
            {
                this.ExportChamadosToExcel(empresa, tipochamado, atividadechamado, submit, idchamado, StatusPesq);
            }

            var portalChamados = db.PortalChamados.Include(p => p.PortalAtividadeChamados).Include(p => p.PortalTipoChamados);

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

            ViewModels.VwChamados _vwChamados = new ViewModels.VwChamados();
            _vwChamados.PortalChamados = portalChamados.ToList().OrderByDescending(e => e.ChamadoID).ToList();
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
                _vwChamados.PortalChamados = _vwChamados.PortalChamados.Where(e => e.Status.Equals(StatusPesq)).ToList();
            }


            _vwChamados.PortalTipoChamados = db.PortalTipoChamados.ToList();
            _vwChamados.PortalEmpresa = db.PortalEmpresa.ToList();
            _vwChamados.PortalAtividadeChamados = db.PortalAtividadeChamados.ToList();
            _vwChamados.DeUsuario = db.PortalUsuario.ToList();
            _vwChamados.ParaUsuario = db.PortalUsuario.ToList();
            _vwChamados.AtenderEmpresa = db.PortalEmpresa.ToList();
            ViewBag.DeUsrID = new SelectList(db.PortalUsuario.ToList().Where(e => e.UsrID == Convert.ToInt32(Session["IdUsuario"])), "UsrID", "UsrNome");
            ViewBag.ParaUsrID = new SelectList(db.PortalUsuario, "UsrID", "UsrNome");
            ViewBag.AtenderEmpID = new SelectList(db.PortalEmpresa, "EmpID", "EmpNomeFantasia");
            ViewBag.TipoChamadoID = new SelectList(db.PortalTipoChamados, "TipoChamadoID", "TipoChamadoNome");
            ViewBag.AtividadeChamadoID = new SelectList(db.PortalAtividadeChamados, "AtividadeChamadoID", "AtividadeChamadoNome");

            TempData["titulo"] = "Chamados";
            TempData["titulo1"] = "Cadastrados";

            return View(_vwChamados);

        }

        [HttpGet]
        public ActionResult EditarChamados(int? id, string Acao)
        {

            var portalChamados = db.PortalChamados.Include(p => p.PortalAtividadeChamados).Include(p => p.PortalTipoChamados);
            PortalChamados _vwChamado = new PortalChamados();
            
            if (id != null)
            {
                if (Session["UsrGrpTratarChamado"].ToString() == "True")
                {
                    if (Acao == "Editar" || Acao == "Adiar" || Acao == "Finalizar")
                    {
                        var _PortalUsuario = db.PortalUsuario.Find(Convert.ToInt32(Session["IdUsuario"]));

                        _vwChamado = portalChamados.ToList().Where(e => e.ChamadoID.Equals(Convert.ToInt32(id))).First();
                        _vwChamado.ChamadoAnexos = db.ChamadoAnexos.ToList().Where(e => e.ChamadoID.Equals(Convert.ToInt32(id))).ToList();
                        ViewBag.DeUsrID = new SelectList(db.PortalUsuario.ToList().Where(e => e.UsrID == Convert.ToInt32(Session["IdUsuario"])), "UsrID", "UsrNome");
                        ViewBag.ParaUsrID = new SelectList(db.PortalUsuario, "UsrID", "UsrNome", _vwChamado.ParaUsrID);
                        if (Session["UsuarioGestor"].ToString() == "True")
                        {
                            ViewBag.AtenderEmpID = new SelectList(db.PortalEmpresa, "EmpID", "EmpNomeFantasia", _vwChamado.AtenderEmpID);
                        }
                        else
                        {
                            ViewBag.AtenderEmpID = new SelectList(db.PortalEmpresa.ToList().Where(e => e.EmpID == _PortalUsuario.UsrEmpID), "EmpID", "EmpNomeFantasia", _vwChamado.AtenderEmpID);
                        }
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
                return RedirectToAction("Index", "PortalChamados");
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
                db.Entry(portalChamados).State = EntityState.Modified;
                db.SaveChanges();
            }
            TempData["titulo"] = "Chamados";
            TempData["titulo1"] = "Cadastrados";

            return RedirectToAction("Index", "Home");

        }


        public void ExportChamadosToExcel(string empresa, string tipochamado, string atividadechamado, string submit, string idchamado, string StatusPesq)
        {

            var portalChamados = db.PortalChamados.Include(p => p.PortalAtividadeChamados).Include(p => p.PortalTipoChamados);

            ViewModels.VwChamados _vwChamados = new ViewModels.VwChamados();
            _vwChamados.PortalChamados = portalChamados.ToList();
            if (idchamado.Length > 0)
            {
                _vwChamados.PortalChamados = _vwChamados.PortalChamados.Where(e => e.ChamadoID.Equals(Convert.ToInt32(idchamado))).ToList();
            }
            if (empresa != "Todas Empresas")
            {
                _vwChamados.PortalChamados = _vwChamados.PortalChamados.Where(e => e.AtenderEmpID.Equals(Convert.ToInt32(empresa))).ToList();
            }
            if (tipochamado != "Todos Tipos")
            {
                _vwChamados.PortalChamados = _vwChamados.PortalChamados.Where(e => e.TipoChamadoID.Equals(Convert.ToInt32(tipochamado))).ToList();
            }
            if (atividadechamado != "Todas Atividades")
            {
                _vwChamados.PortalChamados = _vwChamados.PortalChamados.Where(e => e.AtividadeChamadoID.Equals(Convert.ToInt32(atividadechamado))).ToList();
            }
            if (StatusPesq != "Todos Status")
            {
                _vwChamados.PortalChamados = _vwChamados.PortalChamados.Where(e => e.Status.Equals(StatusPesq)).ToList();
            }


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
                foreach(var itemAt in _vwChamados.AtenderEmpresa)
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


        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
