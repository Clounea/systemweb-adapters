// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net.Http;
using Microsoft.AspNetCore.SystemWebAdapters;
using Microsoft.AspNetCore.SystemWebAdapters.SessionState;
using Microsoft.AspNetCore.SystemWebAdapters.SessionState.RemoteSession;

namespace Microsoft.Extensions.DependencyInjection;

public static class RemoteAppSessionStateExtensions
{
    public static ISystemWebAdapterRemoteClientAppBuilder AddSession(this ISystemWebAdapterRemoteClientAppBuilder builder, Action<RemoteAppSessionStateClientOptions>? configure = null)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Services.AddHttpClient(SessionConstants.SessionClientName)
            // Disable cookies in the HTTP client because the service will manage the cookie header directly
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { UseCookies = false });

        builder.Services.AddTransient<ISessionManager, RemoteAppSessionStateManager>();

        builder.Services.AddOptions<RemoteAppSessionStateClientOptions>()
            .Configure(configure ?? (_ => { }))
            .ValidateDataAnnotations();

        return builder;
    }
}
