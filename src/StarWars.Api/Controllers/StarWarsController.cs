using Microsoft.AspNetCore.Mvc;
using StarWars.Api.Services;
using System;

namespace StarWars.Api.Controllers
{
    /// <summary>
    /// Base class for StarWars.Api Controllers.
    /// </summary>
    public abstract class StarWarsController : Controller
    {
        protected readonly IStarWarsRepository repository;

        public StarWarsController(IStarWarsRepository repository) =>
            this.repository = repository;

        /// <summary>
        /// Save repository. Throws merror with exceptionMessage on fail.
        /// </summary>
        /// <param name="exceptionMessage"></param>
        protected void Save(string exceptionMessage = "")
        {
            if (!repository.Save())
                throw new Exception(exceptionMessage);
        }
    }
}