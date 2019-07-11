using DbManager.Entity.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PointrCdn.Models
{
    public class ExceptionLog : ModifiableDbTable
    {
        public string function { get; set; }
        public string objectClass { get; set; }
        public string exceptionMessage { get; set; }
    }
}