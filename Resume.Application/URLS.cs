namespace Resume.Application
{
    public static class URLS
    {
        public static string Server => "http://localhost:5023/";
        public static string HubLiveMatchEndpoint => $"{Server}{HubLiveMatchEnpointNameOnly_NotUrl}";
        public static string HubLiveMatchEnpointNameOnly_NotUrl => "livematch";

        public static string WebApp => "https://google.com";
    }
}
