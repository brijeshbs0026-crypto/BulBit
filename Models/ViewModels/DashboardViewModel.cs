using AuthMvcApp.Models;

namespace AuthMvcApp.ViewModels
{
    public class DashboardViewModel
    {
        public string UserName { get; set; } = "User";

        public int TotalEmployees { get; set; }

        public int ActiveTasks { get; set; }

        public int PresentToday { get; set; }

        public double AttendancePercentage { get; set; }

        public List<Employee> Employees { get; set; } = new();

        public int TotalTasks { get; set; }

        public int CompletedTasks { get; set; }

        public int InProgressTasks { get; set; }
    }
}