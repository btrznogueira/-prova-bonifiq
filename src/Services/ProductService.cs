using ProvaPub.Commons;
using ProvaPub.Interfaces;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
	public class ProductService : IProductService 
	{
		TestDbContext _ctx;

		public ProductService(TestDbContext ctx)
		{
			_ctx = ctx;
		}

        public PagedResult<Product> ListProducts(int page)
        {
            return _ctx.Products
                .OrderBy(p => p.Id)
                .ToPagedResult(page, 10);
        }
    }
}
