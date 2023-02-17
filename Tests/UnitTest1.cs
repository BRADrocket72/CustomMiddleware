using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
namespace Middleware.Tests;
public class MyAuthTests
{
    [Fact]
    public async Task When_NoParams_Should_NotAuthorize()
    {
        using var host = await new HostBuilder()
        .ConfigureWebHost(webBuilder =>
        {
            webBuilder
    .UseTestServer()
    .ConfigureServices(services =>
    {
    })
    .Configure(app =>
    {
        app.UseMiddleware<AuthMiddleWare>();
    });
        })
        .StartAsync();
        var response = await host.GetTestClient().GetAsync("/");
        Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Not Authorized", result);
    }

    [Fact]
    public async Task When_UsernameIsNotWrong_Should_NotAuth()
    {
        using var host = await new HostBuilder()
        .ConfigureWebHost(webBuilder =>
        {
            webBuilder
    .UseTestServer()
    .ConfigureServices(services =>
    {
    })
    .Configure(app =>
    {
        app.UseMiddleware<AuthMiddleWare>();
    });
        })
        .StartAsync();
        var response = await host.GetTestClient().GetAsync("/?username=wrong&password=password1");
        Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Not Authorized", result);
    }

    [Fact]
    public async Task When_PasswordIsNotWrong_Should_NotAuth()
    {
        using var host = await new HostBuilder()
        .ConfigureWebHost(webBuilder =>
        {
            webBuilder
    .UseTestServer()
    .ConfigureServices(services =>
    {
    })
    .Configure(app =>
    {
        app.UseMiddleware<AuthMiddleWare>();
    });
        })
        .StartAsync();
        var response = await host.GetTestClient().GetAsync("/?username=user1&password=wrongpass");
        Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Not Authorized", result);
    }

    [Fact]
    public async Task When_UserAuthIsCorrect_Should_Auth()
    {
        using var host = await new HostBuilder()
        .ConfigureWebHost(webBuilder =>
        {
            webBuilder
    .UseTestServer()
    .ConfigureServices(services =>
    {
    })
    .Configure(app =>
    {
        app.UseMiddleware<AuthMiddleWare>();
        app.Run(async context =>
{
    await context.Response.WriteAsync("Authenticated!");
});
    });
        })
        .StartAsync();
        var response = await host.GetTestClient().GetAsync("/?username=user1&password=password1");
        Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Authenticated!", result);
    }

}