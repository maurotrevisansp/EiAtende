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
            var _portalTipoChamados = portalTipoChamados.ToList().Where(e => e.EmpID == Convert.ToInt32(Session["UsrEmpID"])).ToList();


            ViewModels.VwTiposChamados _vwTiposChamados = new ViewModels.VwTiposChamados();
            _vwTiposChamados.PortalTipoChamados = _portalTipoChamados;

            IList<PortalEmpresa> _PortalEmpresas = new List<PortalEmpresa>();
            PortalEmpresa _PortalEmpresa = new PortalEmpresa();
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
            ViewBag.EmpID = new SelectList(_PortalEmpresas, "EmpID", "EmpRazao");


            _vwTiposChamados.PortalEmpresa = _PortalEmpresas;
            TempData["titulo"] = "Tipos de Chamados";
            TempData["titulo1"] = "Cadastrados";

            var lEmp = new int[_PortalEmpresas.Count()];
            int i = 0;
            foreach (var item in _PortalEmpresas)
            {
                lEmp[i] = item.EmpID;
                i++;
            }
            var IdUsuario = Convert.ToInt32((Session["IdUsuario"]));

            var Gestor = Session["UsuarioGestor"].ToString();
            if (Gestor == "True")
            {
                _vwTiposChamados.PortalTipoChamados = portalTipoChamados.ToList()
                    .Where(e => lEmp.Contains(e.EmpID)).ToList();
            }
            else
            {
                _vwTiposChamados.PortalTipoChamados = portalTipoChamados.ToList()
                    .Where(e => e.EmpID == IdUsuario || e.EmpID == IdUsuario)
                    .OrderByDescending(e => e.TipoChamadoID).ToList();
            }


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
            IList<PortalEmpresa> _PortalEmpresas = new List<PortalEmpresa>();
            PortalEmpresa _PortalEmpresa = new PortalEmpresa();
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
            ViewBag.EmpID = new SelectList(_PortalEmpresas, "EmpID", "EmpRazao");
            return View();
        }

        // POST: PortalTipoChamados/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TipoChamadoID,TipoChamadoNome,EmpID,Ativo")] PortalTipoChamados portalTipoChamados)
        {
            if (ModelState.IsValid)
            {
                db.PortalTipoChamados.Add(portalTipoChamados);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            IList<PortalEmpresa> _PortalEmpresas = new List<PortalEmpresa>();
            PortalEmpresa _PortalEmpresa = new PortalEmpresa();
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
            ViewBag.EmpID = new SelectList(_PortalEmpresas, "EmpID", "EmpRazao");
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
            IList<PortalEmpresa> _PortalEmpresas = new List<PortalEmpresa>();
            PortalEmpresa _PortalEmpresa = new PortalEmpresa();
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
            ViewBag.EmpID = new SelectList(_PortalEmpresas, "EmpID", "EmpRazao");
            return View(portalTipoChamados);
        }

        // POST: PortalTipoChamados/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TipoChamadoID,TipoChamadoNome,EmpID,Ativo")] PortalTipoChamados portalTipoChamados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portalTipoChamados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            IList<PortalEmpresa> _PortalEmpresas = new List<PortalEmpresa>();
            PortalEmpresa _PortalEmpresa = new PortalEmpresa();
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
            ViewBag.EmpID = new SelectList(_PortalEmpresas, "EmpID", "EmpRazao");
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
