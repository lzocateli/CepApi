﻿
var builder = WebApplication.CreateBuilder(args);



Console.WriteLine(Environment.GetEnvironmentVariable("ENVTEST"));
Console.WriteLine($"**** Environment: {builder.Environment.EnvironmentName} ****");

var pathEnvFile = string.Empty;
if (args.NotNullOrZero())
{
    pathEnvFile = args.FirstOrDefault(x => x.StartsWith("--env="))?.Replace("--env=", "");
}
if (string.IsNullOrWhiteSpace(pathEnvFile))
{
    pathEnvFile = Path.Combine(
        builder.Environment.ContentRootPath, ".env");
}
CustomDotEnv.LoadDotEnv(pathEnvFile);


var configureProxy = new WebProxyConfigureMethod();
WebRequest.DefaultWebProxy = configureProxy.GetProxyWithVariable();
Console.WriteLine($"Proxy configured: {configureProxy.HttpProxyField} no proxy: {string.Join(",", configureProxy.HttpNoProxyField ?? new string[] { })}");

builder.AddVaultBuilder();
builder.AddArchitectures();

var app = builder.Build();

var testVault = builder.Configuration.GetSection("ApplicationInsights:ConnectionString")?.Value;
Console.WriteLine($"Get vault access is: {!string.IsNullOrWhiteSpace(testVault)}");
Console.WriteLine($"**** LogLevelDefault: {app.Configuration.GetSection("Logging:LogLevel:Default")?.Value} ****");


app.MapEndpoints();
app.UseArchitectures();




app.Run();


// Console.WriteLine(Environment.GetEnvironmentVariable("ENVTEST"));
//     Console.WriteLine($"**** Environment: {context.HostingEnvironment.EnvironmentName} ****");

//     var pathEnvFile = string.Empty;
//     if (args.NotNullOrZero())
//     {
//         pathEnvFile = args.FirstOrDefault(x => x.StartsWith("--env="))?.Replace("--env=", "");
// }
// if (string.IsNullOrWhiteSpace(pathEnvFile))
// {
//     pathEnvFile = Path.Combine(
//         context.HostingEnvironment.ContentRootPath, ".env");
// }
// CustomDotEnv.LoadDotEnv(pathEnvFile);

// var configureProxy = new WebProxyConfigureMethod();
// WebRequest.DefaultWebProxy = configureProxy.GetProxyWithVariable();
// Console.WriteLine($"Proxy configured: {configureProxy.HttpProxyField} no proxy: {string.Join(",", configureProxy.HttpNoProxyField ?? new string[] { })}");

// var buildConfig = config.Build();

// Console.WriteLine($"**** LogLevelDefault: {buildConfig.GetSection("Logging:LogLevel:Default")?.Value} ****");

// var keyvaultDns = buildConfig.GetSection("AzureKeyVault:Dns")?.Value;
// Console.WriteLine($"KeyVault credential is {!string.IsNullOrWhiteSpace(keyvaultDns)}");

// Uri.TryCreate(keyvaultDns, UriKind.RelativeOrAbsolute, out Uri vaultDns);
// var vaultTenantId = buildConfig.GetSection("AzureKeyVault:TenantId")?.Value;
// var vaultClientId = buildConfig.GetSection("AzureKeyVault:ClientId")?.Value;
// var vaultClientSecret = buildConfig.GetSection("AzureKeyVault:ClientSecret")?.Value;

// TokenCredential credential = new
//     ClientSecretCredential(
//         tenantId: vaultTenantId,
//         clientId: vaultClientId,
//         clientSecret: vaultClientSecret
//     );

// config.AddAzureKeyVault(vaultDns, credential);

// if (context.HostingEnvironment.IsDevelopment())
// {
//     config.AddUserSecrets<Startup>();
// }

// Console.WriteLine($"**** {nameof(IHostBuilder)} configured ****");
// })
//                 .ConfigureWebHostDefaults(webBuilder =>
//                 {
//                     webBuilder.UseStartup<Startup>();
//                 });

// return host;
