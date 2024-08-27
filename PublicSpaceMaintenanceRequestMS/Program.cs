using AutoMapper;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PublicSpaceMaintenanceRequestMS.Configuration;
using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.Helper;
using PublicSpaceMaintenanceRequestMS.Repositories;
using PublicSpaceMaintenanceRequestMS.Services;
using PublicSpaceMaintenanceRequestMS.Services.Interfaces;
using Serilog;
using System.Text;

namespace PublicSpaceMaintenanceRequestMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Serilog
            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            });


            // Connection String - DB Context
            var connString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<PublicSpaceMaintenanceRequestDbDBContext>(options => options.UseSqlServer(connString));


            // Add ApplicationService
            builder.Services.AddScoped<IApplicationService, ApplicationService>();
            builder.Services.AddRepositories();


            // Add Mapper
            builder.Services.AddScoped(provider =>
                new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new MapperConfig());
                })
            .CreateMapper());

            // Add HttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyCity", Version = "v1" });

                // Authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    //Type = SecuritySchemeType.ApiKey,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
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
                        Array.Empty<string>()
                    }
                });
             });

            // Configure JWT Authentication
            var key = Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretKey"]!);
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });


            // Add CORS policy

            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAll",
            //        builder => builder.AllowAnyOrigin()
            //                          .AllowAnyMethod()
            //                          .AllowAnyHeader());
            //});


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AngularClient",
                    builder => builder.WithOrigins("http://localhost:4201") 
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyCity V1");
                });
            }

            app.UseHttpsRedirection();
            app.UseCors("AngularClient");   // Use the correct CORS policy
            app.UseAuthentication();
            app.UseAuthorization();

            // Global Error Handler
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
