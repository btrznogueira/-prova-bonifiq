using Microsoft.AspNetCore.Mvc;
using ProvaPub.Interfaces;
using ProvaPub.Models;

namespace ProvaPub.Controllers
{
	
	[ApiController]
	[Route("[controller]")]
	public class ProductController :  ControllerBase
	{
        /// <summary>
        /// Precisamos fazer algumas alterações:
        /// 1 - Não importa qual page é informada, sempre são retornados os mesmos resultados. Faça a correção.
        /// 2 - Altere os códigos abaixo para evitar o uso de "new", como em "new ProductService()". Utilize a Injeção de Dependência para resolver esse problema
        /// 3 - Dê uma olhada nos arquivos /Models/CustomerList e /Models/ProductList. Veja que há uma estrutura que se repete. 
        /// Como você faria pra criar uma estrutura melhor, com menos repetição de código? E quanto ao CustomerService/ProductService. Você acha que seria possível evitar a repetição de código?
        /// 
        /// </summary>
        
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
		{
            _productService = productService;
		}

        [HttpGet("products")]
        public PagedResult<Product> ListProducts(int page)
        {
            return _productService.ListProducts(page);
        }
    }
}
