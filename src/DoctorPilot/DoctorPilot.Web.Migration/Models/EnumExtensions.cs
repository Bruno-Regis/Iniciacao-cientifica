using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DoctorPilot.Web.Migration.Models
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            if (member is null) return value.ToString();

            var displayAttr = member.GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.GetName() ?? value.ToString();
        }
    }
}
