using TestABPApp.Models.AppModel;

namespace TestABPApp.Services.Experements
{
    public interface IExperementService
    {
        public string GetValue(int deviceToken, string nameExperement, string defaultABValue);
        public DateTime GetDateTimeExperementStart(string nameExperement);
        public string GenerateABGroup(string nameExperement);
        public void AddRecord(int deviceToken, string groupAB, string nameExperement);
        public bool IsDeviceTokenInExperement(int deviceToken, string nameExperement);
        public ExperementStatsModel[] GetExperementStats();

    }
}
