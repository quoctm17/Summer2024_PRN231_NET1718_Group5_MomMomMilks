using BusinessObject.Entities;
using DataAccess;
using DataAccess.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MomMomMilks.Extensions;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Regist Service From DAO
DataAccessServiceRegistration.AddDataAccessServices(builder.Services, builder.Configuration);

// Add identity services.
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

// Add OData Service
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Order>("Orders");
modelBuilder.EntitySet<Category>("Categories");
modelBuilder.EntitySet<Category>("Milks");
modelBuilder.EntitySet<Cart>("Carts");
modelBuilder.EntitySet<CartItem>("CartItems");

var edmModel = modelBuilder.GetEdmModel();
builder.Services.AddControllers()
    .AddOData(opt => opt.Select().Expand().Filter().OrderBy().Count().SetMaxTop(100))
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddControllers();
builder.Services.AddLogging();
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
app.UseCors("AllowAll");
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
    await Seed.SeedDistrictsAndWards(context);
    await Seed.SeedAddress(context);
    await Seed.PaynmentTypeSeed(context);
    await Seed.OrderStatusSeed(context);
    await Seed.OrderSeed(context);
    await Seed.CouponSeed(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error while seeding data");
}

app.Run();
