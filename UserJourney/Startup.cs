using BizBuzzAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using UserJourney.Data;
using UserJourney.Setup;

namespace UserJourney
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        protected void ConfigureDependencies(IServiceCollection services)
        {
            DependenciesConfig.ConfigureDependencies(services);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // For Entity Framework
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("localDb")));

            // For Identity
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDBContext>()
                .AddDefaultTokenProviders();

            //token expired time for forgot password time
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            opt.TokenLifespan = TimeSpan.FromHours(2));

            services.ConfigureAuth(Configuration);
            ConfigureDependencies(services);
            services.ConfigureCors();
            services.ConfigureSwagger();
            services.AddAuthorization();
            services
                 .AddControllers(opt =>
                 {
                     opt.UseCentralRoutePrefix(new RouteAttribute("api"));
                     opt.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
                 })
                 .AddNewtonsoftJson(options =>
                 {
                     options.SerializerSettings.Converters.Add(new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy() });
                     //options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                 });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseConfiguredSwagger(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
