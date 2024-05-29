using BusinessObject.Entities;
using DataAccess;
using DataAccess.DAO;
using DataAccess.DAO.Interface;
using DataAccess.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MomMomMilks.Extensions;
using Repository;
using Repository.Interface;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.OData;
using System.Reflection.Emit;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

// Add OData Service
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Order>("Orders");
modelBuilder.EntitySet<Category>("Categories");
modelBuilder.EntitySet<Category>("Milks");

var edmModel = modelBuilder.GetEdmModel();
builder.Services.AddControllers().AddOData(opt =>
    opt.Select()
       .Filter()
       .OrderBy()
       .Expand()
       .SetMaxTop(100)
       .Count()
       .AddRouteComponents("odata", edmModel));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

//app.MapControllers();
app.UseEndpoints(endpoints => endpoints.MapControllers());

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await Seed.SeedBranch(context);
    await Seed.MilkAgeSeed(context);
    await Seed.SupplierSeed(context);
    await Seed.CategorySeed(context);
    await Seed.MilkSeed(context);
    await Seed.SeedUser(userManager, roleManager);
    await Seed.PaynmentTypeSeed(context);
    await Seed.OrderSeed(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error while seeding data");
}

app.Run();
