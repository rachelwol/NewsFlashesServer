using MySql.Data.MySqlClient;
using NewsFlashes.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace NewsFlashes.Controllers
{
    [EnableCors(origins: "http://localhost:5173/", headers: "*", methods: "PUT")]
    public class NewsFlashesController : Controller
    {
        private NewsFlashDbContext db = new NewsFlashDbContext();

        public ActionResult Index()
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");
            List<NewsFlash> flashes = new List<NewsFlash>();
            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "SELECT id, title, date FROM news_flashes";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            flashes.Add(new NewsFlash
                            {
                                Id = Convert.ToInt32(sdr["id"]),
                                Title = sdr["title"].ToString(),
                                Date = Convert.ToDateTime(sdr["date"])
                            });
                        }
                    }
                    con.Close();
                }
            }
            return Json(flashes, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsFlash newsFlash = db.NewsFlashes.Find(id);
            if (newsFlash == null)
            {
                return HttpNotFound();
            }
            return View(newsFlash);
        }

        [System.Web.Http.HttpPost]
        public ActionResult Create([FromBody] string title)
        {
            DateTime Date = DateTime.Now;
            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "INSERT INTO news_flashes (title, date) VALUES (@Title, @Date)";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@Date", Date);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            return RedirectToAction("Index");
        }


        [System.Web.Http.HttpPut]
        public ActionResult Edit(NewsFlash flash)
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");
            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "UPDATE news_flashes SET title = @Title, date = @Date WHERE id = @Id";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@Title", flash.Title);
                    cmd.Parameters.AddWithValue("@Date", flash.Date);
                    cmd.Parameters.AddWithValue("@Id", flash.Id);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            return RedirectToAction("Index");
        }

        [System.Web.Http.HttpPost]
        public ActionResult Delete(int id)
        {
            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "DELETE FROM news_flashes WHERE id = @Id";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

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
