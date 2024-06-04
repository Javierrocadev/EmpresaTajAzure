using EmpresaTajAzure.Models;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using EmpresaTajAzure.Data;
using EmpresaTajAzure.Helpers;
using EmpresaTajAzure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string jsonSecrets = await HelperSecretManager.GetSecretsAsync();
KeysModel keysModel = JsonConvert.DeserializeObject<KeysModel>(jsonSecrets);
builder.Services.AddSingleton<KeysModel>(x => keysModel);

builder.Services.AddTransient<ServiceApiTajamar>();
builder.Services.AddTransient<ServiceStorageBlobs>();
//builder.Services.AddTransient<ServiceLogicApps>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => options.IdleTimeout = TimeSpan.FromMinutes(10));







builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient(builder.Configuration.GetSection("KeyVault"));
});


// Recupera el SecretClient
SecretClient secretClient = builder.Services.BuildServiceProvider().GetService<SecretClient>();
//KeyVaultSecret secret = await secretClient.GetSecretAsync("SqlLocal");
//string connectionString = secret.Value;






string connectionString = keysModel.MySql;





// Recupera el ApplicationID
//KeyVaultSecret applicationIDSecret = await secretClient.GetSecretAsync(builder.Configuration["KeyVault:ApplicationIDSecretName"]);
//ApplicationIDSecretName
//string appId = applicationIDSecret.Value;
string appId = keysModel.ApplicationIDSecretName;

// Recupera el SecretKey
//KeyVaultSecret secretKeySecret = await secretClient.GetSecretAsync(builder.Configuration["KeyVault:SecretKeySecretName"]);
//string secretKey = secretKeySecret.Value;
string secretKey = keysModel.SecretKeySecretName;

// Recupera el logic app
//KeyVaultSecret urlLogicAppSecret = await secretClient.GetSecretAsync(builder.Configuration["KeyVault:UrlLogicAppSecretName"]);
//string urlLogicApp = urlLogicAppSecret.Value;
string urlLogicApp = keysModel.UrlLogicAppSecretName;
builder.Services.AddTransient<ServiceLogicApps>(s => new ServiceLogicApps(urlLogicApp));





// Recupera el storageAccount
//KeyVaultSecret StorageAccountSecret = await secretClient.GetSecretAsync(builder.Configuration["AzureKeys:StorageAccountSecretName"]);
//string StorageAccount = StorageAccountSecret.Value;
string StorageAccount = keysModel.StorageAccountSecretName;

BlobServiceClient blobServiceClient = new BlobServiceClient(StorageAccount);
builder.Services.AddTransient<BlobServiceClient>(x => blobServiceClient);



//builder.Services.AddDbContext<ApplicationContext>
//    (options => options.UseSqlServer(connectionString));




builder.Services.AddDbContext<ApplicationContext>
    (options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));













//INDICAMOS QUE UTILIZAREMOS UN USUARIO IdentityUser
//DENTRO DE NUESTRA APP Y QUE LO ADMINISTRARA NUESTRO CONTEXT
builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationContext>();
//SI QUEREMOS UTILIZAR DISTINTOS PROVEEDORES ES AQUI DONDE 
//LOS IREMOS DANDO DE ALTA
builder.Services.AddAuthentication().AddMicrosoftAccount(options =>
{
    options.ClientId = appId;
    options.ClientSecret = secretKey;
});

//COMO VAMOS A UTILIZAR RUTAS PERSONALIZADAS
builder.Services.AddControllersWithViews
    (options => options.EnableEndpointRouting = false);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
