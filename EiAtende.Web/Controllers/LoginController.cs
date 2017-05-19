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
using System.IO;
using EiAtende.ViewModels;

namespace EiAtende.Web.Controllers
{
    public class LoginController : Controller
    {
        private EiAtendeContexto db = new EiAtendeContexto();
        private IEnumerable<PortalUsuario> _PortalUsuario;

        // GET: Login
        [HttpGet]
        public ActionResult Index(PortalUsuario _PortalUsuario)
        {
            if (_PortalUsuario.UsrEmail == null)
            {
                return View();
            }

            this.CarregarSessao(_PortalUsuario);
            return RedirectToAction("", "Home");
        }

        [HttpPost]
        public ActionResult Index(FormCollection _FormColletion, string submit)
        {
            var portalUsuarios = db.PortalUsuario.Include(p => p.PortalUsuarioGrupo);
            PortalUsuario portalUsuario = new PortalUsuario();
            switch (submit)
            {
                case "Login":
                    
                    _PortalUsuario = portalUsuarios.ToList().Where(e => e.UsrEmail.Equals(_FormColletion["UsuarioLogin"]));
                    if (_PortalUsuario.Count() > 0)
                    {
                        if (!_PortalUsuario.First().UsrAtivo)
                        {
                            TempData["msg"] = "Erro: Usuário não esta Ativo";
                            return RedirectToAction("", "Login");
                        }
                        if (_FormColletion["UsuarioLogin"].Length > 0)
                        {
                            if (Helpers.StringChiper.Decrypt(_PortalUsuario.First().UsrSenha, string.Empty) != _FormColletion["UsuarioSenha"])
                            {
                                TempData["msg"] = "Erro: Senhas não Conferem";
                                return RedirectToAction("", "Login");
                            }
                        }
                        else
                        {
                            TempData["msg"] = "Erro: Senha Invalida";
                            return RedirectToAction("", "Login");
                        }
                        this.CarregarSessao(_PortalUsuario.First());
                        ViewBag.Menu = "CONVERSOR";
                        portalUsuario = _PortalUsuario.First();
                        return RedirectToAction("Index", "Home", portalUsuario);
                    }

                    TempData["msg"] = "Erro: Usuário não Encontrado";
                    return RedirectToAction("", "Login");

                //case "Visitante":

                //    _PortalUsuario = db.PortalUsuario.ToList().Where(e => e.UsrEmail.Equals("visitante@eiadvanced.com"));
                //    if (_PortalUsuario.Count() > 0)
                //    {
                //        this.CarregarSessao(_PortalUsuario.First());
                //        ViewBag.Menu = "CONVERSOR";
                //        return RedirectToAction("Index", "Home", _PortalUsuario.First());
                //    }
                //    else
                //    {
                //        TempData["msg"] = "Erro: Usuário não encontrado !";
                //        return RedirectToAction("Login", "Home");
                //    }
                    

                default:
                    break;
            }

            return RedirectToAction("", "Login");
        }

        public string CarregarSessao(PortalUsuario _PortalUsuario)
        {
            System.Web.HttpContext.Current.Session.Clear();
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


            return "Exito";
        }

        [HttpGet]
        public ActionResult Registrar()
        {
            return View(db.PortalUsuarioGrupo.ToList());
        }

        [HttpPost]
        public ActionResult Registrar(FormCollection _FormCollection, string Grupo)
        {
            _PortalUsuario = db.PortalUsuario.ToList().Where(e => e.UsrEmail.Equals(_FormCollection["emailRegistrar"]));

            if (_PortalUsuario.Count() > 0)
            {
                TempData["msg"] = "Erro: Usuário ja Cadastrado !";
                return RedirectToAction("Login", "Home");
            }

            PortalUsuario _Usuario = new PortalUsuario();
            _Usuario.UsrGrpID = Convert.ToInt32(Grupo);
            _Usuario.UsrEmail = _FormCollection["emailRegistrar"];
            _Usuario.UsrNome = _FormCollection["nomeRegistrar"];
            _Usuario.UsrSenha = Helpers.StringChiper.Encrypt(_FormCollection["senhaRegistrar"], string.Empty);
            _Usuario.UsrAdmin = true;
            _Usuario.UsrAdminSistema = true;
            _Usuario.UsrAlteracao = DateTime.Now;
            _Usuario.UsrAlteracaoUsuario = 1;
            _Usuario.UsrAtivo = false;
            _Usuario.UsrEmpID = 0;
            _Usuario.UsrExclusao = null;
            _Usuario.UsrExclusaoUsuario = 1;
            _Usuario.UsrInclusao = DateTime.Now;
            _Usuario.UsrInclusaoUsuario = 1;

            db.PortalUsuario.Add(_Usuario);
            db.SaveChanges();
            TempData["msg"] = "Sucesso: Usuário foi Registrado !";
            return RedirectToAction("", "Login");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
