using System;
using System.Diagnostics.CodeAnalysis;
using Amazon.Runtime;
using Amazon.SQS;
using CP.Pagamentos.CrpssCutting.Configuration;

namespace CP.Pagamentos.CrossCutting.Factories;

[ExcludeFromCodeCoverage]
public static class AwsCredentialsFactory
{
    public static BasicAWSCredentials CreateCredentials(this ClientConfig config, AWSConfiguration configuration)
    {
        var credentials = new BasicAWSCredentials(configuration.AccessKey, configuration.SecretKey);       

        if (!string.IsNullOrEmpty(configuration.ServiceUrl))
            config.ServiceURL = configuration.ServiceUrl;

        return credentials;
    }
}
