namespace TestABPApp.Services.Registration
{
    public interface IRegistrationDeviceTokenService
    {
        bool IsRegistered(int deviceToken);
        void RegistrationUser(int deviceToken);

        DateTime GetDateTimeRegistered(int deviceToken);
        
    }
}
