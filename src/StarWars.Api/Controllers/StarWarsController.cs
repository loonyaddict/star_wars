using Microsoft.AspNetCore.Mvc;
using StarWars.Api.Services;
using StarWars.API.Services;
using System;

namespace StarWars.Api.Controllers
{
    /// <summary>
    /// Base class for StarWars.Api Controllers.
    /// </summary>
    public abstract class StarWarsController : Controller
    {
        /// <summary>
        /// StarWars repository
        /// </summary>
        protected readonly IStarWarsRepository repository;
        /// <summary>
        /// PropertyMapping services. Check Services/PropertyMapping/
        /// </summary>
        protected readonly IPropertyMappingService propertyMappingService;
        /// <summary>
        /// TypeHelper services. Check Services/TypeHelperService
        /// </summary>
        protected readonly ITypeHelperService typeHelperService;

        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="propertyMappingService"></param>
        /// <param name="typeHelperService"></param>
        public StarWarsController(IStarWarsRepository repository,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            this.repository = repository;
            this.propertyMappingService = propertyMappingService;
            this.typeHelperService = typeHelperService;
        }

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