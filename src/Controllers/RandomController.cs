using Microsoft.AspNetCore.Mvc;
using ProvaPub.Services;

namespace ProvaPub.Controllers
{
	/// <summary>
	/// Ao rodar o código abaixo o serviço deveria sempre retornar um número diferente, mas ele fica retornando sempre o mesmo número.
	/// 1 - Faça as alterações para que o retorno seja sempre diferente
	/// 2 - Tome cuidado 
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	public class RandomController :  ControllerBase
	{
		private readonly RandomService _randomService;

		public RandomController(RandomService randomService)
		{
			_randomService = randomService;
		}
		[HttpGet]
		public async Task<int> Index()
		{
			return await _randomService.GetRandom();
		}
	}
}
