using laundryServer.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace laundryServer.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _configurationService;

        public ConfigurationController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        [HttpGet("configuration")]
        public async Task<IActionResult> GetConfiguration()
        {
            // Appel au service pour obtenir les configurations
            var configuration = await _configurationService.GetConfigurationAsync();

            // Vérification si une réponse vide est renvoyée


            return Ok(configuration);

            // Utilisation d'un logger pour enregistrer l'exception
            // _logger.LogError(ex, "Erreur lors de la récupération de la configuration.");

        }
    }
}
