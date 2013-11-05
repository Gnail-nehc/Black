using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IHomeService : IDefaultService
    {
        int ReturnTestCaseNumber();
        int ReturnGlobalSettingNumber();
        int ReturnTestDataNumber();
        int ReturnControlNumber();
    }
}
