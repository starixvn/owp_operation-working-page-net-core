using OWP.LiveGameList.Models;

namespace OWP.LiveGameList.Builder;

public static class IPTVBuilder
{
    public static string Build(IPTVModel iPTVModel)
    {
        var ipTv = new List<string>();

        if (!string.IsNullOrWhiteSpace(iPTVModel.Television))
        {
            ipTv.Add(iPTVModel.Television);
        }

        if (!string.IsNullOrWhiteSpace(iPTVModel.IpTv))
        {
            ipTv.Add(iPTVModel.IpTv);
        }

        if (!string.IsNullOrWhiteSpace(iPTVModel.Channel))
        {
            ipTv.Add(iPTVModel.Channel);
        }

        if (!string.IsNullOrWhiteSpace(iPTVModel.Stream))
        {
            ipTv.Add(iPTVModel.Stream);
        }

        return string.Join("/", ipTv);
    }
}