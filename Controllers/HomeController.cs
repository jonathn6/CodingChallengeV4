using CodingChallengeV4.Models;
using CodingChallengeV4.ViewModels;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodingChallengeV4.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            using (var ctx = new ContactContext())
            {
                var seedContact = new Contact();
                seedContact.FirstName = "seelFirst";
                seedContact.LastName = "seedLast";

                var seedEmailAddress = new EmailAddress();
                seedEmailAddress.EmailAddress1 = "seed@seed.com";
                seedEmailAddress.EmailType = 0;
                seedEmailAddress.ID = 1;

                seedContact.EmailAddress = seedEmailAddress;

                ctx.EmailAddress.Add(seedEmailAddress);
                ctx.Contact.Add(seedContact);
                ctx.SaveChanges();
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ViewAllData()
        {
            ViewBag.Message = "View All Data";

            using (var Context = new ContactContext())
            {

                List<ContactOrig> contactsList = Context.ContactOrig.ToList();
                List<EmailAddress> emailAddressesList = Context.EmailAddress.ToList();

                var contactRecord = (from c in contactsList  // get the Contacts table
                                     join e in emailAddressesList  // implement the join on the emailaddresses table
                                     on c.EmailAddress_ID equals e.ID
                                     select new ContactPassData
                                     {
                                         passedID = e.ID,
                                         passedfName = c.FirstName,
                                         passedlName = c.LastName,
                                         passedeMail = e.EmailAddress1,
                                         passedeMailType = e.EmailType
                                     });
                return View(contactRecord);
            }

        }
        public JsonResult SaveAndUpdate(AddRecordDataFromView model)
        {
            var result = new JSONMessage();
            try
            {
                //check to see if the email address sent by the user already exists in the database
                var Context = new ContactContext();

                if (Context.EmailAddress.Any(u => u.EmailAddress1 == model.EmailAddress))
                {

                    result.ErrorMessage = "The eMail address entered exists already exists.";
                    result.Status = "false";

                } else
                {
                    //record not exists
                    using (var ctx = new ContactContext())
                    {
                        //define the model  

                        var contact = new Contact();
                        contact.FirstName = model.FirstName;
                        contact.LastName = model.LastName;

                        var emailAddress = new EmailAddress();
                        emailAddress.EmailAddress1 = model.EmailAddress;

                        emailAddress.EmailType = model.EmailType == "P" ? EmailType.Personal : EmailType.Business;

                        contact.EmailAddress = emailAddress;

                        //for insert recored.

                        ctx.EmailAddress.Add(emailAddress);
                        ctx.Contact.Add(contact);
                        result.ErrorMessage = "Your contact information has been saved successfully.";
                        result.Status = "true";

                        //ctx.Entry(emailAddress).State = EntityState.Modified;
                        //ctx.Entry(contact).State = EntityState.Modified;
                        //result.ErrorMessage = "Your contact information has been updated successfully..";
                        //result.Status = "true";

                        ctx.SaveChanges(); 
                    }

                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage = "We are unable to process your request at this time. Please try again later.";
                result.Status = "false";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}