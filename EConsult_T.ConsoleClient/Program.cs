using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EConsult_T.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            DateModel DateRange = new DateModel();
            DateModel DateIntersect = new DateModel();

            CultureInfo MyCultureInfo = new CultureInfo("en-US");

        reload:

            string firstDate = "";
            string secondDate = "";

            ColorPrint green = new ColorPrint(ConsoleColor.Green);
            green.Print("Create dates interval:        enter 1");
            green.Print("Find dates intersection:      enter 2");
            green.Print("Get all dates from database:  enter 3");
            Console.WriteLine(new string('-', 37));

            int choice = Int32.Parse(Console.ReadLine());
            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    ColorPrint yellow = new ColorPrint(ConsoleColor.Yellow);

                    yellow.Print("Enter first date in format: year-month-day");
                    firstDate = Console.ReadLine();

                    yellow.Print("Enter second date in format: year-month-day");
                    secondDate = Console.ReadLine();

                    yellow.Print("Added to database!!!");

                    Console.WriteLine();

                    DateRange.startDate = DateTime.ParseExact(firstDate, "yyyy-MM-dd", MyCultureInfo);
                    DateRange.endDate = DateTime.ParseExact(secondDate, "yyyy-MM-dd", MyCultureInfo);

                    UploadAsync(DateRange);

                    goto reload;

                case 2:
                    ColorPrint cyan = new ColorPrint(ConsoleColor.Cyan);

                    cyan.Print("Enter first date in format: year-month-day");
                    firstDate = Console.ReadLine();

                    cyan.Print("Enter second date in format: year-month-day");
                    secondDate = Console.ReadLine();

                    Console.WriteLine();

                    DateIntersect.startDate = DateTime.ParseExact(firstDate, "yyyy-MM-dd", MyCultureInfo);
                    DateIntersect.endDate = DateTime.ParseExact(secondDate, "yyyy-MM-dd", MyCultureInfo);

                    cyan.Print("Date intervals witch intersect with input: ");

                    DownloadAsync(DateIntersect);

                    Thread.Sleep(4000);
                    Console.WriteLine();
                    goto reload;

                case 3:
                    Console.WriteLine("All dates intervals from database:");

                    DownloadAllDatesAsync();

                    Thread.Sleep(4000);
                    Console.WriteLine();
                    goto reload; ;

                default:
                    Console.WriteLine("Incorrect choice, try again");
                    goto reload;
            }
        }

        public static async Task UploadAsync(DateModel LoanDateRange)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49262/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(JsonConvert.SerializeObject(LoanDateRange), Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync("api/date/interval", content);
                }
                catch (Exception)
                { }
            }
        }

        public static async Task DownloadAsync(DateModel LoanDateRange)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49262/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(JsonConvert.SerializeObject(LoanDateRange), Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync("api/date/intersection", content);

                    var json = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<IEnumerable<DateModel>>(json);
                    foreach (var item in result)
                    {
                        Console.WriteLine("{0:yyyy-MM-dd} / {1:yyyy-MM-dd}", item.startDate, item.endDate);
                    }
                }
                catch (Exception)
                { }
            }
        }

        public static async Task DownloadAllDatesAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49262/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/date/all-dates");

                    var json = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<IEnumerable<DateModel>>(json);
                    foreach (var item in result)
                    {
                        Console.WriteLine("{0:yyyy-MM-dd} / {1:yyyy-MM-dd}", item.startDate, item.endDate);
                    }
                }
                catch (Exception)
                { }
            }
        }
    }
}
