using EDW.API.Encryption;
using EDW.Application.Queries;
using EDW.Domain;
using EDW.Domain.Models;
using EDW.Domain.Token;
using EDW.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDW.API
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
            #region Token Decrypt MiddleWare
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = false,
                        ValidIssuer = "EDWAPI",
                        ValidAudience = "EDWAPI",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0FFA06DD318FB46565DB729A60A3154D"))
                    };
                });
            #endregion

            #region Enable Cors
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder => builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials())
            ); // Make sure you call this previous to AddMvc
            #endregion

            services.AddHttpContextAccessor(); //For using IHttpAccessor

            //disable automatic 400 response to let user access to methods even when ModelStat has errors

            #region EF InMemory
            services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("MyDB"));
            #endregion

            
            services.AddControllers();

            services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
                options.ClientErrorMapping[404].Link =
                    "https://httpstatuses.com/404";
            });

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                #region Description
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Experimental Developpement Works.API Swagger",
                    Contact = new OpenApiContact()
                    {
                        Email = "m.elharfi@gmail.com",
                        Name = "Mohssine EL HARFI",
                        Url = new System.Uri("https://melharfi.github.io"),
                    },
                    Description = "EDW.API handle monitoring requests and user management",
                    License = new OpenApiLicense()
                    {
                        Name = "Copyright",
                    }
                });
                #endregion

                #region Add Authentication Bearer
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                    Enter 'Bearer' [space] and then your token in the text input below.
                    \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
                #endregion
                c.EnableAnnotations();
            });
            #endregion

            #region mediatr
            services.AddMediatR(typeof(LoginQueryHandler).Assembly);
            #endregion

            services.AddScoped<IEncrypt, Sha512Encryption>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region Seeding default data to InMemory
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                AddTestData(context);
            }
            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            #region Enable Corse
            // Make sure you call this before calling app.UseMvc() and before auhentication middleware
            app.UseCors("CorsPolicy");
            #endregion

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region Swagger
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EDW Swagger Open API Documentation");
            });
            #endregion
        }

        private void AddTestData(AppDbContext context)
        {
            #region Activities
            Activity on_break = new()
            {
                Id = Guid.Parse("a0a94851-e00d-41a6-b5c0-825fc87dea81"),
                Name = "ON BREAK",
                Code = "ON_BREAK"
            };

            Activity on_call = new()
            {
                Id = Guid.Parse("58be1bcf-6b38-42eb-8e8f-18d9d855495b"),
                Name = "ON CALL",
                Code = "ON_CALL"
            };

            Activity in_meeting = new()
            {
                Id = Guid.Parse("17027a63-f66a-483d-ac4f-95c243e6a932"),
                Name = "IN MEETING",
                Code = "IN_MEETING"
            };

            context.Activities.Add(on_break);
            context.Activities.Add(on_call);
            context.Activities.Add(in_meeting);
            #endregion

            #region Users
            IEncrypt crypt = new Sha512Encryption();

            User james = new()
            {
                Id = Guid.Parse("72594b16-0d99-411a-b799-5d793dc8161b"),
                FirstName = "James",
                LastName = "Taylor",
                Username= "james",
                Password = crypt.HashPassword("james123")
            };

            User matt = new()
            {
                Id = Guid.Parse("15350509-8510-45dc-936f-6798c7494665"),
                FirstName = "Matt",
                LastName = "Griffin",
                Username = "matt",
                Password = crypt.HashPassword("matt123")
            };

            context.Users.Add(james);
            context.Users.Add(matt);

            #endregion
            context.SaveChanges();
        }
    }
}
