using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BAL.Interfaces;
using DAL.Entities;
using DAL.Repository;

namespace BAL.Repositories
{
    public class TestDataRepository : RepositoryBase<TestData>, ITestDataRepository
    {
        public TestDataRepository(QADB_Entities dbContext)
            : base(dbContext)
        { 
        }

        public IEnumerable<TestData> FindAll(int prjId, int pageIndex, int pageSize, out int recordCount)
        {
            IEnumerable<TestData> testDatas = this.Get<int>(td => td.ProjectId == prjId, pageIndex, pageSize, td => td.Id, true);
			recordCount = this.GetCount(td => td.ProjectId == prjId);
            return testDatas;
        }

		private TestData FindById(int id)
		{
			var target = this.Get(td => td.Id == id).FirstOrDefault<TestData>();
			return target;
		}

        public IEnumerable<TestData> QueryTestDatas(int prjId, string tcId, string name, string value, int pageIndex, int pageSize, out int recordCount)
        {
			name = name.Trim();
			value = value.Trim();
			//Expression<Func<TestData, bool>> expression = td =>
			//	name != "" ? td.Name.Contains(name) : true &&
			//	value != "" ? td.Value.Contains(value) : true &&
			//	tcId != "" ? (td.TestCaseId == tcId && td.ProjectId == prjId) : td.ProjectId == prjId;
			IList<Condition4Expression> listCondition = new List<Condition4Expression>
			{
				new Condition4Expression
				{
					FieldName="ProjectId",ComparisonMethod=ComparisonMethod.Equals,ComparisonValue=prjId
				},
				new Condition4Expression
				{
					FieldName="TestCaseId",ComparisonMethod=ComparisonMethod.Equals,ComparisonValue=tcId
				},
				new Condition4Expression
				{
					FieldName="Name",ComparisonMethod=ComparisonMethod.Contains,ComparisonValue=name
				},
				new Condition4Expression
				{
					FieldName="Value",ComparisonMethod=ComparisonMethod.Contains,ComparisonValue=value
				},
			};
			Expression<Func<TestData, bool>> expression = this.GetConditionExpression<TestData>(listCondition, LogicalOperator.And);
			IEnumerable<TestData> testDatas = this.Get(expression,pageIndex, pageSize, td => td.Id, true);
			recordCount = this.Count(expression);
            return testDatas;
        }

        public bool AddTestData(TestData testData, out string log)
        {
            bool isUnique = IsNameUniqueInTestCaseInProject(testData.ProjectId, testData.TestCaseId, testData.Name);
            if (isUnique)
            {
                this.Add(testData);
                log = "done.";
            }
            else
            {
                log = "Fail to Add.The name should be unique in Test Case " + testData.TestCaseId + " in Project " + testData.ProjectId;
            }
            return isUnique;
        }

        public bool DeleteTestData(int id, out string log)
        {
            try
            {
                TestData testData = this.Get(td => td.Id == id).FirstOrDefault<TestData>();
                this.Delete(testData);
                log = "done.";
                return true;
            }
            catch (Exception e)
            {
                log = "Fail to delete." + e.Message;
                return false;
            }
        }

        public bool UpdateTestData(TestData testData, out string log)
        {
			bool isUnique = true;
			TestData td = this.FindById(testData.Id);
			bool ifChanged = (td.Name.Equals(testData.Name) && td.TestCaseId.Equals(testData.TestCaseId)) ? false : true;
			if (ifChanged)
			{
				isUnique = IsNameUniqueInTestCaseInProject(testData.ProjectId, testData.TestCaseId, testData.Name);
			}
			if (isUnique)
            {
                this.Update(testData);
                log = "done.";
            }
            else
            {
                log = "Fail to Update.The name should be unique in Project " + testData.ProjectId;
            }
			return isUnique;
        }

		private bool IfNameChanged(int id, string newName)
		{
			TestData td = this.FindById(id);
			return (!td.Name.Equals(newName)) ? true : false;
		}

        public int GetCount(Expression<Func<TestData, bool>> filter)
        { 
            return base.Count(filter);
        }

        public int GetTestCaseAmount(int prjId)
        {
			var all = this.Get(td => td.ProjectId == prjId);
			return all.GroupBy(td => td.TestCaseId).Select(group => group.First()).Count();
        }

        public bool ExistTestCase(string tcId)
        {
            int count = base.Count(td => td.TestCaseId == tcId);
            return count == 0 ? false : true;
        }

        private bool IsNameUniqueInTestCaseInProject(int prjId,string tcId,string name)
        {
            int count = base.Count(td => td.ProjectId == prjId && td.TestCaseId == tcId && td.Name == name);
            return count == 0 ? true : false;
        }
    }
}
