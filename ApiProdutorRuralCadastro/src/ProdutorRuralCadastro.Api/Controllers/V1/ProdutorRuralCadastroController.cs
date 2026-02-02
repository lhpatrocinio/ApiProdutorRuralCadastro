using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ProdutorRuralCadastro.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProdutorRuralCadastroController : ControllerBase
    {
    }
}
