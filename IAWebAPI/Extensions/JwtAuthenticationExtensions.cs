using System.Text;
using BLL.Configuration;
using DAL;
using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// JwtAuthenticationExtensions
    /// </summary>
    public static class JwtAuthenticationExtensions
    {
        public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwt =>
                {
                    var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

                    jwt.SaveToken = true;
                    jwt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = false,
                        ValidateLifetime = true
                    };
                });
            var jwtSettings = new JwtConfig();
            Configuration.Bind(nameof(JwtConfig), jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.SignIn.RequireConfirmedAccount = true;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<InternetAuctionContext>().AddDefaultTokenProviders();

            return services;
        }
    }
}
