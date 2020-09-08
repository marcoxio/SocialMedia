using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Services;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Filters;
using SocialMedia.Infrastructure.Interfaces;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Infrastructure.Services;

namespace SocialMedia.Api
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
            //Automapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Ignore Reference Circular
            services.AddControllers(options => {
                    options.Filters.Add<GlobalExceptionFilter>();
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                })

                // //Not valid model ApiController
                // .ConfigureApiBehaviorOptions(options => 
                // {
                //     options.SuppressModelStateInvalidFilter = true;
                // })
                .AddFluentValidation(options => 
                {
                    options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
                });

                // Variable Pagination
                services.Configure<PaginationOptions>(Configuration.GetSection("Pagination"));
            #region ConnectionDb
                services.AddDbContext<SocialMediaContext>(opt => {
                    opt.UseSqlServer(Configuration.GetConnectionString("SocialMedia"));
                });
            #endregion
            
            #region Dependency Injection
                services.AddTransient<IPostService,PostService>();    
                // services.AddTransient<IPostRepository,PostRepository>();    
                // services.AddTransient<IUserRepository,UserRepository>();

                //Generic Repository
                services.AddScoped(typeof(IRepository<>),typeof(BaseRepository<>));    
                services.AddTransient<IUnitOfWork,UnitOfWork>();
                services.AddSingleton<IUriService>(provider => 
                {
                    var accesor = provider.GetRequiredService<IHttpContextAccessor>();
                    var request = accesor.HttpContext.Request;
                    var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                    return new UriService(absoluteUri);
                });    
            #endregion


            //Dont Need Use this section
            //Filter Global Validation Model
            // services.AddMvc(options =>
            // {
            //     options.Filters.Add<ValidationFilter>();
            // });

         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
