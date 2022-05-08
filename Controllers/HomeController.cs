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

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult AddAContact()
        {
            ViewBag.Message = "Add a contact";
            return View();
        }

        //
        // If the user wishes to display all the contacts in the database, this function will retrieve
        // all the records and pass them to the view
        //
        public ActionResult ViewAllData()
        {
            ViewBag.Message = "View All Data";

            //
            // create a new instance of the database
            //
            // Context => the instance of the database
            // contactRecord => a list of type ContactPassData => for each data record returned by EF,
            //                  a new contactRecord list item will be created and it will store the data
            //                  returned by the EF
            // PassList => a list of type ContactPassData.  Once all the data records have been processed,
            //             and the contactRecord list is complete, I copy the completed list into PassList.
            //             PassList is then returned to the View.
            //
            //
            // do not pass any records with an ID of 1 or 2 as they were the seed records
            //
            using (var Context = new ContactContext())
            {
                var contactRecord = Context.Contact
                    .Where(c => c.ID == c.EmailAddress.ID && c.ID > 2)
                    .Select(c => new ContactPassData
                    {
                        passedID = c.ID,
                        passedfName = c.FirstName,
                        passedlName = c.LastName,
                        passedeMail = c.EmailAddress.EmailAddress1,
                        passedeMailType = c.EmailAddress.EmailType,
                        passedeMailTypeString = (c.EmailAddress.EmailType == 0) ? "Personal" : "Business"
                    }).ToList();
                List<ContactPassData> PassList = new List<ContactPassData>();
                PassList = contactRecord;
                return View(PassList);
            }

        }

        //
        // If the user wishes to update any of the contacts in the database, this function will retrieve
        // all the records and pass them to the view so the user can select which contact to update
        //
        public ActionResult UpdateAllData()
        {
            ViewBag.Message = "Your application update page.";
            //
            // create a new instance of the database
            //
            // Context => the instance of the database
            // contactRecord => a list of type ContactPassData => for each data record returned by EF,
            //                  a new contactRecord list item will be created and it will store the data
            //                  returned by the EF
            // PassList => a list of type ContactPassData.  Once all the data records have been processed,
            //             and the contactRecord list is complete, I copy the completed list into PassList.
            //             PassList is then returned to the View.
            //
            //
            // do not pass any records with an ID of 1 or 2 as they were the seed records
            //
            using (var Context = new ContactContext())
            {
                var contactRecord = Context.Contact
                    .Where(c => c.ID == c.EmailAddress.ID && c.ID > 2)
                    .Select(c => new ContactPassData
                    {
                        passedID = c.ID,
                        passedfName = c.FirstName,
                        passedlName = c.LastName,
                        passedDName = c.FirstName.Trim() + " " + c.LastName.Trim(),
                        passedeMail = c.EmailAddress.EmailAddress1,
                        passedeMailType = c.EmailAddress.EmailType,
                        passedeMailTypeString = (c.EmailAddress.EmailType == 0) ? "Personal" : "Business"
                    }).ToList();
                List<ContactPassData> PassList = new List<ContactPassData>();
                PassList = contactRecord;
                return View(PassList);
            }
        }

        //
        // When the user wants to update contact information, the user will double click on a grid row
        // The view will execute an ajax call to send the contact ID to this function.  This function will
        // retrieve the specified contact ID data from the database and generate a partial view with the
        // resulting data so the user can modify the data and save the data back to the database.
        //

        [HttpPost]
        public ActionResult Details(AddRecordDataFromView model)
        {
            //
            // passedID => The ID of the contact the user wants to modify.  This is sent from the view as a 
            //             parameter into this function
            // Context => an instance of the database
            // contactRecord => a list of type ContactPassData => for each data record returned by EF,
            //                  a new contactRecord list item will be created and it will store the data
            //                  returned by the EF.  Since the ID is the key of the contact table, EF will 
            //                  only return a single record.
            // PassList => a list of type ContactPassData.  Once data record has been processed,
            //             and the contactRecord list is complete, I copy the completed list into PassList.
            //             PassList is then returned to the Partial View.
            //


            int passedID = int.Parse(model.ID);
            List<ContactPassData> PassList = new List<ContactPassData>();

            if (passedID == 0)
            {
                ContactPassData emptyRecord = new ContactPassData();
                emptyRecord.passedID = 0;
                emptyRecord.passedfName = "";
                emptyRecord.passedlName = "";
                emptyRecord.passedeMail = "";
                emptyRecord.passedeMailType = 0;
                emptyRecord.passedeMailTypeString = "Personal";
                PassList.Add(emptyRecord);
            } else
            {
                using (var Context = new ContactContext())
                {
                    var contactRecord = Context.Contact
                        .Where(c => c.ID == passedID)
                        .Select(c => new ContactPassData
                        {
                            passedID = c.ID,
                            passedfName = c.FirstName,
                            passedlName = c.LastName,
                            passedeMail = c.EmailAddress.EmailAddress1,
                            passedeMailType = c.EmailAddress.EmailType,
                            passedeMailTypeString = (c.EmailAddress.EmailType == 0) ? "Personal" : "Business"
                        }).ToList();
                    PassList = contactRecord;
                }

            }
            return PartialView("Details", PassList);
        }

        //
        // When I user elects to add a new contact, this function will see if the email address entered
        // by the user exits in the database or not and it will respond accordingly.  If it is not found,
        // the new contact is added to the database.
        //

        //
        // Since we use this same code to process adding a contact (which does not require an ID) and to
        // process updating a contact (which does require an ID) we use the ID field to determine if we are
        // adding a participant (ID will = 0) or updating a participant (ID will be greater than 0)
        //
        public JsonResult SaveAndUpdate(AddRecordDataFromView model)
        {
            //
            // result => a variable of type JSONMessage.  This will contain a response which is reported back
            //           to the calling ajax
            //
            // Context => an instance of the database
            var result = new JSONMessage();
            bool AddContact = new Boolean();
            bool EmailExists = new Boolean();
            int EmailExistsID = 0;
            // If the contact is being added, the ID will be 0
            AddContact = (model.ID == "0") ? true : false;
            try
            {

                var Context = new ContactContext();

                //
                // look to see if the email address entered by the user is already in the database
                // if it does, get the ID of the record.  On an update, if the ID is different than
                // the ID of the record being updated, we must reject the update since an email address
                // can not belong to more than 1 contact
                //
                EmailExists = (Context.EmailAddress.Any(u => u.EmailAddress1 == model.EmailAddress)) ? true : false;
                if (EmailExists)
                {
                    EmailAddress emailAddress = Context.EmailAddress.Where(record => record.EmailAddress1 == model.EmailAddress).FirstOrDefault();
                    EmailExistsID = emailAddress.ID;
                }

                if (AddContact == true && EmailExists == true)
                {

                    result.ErrorMessage = "The eMail address entered already exists. Contact was not added.";
                    result.Status = "false";
                } else
                {
                    //
                    // on an update, if the entered email address already exists in the database for an ID other than the ID
                    // being updated, do not update the database or the web grid and display an error message
                    //
                    if (AddContact == false && EmailExists == true && EmailExistsID != int.Parse(model.ID))
                    {
                        result.ErrorMessage = "1";
                        result.Status = "false";
                    } else
                    {
                        //
                        // on an add contact, the email address was not found or on an update, we are updating
                        // the record intended to be updated.  This will create a record with the 
                        // new contact infomation and call EF to commit the new record to the database or it
                        // will update the record with the passed ID
                        //
                        using (var ctx = new ContactContext())
                        {
                            //
                            // contact => a variable of type Contact. This will hold the contact data the user entered.
                            // emailAddress => a variable of type EmailAddress. This will hold the email address data
                            //                 the user entered.
                            //
                            var contact = new Contact();
                            contact.FirstName = model.FirstName;
                            contact.LastName = model.LastName;

                            var emailAddress = new EmailAddress();
                            emailAddress.EmailAddress1 = model.EmailAddress;
                            emailAddress.EmailType = int.Parse(model.EmailType);

                            contact.EmailAddress = emailAddress;

                            //
                            // insert the contact and emailAddress records into the corresponding table
                            //

                            if (AddContact == true)
                            {
                                result.ErrorMessage = "Your contact information has been saved successfully.";
                                ctx.EmailAddress.Add(emailAddress);
                                ctx.Contact.Add(contact);

                            }
                            else
                            {
                                contact.ID = int.Parse(model.ID);
                                emailAddress.ID = int.Parse(model.ID);
                                ctx.EmailAddress.Attach(emailAddress);
                                ctx.Contact.Attach(contact);
                                ctx.Entry(emailAddress).State = EntityState.Modified;
                                ctx.Entry(contact).State = EntityState.Modified;
                                result.ErrorMessage = "Your contact information has been updated successfully.";
                            }
                            result.Status = "true";
                            ctx.SaveChanges();

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                if (AddContact == true)
                {
                    result.ErrorMessage = "We are unable to process your request at this time. Please try again later.";
                    result.Status = "false";
                } else
                {
                    result.ErrorMessage = "2";
                    result.Status = "false";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}