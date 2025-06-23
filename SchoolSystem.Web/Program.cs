using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Infrastructure;
using SchoolSystem.Infrastructure.Common;
using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Services;
using SchoolSystem.Services.Contracts;
using SchoolSystem.Web;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                          throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<SchoolLogContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<SchoolLogContext>()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddAutoMapper(typeof(SchoolSystem.Services.MapperProfiles).Assembly);
builder.Services.AddAutoMapper(typeof(SchoolSystem.Web.MapperProfiles).Assembly);

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPrincipalService, PrincipalService>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IParentService, ParentService>();
builder.Services.AddScoped<ICurriculumService, CurriculumService>();
builder.Services.AddScoped<IGradeService, GradeService>();

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;
    try
    {
        RoleManager<IdentityRole<Guid>> roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        await RoleSeeder.SeedRoles(roleManager);
        await UserSeeder.SeedAsync(services);
    }
    catch (Exception ex)
    {
        ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
