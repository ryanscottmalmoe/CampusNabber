using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseCode.CNQueryFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CampusNabber.Models;

namespace DatabaseCode.CNQueryFolder.Tests
{
    [TestClass()]
    public class AssemblyInfo
    {

        [TestMethod()]
        public void setQueryWhereKeyEqualToConditionTest()
        {
            CNQuery query = new CNQuery("User");
            query.setQueryWhereKeyEqualToCondition("username", "bob");
            Dictionary<string, dynamic> conditions = query.getQueryCondition();
            Assert.AreEqual(conditions["username="], "bob");
        }

        [TestMethod()]
        public void setQueryWhereKeyNotEqualToConditionTest()
        {
            CNQuery query = new CNQuery("User");
            query.setQueryWhereKeyNotEqualToCondition("username", "bob");
            Dictionary<string, dynamic> conditions = query.getQueryCondition();
            Assert.AreEqual(conditions["username!="], "bob");
        }

        [TestMethod()]
        public void buildWhereStringTest()
        {
            CNQuery query = new CNQuery("User");
            query.setQueryWhereKeyNotEqualToCondition("username", "bob");
            string testString = query.buildWhereString();
            Assert.AreEqual("username!=@0 ", testString);
        }

        [TestMethod()]
        public void buildValueStringTest()
        {
            CNQuery query = new CNQuery("User");
            query.setQueryWhereKeyEqualToCondition("username", "james");
            query.setQueryWhereKeyNotEqualToCondition("username", "bob");
            string testString = query.buildValueString();
            Assert.AreEqual("james, bob", testString);
        }

        [TestMethod()]
        public void selectObjectByIdTest()
        {
            CNQuery query = new CNQuery("User");
            Guid guid = new Guid("442ae9ea-2031-46e8-b7a7-986fae80aab8");
            query.setQueryWhereKeyEqualToCondition("object_id", guid);
            User user = query.selectObjectById();
            Assert.AreEqual(user.object_id, guid);
        }

        [TestMethod()]
        public void selectTest()
        {

        }
    }
}