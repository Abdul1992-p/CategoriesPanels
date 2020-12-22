using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CategoriesPanels.Models
{
    public class CategoriesController : Controller
    {
        private CategoriesEntities db = new CategoriesEntities();

       

        // GET: Categories
        public async Task<ActionResult> Index()
        {
            return View(await db.Categories.ToListAsync());
        }

        // GET: ParentCategories
        //////[HttpGet]
        //////public ActionResult GetParentCategories()
        //////{


        //////    //////CategoriesEntities db = new CategoriesEntities();
               
        //////        ////return View(viewModel);
        //////    }
        



        // GET: Categories/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (Category e in db.Categories)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = e.CategoryName,
                    Value = e.CategoryID.ToString(),
                    //Selected = false
                };
                list.Add(item);
            }
            Category viewModel = new Category();
            viewModel.ParentCategoryList = list;
            return View(viewModel);
        }

        public async Task<ActionResult> CategoriesSelection()
        {
           
            var category = await db.Categories.Where(b => b.ParentCategoryID == 0).ToListAsync();

            List<SelectListItem> list = new List<SelectListItem>();
            foreach (Category e in category)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = e.CategoryName,
                    Value = e.CategoryID.ToString(),
                    //Selected = false
                };
                list.Add(item);
            }
            Category viewModel = new Category();
            viewModel.DefaultCategoryList = list;
            

            return View(viewModel);
        }

        public async Task<JsonResult> GetSubCategories(long? ParentID)
        {
            if (ParentID == null)
            {
                return null;
            }
            var category = await db.Categories.Where(b => b.ParentCategoryID == ParentID).ToListAsync(); 
            if (category == null)
            {
              return  null;
            }

            System.Web.Script.Serialization.JavaScriptSerializer jSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //return Json(category, JsonRequestBehavior.AllowGet);
            //var r = new HtmlString(jSerializer.Serialize(category));
            JsonResult jr = Json(category, JsonRequestBehavior.AllowGet);
            return (jr);
        }


        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CategoryID,ParentCategoryID,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CategoryID,ParentCategoryID,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Category category = await db.Categories.FindAsync(id);
            db.Categories.Remove(category);
            await db.SaveChangesAsync();
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
