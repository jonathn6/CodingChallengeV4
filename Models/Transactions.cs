using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CodingChallengeV4.Models
{
    public class SendIDFromView
    {
        public string ID { get; set; }
    }

    public class AddRecordDataFromView
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string EmailType { get; set; }

    }
    public enum EmailType
    {
        Personal, Business
    }
    public class ContactContext : DbContext
    {
        public ContactContext() : base()
        {
            //Database.SetInitializer<ContactContext>(new DropCreateDatabaseAlways<ContactContext>());
        }

        public DbSet<Contact> Contact { get; set; }
        public DbSet<EmailAddress> EmailAddress { get; set; }
    }
}