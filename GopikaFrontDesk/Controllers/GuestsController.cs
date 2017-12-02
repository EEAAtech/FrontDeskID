using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GopikaFrontDesk;
using PagedList;
using GopikaFrontDesk.Models;

namespace GopikaFrontDesk.Controllers
{
    public class GuestsController : Controller
    {
        private GopikaEntities1 db = new GopikaEntities1();

        // GET: Guests
        public ActionResult Index(int? page, string searchString, string RoomNo, DateTime? From, DateTime? To)
        {

            IEnumerable<Guest> gst = Enumerable.Empty<Guest>();
            if (!String.IsNullOrEmpty(searchString))
            {

                gst = db.Guests.Where(p => p.Firstn.Contains(searchString)
                                       || p.Surn.Contains(searchString));

                //page = 1;
                ViewBag.searchString = searchString;
            }


            if (gst.Count() > 0)
            {
                if (!String.IsNullOrEmpty(RoomNo))
                {
                    gst = gst.Where(e => e.RoomNo == RoomNo);
                    ViewBag.RoomNo = RoomNo;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(RoomNo))
                {
                    gst = db.Guests.Where(e => e.RoomNo == RoomNo);
                    ViewBag.RoomNo = RoomNo;
                }
            }

            if (gst.Count()>0)
            {
                if (From !=null)
                {
                    gst = gst.Where(e => e.CheckIn >= From);
                    ViewBag.From = String.Format("{0:dd-MMM-yyyy}", From);
                }
            } else
            {
                if (From.HasValue)
                {
                    gst = db.Guests.Where(e => e.CheckIn >= From);
                    ViewBag.From = String.Format("{0:dd-MMM-yyyy}", From);
                }
            }

            if (gst.Count() > 0)
            {
                if (To != null)
                {
                    gst = gst.Where(e => e.CheckIn < To);
                    ViewBag.To = String.Format("{0:dd-MMM-yyyy}", To);
                }
            }
            else
            {
                if (To.HasValue)
                {
                    gst = db.Guests.Where(e => e.CheckIn < To);
                    ViewBag.To = String.Format("{0:dd-MMM-yyyy}", To);
                }
            }
            gst = gst.OrderBy(e => e.Firstn);

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(gst.ToPagedList(pageNumber, pageSize));
            
        }



        // GET: Guests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = db.Guests.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }

            
            ViewBag.fn = "../../IdImgs/" + guest.Path;
            return View(guest);

            
        }

        // GET: Guests/Print/5
        public ActionResult Print(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = db.Guests.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }


            ViewBag.fn = "../../IdImgs/" + guest.Path;
            return View(guest);


        }


        // POST: GstImg/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GuestImg GstImg)
        {
            if (ModelState.IsValid)
            {
                if (GstImg.UploadedFile != null)
                {
                    string fn = GstImg.UploadedFile.FileName.Substring(GstImg.UploadedFile.FileName.LastIndexOf('\\') + 1);
                   fn = GstImg.gst.Firstn.Substring(0,3) + "_" + fn;
                    //string SavePath = System.IO.Path.Combine(Server.MapPath("~/" + db.Config.FirstOrDefault().ImageSavePath), fn);
                    //GstImg.UploadedFile.SaveAs(SavePath);

                    System.Drawing.Bitmap upimg = new System.Drawing.Bitmap(GstImg.UploadedFile.InputStream);
                    System.Drawing.Bitmap svimg = MyExtensions.CropUnwantedBackground(upimg);
                    svimg.Save(System.IO.Path.Combine(Server.MapPath("~/IdImgs" ), fn));

                    Guest ed = new Guest
                    {
                        Id = GstImg.gst.Id,
                        Firstn = GstImg.gst.Firstn,
                        Surn = GstImg.gst.Surn,
                        Address = GstImg.gst.Address,
                        Mobile = GstImg.gst.Mobile,
                        Email= GstImg.gst.Email,
                        CheckIn = GstImg.gst.CheckIn,
                        CheckOut= GstImg.gst.CheckOut,
                        RoomNo= GstImg.gst.RoomNo,
                        Path= fn                        
                    };

                    db.Guests.Add(ed);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { Id = ed.Id });
                }
                else
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            }

            return View(GstImg);
        }


        // GET: Guests/Create
        public ActionResult Create()
        {
            return View();
        }



        // GET: Guests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = db.Guests.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }

        // POST: Guests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Firstn,Surn,Address,Mobile,Email,CheckIn,CheckOut,RoomNo,Path")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(guest);
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
