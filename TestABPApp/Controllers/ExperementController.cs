using Azure;
using Microsoft.AspNetCore.Mvc;
using TestABPApp.Models.AppModel;
using TestABPApp.Models.DBModel;
using TestABPApp.Services.Experements;
using TestABPApp.Services.Experements.Imple;
using TestABPApp.Services.Registration;

namespace TestABPApp.Controllers
{
    public class ExperementController : Controller
    {
        private readonly IRegistrationDeviceTokenService registrationDeviceTokenService;
        private readonly IExperementService experementService;
        private readonly AppDBContext appDBContext;
        private readonly IConfiguration configuration;

        public ExperementController(IRegistrationDeviceTokenService registrationDeviceTokenService, 
            IExperementService experementService, 
            AppDBContext appDBContext, IConfiguration configuration)
        {
            this.registrationDeviceTokenService = registrationDeviceTokenService;
            this.experementService = experementService;
            this.appDBContext = appDBContext;
            this.configuration = configuration;

        }

        // Action method to get button color based on experiment settings and device token
        public IActionResult ButtonCollor([FromQuery(Name = "device_token")] int deviceToken)
        {
            return this.GenereateResponce(() =>
            {
                string nameExperement = this.configuration["Experement1:Key"];
                string defaultValue = this.configuration["Experement1:DefaultValue"];
                string value = this.GetExperementValue(nameExperement, deviceToken, defaultValue);
                ExperementModel responce = new ExperementModel() { Key = nameExperement, Value = value };
                return responce;
            });
        }

        // Action method to get price based on experiment settings and device token
        public IActionResult Price([FromQuery(Name = "device_token")] int deviceToken)
        {
            return this.GenereateResponce( () =>
            {
                string nameExperement = this.configuration["Experement2:Key"];
                string defaultValue = this.configuration["Experement2:DefaultValue"];
                string value = this.GetExperementValue(nameExperement, deviceToken, defaultValue);
                ExperementModel responce = new ExperementModel() { Key = nameExperement, Value = value };
                return responce;
            });
        }

        // Method to get experiment statistics
        public ExperementStatsModel[] GetExperementStats()
        {
            return this.experementService.GetExperementStats();
        }

        // Private method to generate response for experiment related API request
        private IActionResult GenereateResponce(Func<ExperementModel> func)
        {
            try
            {
                return Ok(func());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Exeption = "The device token is not correct" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exeption = "Something went wrong" });
            }
        }

        // Private method to get experiment value based on experiment name, device token and default value
        private string GetExperementValue(string nameExperement, int deviceToken, string defaultValue)
        {
            if (deviceToken == 0)
            {
                throw new ArgumentException("deviceToken cannot be zero");
            }
            if (!this.registrationDeviceTokenService.IsRegistered(deviceToken))
            {
                this.registrationDeviceTokenService.RegistrationUser(deviceToken);
            }

            if (!this.experementService.IsDeviceTokenInExperement(deviceToken, nameExperement))
            {
                string groupAB = this.experementService.GenerateABGroup(nameExperement);
                this.experementService.AddRecord(deviceToken, groupAB, nameExperement);
            }

            return this.experementService.GetValue(deviceToken, nameExperement, defaultValue);
        }
    }
}
