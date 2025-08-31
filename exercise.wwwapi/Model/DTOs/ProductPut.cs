using System.Text.Json.Serialization;

namespace exercise.wwwapi.Model.DTOs
{
    public class ProductPut
    {
        
        public string? Name { get; set; }
        public string? Category { get; set; }
        public int? Price { get; set; }

        
    }
}
