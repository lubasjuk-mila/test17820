using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Test4Atma.Controllers.Tests
{
    [TestClass()]
    public class ValuesControllerTests
    {
        static ValuesController valuesController = new ValuesController();
        const string articleNumber1Const = "ConstantToday1";
        const string articleNumber2Const = "ConstantToday1";
        const string articleNumber3Const = "ConstantYesterday";
        const double articlePriceConst = 250.0;
        const string wrongNumber = "23543653463nonexisting";
        Entity.ArticleEntity articleToday1 = new Entity.ArticleEntity(articleNumber1Const, articlePriceConst, DateTime.Now);
        Entity.ArticleEntity articleToday2 = new Entity.ArticleEntity(articleNumber2Const, articlePriceConst, DateTime.Now);
        Entity.ArticleEntity articleYesterday = new Entity.ArticleEntity(articleNumber3Const, articlePriceConst, DateTime.Now.AddDays(-1));

        [TestInitialize]
        public void SetUp()
        {
            ActionResult<Entity.ArticleEntity> result = valuesController.Post(articleToday1);
            valuesController.Post(articleToday2);
            result = valuesController.Post(articleYesterday);
            Assert.AreEqual((int)System.Net.HttpStatusCode.OK, (result.Result as StatusCodeResult).StatusCode);
        }

        [TestCleanup]
        public void TearDown()
        {
            ValuesController.articlesListTemp.Clear();
        }

        [TestMethod()]
        public void checkGetIsSuccessfulTest()
        {
            ActionResult<IEnumerable<Entity.ArticleEntity>> result = valuesController.Get();
            Assert.AreEqual((int)System.Net.HttpStatusCode.OK, (result.Result as ObjectResult).StatusCode);
            List<Entity.ArticleEntity> resultValue = (List<Entity.ArticleEntity>)(result.Result as ObjectResult).Value;
            Assert.AreEqual(3, resultValue.Count);
        }

        [TestMethod()]
        public void checkGetByNumberIsSuccessfulTest()
        {
            ActionResult<Entity.ArticleEntity> result = valuesController.Get(articleNumber1Const);
            Assert.AreEqual((int)System.Net.HttpStatusCode.OK, (result.Result as ObjectResult).StatusCode);
            Entity.ArticleEntity resultValue = (Entity.ArticleEntity)(result.Result as ObjectResult).Value;
            Assert.AreEqual(resultValue.Number, articleNumber1Const);
            Assert.AreEqual(resultValue.Price, articlePriceConst);
            Assert.AreEqual(resultValue.Currency, "EUR");
            Assert.IsNotNull(resultValue.DateTime);
        }

        [TestMethod()]
        public void checkGetArticleByWrongNumberReturns404Test()
        {
            ActionResult<Entity.ArticleEntity> result = valuesController.Get(wrongNumber);
            Assert.AreEqual((int)System.Net.HttpStatusCode.NotFound, (result.Result as StatusCodeResult).StatusCode);
        }

        [TestMethod()]
        public void checkGetArticleByInvalidNumberDoesntBreakAnythingTest()
        {
            string invalidNumber = "'someinjection";
            ActionResult<Entity.ArticleEntity> result = valuesController.Get(wrongNumber);
            Assert.AreEqual((int)System.Net.HttpStatusCode.NotFound, (result.Result as StatusCodeResult).StatusCode);
        }

        [TestMethod()]
        public void checkArticleCanBePostedSuccessfullyTest()
        {
            string uniqueNumber = new Random().Next(1, 10) + "uniqueNumber";
            Entity.ArticleEntity article = new Entity.ArticleEntity(uniqueNumber, articlePriceConst, DateTime.Now);
            ActionResult<Entity.ArticleEntity> result = valuesController.Post(article);
            Assert.AreEqual((int)System.Net.HttpStatusCode.OK, (result.Result as StatusCodeResult).StatusCode);
        }

        [TestMethod()]
        public void checkArticleLengthPostNegativeTest()
        {
            string longNumber = new string ('1', 33);
            Entity.ArticleEntity article = new Entity.ArticleEntity(longNumber, articlePriceConst, DateTime.Now);
            ActionResult<Entity.ArticleEntity> result = valuesController.Post(article);
            Assert.AreEqual((int)System.Net.HttpStatusCode.BadRequest, (result.Result as BadRequestObjectResult).StatusCode);
        }

        [TestMethod()]
        public void checkArticleNumberPostNegativeTest()
        {
            Entity.ArticleEntity article = new Entity.ArticleEntity("1111!2222", articlePriceConst, DateTime.Now);
            ActionResult<Entity.ArticleEntity> result = valuesController.Post(article);
            Assert.AreEqual((int)System.Net.HttpStatusCode.BadRequest, (result.Result as BadRequestObjectResult).StatusCode);
        }

        [TestMethod()]
        public void checkArticleNumberEmptyPostNegativeTest()
        {
            Entity.ArticleEntity article = new Entity.ArticleEntity(null, articlePriceConst, DateTime.Now);
            ActionResult<Entity.ArticleEntity> result = valuesController.Post(article);
            Assert.AreEqual((int)System.Net.HttpStatusCode.BadRequest, (result.Result as BadRequestObjectResult).StatusCode);

            article = new Entity.ArticleEntity("", articlePriceConst, DateTime.Now);
            result = valuesController.Post(article);
            Assert.AreEqual((int)System.Net.HttpStatusCode.BadRequest, (result.Result as BadRequestObjectResult).StatusCode);
        }

        [TestMethod()]
        public void checkArticlePriceZeroPostNegativeTest()
        {
            Entity.ArticleEntity article = new Entity.ArticleEntity("testEmptyPrice", 0, DateTime.Now);
            ActionResult<Entity.ArticleEntity> result = valuesController.Post(article);
            Assert.AreEqual((int)System.Net.HttpStatusCode.BadRequest, (result.Result as BadRequestObjectResult).StatusCode);
        }


        [TestMethod()]
        public void checkArticleNumberShouldBeUniquePostTest()
        {
            string uniqueNumber = new Random().Next(1, 10) + "uniqueNumber";
            Entity.ArticleEntity article = new Entity.ArticleEntity(uniqueNumber, articlePriceConst, DateTime.Now);
            ActionResult<Entity.ArticleEntity> result1 = valuesController.Post(article);
            Assert.AreEqual((int)System.Net.HttpStatusCode.OK, (result1.Result as StatusCodeResult).StatusCode);
            ActionResult<Entity.ArticleEntity> result2 = valuesController.Post(article);
            Assert.AreEqual((int)System.Net.HttpStatusCode.BadRequest, (result2.Result as StatusCodeResult).StatusCode);
        }

        [TestMethod()]
        public void checkPutSuccessfulltest()
        {
            double newPrice = 999.99;
            ActionResult<Entity.ArticleEntity> result = valuesController.Get(articleNumber1Const);
            Entity.ArticleEntity resultValue = (Entity.ArticleEntity)(result.Result as ObjectResult).Value;
            Assert.AreEqual(resultValue.Price, articlePriceConst);

            articleToday2.Price = newPrice;
            result = valuesController.Put(articleToday2);
            Assert.AreEqual((int)System.Net.HttpStatusCode.OK, (result.Result as StatusCodeResult).StatusCode);

            result = valuesController.Get(articleNumber1Const);
            resultValue = (Entity.ArticleEntity)(result.Result as ObjectResult).Value;
            Assert.AreEqual(resultValue.Number, articleNumber1Const);
            Assert.AreEqual(resultValue.Price, newPrice);
            Assert.AreEqual(resultValue.Currency, "EUR");
            Assert.IsNotNull(resultValue.DateTime);
        }

        [TestMethod()]
        public void checkPutArticleByWrongNumberReturns404Test()
        {
            Entity.ArticleEntity article = new Entity.ArticleEntity(wrongNumber, articlePriceConst, DateTime.Now);
            ActionResult<Entity.ArticleEntity> result = valuesController.Put(article);
            Assert.AreEqual((int)System.Net.HttpStatusCode.NotFound, (result.Result as StatusCodeResult).StatusCode);
        }

        [TestMethod()]
        public void checkDeleteSuccessfulltest()
        {
            ActionResult<IEnumerable<Entity.ArticleEntity>> result = valuesController.Get();
            List<Entity.ArticleEntity> resultValue = (List<Entity.ArticleEntity>)(result.Result as ObjectResult).Value;
            Assert.AreEqual(3, resultValue.Count);

            result = valuesController.Delete(articleToday1.Number);
            Assert.AreEqual((int)System.Net.HttpStatusCode.OK, (result.Result as StatusCodeResult).StatusCode);

            result = valuesController.Get();
            resultValue = (List<Entity.ArticleEntity>)(result.Result as ObjectResult).Value;
            Assert.AreEqual(2, resultValue.Count);
        }

        [TestMethod()]
        public void checkDeleteArticleByWrongNumberReturns404Test()
        {
            Entity.ArticleEntity article = new Entity.ArticleEntity(wrongNumber, articlePriceConst, DateTime.Now);
            ActionResult<Entity.ArticleEntity> result = valuesController.Delete(wrongNumber);
            Assert.AreEqual((int)System.Net.HttpStatusCode.NotFound, (result.Result as StatusCodeResult).StatusCode);
        }

        [TestMethod()]
        public void checkGetSoldArticlesAmountTodayIsSuccessfulTest()
        {
            ActionResult<int> result = valuesController.GetSoldArticlesAmountPerDay();
            Assert.AreEqual((int)System.Net.HttpStatusCode.OK, (result.Result as ObjectResult).StatusCode);
            Assert.AreEqual(2, (result.Result as ObjectResult).Value);
        }


        [TestMethod()]
        public void checkGetSoldArticlesAmountPerAnyDayIsSuccessfulTest()
        {
            ActionResult<int> result = valuesController.GetSoldArticlesAmountPerDay(DateTime.Now.AddDays(-1));
            Assert.AreEqual((int)System.Net.HttpStatusCode.OK, (result.Result as ObjectResult).StatusCode);
            Assert.AreEqual(1, (result.Result as ObjectResult).Value);
        }

        [TestMethod()]
        public void checkGetSoldArticlesAmountPerAnyDayNegativeTest()
        {
            ActionResult<int> result = valuesController.GetSoldArticlesAmountPerDay(DateTime.Now.AddDays(+4));
            Assert.AreEqual((int)System.Net.HttpStatusCode.OK, (result.Result as ObjectResult).StatusCode);
            Assert.AreEqual(0, (result.Result as ObjectResult).Value);
        }

        [TestMethod()]
        public void checkGetRevenuePerDayIsSuccessfulTest()
        {
            ActionResult<double> result = valuesController.GetRevenuePerDay();
            Assert.AreEqual((int)System.Net.HttpStatusCode.OK, (result.Result as ObjectResult).StatusCode);
            Assert.AreEqual(articlePriceConst * 2, (result.Result as ObjectResult).Value);
        }

        [TestMethod()]
        public void checkGetRevenuePerAnyDayIsSuccessfulTest()
        {
            ActionResult<double> result = valuesController.GetRevenuePerDay(DateTime.Now.AddDays(-1));
            Assert.AreEqual((int)System.Net.HttpStatusCode.OK, (result.Result as ObjectResult).StatusCode);
            Assert.AreEqual(articlePriceConst * 1, (result.Result as ObjectResult).Value);
        }


        [TestMethod()]
        public void checkGetRevenuePerAnyDayNegativeTest()
        {
            ActionResult<double> result = valuesController.GetRevenuePerDay(DateTime.Now.AddDays(-2));
            Assert.AreEqual((int)System.Net.HttpStatusCode.OK, (result.Result as ObjectResult).StatusCode);
            Assert.AreEqual((double)0, (result.Result as ObjectResult).Value);
        }

    }
}