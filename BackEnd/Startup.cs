using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
// Instalamos o Entity framework 
// dotnet tool install --global dotnet-ef

// Baixamos o pacote SQLServer do Entity Framework
// dotnet add package Microsoft.EntityFrameworkCore.SqlServer

// Baixamos o pacote que irá escrever nossos códigos
// dotnet add package Microsoft.EntityFrameworkCore.Design

// Testamos se os pacotes foram instalados
// dotnet restore

// Testamos a intalação do Entity Framework
// dotnet ef

// Código que criará o nosso contexto da Base de Dados e nossos Models
//                    string de conexao
// dotnet ef dbcontext scaffold "Server=DESKTOP-LKMH5JG\SQLEXPRESS; Database=Gufos; User Id=sa; Password=132" Microsoft.EntityFrameworkCore.SqlServer -o Models -d

// Para adicionar a arvore de objetos adicionamos uma nova biblioteca JSON
// dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson

// Swagger - Documentação 
// Instalamos o pacote 
// dotnet add BackEnd.csproj package Swashbuckle.AspNetCore -v 5.0.0-rc4

// JWT - JSON WEB TOKEN
//Adicionamos o pacote JWT
// dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 3.0.0

namespace BackEnd
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
            // Comentar Após ter instalado a biblioteca do JSON
            //services.AddControllers();

            // Configuramos comos os objetos relacionados aparecerao nos retornos, Incluir Após ter instalado a biblioteca do JSON
            services.AddControllersWithViews().AddNewtonsoftJson(
               opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            ); 

            // Configuramos o Swagger
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo{ Title = "API", Version = "v1"});

                // Definimos o caminho e arquivo temporário de documentação
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // Configuramos o JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters{
                    ValidateIssuer = true, // Validar Emissor
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Usamos Efetivamente o SWAGGER
            app.UseSwagger();
            // Especificamos o Endpoint na aplicação
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });

            // Usamos efetivamente a autenticação
            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
