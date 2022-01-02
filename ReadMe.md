# Demo: Add AppSettings To Dependency Injection

Demo to add app settings to dependency injection in a console app.

### Problem

It is difficult to add app settings to dependency injection in a .NET console app.

### Solution

Create a generic `AddAppSettings` extension method that can be called in `ConfigureServices` to bind a section in appsettings.json to `IConfiguration` using a strongly typed class.

1. Add an `appsettings.json` file to a console app.
   - Set build action to Content, Copy to Output if newer.

```json
{
  "MyAppSettings": {
    "StringSetting": "Hello App Settings",
    "IntSetting": 42,
    "BoolSetting": true
  }
}
```

2. Create a class with a name that matches the settings section.

```csharp
public class MyAppSettings
{
    public string StringSetting { get; set; }
    public int IntSetting { get; set; }
    public bool BoolSetting { get; set; }
}
```

3. Add a `ServiceCollectionExtensions` class with a generic `AddAppSettings` extension method.

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppSettings<TSettings>(this IServiceCollection services, IConfiguration config)
        where TSettings : class
    {
        services.Configure<TSettings>(
            config.GetSection(typeof(TSettings).Name));
        services.AddTransient(sp =>
            sp.GetRequiredService<IOptions<TSettings>>().Value);
        return services;
    }
}
```

4. In Program.cs call `Host.CreateDefaultBuilder`, followed by `.ConfigureServices` in which you call `services.AddAppSettings`, passing `IConfiguration`.

```csharp
var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var config = services.BuildServiceProvider()
            .GetRequiredService<IConfiguration>();
        services.AddAppSettings<MyAppSettings>(config);
    })
    .Build();
```

5. Call `host.Services.GetRequiredService` to get the settings and use them.

```csharp
var settings = host.Services.GetRequiredService<MyAppSettings>();

Console.WriteLine("\nMy App Settings:");
Console.WriteLine($"String Setting: {settings.StringSetting}");
Console.WriteLine($"Int Setting: {settings.IntSetting}");
Console.WriteLine($"Bool Setting: {settings.BoolSetting}");
```
