using desafiocoaniquem.Models;
using desafiocoaniquem.Services;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddDistributedMemoryCache();

//Configuraci�n de modelo de envío de mail
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

ILogger logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//Configuramos CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://desafiocoaniquem.cl", "https://desafio.coaniquem.cl/", "http://desafiocoaniquem.cl", "https://pre.desafiocoaniquem.cl");
        });
});

//Certificado de Redpay por ambiente
if (builder.Environment.IsProduction())
{
    var redpayCertificate = new X509Certificate2(builder.Environment.ContentRootPath + "/certs/redpay/prod/desafiocqm.pfx", "5Nf7wsHA6k7yWOLV");
    builder.Services.AddHttpClient("redpayclient", c => { }).ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler();
        handler.ClientCertificates.Add(redpayCertificate);
        return handler;
    });
}
else
{
    var redpayCertificate = new X509Certificate2(builder.Environment.ContentRootPath + "/certs/redpay/dev/desafiocqm.pfx", "12345");
    builder.Services.AddHttpClient("redpayclient", c => { }).ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler();
        handler.ClientCertificates.Add(redpayCertificate);
        return handler;
    });
}

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
//app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate, pre-check=0, post-check=0, max-age=0, s-maxage=0");
        ctx.Context.Response.Headers.Append("Pragma", "no-cache");
        ctx.Context.Response.Headers.Append("X-Frame-Options", "DENY");
        ctx.Context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        ctx.Context.Response.Headers.Append("Content-Security-Policy", "default-src 'self';style-src 'self' fonts.googleapis.com cdn.jsdelivr.net;font-src 'self' fonts.gstatic.com data:;script-src 'self' www.google.com www.gstatic.com cdn.datatables.net www.googletagmanager.com www.google-analytics.com  cdn.jsdelivr.net cdnjs.cloudflare.com www.googletagmanager.com www.google-analytics.com;img-src 'self' blob: data: www.google.cl;frame-src 'self' app.powerbi.com  www.google.com www.gstatic.com;object-src 'self';connect-src 'self' www.google-analytics.com cdn.datatables.net analytics.google.com www.google.com;frame-ancestors; form-action;");
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "voluntarios.coaniquem.cl");
    }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Views}/{action=Index}/{id?}");

//Inicializamos la clase global de propiedades estáticas
new GlobalConfiguration(app.Configuration);

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate, pre-check=0, post-check=0, max-age=0, s-maxage=0");
    context.Response.Headers.Add("Pragma", "no-cache");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self';style-src cdnjs.cloudflare.com 'self' fonts.googleapis.com cdn.jsdelivr.net;font-src 'self' fonts.gstatic.com cdnjs.cloudflare.com data:;script-src 'self' unpkg.com cdn.datatables.net www.googletagmanager.com www.google-analytics.com stats.g.doubleclick.net cdn.jsdelivr.net cdnjs.cloudflare.com www.googletagmanager.com www.google-analytics.com;img-src 'self' blob: data: www.google.cl;frame-src 'self' www.googletagmanager.com;object-src 'self';connect-src 'self' www.google-analytics.com stats.g.doubleclick.net analytics.google.com www.google.com;frame-ancestors; form-action;");
    await next();
});

app.Run();
