using ProvaPub.Models;

namespace ProvaPub.Interfaces
{
    public interface IProductService
    {
        PagedResult<Product> ListProducts(int page);
    }
}
