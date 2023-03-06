using System.IO.Compression;
using FoxtaurServer.Services.Abstract;
using FoxtaurServer.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace FoxtaurServer;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Compression
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = false; // Do not turn on, security risk: https://learn.microsoft.com/en-us/aspnet/core/performance/response-compression?view=aspnetcore-6.0
            options.Providers.Add<BrotliCompressionProvider>(); // Brotli is widespread
            options.Providers.Add<GzipCompressionProvider>(); // GZIP as fallback
        });
        
        services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.SmallestSize;
        });

        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.SmallestSize;
        });
        
        #region  DB Contexts

        /*// Security
        services.AddDbContext<SecurityDbContext>(options =>
            options
                .UseNpgsql(Configuration.GetConnectionString("SecurityConnection")),
            ServiceLifetime.Transient);

        // Main
        services.AddDbContext<MainDbContext>(options =>
            options
                .UseNpgsql(Configuration.GetConnectionString("MainConnection"),
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)),
            ServiceLifetime.Transient);*/

        #endregion
        
        /*// Identity
        services.AddTransient<IUserValidator<User>, OptionalEmailUserValidator<User>>();

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<SecurityDbContext>()
            .AddDefaultTokenProviders();*/

        // TODO: Move me to config
        services.Configure<IdentityOptions>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = false;
            options.Password.RequiredUniqueChars = 6;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 10;
            options.Lockout.AllowedForNewUsers = true;

            // User settings
            options.User.RequireUniqueEmail = true;
        });

        // Session
        services.AddDistributedMemoryCache();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromSeconds(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        services.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromDays(150);

            // If the LoginPath isn't set, ASP.NET Core defaults
            // the path to /Account/Login.
            options.LoginPath = "/Account/Login";

            // If the AccessDeniedPath isn't set, ASP.NET Core defaults
            // the path to /Account/AccessDenied.
            options.AccessDeniedPath = "/Account/AccessDenied";

            options.SlidingExpiration = true;
        });

        /*// Fallback policy - require authentification
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });*/

        services.AddHttpContextAccessor();

        // DI
        // Scoped
        services.AddScoped<IDistancesService, DistancesService>();
        services.AddScoped<IMapsService, MapsService>();
        services.AddScoped<ITeamsService, TeamsService>();
        services.AddScoped<IFoxesService, FoxesService>();
        services.AddScoped<IHuntersService, HuntersService>();
        services.AddScoped<ILocationsService, LocationsService>();
        services.AddScoped<IHuntersLocationsService, HuntersLocationsService>();

        // Singletons
        services.AddSingleton<IConfigurationService, ConfigurationService>();

        // Hosted services


        services.AddMvc();
    }
    
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        // Compression
        app.UseResponseCompression();
        
        /*if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }*/

        app.UseHttpsRedirection();
        
        app.UseStaticFiles();

        app.UseAuthentication();

        // Routing
        app.UseRouting();

        app.UseAuthorization();

        // Session
        app.UseSession();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute("default", "{controller=MainPage}/{action=Index}/{id?}");
        });
        
        // Fancy error messages
        app.UseStatusCodePages();
    }
}