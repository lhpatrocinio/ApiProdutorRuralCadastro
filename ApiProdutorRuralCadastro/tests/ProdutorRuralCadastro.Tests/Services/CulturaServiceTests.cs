using FluentAssertions;
using Moq;
using ProdutorRuralCadastro.Application.Services;
using ProdutorRuralCadastro.Domain.Entities;
using ProdutorRuralCadastro.Domain.Interfaces;

namespace ProdutorRuralCadastro.Tests.Services;

public class CulturaServiceTests
{
    private readonly Mock<ICulturaRepository> _repoMock;
    private readonly CulturaService _service;

    public CulturaServiceTests()
    {
        _repoMock = new Mock<ICulturaRepository>();
        _service = new CulturaService(_repoMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarCulturasAtivas()
    {
        var culturas = new List<Cultura>
        {
            new() { Id = Guid.NewGuid(), Nome = "Soja", Descricao = "Soja desc", UmidadeIdealMin = 40, UmidadeIdealMax = 70, TempIdealMin = 20, TempIdealMax = 30, Ativo = true },
            new() { Id = Guid.NewGuid(), Nome = "Milho", Ativo = true }
        };
        _repoMock.Setup(r => r.GetAllAtivosAsync()).ReturnsAsync(culturas);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
        result.First().Nome.Should().Be("Soja");
    }

    [Fact]
    public async Task GetByIdAsync_QuandoExiste_DeveRetornarCultura()
    {
        var id = Guid.NewGuid();
        var cultura = new Cultura
        {
            Id = id, Nome = "Café", Descricao = "Café arábica",
            UmidadeIdealMin = 50, UmidadeIdealMax = 80,
            TempIdealMin = 18, TempIdealMax = 26, Ativo = true
        };
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(cultura);

        var result = await _service.GetByIdAsync(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Nome.Should().Be("Café");
        result.TempIdealMin.Should().Be(18);
    }

    [Fact]
    public async Task GetByIdAsync_QuandoNaoExiste_DeveRetornarNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Cultura?)null);

        var result = await _service.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_QuandoVazio_DeveRetornarListaVazia()
    {
        _repoMock.Setup(r => r.GetAllAtivosAsync()).ReturnsAsync(new List<Cultura>());

        var result = await _service.GetAllAsync();

        result.Should().BeEmpty();
    }
}
