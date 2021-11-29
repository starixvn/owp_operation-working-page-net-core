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
            var providerModel = new List<ProviderModel>
            {
                BuildModel((int) Provider.Ipsd,
                    otherProvider.IndexOf("ipsd", StringComparison.CurrentCultureIgnoreCase)),
                BuildModel((int) Provider.Ipsd,
                    otherProvider.IndexOf("bg", StringComparison.CurrentCultureIgnoreCase)),
                BuildModel((int) Provider.Rb2,
                    otherProvider.IndexOf("rb2", StringComparison.CurrentCultureIgnoreCase)),
                BuildModel((int) Provider.Rts,
                    otherProvider.IndexOf("rts", StringComparison.CurrentCultureIgnoreCase)),
                BuildModel((int) Provider.Bet,
                    otherProvider.IndexOf("bet", StringComparison.CurrentCultureIgnoreCase))
            };

            var orderProvider = providerModel
                .Where(p => p.IndexWithinString > -1)
                .OrderBy(p => p.IndexWithinString)
                .Select(p => p.ProviderId)
                .Distinct()
                .ToList();
            if (!orderProvider.Any() && otherProvider.Length > 0)
            {
                orderProvider.Add((int)Provider.Other);
            }
            return orderProvider.ToArray();
        }

        private static ProviderModel BuildModel(int providerId, int indexWithinString) => new()
        {
            ProviderId = providerId,
            IndexWithinString = indexWithinString
        };
    }
}