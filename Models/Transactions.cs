using com.sun.tools.doclets.standard;
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

    public class DBInitializer : DropCreateDatabaseAlways<ContactContext>
    {
        protected override void Seed(ContactContext context)
        {
            IList<Standard> defaultStandards = new List<Standard>();

            var seed1Contact = new Contact();
            var seed1EmailAddress = new EmailAddress();
            seed1Contact.FirstName = "seedFirst1";
            seed1Contact.LastName = "seedLast1";
            seed1EmailAddress.EmailAddress1 = "seed1@seed.com";
            seed1EmailAddress.EmailType = 0;
            seed1Contact.EmailAddress = seed1EmailAddress;
            //
            // add the seed record to the collection that will be commited to the database
            //
            context.EmailAddress.Add(seed1EmailAddress);
            context.Contact.Add(seed1Contact);

            //
            // create second seed contact record
            //
            var seed2Contact = new Contact();
            var seed2EmailAddress = new EmailAddress();
            seed2Contact.FirstName = "seedFirst2";
            seed2Contact.LastName = "seedLast2";
            seed2EmailAddress.EmailAddress1 = "seed2@seed.com";
            seed2EmailAddress.EmailType = 0;
            seed2Contact.EmailAddress = seed2EmailAddress;
            //
            // add the seed record to the collection that will be commited to the database
            //
            context.EmailAddress.Add(seed2EmailAddress);
            context.Contact.Add(seed2Contact);
            //
            // commit the records to the database
            //
            context.SaveChanges();

            base.Seed(context);
        }
    }
    public class ContactContext : DbContext
    {
        public ContactContext() : base()
        {

            //Database.SetInitializer(new DBInitializer());
            //Database.SetInitializer<ContactContext>(new DropCreateDatabaseAlways<ContactContext>());
            //Database.SetInitializer<ContactContext>(new DBInitializer());
        }

        public DbSet<Contact> Contact { get; set; }
        public DbSet<EmailAddress> EmailAddress { get; set; }
    }
}