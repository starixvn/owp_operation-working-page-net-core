namespace OWP.AutoAssignment.Services.InternalApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Builder;
    using Constant;
    using Models.InternalApi;


    public interface ProviderService
    {
        string Build(ProviderInfo provider, string pcCode);
    }

    public class ProviderServiceImp : ProviderService
    {

        public string Build(ProviderInfo provider, string pcCode)
        {
            var reporter = GetReporter(provider.Provider);
            var firstProvider = FirstProviderBuilder.GetFirstProvider(provider.OtherProvider);
            var providerItems = new List<string>
            {
                provider.ProviderTv,
                firstProvider,
                reporter,
                pcCode
            };

            return string.Join("/", providerItems.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        private static string GetReporter(string provider)
        {
            if (provider.Contains(AutoConstant.Woo, StringComparison.OrdinalIgnoreCase))
                return AutoConstant.ReporterWoo;
            if (provider.Contains(AutoConstant.Al, StringComparison.OrdinalIgnoreCase))
                return AutoConstant.ReporterAl;
            if (provider.Contains(AutoConstant.Slr, StringComparison.OrdinalIgnoreCase))
                return AutoConstant.ReporterSlr;

            return string.Empty;
        }


    }
}
