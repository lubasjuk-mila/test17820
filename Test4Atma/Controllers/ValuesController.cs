using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Test4Atma.Logic;

namespace Test4Atma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public static List<Entity.ArticleEntity> articlesListTemp = new List<Entity.ArticleEntity>();


        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Entity.ArticleEntity>> Get()
        {
            return Ok(articlesListTemp);
        }

        // GET api/values/5
        [HttpGet("{number}")]
        public ActionResult<Entity.ArticleEntity> Get(string number)
        {
            foreach (var article in articlesListTemp)
            {
                if (article.Number.Equals(number))
                {
                    return Ok(article);
                }
            }
            Console.WriteLine("Article with number: {0} doesn't exist.", number);
            return NotFound();
        }

        // GET api/values/getSoldAmountPerDay
        [HttpGet("getSoldAmountPerDay")]
        public ActionResult<int> GetSoldArticlesAmountPerDay()
        {
            List<Entity.ArticleEntity> list = (List<Entity.ArticleEntity>)(Get().Result as ObjectResult).Value;
            return Ok(BusinessLogic.CountSoldArticlesAmountPerDay(list, DateTime.Now));
        }

        // GET api/values/getSoldAmountPerDay
        [HttpGet("getSoldAmountPerDay/{date}")]
        public ActionResult<int> GetSoldArticlesAmountPerDay(DateTime date)
        {
            List<Entity.ArticleEntity> list = (List<Entity.ArticleEntity>)(Get().Result as ObjectResult).Value;
            return Ok(BusinessLogic.CountSoldArticlesAmountPerDay(list, date));
        }


        // GET api/values/getRevenuePerDay
        [HttpGet("getRevenuePerDay")]
        public ActionResult<double> GetRevenuePerDay()
        {
            List<Entity.ArticleEntity> list = (List<Entity.ArticleEntity>)(Get().Result as ObjectResult).Value;
            return Ok(BusinessLogic.CountRevenuePerDay(list, DateTime.Now));
        }

        // GET api/values/getRevenuePerDay
        [HttpGet("getRevenuePerDay/{date}")]
        public ActionResult<double> GetRevenuePerDay(DateTime date)
        {
            List<Entity.ArticleEntity> list = (List<Entity.ArticleEntity>)(Get().Result as ObjectResult).Value;
            return Ok(BusinessLogic.CountRevenuePerDay(list, date));
        }


        // POST api/values
        [HttpPost]
        //public void Post([FromBody] string value)
        public ActionResult Post([FromBody] Entity.ArticleEntity data)
        {
            if (!BusinessLogic.validateArticleNumberLength(data.Number))
            {
                return BadRequest("Article number length can't be more than 32 characters! Please check the input: " + data.Number);
            }
            if (!BusinessLogic.validateArticleNumberContent(data.Number))
            {
                return BadRequest("Artickle number should be alphanumeric! Please check the input: " + data.Number);
            }

            if (data.DateTime == null)
            {
                data.DateTime = DateTime.Now;
            }
            articlesListTemp.Add(data);
            return Ok();
        }

        // PUT api/values
        [HttpPut]
        public ActionResult Put([FromBody] Entity.ArticleEntity data)
        {

            foreach (var article in articlesListTemp)
            {
                if (article.Number.Equals(data.Number))
                {
                    article.Price = data.Price;
                    article.Currency = data.Currency;
                    article.DateTime = data.DateTime;
                    Console.WriteLine("Article with number: {0} was updated.", data.Number);
                    return Ok();
                }
            }
            return NotFound();
        }

        // DELETE api/values/5
        [HttpDelete("{number}")]
        public ActionResult Delete(string number)
        {
            foreach (var article in articlesListTemp)
            {
                if (article.Number.Equals(number))
                {
                    articlesListTemp.Remove(article);
                    Console.WriteLine("Article with number: {0} was removed.", number);
                    return Ok();
                }
            }
            return NotFound();
        }
    }
}
