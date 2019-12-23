using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using CrypSend.Repository;
using CrypSend.Library;
using Microsoft.Extensions.Configuration;
using CrypSend.Library.Settings;

[assembly: FunctionsStartup(typeof(CrypSend.Functions.Startup))]

namespace CrypSend.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ISettingsProvider, SettingsProvider>();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSingleton<IRepository<SecretPayload>, SecretPayloadRepository>();
            builder.Services.AddScoped<ICrypSendService, CrypSendService>();
            builder.Services.AddSingleton<IEncryptionEngineFactory, EncryptionEngineFactory>();
        }
    }
}
