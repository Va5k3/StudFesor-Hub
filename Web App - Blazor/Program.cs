using BusinessLayer.Abstractions;
using BusinessLayer.Implementations;
using DAL.Abstraction;
using DAL.Implementation;
using Web_App___Blazor.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IAuthBusiness, AuthBusiness>();
builder.Services.AddScoped<IRepositoryUser, RepositoryUser>();
builder.Services.AddScoped<IRepositorySchedule, RepositorySchedule>();
builder.Services.AddScoped<IRepositoryStudent, RepositoryStudent>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();