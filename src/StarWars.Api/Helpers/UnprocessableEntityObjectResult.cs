using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace StarWars.Api.Helpers
{
    /// <summary>
    /// Reusable ModelState error information.
    /// </summary>
    public class UnprocessableEntityObjectResult : ObjectResult
    {
        /// <summary>
        /// Reusable ModelState error information.
        /// </summary>
        /// <param name="modelState"></param>
        public UnprocessableEntityObjectResult(ModelStateDictionary modelState)
            : base(new SerializableError(modelState))
        {
            if (modelState == null)
                throw new ArgumentNullException(nameof(modelState));

            StatusCode = 422;
        }
    }
}