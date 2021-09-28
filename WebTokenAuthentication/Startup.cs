using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTokenAuthentication
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
           /*
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                     .AddJwtBearer(options => {
                         options.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateIssuer = true,
                             ValidateAudience = true,
                             ValidateLifetime = true,
                             ValidateIssuerSigningKey = true,

                             ValidAudience = Configuration["JWT:ValidAudience"],
                             ValidIssuer = Configuration["JWT:ValidIssuer"],
                             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                         };
                         options.Events = new JwtBearerEvents
                         {
                             OnAuthenticationFailed = context =>
                             {
                                 Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                                 return Task.CompletedTask;
                             },
                             OnTokenValidated = context =>
                             {
                                 Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                                 return Task.CompletedTask;
                             }
                         };
                     });
            */


                       // Adding Authentication
                        services.AddAuthentication(options =>
                       {
                           options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                           options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                           options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                       })

                       // Adding Jwt Bearer
                       .AddJwtBearer(options =>
                       {
                           options.SaveToken = true;
                           options.RequireHttpsMetadata = false;
                           options.TokenValidationParameters = new TokenValidationParameters()
                           {
                               ValidateIssuer = true,
                               ValidateAudience = true,
                               ValidAudience = Configuration["JWT:ValidAudience"],
                               ValidIssuer = Configuration["JWT:ValidIssuer"],
                               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                           };
                       }); 

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //these should be in this order

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
