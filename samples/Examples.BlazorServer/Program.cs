using Examples.BlazorServer;
using Examples.BlazorServer.Features.PayU.Notifications;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MudBlazor.Services;

Directory.SetCurrentDirectory(AppContext.BaseDirectory);
var builder = WebApplication.CreateBuilder(args);

// Add presentation services
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddHttpContextAccessor();
builder
    .Services
    .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Add application services
builder.Services.AddAppServices();

// configure services
builder
    .Services
    .Configure<RazorPagesOptions>(options => options.RootDirectory = "/Features");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

// Map gateway endpoints
app.MapPayUEndpoints();

app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();