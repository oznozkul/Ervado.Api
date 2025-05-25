namespace Ervado.Application.Features.Models.Queries.GetModelsByBrand
{
    public class ModelListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int ProductCount { get; set; }
    }
} 