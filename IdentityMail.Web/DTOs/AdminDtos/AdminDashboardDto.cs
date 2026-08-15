namespace IdentityMail.Web.DTOs.AdminDtos
{
    public class AdminDashboardDto
    {
        public int TotalUsers { get; set; }

        public int ActiveUsers { get; set; }

        public int TotalMessages { get; set; }

        public int TodayMessages { get; set; }

        public int UnreadMessages { get; set; }

        public int TrashMessages { get; set; }

        public List<TopSenderDto> TopSenders { get; set; }
            = new();

        public List<CategoryStatisticDto> CategoryStatistics { get; set; }
            = new();
    }


    public class TopSenderDto
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int MessageCount { get; set; }
    }


    public class CategoryStatisticDto
    {
        public string CategoryName { get; set; } = string.Empty;

        public int MessageCount { get; set; }

        public double Percentage { get; set; }
    }
}