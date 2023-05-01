using Microsoft.EntityFrameworkCore;
using TestABPApp.Models;
using TestABPApp.Models.AppModel;
using TestABPApp.Models.DBModel;
using TestABPApp.Services.Registration;

namespace TestABPApp.Services.Experements.Imple
{
    public class ExperementService : IExperementService
    {
        private readonly AppDBContext db;
        private readonly IRegistrationDeviceTokenService registrationDeviceTokenService;


        public ExperementService(AppDBContext db, IRegistrationDeviceTokenService registrationDeviceTokenService)
        {
            this.db = db;
            this.registrationDeviceTokenService = registrationDeviceTokenService;
        }

        public string GetValue(int deviceToken, string nameExperement, string defaultABValue)
        {
            var dateRegistered = this.registrationDeviceTokenService.GetDateTimeRegistered(deviceToken);
            var dateExperementStart = this.GetDateTimeExperementStart(nameExperement);
            if (dateRegistered > dateExperementStart)
            {
                string value = this.db.ABExperimentRecords
                .Where(r => r.UserId == deviceToken && r.ABExperiment.Name == nameExperement)
                .Select(r => r.ABExperimentValue.Value)
                .FirstOrDefault();

                return value;
            }
            else
            {
                return defaultABValue;
            }
        }

        public DateTime GetDateTimeExperementStart(string nameExperement)
        {
            DateTime dateStartExperement = this.db.ABExperiments
            .Where(u => u.Name == nameExperement)
            .Select(u => u.DateStart)
            .FirstOrDefault();
            return dateStartExperement;
        }

        public string GenerateABGroup(string nameExperement)
        {
            List<ABExperimentValue> experementValues = this.db.ABExperimentValues
                .Where(e => e.ABExperiment.Name == nameExperement)
                .ToList();

            var sumСoefficientPerformance = 0f;
            foreach (ABExperimentValue experementValue in experementValues)
            {
                sumСoefficientPerformance += experementValue.СoefficientPerformance;
            }
            if (Math.Abs(sumСoefficientPerformance - 1f) > 0.01)
            {
                throw new Exception("the sum of the coefficients of all displays must be equal to 1");
            }

            List<string> listGroups = new List<string>();
            List<double> listCoefficientPerformance = new List<double>();
            foreach(ABExperimentValue value in experementValues)
            {
                listGroups.Add(value.GroupName);
                listCoefficientPerformance.Add((double)value.СoefficientPerformance);
            }

            string randomGroup = SelectElementWithProbability(listGroups, listCoefficientPerformance);

            return randomGroup;
        }

        public void AddRecord(int deviceToken, string groupAB, string nameExperement)
        {
            
            int experementId = this.db.ABExperiments
                .Where(e => e.Name == nameExperement)
                .Select(r => r.Id)
                .FirstOrDefault();
            ABExperimentValue experimentValue = this.db.ABExperimentValues.Where(v => v.GroupName == groupAB).First();
            int valueId = experimentValue.Id;
            ABExperimentRecord newRecord = new ABExperimentRecord()
            {
                Action = 0,
                UserId = deviceToken,
                ABExperimentValueId = valueId,
                ABExperimentId = experementId
            };
            this.db.ABExperimentRecords.Add(newRecord);
            this.db.SaveChanges();
        }

        public bool IsDeviceTokenInExperement(int deviceToken, string nameExperement)
        {
            bool isDeviceTokenInExperement = this.db.ABExperimentRecords
                .Where(r => r.UserId == deviceToken && r.ABExperiment.Name == nameExperement)
                .Any();

            return isDeviceTokenInExperement;
        }

        public ExperementStatsModel[] GetExperementStats()
        {
            var aBExperiments = this.db.ABExperiments.ToList();
            var experementStatModels = new List<ExperementStatsModel>();
            foreach (var ex in aBExperiments)
            {                
                var experimentRecordsGroupedBy = this.db.ABExperimentRecords
                    .Include(record => record.ABExperimentValue)
                    .Where(record => record.ABExperimentId == ex.Id)
                    .GroupBy(record => record.ABExperimentValue.GroupName)
                    .Select(group => new {
                        GroupName = group.Key,
                        NumberImpressions = group.Count(),
                        NumberActions = group.Sum(record => record.Action)
                    })
                    .ToList();

                var listExperementStatItems = new List<ExperementStatsItemModel>();
                foreach (var experimentRecord in experimentRecordsGroupedBy)
                {
                    ExperementStatsItemModel experementStatItemModel = new ExperementStatsItemModel()
                    {
                        GroupName = experimentRecord.GroupName,
                        NumberActions = experimentRecord.NumberActions,
                        NumberImpressions = experimentRecord.NumberImpressions
                    };
                    listExperementStatItems.Add(experementStatItemModel);
                }
                experementStatModels.Add(new ExperementStatsModel()
                {
                    Key = ex.Name,
                    ExperementStatItemModels = listExperementStatItems.ToArray()
                });

            }
            return experementStatModels.ToArray();
        }

        private static T SelectElementWithProbability<T>(List<T> list, List<double> probabilities)
        {
            if (list.Count != probabilities.Count)
            {
                throw new ArgumentException("The list of elements and list of probabilities must be of equal size.");
            }

            double totalProbability = probabilities.Sum();
            double randomValue = new Random().NextDouble() * totalProbability;

            double cumulativeProbability = 0;
            for (int i = 0; i < list.Count; i++)
            {
                cumulativeProbability += probabilities[i];
                if (randomValue < cumulativeProbability)
                {
                    return list[i];
                }
            }

            return list[list.Count - 1];
        }

    }
}
