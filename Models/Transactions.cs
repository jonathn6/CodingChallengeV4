using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CodingChallengeV4.Models
{
    public class AddRecordDataFromView
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string EmailType { get; set; }
    }

    public class ContactOrig
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int EmailAddress_ID { get; set; }
        public int EmailAddress_ID1 { get; set; }

    }
    public class EmailAddressesOrig
    {
        public int ID { get; set; }
        public string EmailAddress { get; set; }
        public EmailType EmailType { get; set; }
        public ICollection<Contact> Contacts { get; set; }
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