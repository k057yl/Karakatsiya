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

        public IStringLocalizer Messages => GetLocalizer<MessagesLocalizer>();
        public IStringLocalizer WarningMessages => GetLocalizer<WarningMessagesLocalizer>();
        public IStringLocalizer Categories => GetLocalizer<CategoriesLocalizer>();
        public IStringLocalizer Tables => GetLocalizer<TablesLocalizer>();
        public IStringLocalizer Currencies => GetLocalizer<CurrensiesLocalizer>();
        public IStringLocalizer Pages => GetLocalizer<PagesTextLocalizer>();
        public IStringLocalizer CreateItems => GetLocalizer<CreateItemPageLocalozation>();
        public IStringLocalizer SaleItems => GetLocalizer<SaleIPageLocalization>();
        //here is everything that relates to buttons/tables/messages/modals/greatings
        public IStringLocalizer Generals => GetLocalizer<GeneralLocalization>();
    }
}
