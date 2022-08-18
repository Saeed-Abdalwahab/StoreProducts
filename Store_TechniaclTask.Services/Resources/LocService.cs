using Microsoft.Extensions.Localization;
using System.Reflection;

namespace Store_TechniaclTask.Services.Resources
{
    public class LocService
    {
        private readonly IStringLocalizer _localizer;
        public LocService(IStringLocalizerFactory factory)
        {
            //var type = typeof(SharedResource);
            //var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            //_localizer = factory.Create("SharedResource", assemblyName.Name);
                      _localizer = factory.Create("", "");
        }

        public LocalizedString GetLocalizedHtmlString(string key)
        {
            var text = _localizer[key];
            return text;
        }
    }
}