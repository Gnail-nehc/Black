using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.Entities;

namespace UIADM.ViewModels
{
    public class TestDataInfo
    {
        public IEnumerable<TestData> TestDataMore { get; set; }

        public FindConditionEntity FindConditionEntity { get; set; }

        public TestData SelectedTestData { get; set; }
    }
    public class FindConditionEntity
    {
        public string TestCaseId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }


}