using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Narumikazuchi.CreativeGallery.Core;
using Narumikazuchi.CreativeGallery.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args: args);

builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment() is false)
{
    app.UseExceptionHandler(errorHandlingPath: Routes.ERROR_PAGE,
                            createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();