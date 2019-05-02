namespace StarWars.Api.Helpers.Pagination
{
    /// <summary>
    /// Parameters for GetCharacters. Enables paging, search query and filtering.
    /// </summary>
    public class CharacterResourceParameters
    {
        /// <summary>
        /// "an"                                                                            
        /// should return Cassian Andor, Lando Calrissian and characters from planet Ando and Takodana.
        /// Search for characters that contain SearchQuery string in name or planet.
        /// </summary>
        public string SearchQuery { get; set; }
        /// <summary>
        /// "planet desc, name"                                                                         
        /// Set sorting order.
        /// </summary>
        public string OrderBy { get; set; } = "Name";
        
        /// <summary>
        /// "id, name"                                                              
        /// Select fields to display.
        /// </summary>
        public string Fields {get; set; }

        /// <summary>
        /// "Anoat"                                                                                
        /// Search for characters from specified planet.
        /// </summary>
        public string Planet {get; set; }

        private readonly int maxPageSize = 20;
        private int pageSize = 10;

        /// <summary>
        /// (default) 1                                                                                                                  
        /// Page to display
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// (default) 10                                            
        /// Number of records to display on single page. Max: 20.
        /// </summary>
        public int PageSize
        {
            get => pageSize;
            set
            {
                pageSize = (value > maxPageSize)
                    ? maxPageSize
                    : value;
            }
        }
    }
}