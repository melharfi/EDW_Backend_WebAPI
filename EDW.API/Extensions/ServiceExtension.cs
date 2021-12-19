//using EDW.API.Encryption;
//using EDW.Application.Queries;
//using EDW.Domain;
//using EDW.Domain.Token;
//using EDW.Infrastructure;
//using MediatR;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EDW.API.Extensions
//{
//    public static class ServiceExtension
//    {
//        public static IServiceCollection AddServices(this IServiceCollection services)
//        {
//            services.AddScoped<IEncrypt, Sha512Encryption>();
//            services.AddScoped<ITokenGenerator, TokenGenerator>();

//            return services;
//        }
//    }
//}
