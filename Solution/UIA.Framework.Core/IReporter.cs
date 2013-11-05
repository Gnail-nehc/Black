using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UIA.Framework.Core
{
    public interface IReporter
    {
        void LogInfo(string message, params object[] args);

        void LogError(string message, params object[] args);

        void LogWarning(string message, params object[] args);

        void ScenarioStart(int scenarioId, string scenarioTitle);

        void ScenarioEnd(int scenarioId);

    }
}
