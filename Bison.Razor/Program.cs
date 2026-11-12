using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var databasePath = Environment.GetEnvironmentVariable("BISONDBPATH");

if (string.IsNullOrEmpty(databasePath))
{
    databasePath = "./data/bison.db";//Path.Combine(Path.GetTempPath(), "bison.db");
}

builder.Services.AddSingleton(new DBFacade(databasePath));

builder.Services.AddSingleton<IObservationService, ObservationService>();
builder.Services.AddSingleton<IPostRepository, PostRepository>();
builder.Services.AddSingleton<ITaxonFilterService, TaxonFilterService>();

builder.Services.AddDbContext<BisonDBContext>(options =>
    options.UseSqlite($"Data Source={databasePath}"));

builder.Services.AddDbContext<AuthDBContext>(options =>
    options.UseSqlite($"Data Source={databasePath}"));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    options.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<AuthDBContext>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BisonDBContext>();

    context.Database.EnsureCreated();

    DbInitializer.SeedDatabase(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
