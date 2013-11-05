using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UIADM.Controllers
{
    public class ControllerBase : Controller,IDisposable
    {
        public IList<IDisposable> DisposableObjects { get;  private set;  }

        public ControllerBase()
        {
            this.DisposableObjects = new List<IDisposable>();
        }

        protected ActionResult VerifyIfProjectSelected(string sessionKey)
        {
            if (null != Session[sessionKey])
                return null;
            return Redirect("/");
        }

        protected void AddDisposableObject(object obj)
        {
            IDisposable disposable = obj as IDisposable;
            if (null != disposable)
            {
                this.DisposableObjects.Add(disposable);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                foreach (IDisposable obj in this.DisposableObjects)
                {
                    if (null != obj)
                    {
                        obj.Dispose();
                    }
                }
            }
            base.Dispose(disposing);
        }

    }
}
