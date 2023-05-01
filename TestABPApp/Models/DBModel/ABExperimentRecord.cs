namespace TestABPApp.Models.DBModel
{
    public class ABExperimentRecord
    {
        public int Id { get; set; }
        public int Action { get; set; }
        public int UserId { get; set; }
        public int ABExperimentId { get; set; }
        public int ABExperimentValueId { get; set; }
        public User User { get; set; }
        public ABExperiment ABExperiment { get; set; }
        public ABExperimentValue ABExperimentValue { get; set; }
    }

}
