﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyHome.DataClasses.Tests
{
    [TestClass]
    public class IncomeCategoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "name", AllowDerivedTypes = false)]
        public void Category_Throws_If_Name_Is_Empty_String()
        {
            var _ = new IncomeCategory(0, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "name", AllowDerivedTypes = false)]
        public void Category_Throws_If_Name_Is_Null()
        {
            var _ = new IncomeCategory(0, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "name", AllowDerivedTypes = false)]
        public void Category_Throws_If_Name_Is_Only_Whitespace()
        {
            var _ = new IncomeCategory(0, "    \t\r\n");
        }

        [TestMethod]
        public void Equals_All_Fields_Same()
        {
            var first = new IncomeCategory(0, "name");
            var second = new IncomeCategory(0, "name");
            Assert.AreNotSame(first, second);
            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(second.Equals(first));
        }

        [TestMethod]
        public void Equals_Id_Differs()
        {
            var first = new IncomeCategory(0, "name");
            var second = new IncomeCategory(10, "name");
            Assert.AreNotSame(first, second);
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));
        }

        [TestMethod]
        public void Equals_Name_Differs()
        {
            var first = new IncomeCategory(0, "name");
            var second = new IncomeCategory(0, "other name");
            Assert.AreNotSame(first, second);
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));
        }
    }
}
