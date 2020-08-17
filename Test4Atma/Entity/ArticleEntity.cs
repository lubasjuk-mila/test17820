using System;
using System.ComponentModel.DataAnnotations;

namespace Test4Atma.Entity
{
    public class ArticleEntity
    {
        [Required]
        public double Price { get; set; }
        public string Currency { get; set; }
        public DateTime DateTime { get; set; }
        [Required]
        public string Number { get; set; }
        /*{
            get { return this._Number; }
            set
            {
                _Number = value;
                if (this._Number.Length > 32)
                {
                    _Number = null;
                    throw new ArgumentException("Article number length can't be more than 32 characters! Please check the input: " + value);
                }

                if (!Regex.IsMatch(this._Number, "^[a-zA-Z0-9]*$"))
                {
                    _Number = null;
                    throw new ArgumentException("Artickle number should be alphanumeric! Please check the input: " + value);
                }
            }
        }*/


        public ArticleEntity()
        {
        }

        public ArticleEntity(string number, double price, DateTime dateTime)
        {
            this.Number = number;
            this.Price = price;
            this.Currency = "EUR";
            this.DateTime = dateTime;
        }
    }
}
