using BusinessObject.Entities;
using DataAccess;
using DataAccess.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MomMomMilks.Extensions;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using System.Text.Json.Serialization;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Service.Services;
using Service.Helpers;


var builder = WebApplication.CreateBuilder(args);

// Regist Service From DAO
DataAccessServiceRegistration.AddDataAccessServices(builder.Services, builder.Configuration);

// Add identity services.
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

// Add OData Service
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Address>("Addresses");
modelBuilder.EntitySet<AppUser>("Users");
modelBuilder.EntitySet<Category>("Categories");
modelBuilder.EntitySet<Category>("Milks");
modelBuilder.EntitySet<Cart>("Carts");
modelBuilder.EntitySet<CartItem>("CartItems");
modelBuilder.EntitySet<District>("Districts");
modelBuilder.EntitySet<Order>("Orders");
modelBuilder.EntitySet<TimeSlot>("TimeSlots");
modelBuilder.EntitySet<Ward>("Wards");
modelBuilder.EntitySet<Batch>("Batches");
modelBuilder.EntitySet<Shipper>("Shippers");
modelBuilder.EntitySet<Supplier>("Suppliers");
modelBuilder.EntitySet<Brand>("Brands");
modelBuilder.EntitySet<MilkAge>("MilkAges");
modelBuilder.EntitySet<Category>("Category");

var edmModel = modelBuilder.GetEdmModel();
builder.Services.AddControllers()
    .AddOData(opt => opt.Select().Expand().Filter().OrderBy().Count().SetMaxTop(100))
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

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

// Add Quartz services
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory(); // Correct method name
    var jobKey = new JobKey("AssignOrdersJob");
    q.AddJob<AssignOrdersJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("AssignOrdersJob-trigger")
        .WithCronSchedule("0 0 6,12,17 * * ?")); // Schedule at 6AM, 12PM, and 5PM
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


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
    await Seed.MilkBatchSeed(context);
    await Seed.SeedDistrictsAndWards(context);
    await Seed.SeedUser(userManager, roleManager);
    await Seed.SeedAddress(context);
    await Seed.PaynmentTypeSeed(context);
    await Seed.OrderStatusSeed(context);
    await Seed.CouponSeed(context);
    await Seed.TimeSlotsSeed(context);
    await Seed.OrderSeed(context);
    await Seed.OrderDetailSeed(context);
    await Seed.ScheduleSeed(context);
    await Seed.TransactionSeed(context);


}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error while seeding data");
}

app.Run();
