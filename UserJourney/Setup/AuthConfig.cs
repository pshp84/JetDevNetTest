﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserJourney.Common.CommonService;

namespace UserJourney.Setup
{
    public static class AuthConfig
    {
        public static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
        {
            //var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            //XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            var jwtOptions = configuration.GetSection("JwtOptions");
            var smtpSettings = configuration.GetSection("SMTPSettings");

            //var accessSecret = Convert.FromBase64String(jwtOptions["AccessSecret"]);
            var accessSecret =  Encoding.UTF8.GetBytes(jwtOptions["AccessSecret"]);
            var refreshSecret = Encoding.UTF8.GetBytes(jwtOptions["RefreshSecret"]);
            var accessKey = new SymmetricSecurityKey(accessSecret);
            var refreshKey = new SymmetricSecurityKey(refreshSecret);

            services.Configure<JwtOptions>(options =>
            {
                int.TryParse(jwtOptions["AccessExpire"], out var accessExpireMinutes);
                if (accessExpireMinutes > 0)
                {
                    options.AccessValidFor = TimeSpan.FromMinutes(accessExpireMinutes);
                }

                int.TryParse(jwtOptions["RefreshExpire"], out var refreshExpireMinutes);
                if (refreshExpireMinutes > 0)
                {
                    options.RefreshValidFor = TimeSpan.FromMinutes(refreshExpireMinutes);
                }

                options.Issuer = jwtOptions["Issuer"];
                options.Audience = jwtOptions["Audience"];
                options.AccessSecret = accessSecret;
                options.RefreshSecret = refreshSecret;
                options.AccessSigningCredentials = new SigningCredentials(accessKey, SecurityAlgorithms.HmacSha256);
                options.RefreshSigningCredentials = new SigningCredentials(refreshKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                //configureOptions.SaveToken = true;
                //configureOptions.RequireHttpsMetadata = false;
                configureOptions.ClaimsIssuer = jwtOptions["Issuer"];
                configureOptions.TokenValidationParameters = new TokenValidationParameters
                { 
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtOptions["Issuer"],
                    ValidAudience = jwtOptions["Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = accessKey,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
