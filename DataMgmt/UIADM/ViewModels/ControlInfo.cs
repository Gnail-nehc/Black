using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Entities;

namespace UIADM.ViewModels
{
    public class ControlInfo
    {
        public IEnumerable<Control> Controls { get; set; }

        public SearchedConditionEntity SearchedConditionEntity { get; set; }

        public Control SelectedControl { get; set; }

        public IList<SelectListItem> PropertyListWhenEdit { get; set; }
    }

    public class SearchedConditionEntity
    {
        public string ControlType { get; set; }
        public string ControlProperty { get; set; }
        public string PropertyValue { get; set; }
        public string ControlName { get; set; }
    }
}
