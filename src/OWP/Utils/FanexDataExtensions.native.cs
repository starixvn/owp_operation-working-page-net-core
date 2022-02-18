namespace OWP.Utils
{
    
    using Fanex.Data.Repository;
    using Newtonsoft.Json;

    public static class FanexDataExtensions
    {
        public static void ThrowExceptionIfInvalid(this ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                return;
            }

            var message = $"Invalid criteria: {JsonConvert.SerializeObject(criteria)}";

            throw new ArgumentException(message);
        }
    }
}
