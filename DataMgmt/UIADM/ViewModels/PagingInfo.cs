using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace UIADM.ViewModels
{
    public class PagingInfo
    {
        public static int PageSize
        {
            get { return int.Parse(ConfigurationManager.AppSettings["PageSize"]); }
        }
        public int RecordCount { get; set; }
        public int PageIndex { get; set; }
        public int PageCount
        {
            get { return (int)Math.Ceiling((decimal)RecordCount / PageSize); }
        }
    }
}