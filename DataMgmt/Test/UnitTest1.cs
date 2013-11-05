using System;
using System.Collections.Generic;
using BAL.Repositories;
using DAL.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            QADB_Entities context = new QADB_Entities();
            ControlRepository repository = new ControlRepository(context);
            int recordCount;
            IEnumerable<Control> controls = repository.FindAll(1, 1, 10, out recordCount);
            foreach (Control c in controls)
            {
                string p = c.ControlProperty.Property;
                string t = c.ControlProperty.Type;
            }
        }
    }
}
