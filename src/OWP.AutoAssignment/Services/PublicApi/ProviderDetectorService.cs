using OWP.AutoAssignment.Constant;
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
        private readonly IReadOnlyDictionary<string, int> ProviderHasTypeStadium = new Dictionary<string, int>
        {
            { AutoConstant.ProviderBgLabel , AutoConstant.ProviderBgId},
            { AutoConstant.ProviderRb2Label, AutoConstant.ProviderRb2Id},
            { AutoConstant.ProviderRtsLabel, AutoConstant.ProviderRtsId},
            { AutoConstant.ProviderBetLabel, AutoConstant.ProviderBetId}
        };

        public int[] DetectProviderType(string otherProvider)
        {
            var providerArray = otherProvider.Split("/");

            var providerLabel = GetSingleProviderLabel(providerArray);
            var providerTypes = ProviderHasTypeStadium.GetValueOrDefault(providerLabel ?? string.Empty, AutoConstant.ProviderOtherId);
            return new int[] { providerTypes };
        }

        private string GetSingleProviderLabel(string[] providerArray)
        {
            var providerHasTypeStadiumKeys = ProviderHasTypeStadium.Keys.ToList();
            return providerArray.FirstOrDefault(source =>
            providerHasTypeStadiumKeys.Exists(dest =>
                string.Equals(source, dest, StringComparison.OrdinalIgnoreCase)));
        }
    }
}