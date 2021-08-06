using Entities.Concrete;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TMDContactsClient.Models;

namespace TMDContactsClient.Controllers
{
    public class GroupController : Controller
    {
        //[HttpGet]
        public ActionResult Index()
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
                    var responseMessage = client.GetAsync("http://tmdcontacts-api.dev.tmd/api/Groups/GetListByUserId?userId=" + Session["UserId"]).Result;
                    List<Groups> groups = null;

                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        groups = JsonConvert.DeserializeObject<List<Groups>>(responseMessage.Content.ReadAsStringAsync().Result);
                        return View(groups);
                    }
                    else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return View("Wait");
                    }
                    else
                    {
                        return View("NullGroups");
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
                    var responseMessage = client.GetAsync($"http://tmdcontacts-api.dev.tmd/api/Groups/Get?id=" + id).Result;

                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var groups = JsonConvert.DeserializeObject<Groups>(responseMessage.Content.ReadAsStringAsync().Result);
                        return View(groups);
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


        public ActionResult AddToGroup(int id)
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
                    Session["ContactListGroup"] = null;
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    var responseMessage = client.GetAsync("http://tmdcontacts-api.dev.tmd/api/Contacts/Get?Id=" + id).Result;

                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var contacts = JsonConvert.DeserializeObject<Contacts>(responseMessage.Content.ReadAsStringAsync().Result);
                        return View(contacts);
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


        public ActionResult AddToGroupPartial()
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
                    var responseMessagee = clientt.GetAsync($"http://tmdcontacts-api.dev.tmd/api/Groups/GetListByUserId?userId=" + SessionUserId).Result;
                    List<Groups> groups = null;

                    if (responseMessagee.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        groups = JsonConvert.DeserializeObject<List<Groups>>(responseMessagee.Content.ReadAsStringAsync().Result);
                        return View(groups);
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
                    var responseMessage = client.GetAsync($"http://tmdcontacts-api.dev.tmd/api/Groups/Get?id=" + id).Result;
                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var Group = JsonConvert.DeserializeObject<Groups>(responseMessage.Content.ReadAsStringAsync().Result);
                        Session["GroupId"] = null;
                        Session["GroupId"] = Group.Id;
                        return View(Group);
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


        public ActionResult ProfileContacts(int id)
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
                    var responseMessage = client.GetAsync($"http://tmdcontacts-api.dev.tmd/api/GroupsContacts/GetListByGroupId?groupId=" + id).Result;
                    List<Contacts> contacts = null;

                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        contacts = JsonConvert.DeserializeObject<List<Contacts>>(responseMessage.Content.ReadAsStringAsync().Result);
                        return View(contacts);
                    }
                    else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return View("Wait");
                    }
                    else
                    {
                        return View("NullGroupContact");
                    }
                }
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        public ActionResult NullGroups()
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


        public ActionResult NullGroupContact()
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




        public ActionResult DeleteGroupContact(int Id)
        { 
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Member");
                }
                else
                {



                    int GroupId = ((int)(Session["GroupId"]));
                    int SessionUserId = ((int)(Session["UserId"]));
                    string Token = ((string)(Session["Token"]));

                    var groupContact = new GroupContact
                    {
                        Id = SessionUserId,
                        GroupId = GroupId,
                        ContactId = Id
                    };

                    HttpClient client = new HttpClient();
                    StringContent content = new StringContent(JsonConvert.SerializeObject(groupContact), Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    var Url = String.Format("http://tmdcontacts-api.dev.tmd/api/GroupsContacts/Delete?Id={0}&GroupId={1}&ContactId={2}", SessionUserId,  GroupId , Id);
                    var responseMessage = client.PostAsync(Url , content).Result;

                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.Basarili = "Silme işlemi başarılı";

                    }
                    else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return View("Wait");
                    }
                }
                return RedirectToAction("Index");

            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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