using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestABPApp.Models.DBModel
{
    public class User
    {
        public int DeviceToken { get; set; }
        public DateTime DateRegistration { get; set; }
    }
}
