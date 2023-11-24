using UserJourney.Common.CommonService;

namespace UserJourney.Setup
{
    public class DependenciesConfig
    {
        public static void ConfigureDependencies(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<JwtManager>();
        }
    }
}
