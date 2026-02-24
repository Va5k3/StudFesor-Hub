using BusinessLayer.Abstractions;
using BusinessLayer.Implementations;
using Core.Interface;
using DAL.Abstraction;
using DAL.Implementation;
using Web_App___Blazor.Components;
using Web_App___Blazor.Services;
using DAL.Implementations;
using ScheduleImporter.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<UserSessionService>();
builder.Services.AddScoped<IAuthBusiness, AuthBusiness>();
builder.Services.AddScoped<IScheduleBusiness, ScheduleBusiness>();
builder.Services.AddScoped<IRepositoryUser, RepositoryUser>();
builder.Services.AddScoped<IRepositorySchedule, RepositorySchedule>();
builder.Services.AddScoped<IRepositoryStudent, RepositoryStudent>();
builder.Services.AddScoped<IActivityBusiness, ActivityBusiness>();
builder.Services.AddScoped<IRepositoryActivity, RepositoryActivity>();
builder.Services.AddScoped<IUserBusiness, UserBusiness>();
builder.Services.AddScoped<IScheduleImporter, ScheduleImporter.Implementation.ScheduleImporter>();

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