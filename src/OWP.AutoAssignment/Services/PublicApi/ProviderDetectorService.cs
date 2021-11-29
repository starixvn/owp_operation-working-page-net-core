using OWP.AutoAssignment.Enum;
using OWP.AutoAssignment.Models.PublicApi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OWP.AutoAssignment.Services.PublicApi
{
    public interface ProviderDetectorService
    {
        int[] DetectProviderType(string otherProvider);
    }

    public class ProviderDetectorServiceImp : ProviderDetectorService
    {
        public int[] DetectProviderType(string otherProvider)
        {
            var providerModels = BuildListProviderModel(otherProvider);

            var providerTypes = SelectProviderType(providerModels);

            AddTypeOtherInNeed(providerTypes, otherProvider);

            return providerTypes.ToArray();
        }

        private void AddTypeOtherInNeed(IList<int> providerTypes, string otherProvider)
        {
            if (!providerTypes.Any() && otherProvider.Length > 0)
            {
                providerTypes.Add((int)Provider.Other);
            }
        }

        private IList<int> SelectProviderType(IList<ProviderModel> providerModels)
            => providerModels
                .Where(p => p.IndexWithinString > -1)
                .OrderBy(p => p.IndexWithinString)
                .Select(p => p.ProviderId)
                .Distinct()
                .ToList();

        private IList<ProviderModel> BuildListProviderModel(string otherProvider)
            => new List<ProviderModel>
            {
                BuildSingleProviderModel((int) Provider.Ipsd,
                    otherProvider.IndexOf("ipsd", StringComparison.CurrentCultureIgnoreCase)),
                BuildSingleProviderModel((int) Provider.Ipsd,
                    otherProvider.IndexOf("bg", StringComparison.CurrentCultureIgnoreCase)),
                BuildSingleProviderModel((int) Provider.Rb2,
                    otherProvider.IndexOf("rb2", StringComparison.CurrentCultureIgnoreCase)),
                BuildSingleProviderModel((int) Provider.Rts,
                    otherProvider.IndexOf("rts", StringComparison.CurrentCultureIgnoreCase)),
                BuildSingleProviderModel((int) Provider.Bet,
                    otherProvider.IndexOf("bet", StringComparison.CurrentCultureIgnoreCase))
            };

        private static ProviderModel BuildSingleProviderModel(int providerId, int indexWithinString) => new()
        {
            ProviderId = providerId,
            IndexWithinString = indexWithinString
        };
    }
}