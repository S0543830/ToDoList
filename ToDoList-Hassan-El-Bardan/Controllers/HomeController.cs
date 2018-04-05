using System;
using System.Collections.Generic;
using System.Linq;
using ToDoList_Hassan_El_Bardan.Models;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Net;
using System.Data.Entity;

namespace ToDoList_Hassan_El_Bardan.Controllers
{
    public class HomeController : Controller
    {
        private db_ToDoListEntities1 _db = new db_ToDoListEntities1();
        public ActionResult Index(string sortOn, string orderBy,
           string pSortOn, string keyword, int? page)
        {
                int recordsPerPage = 10;
                if (!page.HasValue)
                {
                    page = 1;
                    if (string.IsNullOrWhiteSpace(orderBy) || orderBy.Equals("asc"))
                    {
                        orderBy = "desc";
                    }
                    else
                    {
                        orderBy = "asc";
                    }
                }
                if (!string.IsNullOrWhiteSpace(sortOn) && !sortOn.Equals(pSortOn,
                        StringComparison.CurrentCultureIgnoreCase))
                {
                    orderBy = "asc";
                }

                ViewBag.OrderBy = orderBy;
                ViewBag.SortOn = sortOn;
                ViewBag.Keyword = keyword;

                var list = _db.main.AsQueryable();

                switch (sortOn)
                {
                    case "Titel":
                        if (orderBy.Equals("desc"))
                        {
                            list = list.OrderByDescending(p => p.Titel);
                        }
                        else
                        {
                            list = list.OrderBy(p => p.Titel);
                        }
                        break;
                    default:
                        list = list.OrderBy(p => p.Id);
                        break;
                }
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    list = list.Where(f => f.Titel.StartsWith(keyword));
                }
                var finalList = list.ToPagedList(page.Value, recordsPerPage);
                return View(finalList);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Diese Anwendung dient alleine nur für Bewerbungszwecke";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Gerne können Sie mit mir Kontakt aufnehmen:";

            return View();
        }

        

        [HttpGet]
        public ActionResult DeleteAllDones()
        {
            List<main> fdb = _db.main.ToList();
           
            for(int i = 0;i<fdb.Count;i++)
            {
                if(fdb.ElementAt(i).isDone == true)
                {
                    _db.main.Remove(fdb.ElementAt(i));
                }
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult SaveStatus(int? id)
        {
            main fdb = _db.main.Find(id);
            if (fdb.isDone == true)
            {
                fdb.isDone = false;
            }
            else
            {
                fdb.isDone = true;
            }
            _db.Entry(fdb).State = EntityState.Modified;
            fdb.Änderungsdatum = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet()]
        public ActionResult Details(int? Id)
        {
            main proid = _db.main.Find(Id);
            Kategorie viewKat = _db.Kategorie.Find(proid.Kategorie_Id);
            ViewModelAufgabe _view = new ViewModelAufgabe();
            _view._main = proid;
            _view._kategorie = viewKat;
            return View(_view);
        }

        [HttpGet]
        public ActionResult Create(ViewModelAufgabe vmodel = null)
        {
            ViewModelAufgabe _viewmodel = new ViewModelAufgabe();
            _viewmodel._lKategorie = _db.Kategorie.ToList();
            return View(_viewmodel);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed(ViewModelAufgabe _aufgabe)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _aufgabe._main.Anlegedatum = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    _aufgabe._main.Änderungsdatum = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    _aufgabe._main.isDone = false;
                    _aufgabe._main.Kategorie_Id = _aufgabe._kategorie.Id;
                    _db.main.Add(_aufgabe._main);
                    _db.SaveChanges();
                    int id = _aufgabe._main.Id;
                    //Indikator speichern
                    
                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
            }


            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult AddKategorie()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddKategorie(Kategorie _kategorie)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Kategorie.Add(_kategorie);
                    _db.SaveChanges();
                    return RedirectToAction("Create");
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult DeleteKategorie(Kategorie _Id, string roh)
        {
            if (_Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //_name.Id = Convert.ToInt32(_name.Kategorie_Name);
            var p = from a in _db.Kategorie where a.Id == _Id.Id select a;
            Kategorie viewKat = p.FirstOrDefault();
            //if (_name.Kategorie_Name == null)
            //{
            //    return HttpNotFound();
            //}
            return View(viewKat);
        }

        [HttpGet()]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            main viewModel = _db.main.Find(id);
            if (viewModel == null)
            {
                return HttpNotFound();
            }
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            main aufgabe = _db.main.Find(id);
            if (aufgabe != null)
            {
                _db.main.Remove(aufgabe);
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("DeleteKategorie")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteKategorieConfirmed(Kategorie _name, string roh)
        {
            var p = from a in _db.Kategorie where a.Id == _name.Id select a;
            Kategorie viewKat = p.FirstOrDefault();
            _db.Kategorie.Remove(viewKat);
            _db.SaveChanges();
            return RedirectToAction(roh);
        }

        [HttpGet]
        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var p = from a in _db.main where a.Id == Id select a;
            ViewModelAufgabe vMA = new ViewModelAufgabe();
            vMA._main = p.FirstOrDefault();
            vMA._lKategorie = _db.Kategorie.ToList();

            if (vMA._main == null)
            {
                return HttpNotFound();
            }
            return View(vMA);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewModelAufgabe _produktDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Entry(_produktDetails._main).State = EntityState.Modified;
                    _produktDetails._main.Änderungsdatum = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
            }

            return View(_produktDetails);
        }
    }
}