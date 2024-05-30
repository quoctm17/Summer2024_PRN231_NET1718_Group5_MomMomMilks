using FE.Helpers;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add HttpClient
builder.Services.AddHttpClient("MyClient", opt =>
{
    var config = builder.Configuration.GetSection("ApiSettings");
    opt.BaseAddress = new Uri(config.GetValue<string>("BaseUrl"));
}).AddHttpMessageHandler<CustomDelegatingHandler>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<CustomDelegatingHandler>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

// Add Service
builder.Services.AddScoped<FE.Services.MilkService>();
builder.Services.AddScoped<FE.Services.CategoryService>();
builder.Services.AddScoped<FE.Services.AccountService>();
builder.Services.AddScoped<FE.Services.OrderService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();


app.UseAuthorization();

app.MapRazorPages();

app.Run();
