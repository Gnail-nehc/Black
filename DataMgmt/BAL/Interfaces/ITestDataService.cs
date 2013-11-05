using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace BAL.Interfaces
{
    public interface ITestDataService : IDefaultService
    {
        IEnumerable<TestData> FindAll(int pageIndex, int pageSize, out int recordCount);
        IEnumerable<TestData> QueryTestDatas(string tcId, string name, string value, int pageIndex, int pageSize, out int recordCount);
        bool AddTestData(TestData testData, out string log);
        bool DeleteTestData(int id, out string log);
		bool UpdateTestData(TestData testData, out string log);
        bool ExistTestCase(string tcId);
    }
}
