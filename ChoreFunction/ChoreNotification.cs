using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ChoreFunction.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ChoreFunction
{
    public static class ChoreNotification
    {
        [FunctionName("ChoreNotification")]
        public static void Run([TimerTrigger("0 22 * * *"
            #if DEBUG
            , RunOnStartup = true
            #endif
            )]TimerInfo myTimer, ILogger log)
        {

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var chores = GetChoresDueToday();

            var content = BuildNotificationContent(chores);

            SendEmail(BuildNotificationContent(chores));
            SendWhatsapp(BuildNotificationContent(chores, false));
        }

        private static string BuildNotificationContent(List<DTOChores> chores, bool html = true)
        {
            StringBuilder sb = new StringBuilder();

            if (!html)
            {
                sb.Append("The below chores are due today");

                sb.Append(Environment.NewLine);

                foreach (var item in chores)
                {
                    sb.Append(Environment.NewLine);

                    sb.Append(" - " + item.ChoreType.Name);
                }

                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);

                sb.Append("Go to https://choreweb.azurewebsites.net/ for more info");

                return sb.ToString();
            }

            sb.Append("<h2>The below chores are due today</h2>");

            sb.Append("<ul>");

            foreach (var item in chores)
            {
                sb.Append("<li>");

                sb.Append(item.ChoreType.Name);

                sb.Append("</li>");
            }

            sb.Append("</ul>");

            sb.Append("<h3>");

            sb.Append("Go to https://choreweb.azurewebsites.net/ for more info");

            sb.Append("</h3>");

            return sb.ToString();
        }

        public static List<DTOChores> GetChoresDueToday()
        {
            List<DTOChores> retVal = new List<DTOChores>();

            using (var client = new HttpClient())
            {
                //TODO Add to config
                client.BaseAddress = new Uri("https://choreapi.azurewebsites.net/");

                HttpResponseMessage response = client.GetAsync($"api/Chore/GetChoresbyDueDate/{GetAusDate().ToString("yyyy-MM-dd")}").Result;

                if (response.IsSuccessStatusCode)
                {
                    retVal = response.Content.ReadAsAsync<List<DTOChores>>().Result;
                }
                else
                {
                    //TODO Handle error
                }
            }

            return retVal;
        }

        public static void SendEmail(string content)
        {
            //TODO: Get from app settings
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("notifications@choretracker.com", "Chore Tracker"),
                Subject = "Todays Chores.",
                HtmlContent = "<strong>" + content + "</strong>"
            };
            msg.AddTo(new EmailAddress("alex_901@hotmail.co.uk", "Alex"));
            //msg.AddTo(new EmailAddress("laura_mackay1@hotmail.com", "Laura"));
            //msg.AddTo(new EmailAddress("laura_mackay1@hotmail.co.uk", "Laura"));
            var response = client.SendEmailAsync(msg);
            response.Wait();
        }

        public static void SendWhatsapp(string content)
        {
            //TODO: Get from app settings
            TwilioClient.Init(
               "ACbd2ded232650fe8b256e968fc3bd7012",
               "37dbefe1c4bd68765e8ecccdf7ee6394"
           );

            MessageResource.Create(
                           from: new PhoneNumber("+61485866727"),
                           to: new PhoneNumber("+61477418689"),
                           body: content
                       );

             MessageResource.Create(
               from: new PhoneNumber("+61485866727"),
               to: new PhoneNumber("+61487468224"),
               body: content
           );
        }
        private static DateTime GetAusDate()
        {
            TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeInfo);
        }
    }
}
