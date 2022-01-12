using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Configure;
using BLL.Interfaces;
using BLL.Mapping;
using BLL.Services;
using DAL;
using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace IAWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<InternetAuctionContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;

                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                opt.Lockout.MaxFailedAccessAttempts = 3;
            }).AddEntityFrameworkStores<InternetAuctionContext>().AddDefaultTokenProviders();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutomapperProfile());
            });

            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = AuthOptions.ISSUER,
                        ValidAudience = AuthOptions.AUDIENCE,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey()
                    };
                });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ILotService, LotService>();
            services.AddTransient<ITradeService, TradeService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHttpContextAccessor();
            services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);

            services.AddCors();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "IAWebAPI", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IAWebAPI v1"));
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        }
    }
}
