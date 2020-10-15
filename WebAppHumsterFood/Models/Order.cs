using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAppHumsterFood.Models
{
    [Serializable]
    public class Order
    {
        public string Name { get; set; } //Название
       
        public int Price { get; set; } //Цена
        
    }
}