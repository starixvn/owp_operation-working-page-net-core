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

        private readonly IReadOnlyDictionary<string, int> ProviderHasTypeTV = new Dictionary<string, int>
        {
            { AutoConstant.ProviderBgLabelTV , AutoConstant.ProviderBgId},
            { AutoConstant.ProviderRb2LabelTV, AutoConstant.ProviderRb2Id},
            { AutoConstant.ProviderRtsLabelTV, AutoConstant.ProviderRtsId},
            { AutoConstant.ProviderBetLabelTV, AutoConstant.ProviderBetId}
        };

        public int[] DetectProviderType(string otherProvider)
        {
            var providerArray = otherProvider.Split("/");

            var providerLabel = GetSingleProvider(providerArray, ProviderHasTypeStadium);
            if (providerLabel == null)
            {
                providerLabel = GetSingleProvider(providerArray, ProviderHasTypeTV);
            }
            var providerTypes = ProviderHasTypeStadium.GetValueOrDefault(providerLabel ?? string.Empty, AutoConstant.ProviderOtherId);
            return new int[] { providerTypes };
        }

        private string GetSingleProvider<T>(string[] providerArray, T keyValuePairs)
            where T : IReadOnlyDictionary<string, int>
        {
            var keys = keyValuePairs.Keys.ToList();
            return providerArray.FirstOrDefault(source =>
                keys.Exists(dest =>
                    string.Equals(source, dest, StringComparison.OrdinalIgnoreCase)));
        }
    }
}