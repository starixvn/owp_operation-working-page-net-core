namespace OWP.Utils
{
    public static class TemplateIpTvChannelBuilder
    {
        public static string Build(string ipTv, string channel)
        {
            if (string.IsNullOrWhiteSpace(ipTv) && string.IsNullOrWhiteSpace(channel))
            {
                return string.Empty;
            }
            if (!string.IsNullOrWhiteSpace(ipTv) && !string.IsNullOrWhiteSpace(channel))
            {
                return $"{ipTv}/{channel}";
            }
            return !string.IsNullOrWhiteSpace(ipTv) ? ipTv : channel;
        }
    }
}
