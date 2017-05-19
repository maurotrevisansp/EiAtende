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
    public class PortalAtividadeChamadosController : Controller
    {
        private EiAtendeContexto db = new EiAtendeContexto();

        // GET: PortalAtividadeChamados
        public ActionResult Index()
        {
            var portalAtividadeChamados = db.PortalAtividadeChamados.Include(p => p.PortalEmpresa);
            TempData["msg"] = "Lista de Atividades de Chamado";
            var _portalAtividadeChamados = portalAtividadeChamados.ToList().Where(e => e.EmpID == Convert.ToInt32(Session["UsrEmpID"])).ToList();
            var _portalEmpresas = db.PortalEmpresa.ToList();
            ViewModels.VwAtividadesChamado _vwAtividadesChamados = new ViewModels.VwAtividadesChamado();
            _vwAtividadesChamados.PortalAtividadeChamados = _portalAtividadeChamados;
            _vwAtividadesChamados.PortalEmpresa = _portalEmpresas;
            TempData["titulo"] = "Atividades de Chamados";
            TempData["titulo1"] = "Cadastrados";

            return View(_vwAtividadesChamados);
        }

        // GET: PortalAtividadeChamados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortalAtividadeChamados portalAtividadeChamados = db.PortalAtividadeChamados.Find(id);
            if (portalAtividadeChamados == null)
            {
                return HttpNotFound();
            }
            return View(portalAtividadeChamados);
        }

        // GET: PortalAtividadeChamados/Create
        public ActionResult Create()
        {
            ViewBag.EmpID = new SelectList(db.PortalEmpresa, "EmpID", "EmpRazao");
            return View();
        }

        // POST: PortalAtividadeChamados/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AtividadeChamadoID,AtividadeChamadoNome,EmpID,PrevisaoDias,PrevisaoHoras,PrevisaoMinutos,Ativo")] PortalAtividadeChamados portalAtividadeChamados)
        {
            if (ModelState.IsValid)
            {
                db.PortalAtividadeChamados.Add(portalAtividadeChamados);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmpID = new SelectList(db.PortalEmpresa, "EmpID", "EmpRazao", portalAtividadeChamados.EmpID);
            return View(portalAtividadeChamados);
        }

        // GET: PortalAtividadeChamados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortalAtividadeChamados portalAtividadeChamados = db.PortalAtividadeChamados.Find(id);
            if (portalAtividadeChamados == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpID = new SelectList(db.PortalEmpresa, "EmpID", "EmpRazao", portalAtividadeChamados.EmpID);
            return View(portalAtividadeChamados);
        }

        // POST: PortalAtividadeChamados/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AtividadeChamadoID,AtividadeChamadoNome,EmpID,PrevisaoDias,PrevisaoHoras,PrevisaoMinutos,Ativo")] PortalAtividadeChamados portalAtividadeChamados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portalAtividadeChamados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmpID = new SelectList(db.PortalEmpresa, "EmpID", "EmpRazao", portalAtividadeChamados.EmpID);
            return View(portalAtividadeChamados);
        }

        // GET: PortalAtividadeChamados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortalAtividadeChamados portalAtividadeChamados = db.PortalAtividadeChamados.Find(id);
            if (portalAtividadeChamados == null)
            {
                return HttpNotFound();
            }
            return View(portalAtividadeChamados);
        }

        // POST: PortalAtividadeChamados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PortalAtividadeChamados portalAtividadeChamados = db.PortalAtividadeChamados.Find(id);
            db.PortalAtividadeChamados.Remove(portalAtividadeChamados);
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
