namespace VSBaseAngular.Helpers.Options
{
    public class ApiConfig
    {
        public Config[] Configs { get; set; }
    }

    public class Config
    {
        public string ApplicationName { get; set; }
        public string Url { get; set; }
    }

}
