using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ProdutorRuralCadastro.Application.DTOs.Cultura;
using ProdutorRuralCadastro.Application.Services.Interfaces;

namespace ProdutorRuralCadastro.Api.Controllers.V1
{
    /// <summary>
    /// Controller para gerenciamento de Culturas
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CulturasController : ControllerBase
    {
        private readonly ICulturaService _culturaService;

        public CulturasController(ICulturaService culturaService)
        {
            _culturaService = culturaService;
        }

        /// <summary>
        /// Lista todas as culturas disponíveis
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CulturaResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CulturaResponse>>> GetAll()
        {
            var culturas = await _culturaService.GetAllAsync();
            return Ok(culturas);
        }

        /// <summary>
        /// Obtém uma cultura pelo ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CulturaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CulturaResponse>> GetById(Guid id)
        {
            var cultura = await _culturaService.GetByIdAsync(id);
            if (cultura == null)
                return NotFound(new { message = "Cultura não encontrada" });

            return Ok(cultura);
        }
    }
}
