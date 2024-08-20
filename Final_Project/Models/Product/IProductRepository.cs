namespace Final_Project.Models.Product
{
    public interface IProductRepository
    {

        Task<IEnumerable<Product>> GetProductsByCategory(int categoryId);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);

       
        public void AddProduct(Product P);
    }
}
