using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Remedy.Search;
using Remedy.Search.Query;

namespace Remedy.Search.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var q = new Query.QueryBuilder()
            .Assignee().Is(new FirstAndLastName("Jeremy", "Shantz"))  
            .Submitted().InThePast(2).Weeks()
            .Summary().ContainsAnyOf("a", "b", "c")
            .StatusIs().Any()
            .Status().DoesNotStartWith("Pend")
            .People().Modifier().IsSet()
            .ToString();

            Assert.AreEqual("", q);
        }
    }
}
