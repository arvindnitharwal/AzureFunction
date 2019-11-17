using System;
using Assignment.Business;
using Assignment.Business.Interfaces;
using Assignment.DataAccess.Interfaces;
using Assignment.DataAccess;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly : FunctionsStartup (typeof (MyNamespace.Startup))]

namespace MyNamespace {
    public class Startup : FunctionsStartup {
        public override void Configure (IFunctionsHostBuilder builder) {
            builder.Services.AddSingleton<IRequestLogic, RequestLogic> ();
            builder.Services.AddSingleton<IAdmin, Admin> ();
            builder.Services.AddSingleton<IConnection, Connection> ();
            builder.Services.AddSingleton<ICallRequest, CallRequest> ();
        }
    }
}