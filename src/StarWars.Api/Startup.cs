using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using StarWars.Api.Entities;
using StarWars.Api.Models;
using StarWars.Api.Services;
using StarWars.API.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace testWebNet
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
            services.AddMvc(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver
                = new CamelCasePropertyNamesContractResolver();
            });

            services.AddDbContext<StarWarsContext>(options =>
            {
                options.UseInMemoryDatabase("StarWars-api");
            });

            //Register repository
            services.AddScoped<IStarWarsRepository, StarWarsRepository>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<IUrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>()
                .ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            services.AddTransient<ITypeHelperService, TypeHelperService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "StarWars.API",
                    Version = "V1",
                    Contact = new Contact
                    {
                        Name = "Krzysztof Soporek",
                        Email = "k.soporek@gmail.com",
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, StarWarsContext starWarsContext)
        {
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync(
                            "Unexcepted fault happend. Try again later.");
                    });
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            AutoMapper.Mapper.Initialize(config =>
           {
               //config.CreateMap<Author, AuthorDto>()
               //.ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
               //$"{src.LastName} {src.FirstName}"))
               //.ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
               //src.DateOfBirth.GetCurrentAge()));

               config.CreateMap<Character, CharacterDto>();
               //.ForMember(dest => dest.Friends, opt => opt.MapFrom(src =>
               //new List<string>(src.Episodes.Select(e => e.Name))));

               config.CreateMap<Character, CharacterForCreationDto>();
               config.CreateMap<CharacterForCreationDto, Character>();
               config.CreateMap<CharacterForUpdateDto, Character>();
               config.CreateMap<CharacterForUpdateDto, CharacterDto>();
               config.CreateMap<Character, CharacterForUpdateDto>();

               config.CreateMap<EpisodeDto, Episode>();
               config.CreateMap<Episode, EpisodeDto>();

               config.CreateMap<EpisodeForCreationDto, Episode>();
               config.CreateMap<EpisodeForUpdateDto, Episode>();
           });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "StarWars.API V1");
            });

            starWarsContext.StartWithFreshData();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}