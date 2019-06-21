using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Google.Cloud.Dialogflow.V2;
using System.Text;

namespace Pillbox
{
    public static class GetPlilbox
    {
        [FunctionName("GetPillboxStatus")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] WebhookRequest request,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            //string name = req.Query["name"];
            
            var pas = request.QueryResult.Parameters;
            var askingName = pas.Fields.ContainsKey("name") && pas.Fields["name"].ToString().Replace('\"', ' ').Trim().Length > 0;
            var askingAddress = pas.Fields.ContainsKey("address") && pas.Fields["address"].ToString().Replace('\"', ' ').Trim().Length > 0;
            var askingBusinessHour = pas.Fields.ContainsKey("business-hours") && pas.Fields["business-hours"].ToString().Replace('\"', ' ').Trim().Length > 0;
            var response = new WebhookResponse();

            string name = "Jeffson Library", address = "1234 Brentwood Lane, Dallas, TX 12345", businessHour = "8:00 am to 8:00 pm";

            StringBuilder sb = new StringBuilder();

            if (askingName)
            {
                sb.Append("The name of library is: " + name + "; ");
            }

            if (askingAddress)
            {
                sb.Append("The Address of library is: " + address + "; ");
            }

            if (askingBusinessHour)
            {
                sb.Append("The Business Hour of library is: " + businessHour + "; ");
            }

            if (sb.Length == 0)
            {
                sb.Append("Greetings from our Webhook API!");
            }

            response.FulfillmentText = sb.ToString();


            if (response != null)
                return new JsonResult(response);
            else
                return new BadRequestObjectResult("There is no order stats infomration");
        }
    }
}
