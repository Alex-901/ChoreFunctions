using ChoreFunction;
using ChoreFunction.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Startup))]
namespace ChoreFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<ConfigurationItems>().Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("ConfigurationItems").Bind(settings);
            });

            Console.WriteLine("Host started!!!!!!!!");
        }
    }
}
