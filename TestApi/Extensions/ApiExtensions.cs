using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TestApi.Application.Interfaces;
using TestApi.Application.Services;
using TestApi.Application.Services.Mapper;
using TestApi.Application.Validators;
using TestApi.Domain.Models.Posts;
using TestApi.Domain.Models.Users;
using TestApi.Infrastructure.Authorization.Interfaces;
using TestApi.Infrastructure.Authorization.Services;
using TestApi.Infrastructure.DbContexts;
using TestApi.Infrastructure.Extensions;
using TestApi.Infrastructure.Models;
using TestApi.Infrastructure.Repositories;

namespace TestApi.Extensions
{
    public static class ApiExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AppSettings>(config);
            var appSettings = services.GetAppSettings();
            services.AddHttpContextAccessor();

            services.AddDbContext<MainDbContext>(options =>
                options.UseOracle(appSettings.ConnectionStrings.DbConnectionString)
            );

            services.AddAuthorization();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestApi", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization using Bearer Token.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = appSettings.JwtConfig.Issuer,
                    ValidAudience = appSettings.JwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JwtConfig.SecretKey))
                };
            });

            services.RegisterServices();
            services.AddControllers();
        }

        public static void Configure(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestApi V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Migrate();
        }

        public static void RegisterServices(this IServiceCollection services)
        {
            services.RegisterValidators();
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddAutoMapper(typeof(AppMapper).Assembly);
            services.AddScoped<IUserAuthService, UserAuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();
        }

        public static void RegisterValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<AddUserModel>, AddUserModelValidator>();
            services.AddScoped<IValidator<UpdateUserModel>, UpdateUserModelValidator>();
            services.AddScoped<IValidator<AddPostModel>, AddPostModelValidator>();
            services.AddScoped<IValidator<UpdatePostModel>, UpdatePostModelValidator>();
        }
    }
}
