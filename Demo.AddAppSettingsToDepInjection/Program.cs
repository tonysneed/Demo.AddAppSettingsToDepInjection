// See https://aka.ms/new-console-template for more information

using Demo.AddAppSettingsToDepInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Add App Settings to Dependency Injection Demo");

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var config = services.BuildServiceProvider()
            .GetRequiredService<IConfiguration>();
        services.AddAppSettings<MyAppSettings>(config);
    })
    .Build();

var settings = host.Services.GetRequiredService<MyAppSettings>();

Console.WriteLine("\nMy App Settings:");
Console.WriteLine($"String Setting: {settings.StringSetting}");
Console.WriteLine($"Int Setting: {settings.IntSetting}");
Console.WriteLine($"Bool Setting: {settings.BoolSetting}");
