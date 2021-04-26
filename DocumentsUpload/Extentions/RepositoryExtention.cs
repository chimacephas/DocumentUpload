using DocumentsUpload.Messaging;
using DocumentsUpload.Repository.Implementation;
using DocumentsUpload.Repository.Interface;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentsUpload.Extentions
{
    public static class RepositoryExtention
    {
        public static IServiceCollection RegisterTypes(this IServiceCollection services)
        {
            services.AddSingleton<ICustomEmailSender, EmailSender>();
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<IDocumentRepository,DocumentRepository>();

            return services;
        }
    }
}
