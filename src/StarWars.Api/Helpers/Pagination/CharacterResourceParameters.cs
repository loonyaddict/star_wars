namespace StarWars.Api.Helpers.Pagination
{
    public class CharacterResourceParameters
    {
        public string SearchQuery { get; set; }
        public string OrderBy { get; set; } = "Name";
        public string Fields {get; set; }
        public string Planet {get; set; }

        private readonly int maxPageSize = 20;
        private int pageSize = 10;

        public int PageNumber { get; set; } = 1;

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