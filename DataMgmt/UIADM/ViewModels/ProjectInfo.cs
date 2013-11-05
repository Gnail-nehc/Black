using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UIADM.ViewModels
{
    public class ProjectInfo
    {
        public string TProperty { get; set; }

        public IList<SelectListItem> ProjectItems { get; set; }
    }
}