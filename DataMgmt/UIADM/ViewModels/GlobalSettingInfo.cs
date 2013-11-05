using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DAL.Entities;

namespace UIADM.ViewModels
{
    public class GlobalSettingInfo
    {
        public IEnumerable<GlobalSetting> GlobalSettings { get; set; }

        public QueryConditionEntity QueryConditionEntity { get; set; }

        public GlobalSetting SelectedGlobalSetting { get; set; }
    }
    public class QueryConditionEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}