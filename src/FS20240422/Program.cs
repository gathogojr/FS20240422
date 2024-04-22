using FS20240422.Data;
using FS20240422.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

// Build the Edm model
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Customer>("Customers");
modelBuilder.EntitySet<Order>("Orders");

// Configure the OData service
builder.Services.AddControllers().AddOData(
    options =>
    {
        options.EnableQueryFeatures();
        options.AddRouteComponents(
            model: modelBuilder.GetEdmModel());
    });

// Configure the DbContext
builder.Services.AddDbContext<FsDbContext>(
    options => options.UseInMemoryDatabase("FsDb"));

var app = builder.Build();

app.UseRouting();
app.MapControllers();

// Add seed data to the database
using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var db = serviceScope.ServiceProvider.GetRequiredService<FsDbContext>();

    FsDbHelper.SeedDb(db);
}

app.Run();
