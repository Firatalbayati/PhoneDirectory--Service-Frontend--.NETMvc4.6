using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TMDContactsClient.Models;
using PagedList;
using PagedList.Mvc;
using System.Net;
using System.Collections;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TMDContactsClient.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet]
        public ActionResult Index(int? Id)
        {
            try
            {
                int? Page = Id;
                if (Session["UserId"] == null)
                {
                    HttpCookie cookieEmail = Request.Cookies["Email"];
                    if (cookieEmail != null)
                    {
                        HttpCookie cookieToken = Request.Cookies["Token"];
                        string Token = cookieToken.Value;
                        Session["Token"] = cookieToken.Value;
                        string Email = cookieEmail.Value;
                        //Get Email
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                        var responseMessage = client.GetAsync("http://tmdcontacts-api.dev.tmd/api/Users/GetByEmail?email=" + Email).Result;
                        var User = JsonConvert.DeserializeObject<User>(responseMessage.Content.ReadAsStringAsync().Result);
                        if (User.Photo != null)
                        {
                            Session["Photo"] = User.Photo;
                        }
                        else
                        {
                            Session["Photo"] = "";
                        }

                        int SessionUserId = ((int)(Session["UserId"] = User.Id)); //Add id to session and convert to int

                        ////Set Id
                        string UserId = User.Id.ToString();
                        HttpCookie IdCookies = new HttpCookie("UserId");
                        IdCookies.Value = UserId;
                        IdCookies.Expires = DateTime.Now.AddHours(1);
                        Response.SetCookie(IdCookies);

                        HttpClient clientt = new HttpClient();
                        clientt.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token); //Add token to clientt header
                        var Url = String.Format("http://tmdcontacts-api.dev.tmd/api/Contacts/GetListByUserIdPagination?userId={0}&pageNumber=1&pageSize=10", SessionUserId);
                        var responseMessagee = clientt.GetAsync(Url).Result;
                        ContactPaginationListViewModel Contacts = null;

                        try
                        {
                            if (responseMessagee.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                Contacts = JsonConvert.DeserializeObject<ContactPaginationListViewModel>(responseMessagee.Content.ReadAsStringAsync().Result);
                            }
                            else if (responseMessagee.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                            {
                                return View("Wait");
                            }
                            else
                            {
                                return View("ProfileAndNull");
                            }

                            return View(Contacts);
                        }
                        catch
                        {
                            return View("ProfileAndNull");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Login", "Member");
                    }
                }
                else
                {
                    int SessionUserId = ((int)(Session["UserId"]));
                    string Token = ((string)(Session["Token"]));
                    if (Page == null)
                    {
                        Page = 1;
                    }

                    //Get ContactList
                    HttpClient clientt = new HttpClient();
                    clientt.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);  //Add token to clientt header
                    var Url = String.Format("http://tmdcontacts-api.dev.tmd/api/Contacts/GetListByUserIdPagination?userId={0}&pageNumber={1}&pageSize=10", SessionUserId, Page);
                    var responseMessagee = clientt.GetAsync(Url).Result;
                    ContactPaginationListViewModel Contacts = null;

                    try
                    {
                        if (responseMessagee.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            Contacts = JsonConvert.DeserializeObject<ContactPaginationListViewModel>(responseMessagee.Content.ReadAsStringAsync().Result);
                        }
                        else if (responseMessagee.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            return View("Wait");
                        }
                        else
                        {
                            return View("ProfileAndNull");
                        }

                        return View(Contacts);
                    }
                    catch
                    {
                        return View("ProfileAndNull");
                    }
                }
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        [HttpGet]
        public ActionResult Add()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Member");
                }
                else
                {
                    Session["ContactList"] = null;
                    return View();
                }
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        [HttpGet]
        public ActionResult Update(int id)
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
                    var responseMessage = client.GetAsync("http://tmdcontacts-api.dev.tmd/api/Contacts/Get?Id=" + id).Result;

                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var Contact = JsonConvert.DeserializeObject<Contacts>(responseMessage.Content.ReadAsStringAsync().Result);
                        return View(Contact);
                    }
                    else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return View("Wait");
                    }
                    else
                    {
                        return View("ProfileAndNull");
                    }
                }
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        [HttpGet]
        public ActionResult Profiles(int id)
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
                    var responseMessage = client.GetAsync("http://tmdcontacts-api.dev.tmd/api/Contacts/Get?Id=" + id).Result;

                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var contacts = JsonConvert.DeserializeObject<Contacts>(responseMessage.Content.ReadAsStringAsync().Result);
                        return View(contacts);
                    }
                    else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return View("Wait");
                    }
                    else
                    {
                        return View("ProfileAndNull");
                    }
                }
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        public ActionResult SearchByName(string Search)
        {
            try
            {
                string SearchLower = Search.ToLower();

                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Member");
                }
                else if (Session["ContactList"] == null)
                {
                    string Token = ((string)(Session["Token"]));
                    int SessionUserId = ((int)(Session["UserId"]));
                    HttpClient clientt = new HttpClient();
                    clientt.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    var responseMessagee = clientt.GetAsync("http://tmdcontacts-api.dev.tmd/api/Contacts/GetListByUserId?userId=" + SessionUserId).Result;
                    List<Contacts> Contacts = null;

                    if (responseMessagee.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Contacts = JsonConvert.DeserializeObject<List<Contacts>>(responseMessagee.Content.ReadAsStringAsync().Result);
                    }
                    else if (responseMessagee.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return View("Wait");
                    }
                    else
                    {
                        return View("ProfileAndNull");
                    }

                    List<Contacts> contacts = Contacts;
                    Session.Add("ContactList", contacts);

                    var ListToLower = contacts.Select(c => new Contacts
                    {
                        Id = c.Id,
                        Photo = c.Photo,
                        Name = c.Name.ToLower(),
                        Surname = c.Surname.ToLower(),
                        Tel = c.Tel
                    }).ToList();

                    var Contacts1 = ListToLower.Where(m => m.Name.Contains(SearchLower)).ToList();
                    if (Contacts1.Count == 0)
                    {
                        ViewBag.Basarisiz = "kayıt bulunamadı";
                    }

                    var ListFirstToUpper = Contacts1.Select(c => new Contacts
                    {
                        Id = c.Id,
                        Photo = c.Photo,
                        Name = c.Name.First().ToString().ToUpper() + c.Name.Substring(1),
                        Surname = c.Surname.First().ToString().ToUpper() + c.Surname.Substring(1),
                        Tel = c.Tel
                    }).ToList();

                    return View(ListFirstToUpper);
                }
                else
                {
                    var StringObject = Session["ContactList"]; //Convert Object to string 
                    List<Contacts> Listcontacts = (List<Contacts>)StringObject; //Convert string to list

                    var ListToLowerr = Listcontacts.Select(c => new Contacts
                    {
                        Id = c.Id,
                        Photo = c.Photo,
                        Name = c.Name.ToLower(),
                        Surname = c.Surname.ToLower(),
                        Tel = c.Tel
                    }).ToList();

                    var Contacts2 = ListToLowerr.Where(m => m.Name.Contains(SearchLower)).ToList();
                    if (Contacts2.Count == 0)
                    {
                        ViewBag.Basarisiz = "kayıt bulunamadı";
                    }

                    var ListFirstToUpperr = Contacts2.Select(c => new Contacts
                    {
                        Id = c.Id,
                        Photo = c.Photo,
                        Name = c.Name.First().ToString().ToUpper() + c.Name.Substring(1),
                        Surname = c.Surname.First().ToString().ToUpper() + c.Surname.Substring(1),
                        Tel = c.Tel
                    }).ToList();

                    return View(ListFirstToUpperr);
                }
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        [HttpGet]
        public ActionResult ProfileAndNull()
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


        public ActionResult Wait()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Member");
            }
            else
            {
                Session["Token"] = null;
                return View();
            }
        }

    }
}

