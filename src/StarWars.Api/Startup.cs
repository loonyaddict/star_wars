using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StarWars.Api.Entities;
using StarWars.Api.Models;
using StarWars.Api.Services;

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

                //setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
                //setupAction.OutputFormatters.Add(new JsonProtocolDependencyInjectionExtensions());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<StarWarsContext>(options =>
            {
                options.UseInMemoryDatabase("StarWars-api");
            });

            

            services.AddScoped<IStarWarsRepository, StarWarsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            StarWarsContext starWarsContext)
        {
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
               config.CreateMap<Character, CharacterForCreationDto>();
               config.CreateMap<CharacterForCreationDto, Character>()
               .ForMember(dest => dest.Episodes, opt => opt.MapFrom(src =>
               string.Join(", ", src.Episodes)));
               
               
               //config.CreateMap<Book, BookDto>();
               //config.CreateMap<BookForCreationDto, Book>();

               //config.CreateMap<BookForUpdateDto, Book>();
               //config.CreateMap<Book, BookForUpdateDto>();
           });

            starWarsContext.StartWithFreshData();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}