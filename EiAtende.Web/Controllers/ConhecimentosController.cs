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

namespace EiAtende.Web.Controllers
{
    public class PortalConhecimentosController : Controller
    {
        private EiAtendeContexto db = new EiAtendeContexto();

        // GET: PortalConhecimentos
        public ActionResult Index(string submit, FormCollection formColletion)
        {
            if (System.Web.HttpContext.Current.Session["NomeUsuario"] == null ||
                System.Web.HttpContext.Current.Session["NomeUsuario"].ToString() == string.Empty)
            {
                return RedirectToAction("", "Login");
            }

            IEnumerable<PortalConhecimentos> _PortalConhecimentos;
            _PortalConhecimentos = db.PortalConhecimentos.ToList();
            if (submit != null)
            {
                if (submit == "pesquizar")
                {
                    _PortalConhecimentos = db.PortalConhecimentos.ToList()
                        .Where(e => e.CnhTitulo.ToUpper().Contains(formColletion["pesq_titulo"].ToUpper())
                        || e.CnhTagsBusca.ToUpper().Contains(formColletion["pesq_titulo"].ToUpper()));
                }
            }
            TempData["titulo"] = "Manuais";
            TempData["titulo1"] = "Cadastrados";

            return View(_PortalConhecimentos);
        }

        // GET: EditarPortalConhecimentos
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult NovoConhecimento(int? id)
        {
            if (System.Web.HttpContext.Current.Session["LoginUsuario"] == null || System.Web.HttpContext.Current.Session["LoginUsuario"].ToString() == string.Empty)
            {
                return RedirectToAction("Login", "Home");
            }

            PortalConhecimentos _conhecimento = new PortalConhecimentos();
            if (id != null)
            {
                TempData["Acao"] = "Editar";
                _conhecimento = db.PortalConhecimentos.Find(id);
                TempData["titulo"] = "Editar";
                TempData["titulo1"] = "Manual";
            }
            else
            {
                TempData["titulo"] = "Novo";
                TempData["titulo1"] = "Manual";
            }

            return View(_conhecimento);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NovoConhecimento([Bind(Include = "CnhID,CnhTitulo,CnhDescricao,CnhTagsBusca,CnhDataCriacao,CnhFonte,CnhDataAlteracao,CnhUrlParcial,CnhTexto")] PortalConhecimentos PortalConhecimentos, FormCollection formColletion)
        {
            if (ModelState.IsValid)
            {
                PortalConhecimentos.CnhTitulo = formColletion["titulo"];
                PortalConhecimentos.CnhDescricao = formColletion["descricao"];
                PortalConhecimentos.CnhDataCriacao = DateTime.Now;
                PortalConhecimentos.CnhFonte = formColletion["fonte"];
                PortalConhecimentos.CnhTagsBusca = formColletion["tags_busca"];
                PortalConhecimentos.CnhTexto = formColletion["texto"];

                if (PortalConhecimentos.CnhID == 0)
                {
                    db.PortalConhecimentos.Add(PortalConhecimentos);
                    db.SaveChanges();
                }
                else
                {
                    PortalConhecimentos.CnhUrlParcial = "../PortalConhecimentos/VisualizarConhecimento?id=" + PortalConhecimentos.CnhID;
                    db.Entry(PortalConhecimentos).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            TempData["titulo"] = "Manuais";
            TempData["titulo1"] = "Cadastrados";

            return RedirectToAction("ListaDeConhecimentos");
        }

        [HttpGet]
        public ActionResult VisualizarConhecimento(int? id)
        {
            PortalConhecimentos _conhecimento = new PortalConhecimentos();
            _conhecimento = db.PortalConhecimentos.Find(id);
            TempData["titulo"] = "Visualizar";
            TempData["titulo1"] = "Manual";
            return View(_conhecimento);

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
