using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace UIA.Framework.Core
{
    public class AppUnderTest : ISearchCriteria
    {
        private string processname;
        private AutomationElement rootWindow;
        private volatile static AppUnderTest _instance = null;
        private static readonly object lockHelper = new object();
        
        private AppUnderTest() 
        {
        }

        public AutomationElement Self
        {
            get 
            {
                if (rootWindow != null)
                    return rootWindow;
                else
                    return AutomationElement.RootElement;
            }
        }

        public int TimeoutSecond
        {
            get;
            set;
        }

        public static AppUnderTest CreateInstance()
        {
            if(_instance == null)
            {
                lock(lockHelper)
                {
                    if(_instance == null)
                        _instance = new AppUnderTest();
                }
            }
            return _instance;
        }

        public void Launch(string _exeFilePath, string arguments = "")
        {
            this.processname = _exeFilePath.Split('\\').Last<string>();
            this.processname = processname.Substring(0, processname.IndexOf('.'));
            Process p = Process.Start(_exeFilePath, arguments);
            p.WaitForInputIdle();
			this.rootWindow = this.GetMainWindowByProcessName(processname);
			while (!this.rootWindow.Current.IsEnabled)
			{
				Thread.Sleep(600);
			}
        }

        public AutomationElement GetMainWindowByProcessName(string processName, int timeoutSecond = 3600)
        {
            timeoutSecond = (this.TimeoutSecond == 0) ? timeoutSecond : this.TimeoutSecond;
            while (0<timeoutSecond)
            {
                Process p = Process.GetProcessesByName(processName).FirstOrDefault();
                if (null!=p && !String.IsNullOrEmpty(p.MainWindowTitle))
                {
					this.rootWindow = AutomationElement.FromHandle(p.MainWindowHandle);
					return this.rootWindow;
                }
                timeoutSecond--;
                Thread.Sleep(600);
            }
			this.rootWindow = null;
			return null;
        }

        public AutomationElement GetMainWindowByNameProperty(string name, int timeoutSecond = 3600)
        {
            timeoutSecond = (this.TimeoutSecond == 0) ? timeoutSecond : this.TimeoutSecond;
            var desktop = AutomationElement.RootElement;
            return GetControlTileEnable(SearchBy.ByName, name, false, timeoutSecond, desktop);
        }

		public AutomationElement GetTopWindowContainsName(string name, int timeoutSecond = 3600)
		{
			timeoutSecond = (this.TimeoutSecond == 0) ? timeoutSecond : this.TimeoutSecond;
			while (0 < timeoutSecond)
			{
				var desktop = AutomationElement.RootElement;
				var apps = desktop.FindAll(TreeScope.Children, new PropertyCondition(SearchBy.ByControlType, ControlType.Window));
				foreach (AutomationElement app in apps)
				{
					if (app.Current.IsEnabled && app.Current.Name.Contains(name))
					{
						return app;
					}
				}
				Thread.Sleep(800);
				timeoutSecond--;
			}
			return null;
		}

        public AutomationElement GetMainWindow(AutomationProperty searchBy, object value, int timeoutSecond = 3600)
        {
            timeoutSecond = (this.TimeoutSecond == 0) ? timeoutSecond : this.TimeoutSecond;
            var desktop = AutomationElement.RootElement;
            return GetControlTileEnable(searchBy, value, false, timeoutSecond, desktop);
        }

		public AutomationElement GetUpdatedAEWhenStructureChanged(AutomationElement mainWidow, AutomationPattern patternToCache = null, AutomationProperty propertyToCache = null)
        {
            CacheRequest cacheRequest = new CacheRequest();
			cacheRequest.TreeScope = TreeScope.Element | TreeScope.Descendants | TreeScope.Children;
			cacheRequest.AutomationElementMode = AutomationElementMode.Full;
			cacheRequest.TreeFilter = System.Windows.Automation.Automation.RawViewCondition;
			if (patternToCache != null)
			{
				cacheRequest.Add(patternToCache);
			}
			else
			{
				cacheRequest.Add(SelectionPattern.Pattern);
				cacheRequest.Add(WindowPattern.Pattern);
				cacheRequest.Add(InvokePattern.Pattern);
				cacheRequest.Add(TogglePattern.Pattern);
				cacheRequest.Add(ExpandCollapsePattern.Pattern);
				cacheRequest.Add(ValuePattern.Pattern);
				cacheRequest.Add(SelectionItemPattern.Pattern);
			}
			if (propertyToCache != null)
			{
				cacheRequest.Add(propertyToCache);
			}
			else
			{
				cacheRequest.Add(AutomationElement.NameProperty);
				cacheRequest.Add(AutomationElement.AutomationIdProperty);
				cacheRequest.Add(AutomationElement.ClassNameProperty);
				cacheRequest.Add(AutomationElement.ControlTypeProperty);
			}
			AutomationElement updatedElement;
			using (cacheRequest.Activate())
			{
				updatedElement = mainWidow.GetUpdatedCache(cacheRequest);
			}
			return updatedElement;
        }

        public void Close(string processName = "")
        {
            if (String.IsNullOrEmpty(processName))
                processName = this.processname;
            foreach (Process p in Process.GetProcessesByName(processName))
            { 
                p.Kill();
            }
        }

        private AutomationElement GetControlTileEnable(AutomationProperty searchBy, object value, bool searchDescendants, int timeoutSecond = 60, AutomationElement retrieveControl = null)
        {
            return GetControlTileEnable(new PropertyCondition(searchBy, value), searchDescendants, timeoutSecond, retrieveControl);
        }

        private AutomationElement GetControlTileEnable(Dictionary<AutomationProperty, object> searchCriteria, bool searchDescendants, int timeoutSecond = 60, AutomationElement retrieveControl = null)
        {
            Condition condition = MergeMultiConditions(searchCriteria);
            return GetControlTileEnable(condition,searchDescendants,timeoutSecond,retrieveControl);
        }

        private AutomationElementCollection GetControlsTileEnable(AutomationProperty searchBy, object value, bool searchDescendants, int timeoutSecond = 60, AutomationElement retrieveControl = null)
        {
            return GetControlsTileEnable(new PropertyCondition(searchBy, value), searchDescendants, timeoutSecond, retrieveControl);
        }

        private AutomationElementCollection GetControlsTileEnable(Dictionary<AutomationProperty, object> searchCriteria, bool searchDescendants, int timeoutSecond = 60, AutomationElement retrieveControl = null)
        {
            Condition condition = MergeMultiConditions(searchCriteria);
            return GetControlsTileEnable(condition, searchDescendants, timeoutSecond, retrieveControl);
        }

        private AutomationElement GetControlTileEnable(Condition condition, bool searchDescendants, int timeoutSecond = 60, AutomationElement retrieveControl = null)
        {
            timeoutSecond = (this.TimeoutSecond == 0) ? timeoutSecond : this.TimeoutSecond;
			AutomationElement controlSearched = retrieveControl ?? this.rootWindow;
            var scope = searchDescendants ? TreeScope.Descendants : TreeScope.Children;
            var target = controlSearched.FindFirst(scope, condition);
            while (0 < timeoutSecond)
            {
                if (null != target && target.Current.IsEnabled)
                {
                    return target;
                }
                Thread.Sleep(800);
                target = controlSearched.FindFirst(scope, condition);
                timeoutSecond--;
            }
            return null;
        }

        private AutomationElementCollection GetControlsTileEnable(Condition condition, bool searchDescendants, int timeoutSecond = 60, AutomationElement retrieveControl = null)
        {
            timeoutSecond = (this.TimeoutSecond == 0) ? timeoutSecond : this.TimeoutSecond;
            AutomationElement controlSearched = retrieveControl ?? this.rootWindow;
            var scope = searchDescendants ? TreeScope.Descendants : TreeScope.Children;
            var target = controlSearched.FindAll(scope, condition);
            while (0 < timeoutSecond)
            {
                if (null != target && target.Count>0 && target[0].Current.IsEnabled)
                {
                    return target;
                }
                Thread.Sleep(800);
                target = controlSearched.FindAll(scope, condition);
                timeoutSecond--;
            }
            return null;
        }

        private Condition MergeMultiConditions(Dictionary<AutomationProperty, object> searchCriteria)
        {
            Condition condition;
            if (1 == searchCriteria.Count)
            {
                condition = new PropertyCondition(searchCriteria.Keys.First() as AutomationProperty, searchCriteria.Values.First() as object);
            }
            else
            {
                List<Condition> listCondition = new List<Condition>();
                foreach (KeyValuePair<AutomationProperty, object> item in searchCriteria)
                {
                    listCondition.Add(new PropertyCondition(item.Key, item.Value));
                }
                condition = new AndCondition(listCondition.ToArray());
            }
            return condition;
        }

        public AutomationElementCollection FindChildren(Dictionary<AutomationProperty, object> searchCriteria, AutomationElement retrieveControl = null)
        {
            var ret = GetControlsTileEnable(searchCriteria, false, 30, retrieveControl);
            if (ret != null)
                return ret;
            else
                throw ThrowControlNotFoundException(searchCriteria);
        }

        public AutomationElement FindChild(Dictionary<AutomationProperty, object> searchCriteria, AutomationElement retrieveControl = null)
        {
            var ret = GetControlTileEnable(searchCriteria, false, 30, retrieveControl);
            if (ret != null)
                return ret;
            else
				throw ThrowControlNotFoundException(searchCriteria);
        }

        public AutomationElementCollection FindDescendants(Dictionary<AutomationProperty, object> searchCriteria, AutomationElement retrieveControl = null)
        {
            var ret = GetControlsTileEnable(searchCriteria, true, 30, retrieveControl);
            if (ret != null)
                return ret;
            else
				throw ThrowControlNotFoundException(searchCriteria);
        }

        public AutomationElement FindDescendant(Dictionary<AutomationProperty, object> searchCriteria, AutomationElement retrieveControl = null)
        {
            var ret = GetControlTileEnable(searchCriteria, true, 30, retrieveControl);
            if (ret != null)
                return ret;
            else
				throw ThrowControlNotFoundException(searchCriteria);
        }

        public AutomationElementCollection FindChildren(AutomationProperty searchBy, object value, AutomationElement retrieveControl = null)
        {
            var ret = GetControlsTileEnable(searchBy, value, false, 30, retrieveControl);
            if (ret != null)
                return ret;
            else
                throw new ControlNotFoundException(searchBy.ToString()+":"+value.ToString());
        }

        public AutomationElement FindChild(AutomationProperty searchBy, object value, AutomationElement retrieveControl = null)
        {
            var ret = GetControlTileEnable(searchBy, value, false, 30, retrieveControl);
            if (ret != null)
                return ret;
            else
                throw new ControlNotFoundException(searchBy.ToString() + ":" + value.ToString());
        }

        public AutomationElementCollection FindDescendants(AutomationProperty searchBy, object value, AutomationElement retrieveControl = null)
        {
            var ret = GetControlsTileEnable(searchBy, value, true, 30, retrieveControl);
            if (ret != null)
                return ret;
            else
                throw new ControlNotFoundException(searchBy.ToString() + ":" + value.ToString());
        }

        public AutomationElement FindDescendant(AutomationProperty searchBy, object value, AutomationElement retrieveControl = null)
        {
            var ret = GetControlTileEnable(searchBy, value, true, 30, retrieveControl);
            if (ret != null)
                return ret;
            else
                throw new ControlNotFoundException(searchBy.ToString() + ":" + value.ToString());
        }

        private TreeWalker GetTreeWalkerByCondition(Dictionary<AutomationProperty, object> searchCriteria = null)
        {
            Condition condition = (null != searchCriteria) ? MergeMultiConditions(searchCriteria) :
                new AndCondition(
                    new PropertyCondition(AutomationElement.IsControlElementProperty, true),
                    new PropertyCondition(AutomationElement.IsEnabledProperty, true)
                    );
            return new TreeWalker(condition);
        }

        public AutomationElement FindParent(AutomationElement retrieveControl = null)
        {
			retrieveControl = retrieveControl ?? this.rootWindow;
            TreeWalker walker = GetTreeWalkerByCondition();
            return walker.GetParent(retrieveControl);
        }

        public AutomationElement FindNextSibling(AutomationElement retrieveControl = null)
        {
			retrieveControl = retrieveControl ?? this.rootWindow;
            TreeWalker walker = GetTreeWalkerByCondition();
            return walker.GetNextSibling(retrieveControl);
        }

        public AutomationElement FindPreviousSibling(AutomationElement retrieveControl = null)
        {
			retrieveControl = retrieveControl ?? this.rootWindow;
            TreeWalker walker = GetTreeWalkerByCondition();
            return walker.GetPreviousSibling(retrieveControl);
        }

        public AutomationElement FindLastChild(AutomationElement retrieveControl = null)
        {
			retrieveControl = retrieveControl ?? this.rootWindow;
            TreeWalker walker = GetTreeWalkerByCondition();
            return walker.GetLastChild(retrieveControl);
        }

        public AutomationElement FindFirstChild(AutomationElement retrieveControl = null)
        {
			retrieveControl = retrieveControl ?? this.rootWindow;
            TreeWalker walker = GetTreeWalkerByCondition();
            return walker.GetFirstChild(retrieveControl);
        }

		private ControlNotFoundException ThrowControlNotFoundException(Dictionary<AutomationProperty, object> searchCriteria)
		{
			string error = "";
			foreach (KeyValuePair<AutomationProperty, object> item in searchCriteria)
			{
				error += "Property: " + item.Key + " Value: " + item.Value + "\r\n";
			}
			return new ControlNotFoundException(error);
		}
    }

    
}
