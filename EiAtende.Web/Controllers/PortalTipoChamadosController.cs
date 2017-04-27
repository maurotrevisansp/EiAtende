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

namespace EiAtende.Controllers
{
    public class PortalTipoChamadosController : Controller
    {
        private EiAtendeContexto db = new EiAtendeContexto();

        // GET: PortalTipoChamados
        public ActionResult Index()
        {
            var portalTipoChamados = db.PortalTipoChamados.Include(p => p.PortalEmpresa);
            TempData["msg"] = "Lista de Tipos de Chamado";
            var _portalTipoChamados = portalTipoChamados.ToList();
            var _portalEmpresas = db.PortalEmpresa.ToList();
            ViewModels.VwTiposChamados _vwTiposChamados = new ViewModels.VwTiposChamados();
            _vwTiposChamados.PortalTipoChamados = _portalTipoChamados;
            _vwTiposChamados.PortalEmpresa = _portalEmpresas;
            TempData["titulo"] = "Tipos de Chamados";
            TempData["titulo1"] = "Cadastrados";

            return View(_vwTiposChamados);
        }

        // GET: PortalTipoChamados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortalTipoChamados portalTipoChamados = db.PortalTipoChamados.Find(id);
            if (portalTipoChamados == null)
            {
                return HttpNotFound();
            }
            return View(portalTipoChamados);
        }

        // GET: PortalTipoChamados/Create
        public ActionResult Create()
        {
            ViewBag.EmpID = new SelectList(db.PortalEmpresa, "EmpID", "EmpRazao");
            return View();
        }

        // POST: PortalTipoChamados/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TipoChamadoID,TipoChamadoNome,EmpID")] PortalTipoChamados portalTipoChamados)
        {
            if (ModelState.IsValid)
            {
                db.PortalTipoChamados.Add(portalTipoChamados);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmpID = new SelectList(db.PortalEmpresa, "EmpID", "EmpRazao", portalTipoChamados.EmpID);
            return View(portalTipoChamados);
        }

        // GET: PortalTipoChamados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortalTipoChamados portalTipoChamados = db.PortalTipoChamados.Find(id);
            if (portalTipoChamados == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpID = new SelectList(db.PortalEmpresa, "EmpID", "EmpRazao", portalTipoChamados.EmpID);
            return View(portalTipoChamados);
        }

        // POST: PortalTipoChamados/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TipoChamadoID,TipoChamadoNome,EmpID")] PortalTipoChamados portalTipoChamados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portalTipoChamados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmpID = new SelectList(db.PortalEmpresa, "EmpID", "EmpRazao", portalTipoChamados.EmpID);
            return View(portalTipoChamados);
        }

        // GET: PortalTipoChamados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortalTipoChamados portalTipoChamados = db.PortalTipoChamados.Find(id);
            if (portalTipoChamados == null)
            {
                return HttpNotFound();
            }
            return View(portalTipoChamados);
        }

        // POST: PortalTipoChamados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PortalTipoChamados portalTipoChamados = db.PortalTipoChamados.Find(id);
            db.PortalTipoChamados.Remove(portalTipoChamados);
            db.SaveChanges();
            return RedirectToAction("Index");
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
