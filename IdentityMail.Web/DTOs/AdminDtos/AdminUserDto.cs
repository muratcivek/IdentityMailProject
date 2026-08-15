namespace IdentityMail.Web.DTOs.AdminDtos
{
    public class AdminUserDto
    {
        public int Id { get; set; }

        public string FullName { get; set; }
            = string.Empty;

        public string Email { get; set; }
            = string.Empty;

        public string UserName { get; set; }
            = string.Empty;

        public bool IsActive { get; set; }

        public List<string> Roles { get; set; }
            = new();
    }
}