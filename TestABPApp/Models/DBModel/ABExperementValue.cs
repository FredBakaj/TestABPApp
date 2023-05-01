namespace TestABPApp.Models.DBModel
{
    public class ABExperimentValue
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string GroupName { get; set; }
        public float СoefficientPerformance { get; set; }
        public int ABExperimentId { get; set; }
        public ABExperiment ABExperiment { get; set; }
    }

}
