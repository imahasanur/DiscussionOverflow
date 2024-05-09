//using DiscussionOverflow.Web.Data;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DiscussionOverflow.Application;
using DiscussionOverflow.Infrastructure;
using DiscussionOverflow.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Reflection;
using DiscussionOverflow.Infrastructure.Extensions;
using DiscussionOverflow.Infrastructure.Email;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var builder = WebApplication.CreateBuilder(args);

try { 
    // Add services to the container.
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    var migrationAssembly = Assembly.GetExecutingAssembly().FullName;

    builder.Host.UseSerilog((ctx, lc) => lc
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(builder.Configuration));

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new ApplicationModule());
        containerBuilder.RegisterModule(new InfrastructureModule(connectionString, migrationAssembly));
        containerBuilder.RegisterModule(new WebModule());
        
    });

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString,
        (m) => m.MigrationsAssembly(migrationAssembly)));

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    //    .AddEntityFrameworkStores<ApplicationDbContext>();
    builder.Services.AddIdentity();

    builder.Services.AddJwtAuthentication(builder.Configuration["Jwt:Key"], builder.Configuration["Jwt:Issuer"],
    builder.Configuration["Jwt:Audience"]);

    builder.Services.AddControllersWithViews();
    builder.Services.AddCookieAuthentication();

    builder.Services.AddAuthorization(options => {
        options.AddPolicy("QuestionCreatePolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("CreateQuestion", "true");

        });

		options.AddPolicy("EditProfilePolicy", policy =>
		{
			policy.RequireAuthenticatedUser();
			policy.RequireClaim("ProfileEdit", "true");

		});
		
		options.AddPolicy("ViewProfilePolicy", policy =>
		{
			policy.RequireAuthenticatedUser();
			policy.RequireClaim("ProfileView", "true");

		});

		options.AddPolicy("HandleVotePolicy", policy =>
		{
			policy.RequireAuthenticatedUser();
			policy.RequireClaim("VoteHandle", "true");

		});

		options.AddPolicy("HandleCommentPolicy", policy =>
		{
			policy.RequireAuthenticatedUser();
			policy.RequireClaim("CommentHandle", "true");

		});

	});

    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    builder.Services.Configure<Smtp>(builder.Configuration.GetSection("Smtp"));

    //this commented kestrel section is not working
    //builder.Services.Configure<KestrelServerOptions>(builder.Configuration.GetSection("Kestrel"));
    //builder.WebHost.UseUrls("http://*:80");

    var app = builder.Build();

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

    //To make "SignInManager.IsSignedIn(User)" false to true in _LoginPartial.cshtml
    app.UseAuthentication(); 
    app.UseAuthorization();
    app.UseSession();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Failed to start the application.");
}
finally
{
    Log.CloseAndFlush();
}
