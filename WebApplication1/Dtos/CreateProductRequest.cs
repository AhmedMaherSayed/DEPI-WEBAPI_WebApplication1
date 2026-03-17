namespace WebApplication1.Dtos
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public double Price { get; set; }

        public int CategoryId { get; set; }
    }
}
