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
    public class PortalUsuariosController : Controller
    {
        private EiAtendeContexto db = new EiAtendeContexto();

        // GET: PortalUsuarios
        public ActionResult Index()
        {
            var portalUsuario = db.PortalUsuario.Include(p => p.PortalUsuarioGrupo);
            return View(portalUsuario.ToList());
        }

        // GET: PortalUsuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortalUsuario portalUsuario = db.PortalUsuario.Find(id);
            if (portalUsuario == null)
            {
                return HttpNotFound();
            }
            return View(portalUsuario);
        }

        // GET: PortalUsuarios/Create
        public ActionResult Create()
        {
            ViewBag.UsrGrpID = new SelectList(db.PortalUsuarioGrupo, "UsrGrpID", "UsrGrpNome");
            return View();
        }

        // POST: PortalUsuarios/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UsrID,UsrEmpID,UsrNome,UsrEmail,UsrSenha,UsrAdmin,UsrAdminSistema,UsrAtivo,UsrInclusao,UsrInclusaoUsuario,UsrAlteracao,UsrAlteracaoUsuario,UsrExclusao,UsrExclusaoUsuario,UsrGrpID")] PortalUsuario portalUsuario)
        {
            if (ModelState.IsValid)
            {
                db.PortalUsuario.Add(portalUsuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UsrGrpID = new SelectList(db.PortalUsuarioGrupo, "UsrGrpID", "UsrGrpNome", portalUsuario.UsrGrpID);
            return View(portalUsuario);
        }

        // GET: PortalUsuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortalUsuario portalUsuario = db.PortalUsuario.Find(id);
            if (portalUsuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsrGrpID = new SelectList(db.PortalUsuarioGrupo, "UsrGrpID", "UsrGrpNome", portalUsuario.UsrGrpID);
            return View(portalUsuario);
        }

        // POST: PortalUsuarios/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UsrID,UsrEmpID,UsrNome,UsrEmail,UsrSenha,UsrAdmin,UsrAdminSistema,UsrAtivo,UsrInclusao,UsrInclusaoUsuario,UsrAlteracao,UsrAlteracaoUsuario,UsrExclusao,UsrExclusaoUsuario,UsrGrpID")] PortalUsuario portalUsuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portalUsuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsrGrpID = new SelectList(db.PortalUsuarioGrupo, "UsrGrpID", "UsrGrpNome", portalUsuario.UsrGrpID);
            return View(portalUsuario);
        }

        // GET: PortalUsuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortalUsuario portalUsuario = db.PortalUsuario.Find(id);
            if (portalUsuario == null)
            {
                return HttpNotFound();
            }
            return View(portalUsuario);
        }

        // POST: PortalUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PortalUsuario portalUsuario = db.PortalUsuario.Find(id);
            db.PortalUsuario.Remove(portalUsuario);
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
