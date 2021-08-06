using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TMDContactsClient.Models;

namespace TMDContactsClient.Controllers
{
    public class MemberController : Controller
    {
        public ActionResult Login()
        {
            try
            {
                Session["UserId"] = null;
                return View();
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        public ActionResult Register()
        {
            try
            {
                return View();
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult ForgotPassword()
        {
            try
            {
                return View();
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult CheckTheNumber()
        {
            try
            {

                return View();
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult NewPassword()
        {
            try
            {
                return View();
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult NewtPassword(int id)
        {
            try
            {
                return View();
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult MemberProfile()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Member");
                }
                else
                {
                    string Token = ((string)(Session["Token"]));
                    int SessionUserId = ((int)(Session["UserId"]));
                    HttpClient clientt = new HttpClient();
                    clientt.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    var responseMessagee = clientt.GetAsync("http://tmdcontacts-api.dev.tmd/api/Users/Get?Id=" + SessionUserId).Result;

                    if (responseMessagee.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var Users = JsonConvert.DeserializeObject<User>(responseMessagee.Content.ReadAsStringAsync().Result);
                        return View(Users);
                    }
                    else
                    {
                        return View("Wait");
                    }
                }
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }



        [HttpGet]
        public ActionResult MemberUpdate(int id)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Member");
                }
                else
                {
                    string Token = ((string)(Session["Token"]));
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    var responseMessage = client.GetAsync("http://tmdcontacts-api.dev.tmd/api/Users/Get?Id=" + id).Result;

                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var User = JsonConvert.DeserializeObject<User>(responseMessage.Content.ReadAsStringAsync().Result);
                        return View(User);
                    }
                    else
                    {
                        return View("Wait");
                    }
                }
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        public ActionResult ResetPasswordVerification()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Member");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }
}