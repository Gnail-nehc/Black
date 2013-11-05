using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.Interfaces;
using DAL.Entities;
using DAL.Repository;

namespace BAL.Services
{
    public class TestDataService : ServiceBase, ITestDataService
    {
        public ITestDataRepository TestDataRepository { get; private set; }
        public TestDataService(ITestDataRepository testDataRepository)
        {
            this.TestDataRepository = testDataRepository;
            this.AddDisposableObject(testDataRepository);
        }

        public IEnumerable<TestData> FindAll(int pageIndex, int pageSize, out int recordCount)
        {
            int pid = (this.CurrentProjectId > 0) ? this.CurrentProjectId : 1;
            return this.TestDataRepository.FindAll(pid, pageIndex, pageSize, out recordCount);
        }

        public IEnumerable<TestData> QueryTestDatas(string tcId, string name, string value,int pageIndex, int pageSize, out int recordCount)
        {
            if (null == tcId) tcId = "";
            if (null == name) name = "";
            if (null == value) value = "";
            int pid = (this.CurrentProjectId > 0) ? this.CurrentProjectId : 1;
            return this.TestDataRepository.QueryTestDatas(pid, tcId, name, value,pageIndex,pageSize,out recordCount);
        }

        public bool AddTestData(TestData testData, out string log)
        {
            Guard.ArgumentNotNull(testData, "testDataObject");
            int pid = (this.CurrentProjectId > 0) ? this.CurrentProjectId : 1;
            testData.ProjectId = pid;
            return this.TestDataRepository.AddTestData(testData, out log);
        }

        public bool DeleteTestData(int id, out string log)
        {
            return this.TestDataRepository.DeleteTestData(id, out log);
        }

        public bool UpdateTestData(TestData testData, out string log)
        {
            Guard.ArgumentNotNull(testData, "testDataObject");
            testData.ProjectId = (this.CurrentProjectId > 0) ? this.CurrentProjectId : 1;
            return this.TestDataRepository.UpdateTestData(testData, out log);
        }

        public bool ExistTestCase(string tcId)
        {
            Guard.ArgumentNotNullOrEmpty(tcId, "testCaseId");
            return this.TestDataRepository.ExistTestCase(tcId);
        }

        public int CurrentProjectId
        {
            get;
            set;
        }
    }
}
