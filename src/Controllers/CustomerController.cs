using Microsoft.AspNetCore.Mvc;
using ProvaPub.Interfaces;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services;

namespace ProvaPub.Controllers
{
	
	[ApiController]
	[Route("[controller]")]
	public class CustomerController :  ControllerBase
	{
        /// <summary>
        /// Precisamos fazer algumas alterações:
        /// 1 - Não importa qual page é informada, sempre são retornados os mesmos resultados. Faça a correção.
        /// 2 - Altere os códigos abaixo para evitar o uso de "new", como em "new ProductService()". Utilize a Injeção de Dependência para resolver esse problema
        /// 3 - Dê uma olhada nos arquivos /Models/CustomerList e /Models/ProductList. Veja que há uma estrutura que se repete. 
        /// Como você faria pra criar uma estrutura melhor, com menos repetição de código? E quanto ao CustomerService/ProductService. Você acha que seria possível evitar a repetição de código?
        /// 
        /// </summary>
        
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
		{
			_customerService = customerService;
		}

        [HttpGet("customers")]
        public PagedResult<Customer> ListCustomers(int page)
        {
            return _customerService.ListCustomers(page);
        }

        /// <summary>
        /// O Código abaixo faz uma chmada para a regra de negócio que valida se um consumidor pode fazer uma compra.
        /// Crie o teste unitário para esse Service. Se necessário, faça as alterações no código para que seja possível realizar os testes.
        /// Tente criar a maior cobertura possível nos testes. 
        /// 
        /// Utilize o framework de testes que desejar. 
        /// Crie o teste na pasta "Tests" da solution
        /// </summary>
        [HttpGet("CanPurchase")]
        public async Task<bool> CanPurchase(int customerId, decimal purchaseValue)
        {
            return await _customerService.CanPurchase(customerId, purchaseValue);
        }
    }
}
