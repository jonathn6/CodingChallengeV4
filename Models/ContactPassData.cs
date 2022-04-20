using CodingChallengeV4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodingChallengeV4.ViewModels
{
    public class ContactPassData
    {
        public int passedID { get; set; }
        public string passedfName { get; set; }
        public string passedlName { get; set; }
        public string passedeMail { get; set; }
        public EmailType passedeMailType { get; set; }
    }
}