using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MainProcessingService.Helpers
{
    public static class EnumHelper
    {
        public static T GetValueFromDescription<T>(string description)
        {
            foreach (var field in typeof(T).GetFields())
            {
                var descriptions = (DescriptionAttribute[])
                       field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (descriptions.Any(x => x.Description == description))
                {
                    return (T)field.GetValue(null);
                }
            }
            throw new Exception("Description not found");
        }
    }
}
