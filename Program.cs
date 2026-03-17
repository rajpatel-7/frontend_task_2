using Microsoft.AspNetCore.Authentication.Cookies;
using frontend_task_2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// (Service to talk with API)
builder.Services.AddHttpClient<ApiService>();


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ApiService>();



// (Cookie Auth for Login and Session)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                   .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.Cookie.Name = "ClinicAuthCookie";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// (Authentication first)
    app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
                name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
