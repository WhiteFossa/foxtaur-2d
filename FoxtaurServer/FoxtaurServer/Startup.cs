using System.IO.Compression;
using System.Text;
using FoxtaurServer.Dao;
using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Implementations;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Mappers.Implementations;
using FoxtaurServer.Models.Identity;
using FoxtaurServer.Services.Abstract;
using FoxtaurServer.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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

        // Security
        services.AddDbContext<SecurityDbContext>(options =>
            options
                .UseNpgsql(Configuration.GetConnectionString("MainConnection")),
            ServiceLifetime.Transient);

        // Main
        services.AddDbContext<MainDbContext>(options =>
            options
                .UseNpgsql(Configuration.GetConnectionString("MainConnection"),
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)),
            ServiceLifetime.Transient);

        #endregion
        
        // Identity
        services.AddIdentity<User, IdentityRole>()  
            .AddEntityFrameworkStores<SecurityDbContext>()  
            .AddDefaultTokenProviders(); 

        // TODO: Move me to config
        services.Configure<IdentityOptions>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 16;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequiredUniqueChars = 6;
            
            // User settings
            options.User.RequireUniqueEmail = true;
        });

        // Adding Authentication  
        services.AddAuthentication(options =>  
            {  
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;  
            })  
  
            // Adding Jwt Bearer  
            .AddJwtBearer(options =>  
            {  
                options.SaveToken = true;  
                options.RequireHttpsMetadata = false;  
                options.TokenValidationParameters = new TokenValidationParameters()  
                {  
                    ValidateIssuer = true,  
                    ValidateAudience = true,  
                    ValidAudience = Configuration["JWT:ValidAudience"],  
                    ValidIssuer = Configuration["JWT:ValidIssuer"],  
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))  
                };  
            });  

        // DI
        // Scoped
        services.AddScoped<IDistancesService, DistancesService>();
        services.AddScoped<IMapsService, MapsService>();
        services.AddScoped<ITeamsService, TeamsService>();
        services.AddScoped<IFoxesService, FoxesService>();
        services.AddScoped<IHuntersService, HuntersService>();
        services.AddScoped<ILocationsService, LocationsService>();
        services.AddScoped<IHuntersLocationsService, HuntersLocationsService>();
        services.AddScoped<IAccountsService, AccountsService>();
        services.AddScoped<ITeamsDao, TeamsDao>();

        // Singletons
        services.AddSingleton<IConfigurationService, ConfigurationService>();
        services.AddSingleton<ITeamsMapper, TeamsMapper>();
        services.AddSingleton<IColorsMapper, ColorsMapper>();

        // Hosted services
        
        // Various settings
        services.AddMvc();
        services.AddHttpContextAccessor();
    }
    
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        // Compression
        app.UseResponseCompression();

        app.UseStaticFiles();

        app.UseAuthentication();

        // Routing
        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute("default", "{controller=MainPage}/{action=Index}/{id?}");
        });
        
        // Fancy error messages
        app.UseStatusCodePages();
    }
}