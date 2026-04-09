using System.Collections.Generic;

namespace DummyPos.Models
{
    public class StaffSearchViewModel
    {
        // Just ONE search string now!
        public string SearchTerm { get; set; }
        public List<StaffResult> StaffList { get; set; } = new List<StaffResult>();
    }

    public class StaffResult
    {
        public int Staff_Id { get; set; }
        public string Staff_Name { get; set; }
        public string Username { get; set; }
        public decimal Salary { get; set; }
        public string Branch_Name { get; set; }
        public string Role_Desc { get; set; }
        public string Phone { get; set; }
        public bool Is_Active { get; set; }
    }
}