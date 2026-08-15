using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace IdentityMail.Web.Helpers
{
    public static class EnumHelper
    {
        public static string GetDisplayName(this Enum value)
        {
            var member = value
                .GetType()
                .GetMember(value.ToString())
                .FirstOrDefault();

            if (member == null)
                return value.ToString();

            var displayAttribute = member
                .GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.Name
                   ?? value.ToString();
        }
    }
}
