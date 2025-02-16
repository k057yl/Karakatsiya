using Microsoft.Extensions.Localization;
using System.Reflection;

namespace Karakatsiya.Localizations
{
    public class SharedLocalizationService
    {
        private readonly IStringLocalizerFactory _factory;

        public SharedLocalizationService(IStringLocalizerFactory factory)
        {
            _factory = factory;
        }

        public IStringLocalizer GetLocalizer<T>() where T : class
        {
            var type = typeof(T);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            return _factory.Create(type.Name, assemblyName.Name);
        }

        public IStringLocalizer Buttons => GetLocalizer<ButtonsLocalizer>();
        public IStringLocalizer Messages => GetLocalizer<MessagesLocalizer>();
        public IStringLocalizer WarningMessages => GetLocalizer<WarningMessagesLocalizer>();
        /*
        public IStringLocalizer Tables => GetLocalizer<TableResources>();
        public IStringLocalizer Pages => GetLocalizer<PageResources>();
        public IStringLocalizer Categories => GetLocalizer<CategoriesResources>();
        */
    }
}
