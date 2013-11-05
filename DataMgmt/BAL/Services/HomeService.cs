using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.Interfaces;

namespace BAL.Services
{
    public class HomeService : ServiceBase, IHomeService
    {
        public IGlobalSettingRepository GlobalSettingRepository { get; private set; }
        public ITestDataRepository TestDataRepository { get; private set; }
        public IControlRepository ControlRepository { get; private set; }
        public HomeService(IGlobalSettingRepository globalSettingRepository, ITestDataRepository testDataRepository, IControlRepository controlRepository)
        {
            this.GlobalSettingRepository = globalSettingRepository;
            this.TestDataRepository = testDataRepository;
            this.ControlRepository = controlRepository;
            this.AddDisposableObject(globalSettingRepository);
            this.AddDisposableObject(testDataRepository);
            this.AddDisposableObject(controlRepository);
        }

        public int ReturnTestCaseNumber()
        {
            return this.TestDataRepository.GetTestCaseAmount(this.CurrentProjectId);
        }

        public int ReturnGlobalSettingNumber()
        {
            return this.GlobalSettingRepository.GetCount(gs => gs.ProjectId == this.CurrentProjectId);
        }

        public int ReturnTestDataNumber()
        {
            return this.TestDataRepository.GetCount(td => td.ProjectId == this.CurrentProjectId);
        }

        public int ReturnControlNumber()
        {
            return this.ControlRepository.GetCount(c => c.ProjectId == this.CurrentProjectId);
        }

        public int CurrentProjectId
        {
            get;
            set;
        }
    }
}
