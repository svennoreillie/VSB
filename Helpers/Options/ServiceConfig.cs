namespace VSBaseAngular.Helpers.Options
{
    public class ServiceConfig
    {
        public ServiceBaseConfig[] Configs { get; set; }
    }

    public class ServiceBaseConfig {
        public string ServiceName { get; set; }
        public string ApplicationName { get; set; }
        public string ServiceUrl { get; set; }
        public BindingMode Binding { get; set; }
        public string BasicHttpSecurityMode { get; set; } 
        public ServiceConfigTransport Transport { get; set; }
    }

    public class ServiceConfigTransport {
        public string ClientCredentialsType { get; set; }
        public string ProxyCredentialsType { get; set; }
    }

    public enum BindingMode {
        BasicHttp = 0,
        BasicHttps = 1,
    }
}
