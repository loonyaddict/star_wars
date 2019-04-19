using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using StarWars.Characters;
using StarWars.Utils.JsonUtils;
using System.Collections.Generic;

namespace StarWars
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGreeter, BasicGreeter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IGreeter greeter)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                List<MovieCharacter> characters = new List<MovieCharacter>
                {
                    new MovieCharacter("Luke"),
                    new MovieCharacter("Leia"),
                    new MovieCharacter("Wookie")
                };

                await context.Response.WriteAsync(greeter.GreetingMessage);
                await context.Response.WriteAsync(characters.ToJson());
            });
        }
    }
}