using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodingChallengeV4.ViewModels
{
    public class ContactPassDataList
    {
        List<ContactPassData> PassDataList { get; set; }
    }
    public class ContactPassData
    {
        public int passedID { get; set; }
        public string passedfName { get; set; }
        public string passedlName { get; set; }
        public string passedDName { get; set; }
        public string passedeMail { get; set; }
        public int passedeMailType { get; set; }
        public string passedeMailTypeString { get; set; }
    }
}