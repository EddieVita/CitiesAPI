using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using CityInfo.API.Services;
using Microsoft.Extensions.Configuration;
using CityInfo.API.Models;
using Microsoft.EntityFrameworkCore;
using CityInfo.API.ViewModels;

namespace CityInfo.API
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //Configuration = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //    //.AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json",true)
            //    .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc()
                .AddMvcOptions(O => O.OutputFormatters.Add(
                        new XmlDataContractSerializerOutputFormatter()
                    ));

            //TODO: Test getting the connection string in system environment variables (search for 'Edit the system environment variables').
            services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

#if DEBUG
            services.AddTransient<IMailService, MailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif


            services.AddScoped<ICityRepsitory, CityRepository>();

            //var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            ////configure NLog
            //loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            //loggerFactory.ConfigureNLog("nlog.config");

            //use this if you want to use pascal case for the jason output
            //services.AddMvc().AddJsonOptions(o =>
            //{
            //    if (o.SerializerSettings.ContractResolver != null)
            //    {
            //        var castedResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;
            //        castedResolver.NamingStrategy = null;
            //    }
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, AppDBContext appDbContext)
        {
         
            //NLog.LogManager.LoadConfiguration("nlog.config");
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<City, CitySummary>();
                cfg.CreateMap<City, CityViewModel>();
                cfg.CreateMap<PointOfInterest, PointOfInterestViewModel>();
                cfg.CreateMap<PointOfInterestViewModel, PointOfInterest>();
            });

            app.UseMvc();

            AppDBContextExtensions.Seed(appDbContext);
        }
    }
}
