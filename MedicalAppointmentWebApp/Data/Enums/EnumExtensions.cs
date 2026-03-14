using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MedicalAppointmentWebApp.Data.Enums
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var member = value.GetType().GetMember(value.ToString());
            var attribute = member[0].GetCustomAttribute<DisplayAttribute>();

            return attribute?.Name ?? value.ToString();
        }
    }
}