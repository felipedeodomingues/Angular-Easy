using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace ControleFinanceiro.API
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
            //AddCors compartilha recursos entre o back e o servidor 
            services.AddCors();
            //Adicionando o diretorio
            services.AddSpaStaticFiles(diretorio =>
           {
               diretorio.RootPath = "ControleFinanceiro-UI";
           });

            services.AddControllers()
                   .AddJsonOptions(opcoes =>
                    {
                        opcoes.JsonSerializerOptions.IgnoreNullValues = true;
                    })
                   .AddNewtonsoftJson(opcoes => {
                       opcoes.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                   });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //usando o Cors
            app.UseCors(opcoes => opcoes.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //aplicativo utiliza aplicativos estaticos

            app.UseStaticFiles();

            app.UseSpaStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //configurando o spa para ele ser reconhecido
            app.UseSpa(spa =>
           {
               spa.Options.SourcePath = Path.Combine(Directory.GetCurrentDirectory(), "ControleFinanceiro-UI");

               if (env.IsDevelopment())
               {
                   spa.UseProxyToSpaDevelopmentServer($"http://localhost:4200/");
               }
           });
            //fim da aula 2
        }
    }
}
