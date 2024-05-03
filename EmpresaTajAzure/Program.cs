using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using EmpresaTajAzure.Data;
using EmpresaTajAzure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<ServiceApiTajamar>();
builder.Services.AddTransient<ServiceStorageBlobs>();
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
string connectionString =builder.Configuration.GetConnectionString("SqlLocal");
// Recupera el ApplicationID
KeyVaultSecret applicationIDSecret = await secretClient.GetSecretAsync(builder.Configuration["KeyVault:ApplicationIDSecretName"]);
string appId = applicationIDSecret.Value;

// Recupera el SecretKey
KeyVaultSecret secretKeySecret = await secretClient.GetSecretAsync(builder.Configuration["KeyVault:SecretKeySecretName"]);
string secretKey = secretKeySecret.Value;


// Recupera el storageAccount
KeyVaultSecret StorageAccountSecret = await secretClient.GetSecretAsync(builder.Configuration["AzureKeys:StorageAccountSecretName"]);
string StorageAccount = StorageAccountSecret.Value;

BlobServiceClient blobServiceClient = new BlobServiceClient(StorageAccount);
builder.Services.AddTransient<BlobServiceClient>(x => blobServiceClient);



builder.Services.AddDbContext<ApplicationContext>
    (options => options.UseSqlServer(connectionString));
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
