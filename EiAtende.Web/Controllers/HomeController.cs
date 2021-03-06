﻿using System;
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
        public ActionResult Index(PortalUsuario portalUsuario, FormCollection formColletion, string submit, ViewModels.VwChamados model, string AtenderEmpID, string DeUsrID, 
            string ParaUsrID, string TipoChamadoID, string AtividadeChamadoID, string Status, string atenderEmpresa,
            string idChamadoHist, string empresa, string tipochamado, string atividadechamado, string Historico, string msg,
            string idchamado, string StatusPesq, string StatusHist, string deStatus, string Descricao, string dtAdiar, string paraStatus,
            string filtroempresa, string filtrotipochamado, string filtroatividadechamado, string filtroidchamado, string filtroStatusPesq, 
            string filtroempresa1, string Acao, string filtrotipochamado1, string manterStatus, string filtroatividadechamado1, string filtroidchamado1, string filtroStatusPesq1, string Aprovar)
        {
            if (Request.QueryString["idDoChamado"] != null)
            {
                idchamado = Request.QueryString["idDoChamado"];
                submit = "PesquisarGeral";
            }
          
            if (msg != null)
            {
                ViewData["msg"] = msg;
            }


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
                ViewData["msg"] = "Arquivo Excell Gerado com Sucesso";
                _vwChamados = PesquisaGeral(empresa, tipochamado, atividadechamado, idchamado, StatusPesq);
                return View(_vwChamados);
            }

            if (submit == "PesquisarGeral")
            {
                _vwChamados = PesquisaGeral(empresa, tipochamado, atividadechamado, idchamado, StatusPesq);
                TempData["titulo"] = "Chamados";
                TempData["titulo1"] = "Cadastrados";
                if (ViewData["msg"] == null)
                {
                    ViewData["msg"] = "Pesquisa Efetuada com Sucesso";
                }

                return View(_vwChamados);

            }

            var _PortalUsuario = db.PortalUsuario.Find(Convert.ToInt32(Session["IdUsuario"]));
            _PortalUsuario.PortalUsuarioGrupo = db.PortalUsuarioGrupo.Find(_PortalUsuario.UsrGrpID);

            if (submit == "NovoHistorico")
            {
                PortalChamadosHistorico _PortalChamadosHistorico = new PortalChamadosHistorico();
                _PortalChamadosHistorico.ChamadoID = Convert.ToInt32(idChamadoHist);
                _PortalChamadosHistorico.Descricao = Descricao + " - Usuário: " + Session["NomeUsuario"];
                _PortalChamadosHistorico.DeStatus = deStatus;
                _PortalChamadosHistorico.DtIncl = DateTime.Now;
                if (manterStatus != null)
                {
                    _PortalChamadosHistorico.ParaStatus = paraStatus;
                }
                else
                {
                    _PortalChamadosHistorico.ParaStatus = deStatus;
                }
                db.PortalChamadosHistorico.Add(_PortalChamadosHistorico);
                db.SaveChanges();

                if (manterStatus != null)
                {
                    PortalChamados _portalChamados = db.PortalChamados.Find(Convert.ToInt32(idChamadoHist));
                    _portalChamados.Status = paraStatus;
                    if (StatusHist == "Finalizado")
                    {
                        _portalChamados.ChamadoDtTermino = DateTime.Now;
                    }
                    db.Entry(_portalChamados).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ViewData["msg"] = "Histórico para o Chamado " + idChamadoHist + " Incluido com Sucesso";

                _vwChamados = PesquisaGeral(filtroempresa1, filtrotipochamado1, filtroatividadechamado1, filtroidchamado1, filtroStatusPesq1);
                return View(_vwChamados);

            }

            _vwChamados = PesquisaGeral(filtroempresa1, filtrotipochamado1, filtroatividadechamado1, filtroidchamado1, filtroStatusPesq1);
            return View(_vwChamados);

        }

        [HttpPost]
        public ActionResult Index(FormCollection formColletion, string idChamadoAdiar, string idChamadoEmail, string Mensagem, string idChamado, string Historico, string dtAdiar, string dtAdiar2, string submit, string btnAprovar, string btnReprovar, string AtividadeChamadoID, string atenderEmpresa, ViewModels.VwChamados model, string DeUsrID, string ParaUsrID, string TipoChamadoID)
        {
            ViewData["msg"] = "Lista de Chamados";
            var idHistoricoAprovar = string.Empty;
            var idHistoricoReprovar = string.Empty;
            var portalChamados = db.PortalChamados.Include(p => p.PortalAtividadeChamados).Include(p => p.PortalTipoChamados);
            ViewModels.VwChamados _vwChamados = new ViewModels.VwChamados();
            PortalAtividadeChamados _Atividades = new PortalAtividadeChamados();
            PortalChamadosHistorico _HistoricoAR = new PortalChamadosHistorico();

            if (Session["LoginUsuario"] == null)
            {
                return RedirectToAction("", "Login");
            }

            var _PortalUsuario = db.PortalUsuario.Find(Convert.ToInt32(Session["IdUsuario"]));
            _PortalUsuario.PortalUsuarioGrupo = db.PortalUsuarioGrupo.Find(_PortalUsuario.UsrGrpID);
            if (idChamadoAdiar == null)
            {
                idChamadoAdiar = "0";
            }
            var _chamado = db.PortalChamados.Find(Convert.ToInt32(idChamadoAdiar));
            PortalChamadosHistorico _hist = new PortalChamadosHistorico();
            if (btnAprovar != null)
            {
                idHistoricoAprovar = btnAprovar;
                submit = "Aprovar";
            }
            if (btnReprovar != null)
            {
                idHistoricoReprovar = btnReprovar;
                submit = "Reprovar";
            }
            switch (submit)
            {

                case "Adiar":
                    _chamado = db.PortalChamados.Find(Convert.ToInt32(idChamadoAdiar));
                    _hist.ChamadoID = Convert.ToInt32(idChamadoAdiar);
                    _hist.Descricao = Historico + " - Solicitado por: " + Session["NomeUsuario"].ToString();
                    _hist.DeStatus = _chamado.Status;
                    _hist.ParaStatus = "Pendente Aprovar";
                    _hist.DtAdiar = Convert.ToDateTime(dtAdiar, culture.DateTimeFormat);
                    _hist.DtAdiar = _hist.DtAdiar.Value.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute);
                    
                    _hist.DtIncl = DateTime.Now;
                    db.PortalChamadosHistorico.Add(_hist);
                    db.SaveChanges();

                    _chamado.Status = "Pendente Aprovar";
                    db.Entry(_chamado).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewData["msg"] = "Solicitação para Adiar Incluida com Sucesso";

                    break;

                case "Aprovar":

                    _HistoricoAR = db.PortalChamadosHistorico.Find(Convert.ToInt32(idHistoricoAprovar));
                    _HistoricoAR.Descricao = _HistoricoAR.Descricao + " - " + Historico + " - Aprovado por: " + Session["NomeUsuario"].ToString();
                    _HistoricoAR.ParaStatus = "Aprovado";
                    db.Entry(_HistoricoAR).State = EntityState.Modified;
                    db.SaveChanges();

                    _chamado.Status = _HistoricoAR.DeStatus;
                    _chamado.ChamadoDtAdiada = _HistoricoAR.DtAdiar;
                    db.Entry(_chamado).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewData["msg"] = "Solicitação Aprovada";

                    break;

                case "Reprovar":

                    _HistoricoAR = db.PortalChamadosHistorico.Find(Convert.ToInt32(idHistoricoReprovar));
                    _HistoricoAR.Descricao = _HistoricoAR.Descricao + " - " + Historico + " - REPROVADO por: " + Session["NomeUsuario"].ToString();
                    _HistoricoAR.ParaStatus = "Reprovado";
                    db.Entry(_HistoricoAR).State = EntityState.Modified;
                    db.SaveChanges();

                    _chamado.Status = _HistoricoAR.DeStatus;
                    db.Entry(_chamado).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewData["msg"] = "Solicitação Reprovada";

                    break;

                case "SalvarChamado":

                    PortalChamados _PortalChamados = new PortalChamados();
                    _Atividades = db.PortalAtividadeChamados.Find(Convert.ToInt32(AtividadeChamadoID));

                    _PortalChamados.AtenderEmpID = Convert.ToInt32(atenderEmpresa);
                    _PortalChamados.AtividadeChamadoID = Convert.ToInt32(AtividadeChamadoID);
                    _PortalChamados.Avaliacao = string.Empty;
                    _PortalChamados.ChamadoConhecimento = string.Empty;
                    _PortalChamados.ChamadoDtAbertura = DateTime.Now; 
                    if (dtAdiar2 != string.Empty)
                    {
                        _PortalChamados.ChamadoDtPrevista = Convert.ToDateTime(dtAdiar2);
                    }
                    else
                    {
                        _PortalChamados.ChamadoDtPrevista = DateTime.Now.AddDays(_Atividades.PrevisaoDias).AddHours(_Atividades.PrevisaoHoras).AddMinutes(_Atividades.PrevisaoMinutos);
                    }
                    _PortalChamados.ChamadoHistorico = Historico;
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
                    var ultimochamado = db.PortalChamados.ToList().Last();

                    this.ImportarImagens(formColletion, ultimochamado.ChamadoID);

                    _hist.ChamadoID = Convert.ToInt32(ultimochamado.ChamadoID);
                    _hist.Descricao = Historico + " - Aberto pelo usuário: " + Session["NomeUsuario"]; 
                    _hist.DeStatus = _PortalChamados.Status;
                    _hist.ParaStatus = _PortalChamados.Status;
                    _hist.DtIncl = DateTime.Now;
                    db.PortalChamadosHistorico.Add(_hist);
                    db.SaveChanges();

                    ViewData["msg"] = "Chamado Incluido com Sucesso";
                    break;

                case "ColetarAnexos":

                    this.ImportarImagens(formColletion, Convert.ToInt32(idChamado));
                    ViewData["msg"] = "Anexos Coletados com Sucesso";

                    break;

                case "EnviarEmail":

                    this.EnviarEmail(Convert.ToInt32(idChamadoEmail), Mensagem);
                    ViewData["msg"] = "Email Enviado com Sucesso";

                    break;

                default:
                    break;
            }

            return RedirectToAction("Index", "Home", new { msg = ViewData["msg"] });

        }

        public ActionResult EnviarEmail(int? IdChamado, string Mensagem)
        {

            var IportalChamados = db.PortalChamados.Include(p => p.PortalAtividadeChamados).Include(p => p.PortalTipoChamados);
            var portalChamados = IportalChamados.ToList().Where(e => e.ChamadoID == Convert.ToInt32(IdChamado)).First();
            portalChamados.PortalChamadosHistorico =  db.PortalChamadosHistorico.ToList().Where(e => e.ChamadoID.Equals(portalChamados.ChamadoID)).ToList();
            var usr = db.PortalUsuario.Find(Convert.ToInt32(portalChamados.ParaUsrID));
            string body = "Olá, " + usr.UsrNome + "<br>Segue Histórico de Chamado sob sua responsabilidade <br><br>";
            foreach (var item in portalChamados.PortalChamadosHistorico)
            {
                body = body + item.DtIncl.ToShortDateString() + " " + item.Descricao + "<br />";
            }
            body = body + "<br />" + Mensagem;
            TempData["msg"] = _enviarEmail.EnviarEmail(ConfigurationManager.AppSettings["smtpDominio"], ConfigurationManager.AppSettings["smtpUsuario"], ConfigurationManager.AppSettings["smtpSenha"], ConfigurationManager.AppSettings["smtpPorta"], usr.UsrEmail, "Chamado " + portalChamados.ChamadoID + " - " + portalChamados.ChamadoTitulo, body, string.Empty, string.Empty);
            var msg = "Email Enviado com Sucesso";
            return RedirectToAction("Index", "Home", new { msg = msg });
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
            var lEmp = new int[_PortalEmpresas.Count()];
            int i = 0;
            foreach (var item in _PortalEmpresas)
            {
                lEmp[i] = item.EmpID;
                i++;
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
                    .Where(e => lEmp.Contains(e.AtenderEmpID)).ToList();
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

            _vwChamados.PortalTipoChamados = db.PortalTipoChamados.ToList().Where(e => e.Ativo.Equals(true) && e.EmpID == _PortalUsuario.UsrEmpID).ToList();

            _vwChamados.PortalEmpresa = _PortalEmpresas;
            _vwChamados.PortalAtividadeChamados = db.PortalAtividadeChamados.ToList().Where(e => e.Ativo.Equals(true) && e.EmpID == _PortalUsuario.UsrEmpID).ToList();
            _vwChamados.DeUsuario = db.PortalUsuario.ToList();
            _vwChamados.ParaUsuario = db.PortalUsuario.ToList();
            _vwChamados.AtenderEmpresa = _PortalEmpresas;
            ViewBag.DeUsrID = new SelectList(db.PortalUsuario.ToList().Where(e => e.UsrID == Convert.ToInt32(Session["IdUsuario"])), "UsrID", "UsrNome");
            ViewBag.ParaUsrID = new SelectList(_PortalUsuariosDasEmpresas, "UsrID", "UsrNome");
            ViewBag.AtenderEmpID = new SelectList(_PortalEmpresas, "EmpID", "EmpNomeFantasia");
            ViewBag.TipoChamadoID = new SelectList(db.PortalTipoChamados.ToList().Where(e => e.Ativo.Equals(true) && e.EmpID == _PortalUsuario.UsrEmpID).ToList(), "TipoChamadoID", "TipoChamadoNome");
            ViewBag.AtividadeChamadoID = new SelectList(db.PortalAtividadeChamados.ToList().Where(e => e.Ativo.Equals(true) && e.EmpID == _PortalUsuario.UsrEmpID).ToList(), "AtividadeChamadoID", "AtividadeChamadoNome");

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
                        ViewBag.TipoChamadoID = new SelectList(db.PortalTipoChamados.ToList().Where(e => e.Ativo.Equals(true)).ToList(), "TipoChamadoID", "TipoChamadoNome", _vwChamado.TipoChamadoID);
                        ViewBag.AtividadeChamadoID = new SelectList(db.PortalAtividadeChamados.ToList().Where(e => e.Ativo.Equals(true)).ToList(), "AtividadeChamadoID", "AtividadeChamadoNome", _vwChamado.AtividadeChamadoID);
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
                    ViewData["msg"] = "Usuário sem permissão para Tratar Chamados";
                    return RedirectToAction("Index", "Home", (PortalUsuario)Session["PortalUsuarioessao"]);
                }

            }
            TempData["titulo"] = "Chamados";
            TempData["titulo1"] = "Cadastrados";
            if (Session["UsrGrpAbrirChamado"].ToString() == "True")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["msg"] = "Usuário sem permissão de Abrir Chamados";
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


            _vwChamados.PortalTipoChamados = db.PortalTipoChamados.ToList().Where(e => e.Ativo.Equals(true)).ToList();
            _vwChamados.PortalEmpresa = db.PortalEmpresa.ToList();
            _vwChamados.PortalAtividadeChamados = db.PortalAtividadeChamados.ToList().Where(e => e.Ativo.Equals(true)).ToList();
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

        public void ImportarImagens(FormCollection formCollection, int idChamado)
        {
            string caminhoArquivo = string.Empty;
            var seq = "0";
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase hpf = Request.Files[i] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    string fileName = hpf.FileName;
                    string fileContentType = hpf.ContentType;
                    byte[] fileBytes = new byte[hpf.ContentLength];
                    hpf.InputStream.Read(fileBytes, 0, Convert.ToInt32(hpf.ContentLength));
                    caminhoArquivo = Server.MapPath("/Anexos/" + idChamado.ToString());// + "/" + hpf.FileName);
                    if (!Directory.Exists(caminhoArquivo))
                    {
                        Directory.CreateDirectory(caminhoArquivo);
                    }
                    caminhoArquivo = caminhoArquivo + "/" + hpf.FileName;
                    seq = "0";
                    while (true)
                    {
                        if (System.IO.File.Exists(caminhoArquivo))
                        {
                            seq = (Convert.ToInt32(seq) + 1).ToString("000");
                            fileName = fileName.Substring(0, fileName.IndexOf(".")) + "_" + seq + fileName.Substring(fileName.IndexOf("."), fileName.Length - fileName.IndexOf("."));
                            fileName = fileName.Replace("_" + (Convert.ToInt32(seq) - 1).ToString("000"), "");
                            caminhoArquivo = Server.MapPath("/Anexos/" + idChamado.ToString() + "/" + fileName);
                        }
                        else
                        {
                            break;
                        }
                    }
                    hpf.SaveAs(caminhoArquivo);

                    ChamadoAnexos Anexos = new ChamadoAnexos();
                    Anexos.Ds_Anexo = formCollection["dsAnexo"];
                    Anexos.Dt_Incl_Anexo = DateTime.Now;
                    Anexos.ChamadoID = idChamado;
                    Anexos.Nome_Arquivo_Anexo = fileName;
                    Anexos.Patch_Anexo = idChamado.ToString() + "/" + fileName;
                    var chamado = db.PortalChamados.Find(Anexos.ChamadoID);
                    Anexos.PortalChamados = chamado;
                    db.ChamadoAnexos.Add(Anexos);
                    db.SaveChanges();
                }
            }

            //foreach (string file in Request.Files)
            //{
            //    HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
            //    if (hpf.ContentLength == 0)
            //        continue;
            //    string fileName = hpf.FileName;
            //    string fileContentType = hpf.ContentType;
            //    byte[] fileBytes = new byte[hpf.ContentLength];
            //    hpf.InputStream.Read(fileBytes, 0, Convert.ToInt32(hpf.ContentLength));
            //    caminhoArquivo = Server.MapPath("/Anexos/" + formCollection["ChamadoTitulo"] + "/" + hpf.FileName);
            //    hpf.SaveAs(caminhoArquivo);

            //    ChamadoAnexos Anexos = new ChamadoAnexos();
            //    Anexos.Ds_Anexo = formCollection["dsAnexo"];
            //    Anexos.Dt_Incl_Anexo = DateTime.Now;
            //    Anexos.ChamadoID = idChamado;
            //    Anexos.Nome_Arquivo_Anexo = fileName;
            //    Anexos.Patch_Anexo = formCollection["ChamadoTitulo"] + "/" + hpf.FileName;
            //    var chamado = db.PortalChamados.Find(Anexos.ChamadoID);
            //    Anexos.PortalChamados = chamado;
            //    db.ChamadoAnexos.Add(Anexos);
            //    db.SaveChanges();
            //    ifile++;

            //    //do something with file
            //}

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
            ViewData["msg"] = "´Somente Usuário Gestor tem Acesso a Contas de Usuário";
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
                        ViewData["msg"] = "Senhas não conferem !";
                        return RedirectToAction("Index", "Home");
                    }
                    if (senha_atual != System.Web.HttpContext.Current.Session["UsuarioSenha"].ToString())
                    {
                        ViewData["msg"] = "Senha Atual não Confere !";
                        return RedirectToAction("Index", "Home");
                    }

                    IEnumerable<PortalUsuario> lUsuarios = db.PortalUsuario.ToList().Where(e => e.UsrEmail.Equals(System.Web.HttpContext.Current.Session["LoginUsuario"]));
                    lUsuarios.First().UsrSenha = Helpers.StringChiper.Encrypt(senha_nova, string.Empty);
                    db.Entry(lUsuarios.First()).State = EntityState.Modified;
                    db.SaveChanges();

                    ViewData["msg"] = "Senha alterada com Sucesso !";
                    TempData["idUsuario"] = selecUsuarios;
                    return RedirectToAction("ContaUsuario");
            }
            TempData["idUsuario"] = selecUsuarios;
            return RedirectToAction("ContaUsuario");

        }

        public ActionResult PagSeguro()
        {
            return View();
        }


    }
}