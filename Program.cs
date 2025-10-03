using Leave_Mangement_System.Data;
using Leave_Mangement_System.Models;
using Leave_Mangement_System.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.SlidingExpiration = true;
});

builder.Services.AddAuthorization(options =>
{
   
    options.AddPolicy("HRPrimaryOnly", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("HRPrimary");
    });

    
    options.AddPolicy("ManagerOnly", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Manager");
    });

   
    options.AddPolicy("HRPrimaryOrManager", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("HRPrimary", "Manager");
    });

 
    options.AddPolicy("EmployeeOnly", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Employee", "HR", "Manager", "HRPrimary");
    });
});

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ILeavePolicyService, LeavePolicyService>();
builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapStaticAssets();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.MigrateAsync();
        await SeedRolesAndUsers(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding data.");
    }
}

app.Run();

static async Task SeedRolesAndUsers(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
{
   
    string[] roleNames = { "HRPrimary", "HR", "Manager", "Employee" };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

   
    var hrPrimaryEmail = "hrprimary@company.com";
    var hrPrimaryUser = await userManager.FindByEmailAsync(hrPrimaryEmail);
    if (hrPrimaryUser == null)
    {
        hrPrimaryUser = new ApplicationUser
        {
            UserName = hrPrimaryEmail,
            Email = hrPrimaryEmail,
            FullName = "HR Primary Manager",
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(hrPrimaryUser, "HRPrimary@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(hrPrimaryUser, "HRPrimary");
        }
    }

   
    var hrEmail = "hr@company.com";
    var hrUser = await userManager.FindByEmailAsync(hrEmail);
    if (hrUser == null)
    {
        hrUser = new ApplicationUser
        {
            UserName = hrEmail,
            Email = hrEmail,
            FullName = "HR Staff",
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(hrUser, "HR@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(hrUser, "HR");
        }
    }

    
    var managerEmail = "manager@company.com";
    var managerUser = await userManager.FindByEmailAsync(managerEmail);
    if (managerUser == null)
    {
        managerUser = new ApplicationUser
        {
            UserName = managerEmail,
            Email = managerEmail,
            FullName = "Project Manager",
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(managerUser, "Manager@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(managerUser, "Manager");
        }
    }
    else
    {
        if (!await userManager.IsInRoleAsync(managerUser, "Manager"))
        {
            await userManager.AddToRoleAsync(managerUser, "Manager");
        }
    }

   
    var empEmail = "employee@company.com";
    var empUser = await userManager.FindByEmailAsync(empEmail);
    if (empUser == null)
    {
        empUser = new ApplicationUser
        {
            UserName = empEmail,
            Email = empEmail,
            FullName = "Regular Employee",
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(empUser, "Employee@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(empUser, "Employee");
        }
    }
}