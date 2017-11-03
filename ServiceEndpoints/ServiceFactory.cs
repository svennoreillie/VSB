using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.Extensions.Options;
using VSBaseAngular.Helpers.Options;

public class ServiceFactory<T> : IServiceFactory<T> where T : class
{
    private readonly IOptions<ServiceConfig> options;

    public ServiceFactory(IOptions<ServiceConfig> options)
    {
        this.options = options;
    }

    public T GetService()
    {
        ServiceBaseConfig config = this.GetOptionBlock();

        EndpointAddress endpointAddress = new EndpointAddress(config.ServiceUrl);

        Binding binding;
        HttpClientCredentialType clientType = HttpClientCredentialType.Basic;
        HttpProxyCredentialType proxyType =  HttpProxyCredentialType.Basic;

        if (config.Transport != null)
        {
            Enum.TryParse<HttpClientCredentialType>(config.Transport?.ClientCredentialsType, out clientType);
            Enum.TryParse<HttpProxyCredentialType>(config.Transport?.ProxyCredentialsType, out proxyType);
        }

        switch (config.Binding)
        {
            case BindingMode.BasicHttp:
                BasicHttpSecurityMode basicHttpSecurityMode;
                if (!Enum.TryParse<BasicHttpSecurityMode>(config.BasicHttpSecurityMode.ToString(), out basicHttpSecurityMode))
                {
                    throw new Exception("SecurityMode not found");
                }
                var httpbinding = new BasicHttpBinding(basicHttpSecurityMode);

                if (config.Transport != null)
                {
                    httpbinding.Security.Transport.ClientCredentialType = clientType;
                    httpbinding.Security.Transport.ProxyCredentialType = proxyType;
                }
                binding = httpbinding;
                break;

            case BindingMode.BasicHttps:
                BasicHttpsSecurityMode basicHttpsSecurityMode;
                if (!Enum.TryParse<BasicHttpsSecurityMode>(config.BasicHttpSecurityMode.ToString(), out basicHttpsSecurityMode))
                {
                    throw new Exception("SecurityMode not found");
                }
                var httpsbinding = new BasicHttpsBinding(basicHttpsSecurityMode);
                if (config.Transport != null)
                {
                    httpsbinding.Security.Transport.ClientCredentialType = clientType;
                    httpsbinding.Security.Transport.ProxyCredentialType = proxyType;
                }
                binding = httpsbinding;
                break;

            default:
                throw new Exception("Binding mode not set");
        }

        var factory = new ChannelFactory<T>(binding, endpointAddress);
        var serviceProxy = factory.CreateChannel();

        return serviceProxy;
    }

    private ServiceBaseConfig GetOptionBlock()
    {
        Type genericType = typeof(T);
        string genericName = genericType.Name;

        Type serviceConfigType = this.options.Value.GetType();

        if (serviceConfigType.GetProperties().Any(p => p.Name == genericName))
        {
            object block = serviceConfigType.GetProperty(genericName).GetValue(this.options.Value);
            return block as ServiceBaseConfig;
        }
        else
        {
            throw new Exception($"No configuration object found for {genericName}");
        }
    }
}