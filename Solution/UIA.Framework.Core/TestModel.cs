using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIA.Framework.Core
{
    public abstract class TestModel
    {
        public Dictionary<string, string> GlobalData { get; set; }

        public Dictionary<string, string> TestData { get; set; }

		public TestContext TestContext { get; set; }

        public IReporter Logger
		{
			get
			{
				return Reporter.CreateInstance(TestContext);
			}
		}

        [TestInitialize]
        public abstract void Init();

        [TestCleanup]
        public abstract void End();

        protected TestModel(string projectId, string testCaseId)
        {
			Dictionary<string, string> dictGlobalData, dictLocalData;
			RetrieveData2Dictionary(projectId, testCaseId, out dictGlobalData, out dictLocalData);
			this.GlobalData = dictGlobalData;
			this.TestData = dictLocalData;
        }

        private void RetrieveData2Dictionary(string projectId, string testCaseId, out Dictionary<string, string> globalData, out Dictionary<string, string> localData)
        {
            string sql = "select " + ReturnedDataFuncStruct.Name + "," + ReturnedDataFuncStruct.Value + "," + ReturnedDataFuncStruct.IsGlobal + " from " + ReturnedDataFuncStruct.FuncInDB + "(" + projectId + "," + testCaseId + ")";
            DataTable dt = SqlHelper.GetDataTableBySql(sql);
            globalData = ConvertTable2Dictionary(dt, true);
            localData = ConvertTable2Dictionary(dt, false);
        }

        private Dictionary<string, string> ConvertTable2Dictionary(DataTable table, bool isGlobal)
        {
            try
            {
                var query = from row in table.AsEnumerable()
                            where row.Field<bool>(ReturnedDataFuncStruct.IsGlobal) == isGlobal
                            select new
                            {
                                columnName = row.Field<string>(ReturnedDataFuncStruct.Name),
                                columnValue = row.Field<string>(ReturnedDataFuncStruct.Value)
                            };
                if (query.Count() != 0)
                {
                    return query.AsEnumerable().ToDictionary(k => k.columnName, v => v.columnValue);
                }
                else
                    return new Dictionary<string, string>();
            }
            catch (Exception ex)
            {
				this.Logger.LogError(ex.Message);
                throw ex;
            }
        }

        protected DataTable LoadControlsInfo(string projectId)
        {
            try
            {
                string sql = "select " + ReturnedControlFuncStruct.UIType + "," + ReturnedControlFuncStruct.UIProperty + "," + ReturnedControlFuncStruct.PropValue + "," + ReturnedControlFuncStruct.UIName + " from " + ReturnedControlFuncStruct.FuncInDB + "(" + projectId + ")";
                return SqlHelper.GetDataTableBySql(sql);
            }
            catch (Exception ex)
            {
				this.Logger.LogError("Fail to load controls in project [{0}].", projectId);
                throw ex;
            }
        }
    }
}
