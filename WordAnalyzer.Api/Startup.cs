﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WordAnalyzer.Core.Repositories;
using WordAnalyzer.Infrastructure.Repositories;
using WordAnalyzer.Infrastructure.Services;

namespace WordAnalyzer.Api {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_1);
            services.AddSingleton<ISentenceRepository, SentenceRepository> ();
            services.AddScoped<ISentenceService, SentenceService> ();
            services.AddScoped<ISentenceCreator, SentenceCreator> ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseHsts ();
            }
            
            app.UseStaticFiles ();
            app.UseHttpsRedirection ();
            app.UseMvc (routes => {
                routes.MapRoute (
                    name: "default",
                    template: "{controller=Converts}/{action=Index}");
            });
        }
    }
}