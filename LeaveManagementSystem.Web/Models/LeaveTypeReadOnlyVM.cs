namespace LeaveManagementSystem.Web.Models
{
    public class LeaveTypeReadOnlyVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Days { get; set; }
    }
}
