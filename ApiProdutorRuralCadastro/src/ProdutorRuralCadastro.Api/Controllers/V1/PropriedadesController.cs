using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ProdutorRuralCadastro.Application.DTOs.Propriedade;
using ProdutorRuralCadastro.Application.DTOs.Talhao;
using ProdutorRuralCadastro.Application.Services.Interfaces;

namespace ProdutorRuralCadastro.Api.Controllers.V1
{
    /// <summary>
    /// Controller para gerenciamento de Propriedades Rurais
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PropriedadesController : ControllerBase
    {
        private readonly IPropriedadeService _propriedadeService;
        private readonly ITalhaoService _talhaoService;

        public PropriedadesController(IPropriedadeService propriedadeService, ITalhaoService talhaoService)
        {
            _propriedadeService = propriedadeService;
            _talhaoService = talhaoService;
        }

        /// <summary>
        /// Lista todas as propriedades
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PropriedadeResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PropriedadeResponse>>> GetAll()
        {
            var propriedades = await _propriedadeService.GetAllAsync();
            return Ok(propriedades);
        }

        /// <summary>
        /// Lista propriedades de um produtor específico
        /// </summary>
        [HttpGet("produtor/{produtorId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<PropriedadeResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PropriedadeResponse>>> GetByProdutorId(Guid produtorId)
        {
            var propriedades = await _propriedadeService.GetByProdutorIdAsync(produtorId);
            return Ok(propriedades);
        }

        /// <summary>
        /// Obtém uma propriedade pelo ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PropriedadeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PropriedadeResponse>> GetById(Guid id)
        {
            var propriedade = await _propriedadeService.GetByIdAsync(id);
            if (propriedade == null)
                return NotFound(new { message = "Propriedade não encontrada" });

            return Ok(propriedade);
        }

        /// <summary>
        /// Obtém uma propriedade com seus talhões
        /// </summary>
        [HttpGet("{id:guid}/talhoes")]
        [ProducesResponseType(typeof(PropriedadeComTalhoesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PropriedadeComTalhoesResponse>> GetByIdWithTalhoes(Guid id)
        {
            var propriedade = await _propriedadeService.GetByIdWithTalhoesAsync(id);
            if (propriedade == null)
                return NotFound(new { message = "Propriedade não encontrada" });

            return Ok(propriedade);
        }

        /// <summary>
        /// Cadastra um novo talhão vinculado a uma propriedade
        /// </summary>
        [HttpPost("{id:guid}/talhoes")]
        [ProducesResponseType(typeof(TalhaoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TalhaoResponse>> CreateTalhao(Guid id, [FromBody] TalhaoCreateForPropriedadeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verificar se propriedade existe
            var propriedade = await _propriedadeService.GetByIdAsync(id);
            if (propriedade == null)
                return NotFound(new { message = "Propriedade não encontrada" });

            // Criar o request completo com o PropriedadeId
            var talhaoRequest = new TalhaoCreateRequest(
                PropriedadeId: id,
                CulturaId: request.CulturaId,
                Nome: request.Nome,
                Codigo: request.Codigo,
                AreaHa: request.AreaHa,
                Latitude: request.Latitude,
                Longitude: request.Longitude,
                DataPlantio: request.DataPlantio,
                PrevisaoColheita: request.PrevisaoColheita
            );

            var talhao = await _talhaoService.CreateAsync(talhaoRequest);
            return CreatedAtAction("GetById", "Talhoes", new { id = talhao.Id }, talhao);
        }

        /// <summary>
        /// Cadastra uma nova propriedade
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(PropriedadeResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PropriedadeResponse>> Create([FromBody] PropriedadeCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var propriedade = await _propriedadeService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = propriedade.Id }, propriedade);
        }

        /// <summary>
        /// Atualiza uma propriedade existente
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(PropriedadeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PropriedadeResponse>> Update(Guid id, [FromBody] PropriedadeUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var propriedade = await _propriedadeService.UpdateAsync(id, request);
            if (propriedade == null)
                return NotFound(new { message = "Propriedade não encontrada" });

            return Ok(propriedade);
        }

        /// <summary>
        /// Remove uma propriedade (soft delete)
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _propriedadeService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = "Propriedade não encontrada" });

            return NoContent();
        }
    }
}
