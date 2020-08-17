using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace Test4Atma.Logic.Tests
{
    [TestClass()]
    public class BusinessLogicTests
    {

        static List<Entity.ArticleEntity> list = new List<Entity.ArticleEntity>();
        static List<Entity.ArticleEntity> list5000 = new List<Entity.ArticleEntity>();

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            
            list.Add(new Entity.ArticleEntity("111aaa", 10.0, DateTime.Now));
            list.Add(new Entity.ArticleEntity("222bbb", 14.05, DateTime.Now));
            list.Add(new Entity.ArticleEntity("333ccc", 15.95, DateTime.Now));
            list.Add(new Entity.ArticleEntity("333ccc", 50.00, DateTime.Now.AddDays(-2)));

            for (int i = 0; i < 5000; i++)
            {
                list5000.Add(new Entity.ArticleEntity("A" + i, 10, DateTime.Now));
            }
        }

        [TestMethod()]
        public void checkRevenuePerTodayTest()
        {
            var result = BusinessLogic.CountRevenuePerDay(list, DateTime.Now);
            Assert.AreEqual(40.00, result, "CountRevenuePerDay for Today date is not calculated properly!");
        }

        [TestMethod()]
        public void checkRevenueForAnotherDayTest()
        {
            var result = BusinessLogic.CountRevenuePerDay(list, DateTime.Now.AddDays(-2));
            Assert.AreEqual(50.00, result, "CountRevenuePerDay for Date in the past is not calculated properly!");
        }

        [TestMethod()]
        public void InvalidArticleNumberShouldReturnException()
        {
            Assert.IsFalse(BusinessLogic.validateArticleNumberContent("11@"));
            Assert.IsFalse(BusinessLogic.validateArticleNumberContent("12sfdsdfg!"));
            Assert.IsFalse(BusinessLogic.validateArticleNumberContent("13'45"));
            Assert.IsFalse(BusinessLogic.validateArticleNumberContent("13.45"));
            Assert.IsFalse(BusinessLogic.validateArticleNumberContent(".054645"));
        }

        [TestMethod()]
        public void MoreThan32CharacterShouldReturnException()
        {
            Assert.IsFalse(BusinessLogic.validateArticleNumberLength(new string('A', 33)));
        }

        [TestMethod]
        [Timeout(50)]
        public void check5000ArticlesListRevenueHasBeenProcessedFast()
        {
            BusinessLogic.CountRevenuePerDay(list5000, DateTime.Now);
            
        }

        [TestMethod]
        [Timeout(5)]
        public void check5000ArticlesListAmountHasBeenProcessedFast()
        {
            var result = BusinessLogic.CountSoldArticlesAmountPerDay(list5000, DateTime.Now);
            Assert.AreEqual(5000, result, "Articles amount was not properly calculated.");
        }

    }
}