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
    public class PortalChamadoHistoricosController : Controller
    {
        private EiAtendeContexto db = new EiAtendeContexto();
        CultureInfo culture = new CultureInfo("pt-BR");


        // GET: PortalChamadoHistoricos
        public ActionResult Index(int? id, FormCollection _Formcollection, string submit, ViewModels.VwChamadosHistorico model, string DeStatus, string ParaStatus, string DtAdiar, string finalizar)
        {
            if (submit == "SalvarHistorico")
            {
                PortalChamadosHistorico _PortalChamadosHistorico = new PortalChamadosHistorico();
                _PortalChamadosHistorico.ChamadoID = model.PortalChamadosHistorico.ChamadoID;
                id = model.PortalChamadosHistorico.ChamadoID;
                _PortalChamadosHistorico.Descricao = model.PortalChamadosHistorico.Descricao;
                _PortalChamadosHistorico.DeStatus = DeStatus;
                if (DtAdiar.Length > 0)
                {
                    _PortalChamadosHistorico.DtAdiar = Convert.ToDateTime(DtAdiar, culture);
                }
                _PortalChamadosHistorico.DtIncl = model.PortalChamadosHistorico.DtIncl;
                _PortalChamadosHistorico.ParaStatus = ParaStatus;
                db.PortalChamadosHistorico.Add(_PortalChamadosHistorico);
                db.SaveChanges();
                var _portalChamado = db.PortalChamados.Find(id);
                _portalChamado.Status = ParaStatus;
                if (DtAdiar.Length > 0)
                {
                    _portalChamado.ChamadoDtAdiada = Convert.ToDateTime(DtAdiar, culture);
                }

                if (finalizar == "on")
                {
                    _portalChamado.ChamadoDtTermino = DateTime.Now;
                }
                db.Entry(_portalChamado).State = EntityState.Modified;
                db.SaveChanges();

            }

            ViewModels.VwChamadosHistorico _VwChamadosHistorico = new ViewModels.VwChamadosHistorico();
            var portalChamadosHistorico = db.PortalChamadosHistorico.Include(p => p.PortalChamados);
            var _portalChamados = db.PortalChamados.Find(id);
            var _portalEmpresa = db.PortalEmpresa.Find(_portalChamados.AtenderEmpID);
            _VwChamadosHistorico.PortalChamadosHistoricos = portalChamadosHistorico.ToList().Where(e => e.ChamadoID.Equals(id)).ToList();
            _VwChamadosHistorico.PortalChamadosHistorico = new PortalChamadosHistorico();
            _VwChamadosHistorico.PortalChamadosHistorico.PortalChamados = new PortalChamados();
            _VwChamadosHistorico.PortalChamadosHistorico.DtIncl = DateTime.Now;
            _VwChamadosHistorico.PortalChamadosHistorico.PortalChamados.ChamadoDtPrevista = _portalChamados.ChamadoDtPrevista;
            _VwChamadosHistorico.PortalChamadosHistorico.ChamadoID = Convert.ToInt32(id);
            if (_VwChamadosHistorico.PortalChamadosHistoricos.Count > 0)
            {
                _VwChamadosHistorico.PortalChamadosHistorico.DeStatus = _portalChamados.Status;
            }
            else
            {
                _VwChamadosHistorico.PortalChamadosHistorico.DeStatus = "Postado";
            }
            if (submit == "Excell")
            {
                this.ExportHistoricosToExcel(_VwChamadosHistorico.PortalChamadosHistoricos);
            }
            TempData["titulo"] = "Históricos";
            TempData["titulo1"] = "do Chamado: " + _portalChamados.ChamadoTitulo + " / " + _portalEmpresa.EmpNomeFantasia;

            return View(_VwChamadosHistorico);
        }

        // GET: PortalChamadoHistoricos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortalChamadosHistorico portalChamadosHistorico = db.PortalChamadosHistorico.Find(id);
            if (portalChamadosHistorico == null)
            {
                return HttpNotFound();
            }
            return View(portalChamadosHistorico);
        }

        // GET: PortalChamadoHistoricos/Create
        public ActionResult Create()
        {
            ViewBag.ChamadoID = new SelectList(db.PortalChamados, "ChamadoID", "ChamadoTitulo");
            return View();
        }

        // POST: PortalChamadoHistoricos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChamadoHistID,ChamadoID,Descricao,DtIncl,DeStatus,ParaStatus,DtAdiar")] PortalChamadosHistorico portalChamadosHistorico)
        {
            if (ModelState.IsValid)
            {
                db.PortalChamadosHistorico.Add(portalChamadosHistorico);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ChamadoID = new SelectList(db.PortalChamados, "ChamadoID", "ChamadoTitulo", portalChamadosHistorico.ChamadoID);
            return View(portalChamadosHistorico);
        }

        // GET: PortalChamadoHistoricos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortalChamadosHistorico portalChamadosHistorico = db.PortalChamadosHistorico.Find(id);
            if (portalChamadosHistorico == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChamadoID = new SelectList(db.PortalChamados, "ChamadoID", "ChamadoTitulo", portalChamadosHistorico.ChamadoID);
            return View(portalChamadosHistorico);
        }

        // POST: PortalChamadoHistoricos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChamadoHistID,ChamadoID,Descricao,DtIncl,DeStatus,ParaStatus,DtAdiar")] PortalChamadosHistorico portalChamadosHistorico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portalChamadosHistorico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ChamadoID = new SelectList(db.PortalChamados, "ChamadoID", "ChamadoTitulo", portalChamadosHistorico.ChamadoID);
            return View(portalChamadosHistorico);
        }

        // GET: PortalChamadoHistoricos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortalChamadosHistorico portalChamadosHistorico = db.PortalChamadosHistorico.Find(id);
            if (portalChamadosHistorico == null)
            {
                return HttpNotFound();
            }
            var idChamado = portalChamadosHistorico.ChamadoID;
            db.PortalChamadosHistorico.Remove(portalChamadosHistorico);
            db.SaveChanges();
            return RedirectToAction("Index", "PortalChamados");
        }

        // POST: PortalChamadoHistoricos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PortalChamadosHistorico portalChamadosHistorico = db.PortalChamadosHistorico.Find(id);
            db.PortalChamadosHistorico.Remove(portalChamadosHistorico);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void ExportHistoricosToExcel(ICollection<PortalChamadosHistorico> model)
        {

            System.Text.Encoding encoding = System.Text.Encoding.Unicode;
            StringWriter sw = new StringWriter();
            string linha = string.Empty;
            linha = "NR.HISTÓRICO;DATA INCLUSÃO;DATA PREVISTA;DATA ADIADA;DE STATUS;PARA STATUS;DESCRIÇÃO";
            sw.WriteLine(linha);

            foreach (var item in model)
            {
                linha = string.Empty;
                linha += item.ChamadoHistID + ";";
                linha += item.DtIncl.ToShortDateString() + ";";
                linha += item.PortalChamados.ChamadoDtPrevista.ToShortDateString() + ";";
                if (item.DtAdiar != null)
                {
                    linha += item.DtAdiar + ";";
                }
                else
                {
                    linha += ";";
                }
                linha += item.DeStatus + ";";
                linha += item.ParaStatus + ";";
                linha += item.Descricao;
                sw.WriteLine(linha);
            }
            sw.Close();

            Response.ClearContent();
            Response.ContentEncoding = encoding;
            Response.AddHeader("content-disposition", "attachment; filename=Historico_" + model.First().PortalChamados.ChamadoTitulo + "_" + System.Web.HttpContext.Current.Session["NomeUsuario"].ToString() + ".csv");
            Response.ContentType = "application/excel";
            Response.Write(sw);
            Response.End();

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
