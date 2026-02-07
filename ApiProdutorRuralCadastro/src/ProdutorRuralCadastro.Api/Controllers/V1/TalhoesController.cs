using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ProdutorRuralCadastro.Application.DTOs.Talhao;
using ProdutorRuralCadastro.Application.Services.Interfaces;

namespace ProdutorRuralCadastro.Api.Controllers.V1
{
    /// <summary>
    /// Controller para gerenciamento de Talhões (áreas de plantio)
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TalhoesController : ControllerBase
    {
        private readonly ITalhaoService _talhaoService;

        public TalhoesController(ITalhaoService talhaoService)
        {
            _talhaoService = talhaoService;
        }

        /// <summary>
        /// Lista todos os talhões
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TalhaoResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TalhaoResponse>>> GetAll()
        {
            var talhoes = await _talhaoService.GetAllAsync();
            return Ok(talhoes);
        }

        /// <summary>
        /// Lista talhões de uma propriedade específica
        /// </summary>
        [HttpGet("propriedade/{propriedadeId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<TalhaoResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TalhaoResponse>>> GetByPropriedadeId(Guid propriedadeId)
        {
            var talhoes = await _talhaoService.GetByPropriedadeIdAsync(propriedadeId);
            return Ok(talhoes);
        }

        /// <summary>
        /// Lista talhões de um produtor (todas as propriedades)
        /// </summary>
        [HttpGet("produtor/{produtorId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<TalhaoComDetalhesResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TalhaoComDetalhesResponse>>> GetByProdutorId(Guid produtorId)
        {
            var talhoes = await _talhaoService.GetByProdutorIdAsync(produtorId);
            return Ok(talhoes);
        }

        /// <summary>
        /// Obtém um talhão pelo ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(TalhaoComDetalhesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TalhaoComDetalhesResponse>> GetById(Guid id)
        {
            var talhao = await _talhaoService.GetByIdWithDetailsAsync(id);
            if (talhao == null)
                return NotFound(new { message = "Talhão não encontrado" });

            return Ok(talhao);
        }

        /// <summary>
        /// Cadastra um novo talhão
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(TalhaoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TalhaoResponse>> Create([FromBody] TalhaoCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var talhao = await _talhaoService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = talhao.Id }, talhao);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um talhão existente
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(TalhaoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TalhaoResponse>> Update(Guid id, [FromBody] TalhaoUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var talhao = await _talhaoService.UpdateAsync(id, request);
                if (talhao == null)
                    return NotFound(new { message = "Talhão não encontrado" });

                return Ok(talhao);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza o status de um talhão (Normal, Alerta, Crítico)
        /// </summary>
        [HttpPatch("{id:guid}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] TalhaoUpdateStatusRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _talhaoService.UpdateStatusAsync(id, request);
            if (!updated)
                return NotFound(new { message = "Talhão não encontrado" });

            return NoContent();
        }

        /// <summary>
        /// Remove um talhão (soft delete)
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _talhaoService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = "Talhão não encontrado" });

            return NoContent();
        }
    }
}
