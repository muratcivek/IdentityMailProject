namespace IdentityMail.Web.DTOs.AdminDtos
{
    public class RoleManagementDto
    {
        public List<RoleDto> Roles { get; set; } = new();

        public List<UserRoleDto> Users { get; set; } = new();
    }

    public class RoleDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }

    public class UserRoleDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new();
    }
}