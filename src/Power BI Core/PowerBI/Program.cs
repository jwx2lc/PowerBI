using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PowerBI.Services.Key_Vault;

namespace PowerBI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                if (1==1)//context.HostingEnvironment.IsDevelopment())
                {
                    var builtConfig = config.Build();

                    var prefixSection = builtConfig.GetSection("AzureKeyVaultConfig:Prefix");

                    var prefixes = new List<string>();

                    foreach(var prefix in prefixSection.GetChildren())
                    {
                        prefixes.Add(prefix.Value);
                    }

                    config.AddAzureKeyVault(
                        $"https://{builtConfig["AzureKeyVaultConfig:VaultName"]}.vault.azure.net/",
                        builtConfig["AzureKeyVaultConfig:ClientId"],
                        builtConfig["AzureKeyVaultConfig:ClientKey"],
                        new PrefixKeyVaultSecretManager(prefixes));
                }
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}
