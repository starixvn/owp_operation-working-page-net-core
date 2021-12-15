namespace OWP.AutoAssignment.Builder
{
    using System;
    using System.Linq;
    using Constant;

    public static class FirstProviderBuilder
    {
        public static string GetFirstProvider(string otherProvider)
        {
            var providers = otherProvider
                .Split("/")
                .Where(IsProvider).ToList();

            var providerStadium = providers
                .Intersect(AutoConstant.ProviderType)
                .FirstOrDefault();

            return providerStadium ?? providers.FirstOrDefault();
        }

        private static bool IsProvider(string provider)
            => AutoConstant.ProviderType
                .Any(i => provider.Contains(i, StringComparison.OrdinalIgnoreCase));
    }
}
