namespace OWP.AutoAssignment.Services.PublicApi
{
    using System.Collections.Generic;
    using Builder;
    using OWP.AutoAssignment.Constant;

    public interface ProviderDetectorService
    {
        int[] DetectProviderType(string otherProvider);
    }

    public class ProviderDetectorServiceImp : ProviderDetectorService
    {
        private readonly IReadOnlyDictionary<string, int> providerHasType = new Dictionary<string, int>
        {
            { AutoConstant.ProviderBgLabel , AutoConstant.ProviderBgId},
            { AutoConstant.ProviderRb2Label, AutoConstant.ProviderRb2Id},
            { AutoConstant.ProviderRtsLabel, AutoConstant.ProviderRtsId},
            { AutoConstant.ProviderBetLabel, AutoConstant.ProviderBetId},
            { AutoConstant.ProviderBgLabelTV , AutoConstant.ProviderBgId},
            { AutoConstant.ProviderRb2LabelTV, AutoConstant.ProviderRb2Id},
            { AutoConstant.ProviderRtsLabelTV, AutoConstant.ProviderRtsId},
            { AutoConstant.ProviderBetLabelTV, AutoConstant.ProviderBetId}
        };

        public int[] DetectProviderType(string otherProvider)
        {
            var firstLabel = FirstProviderBuilder.GetFirstProvider(otherProvider)?.ToUpper();
            var providerTypes = providerHasType.GetValueOrDefault(firstLabel ?? string.Empty, AutoConstant.ProviderOtherId);
            return new [] { providerTypes };
        }

    }
}
