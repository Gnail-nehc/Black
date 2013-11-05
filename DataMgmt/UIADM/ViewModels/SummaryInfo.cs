using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DAL.Entities;

namespace UIADM.ViewModels
{
    public class SummaryInfo
    {
        [DisplayName("Total Test Case Number: ")]
        public int TestCaseNo { get; set; }

        [DisplayName("Total Global Setting Parameter Number: ")]
        public int GlobalSettingNo { get; set; }

        [DisplayName("Total Test Data Parameter Number: ")]
        public int TestDataNo { get; set; }

        [DisplayName("Total Control Parameter Number: ")]
        public int ControlNo { get; set; }
    }
}