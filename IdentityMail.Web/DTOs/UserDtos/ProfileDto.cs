namespace IdentityMail.Web.DTOs.UserDtos
{
    public class ProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? ProfileImageUrl { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }
}
