using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace BAL.Interfaces
{
    public interface ITestDataRepository
    {
        IEnumerable<TestData> FindAll(int prjId, int pageIndex, int pageSize, out int recordCount);
        IEnumerable<TestData> QueryTestDatas(int prjId, string tcId, string name, string value, int pageIndex, int pageSize, out int recordCount);
        bool AddTestData(TestData testData, out string log);
        bool DeleteTestData(int id, out string log);
        bool UpdateTestData(TestData  testData, out string log);
        int GetCount(Expression<Func<TestData, bool>> filter);
        int GetTestCaseAmount(int prjId);
        bool ExistTestCase(string tcId);
    }
}
