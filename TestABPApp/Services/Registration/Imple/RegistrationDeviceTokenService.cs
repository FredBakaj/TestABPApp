using Microsoft.EntityFrameworkCore;
using TestABPApp.Models.DBModel;

namespace TestABPApp.Services.Registration.Imple
{
    public class RegistrationDeviceTokenService : IRegistrationDeviceTokenService
    {
        private readonly AppDBContext db;

        public RegistrationDeviceTokenService(AppDBContext db)
        {
            this.db = db;
        }

        public bool IsRegistered(int deviceToken)
        {
            bool exists = this.db.Users.Any(u => u.DeviceToken == deviceToken);
            return exists;

        }

        public void RegistrationUser(int deviceToken)
        {
            var user = new User() { DeviceToken = deviceToken, DateRegistration = DateTime.Now };
            this.db.Users.Add(user);
            this.db.SaveChanges();
        }

        public DateTime GetDateTimeRegistered(int deviceToken)
        {
            DateTime registrationDateTime = this.db.Users
            .Where(u => u.DeviceToken == deviceToken)
            .Select(u => u.DateRegistration)
            .FirstOrDefault();

            return registrationDateTime;
        }
    }
}
