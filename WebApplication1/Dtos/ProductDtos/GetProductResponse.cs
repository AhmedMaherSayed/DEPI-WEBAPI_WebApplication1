using WebApplication1.Entities;

namespace WebApplication1.Dtos.ProductDtos
{
    public class GetProductResponse
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
