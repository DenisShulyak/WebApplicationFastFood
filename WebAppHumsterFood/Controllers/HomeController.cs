using Microsoft.Net.Http.Headers;
using MimeKit;
using Newtonsoft.Json;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebAppHumsterFood.Models;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Json;

namespace WebAppHumsterFood.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            //Response.Cookies["countBasket"].Value;
            ViewBag.Message = "0";
            // задаем срок истечения срока действия cookie
            if (Request.Cookies["countBasket"] == null)
            {
                Response.Cookies["countBasket"].Expires = DateTime.Now.AddDays(2);
            }
            else
            {
                ViewBag.Message = Request.Cookies["countBasket"].Value.ToString();
            }

            string json;
            List<Order> orders = new List<Order>();
            if (Request.Cookies["Orders"] != null)
            {

                var inputBytes = Convert.FromBase64String(Request.Cookies["Orders"].Value);
                json = Encoding.GetEncoding("Windows-1251").GetString(inputBytes);

                // json = Request.Cookies["Orders"].Value.ToString();
                orders = JsonConvert.DeserializeObject<List<Order>>(json);
            }
            else if (Request.Cookies["Orders"] == null)
            {
                Response.Cookies["Orders"].Expires = DateTime.Now.AddDays(2);
            }

            ViewBag.Orders = orders;
            return View();
        }


        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}



        public ActionResult Basket()
        {
            // Подсчет что лежит в корзине на текущий момент
            string json;
            List<Order> orders = new List<Order>();
            if (Request.Cookies["Orders"] != null)
            {

                var inputBytes = Convert.FromBase64String(Request.Cookies["Orders"].Value);
                json = Encoding.GetEncoding("Windows-1251").GetString(inputBytes);

               // json = Request.Cookies["Orders"].Value.ToString();
                orders = JsonConvert.DeserializeObject<List<Order>>(json);
            }
            else if (Request.Cookies["Orders"] == null)
            {
                Response.Cookies["Orders"].Expires = DateTime.Now.AddDays(2);
            }

            ViewBag.Orders = orders;

            //foreach(var order in orders)
            //{
            //    var str = Encoding.UTF8.GetString(order.Name);
            //}

            ViewBag.Message = "0";
            // задаем срок истечения срока действия cookie
            if (Request.Cookies["countBasket"] == null)
            {
                Response.Cookies["countBasket"].Expires = DateTime.Now.AddDays(2);
            }
            else
            {
                ViewBag.Message = Request.Cookies["countBasket"].Value.ToString();
            }



            return View("Index");
        }
        
        public ActionResult SendToEmail(string name, string phone)
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("sendmailtest@inbox.ru", "Homyc");
            // кому отправляем
            MailAddress to = new MailAddress("Xomyaksushi@gmail.com");//Xomyaksushi@gmail.com
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = "Заказ";
            // текст письма

            string json = ""; 
            List<Order> orders = new List<Order>();
            if (Request.Cookies["Orders"] != null)
            {

                var inputBytes = Convert.FromBase64String(Request.Cookies["Orders"].Value);
                json = Encoding.GetEncoding("Windows-1251").GetString(inputBytes);

                // json = Request.Cookies["Orders"].Value.ToString();
                orders = JsonConvert.DeserializeObject<List<Order>>(json);
            }
            else if (Request.Cookies["Orders"] == null)
            {
                Response.Cookies["Orders"].Expires = DateTime.Now.AddDays(2);
            }
            int countPrice = 0;
            foreach(var order in orders)
            {
            m.Body += "Название: " + order.Name  + " | " + " Price: "+ order.Price + "\n";
                countPrice += order.Price;
            }

            m.Body += "Итого: " + countPrice + " тг";
            m.Body += "  Имя: " + name + "\n Телефон: " + phone;
            
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.inbox.ru", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("sendmailtest@inbox.ru", "test1234mail");
            smtp.EnableSsl = true;
            smtp.Send(m);

           
            Response.Cookies["Orders"].Expires = DateTime.Now.AddDays(2);




            
            return Redirect("/home");
        }
        public ActionResult AddOrderInBasket1(string nameFood) //Добавление заказа в корзину
        {
            //добавляем
            string value = null;
            if (Request.Cookies["countBasket"] != null)
            {
                value = Request.Cookies["countBasket"].Value.ToString();
            }
            // сначала нам требуется проверить на null наличие cookie
            var count = 0;
            if (value != "" && value != null)
            {
                count = int.Parse(value);
            }
            count++;
            Response.Cookies["countBasket"].Value = count.ToString();

            //Добавление в куки заказ
            string id = Guid.NewGuid().ToString();
            Order order = new Order();
            if (nameFood == "pepperoni")
            {
                order.Name = "Пепперони";
                order.Price = 1500;
            }
            else if (nameFood == "margarita")
            {
                order.Name = "Маргарита";
                order.Price = 1200;
            }
            else if (nameFood == "gribn")
            {
                order.Name = "Грибная";
                order.Price = 1500;
            }
            else if (nameFood == "filadelf")
            {
                order.Name = "Филадельфия (1 порция 10шт)";
                order.Price = 1800;
            }
            else if (nameFood == "losostemp")
            {
                order.Name = "Лосось темпура";
                order.Price = 1500;
            }
            else if (nameFood == "spasiroll")
            {
                order.Name = "Спайси ролл";
                order.Price = 1500;
            }
            else if (nameFood == "chizroll")
            {
                order.Name = "Чиз ролл";
                order.Price = 1500;
            }
            else if (nameFood == "segun")
            {
                order.Name = "Сегун";
                order.Price = 1300;
            }
            else if (nameFood == "fudziyama")
            {
                order.Name = "Фудзияма";
                order.Price = 1300;
            }
            else if (nameFood == "kappamaki")
            {
                order.Name = "Каппа маки";
                order.Price = 500;
            }
            else if (nameFood == "sakemaki")
            {
                order.Name = "Саке маки";
                order.Price = 600;
            }
            else if (nameFood == "claleforn")
            {
                order.Name = "Калифорния";
                order.Price = 1400;
            }
            else if (nameFood == "fre")
            {
                order.Name = "Фри 1 порция 250гр";
                order.Price = 500;
            }
            else if (nameFood == "canada")
            {
                order.Name = "Сет 'Канада' 40шт";
                order.Price = 4800;
            }
            else if (nameFood == "ninja")
            {
                order.Name = "Сет 'Ниндзя' 30шт";
                order.Price = 4300;
            }
            else if (nameFood == "paris")
            {
                order.Name = "Сет 'Париж' 40шт";
                order.Price = 4800;
            }
            else if (nameFood == "venecia")
            {
                order.Name = "Сет 'Венеция' 40шт";
                order.Price = 3800;
            }
            else if (nameFood == "maiami")
            {
                order.Name = "Сет 'Майами' 60шт";
                order.Price = 5800;
            }
            else if (nameFood == "madrid")
            {
                order.Name = "Сет 'Мадрид' 50шт";
                order.Price = 5800;
            }
            else if (nameFood == "vulcan")
            {
                order.Name = "Сет 'Вулкан' 90шт";
                order.Price = 6300;
            }
            else if (nameFood == "dragon")
            {
                order.Name = "Сет 'Сердце дракона' 90шт";
                order.Price = 6300;
            }
            else if (nameFood == "samurai")
            {
                order.Name = "Сет 'Искушение самурая' 110шт";
                order.Price = 6800;
            }
            else if (nameFood == "raiskoe")
            {
                order.Name = "Сет 'Райское наслаждение' 130шт";
                order.Price = 7800;
            }
            else if (nameFood == "imperator")
            {
                order.Name = "Сет 'Император' 130шт";
                order.Price = 8300;
            }
            else if (nameFood == "triumf")
            {
                order.Name = "Сет 'Триумф' 150шт";
                order.Price = 9000;
            }
            else if (nameFood == "astana")
            {
                order.Name = "Сет 'Астана' 30шт";
                order.Price = 4300;
            }
            else if (nameFood == "japan")
            {
                order.Name = "Сет 'Дары Японии' 40шт";
                order.Price = 5300;
            }
            else if (nameFood == "lotos")
            {
                order.Name = "Сет 'Цветок жизни' 50шт";
                order.Price = 5800;
            }
            else if (nameFood == "setfiladelf")
            {
                order.Name = "Сет 'Филадельфия' 40шт";
                order.Price = 5800;
            }
            string json;
            List<Order> orders = new List<Order>();
           
            if(orders == null)
            {
                orders.Add(order);

            }
            

            if (Request.Cookies["Orders"] != null)
            {
                var inputBytes = Convert.FromBase64String(Request.Cookies["Orders"].Value);
                 json = Encoding.GetEncoding("Windows-1251").GetString(inputBytes);


               // json = Request.Cookies["Orders"].Value.ToString();
                orders = JsonConvert.DeserializeObject<List<Order>>(json);
            }
            else if (Request.Cookies["Orders"] == null)
            {
                Response.Cookies["Orders"].Expires = DateTime.Now.AddDays(2);
            }

            if (orders == null)
            {
                List<Order> orders2 = new List<Order> {
               new Order{Name = order.Name, Price = order.Price } };
                string data = JsonConvert.SerializeObject(orders2);



                var bytes = Encoding.GetEncoding("Windows-1251").GetBytes(data);

                Response.Cookies["Orders"].Expires = DateTime.Now.AddDays(2);
                // Response.Cookies["Orders"].Value = data;
                Response.Cookies["Orders"].Value = Convert.ToBase64String(bytes);


            }
            else
            {
                orders.Add(order);
            string data = JsonConvert.SerializeObject(orders);


           
            var bytes = Encoding.GetEncoding("Windows-1251").GetBytes(data);

            Response.Cookies["Orders"].Expires = DateTime.Now.AddDays(2);
           // Response.Cookies["Orders"].Value = data;
            Response.Cookies["Orders"].Value = Convert.ToBase64String(bytes);
            
            }


            return Redirect("/home");
        }
    }
}