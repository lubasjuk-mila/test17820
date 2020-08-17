using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Test4Atma.Entity;

namespace Test4Atma.Logic
{
    public class BusinessLogic
    {
        public BusinessLogic()
        {

        }

        public static int CountSoldArticlesAmountPerDay(List<ArticleEntity> allSoldArticles, DateTime dateTime)
        {

            return FilterSoldArticlesPerDay(allSoldArticles, dateTime).Count;
        }

        public static double CountRevenuePerDay(List<ArticleEntity> allSoldArticles, DateTime dateTime)
        {
            double revenue = 0.0;
            var list = FilterSoldArticlesPerDay(allSoldArticles, dateTime);
            foreach (var article in list)
            {
                revenue += article.Price;
            }

            return revenue;
        }

        public static Boolean validateArticleNumberLength(string articleNumber)
        {
            if (articleNumber == null || articleNumber.Length == 0 || articleNumber.Length > 32) 
            { 
                return false; 
            }
            return true;
        }


        public static Boolean validateArticleNumberContent(string articleNumber)
        {
            if (!Regex.IsMatch(articleNumber, "^[a-zA-Z0-9]*$"))
            {
                return false;
            }
            return true;
        }

        private static List<ArticleEntity> FilterSoldArticlesPerDay(List<ArticleEntity> allSoldArticles, DateTime dateTime)
        {
            var filtered = new List<ArticleEntity>();

            foreach (var article in allSoldArticles)
            {
                if (CompareArticleDateIsWithinDate(article, dateTime))
                {
                    filtered.Add(article);
                    Console.WriteLine(" Article number was filtered: {0}", article.Number);
                }
            }

            return filtered;

        }

        private static Boolean CompareArticleDateIsWithinDate(ArticleEntity article, DateTime dateTime)
        {
            var date = dateTime.Date;

            if (article.DateTime.Date.Equals(date))
            {
                return true;
            }

            return false;
        }



    }


}
