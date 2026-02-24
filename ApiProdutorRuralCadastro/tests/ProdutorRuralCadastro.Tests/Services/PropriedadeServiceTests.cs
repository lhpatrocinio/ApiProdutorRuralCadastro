using FluentAssertions;
using Moq;
using ProdutorRuralCadastro.Application.DTOs.Propriedade;
using ProdutorRuralCadastro.Application.Services;
using ProdutorRuralCadastro.Domain.Entities;
using ProdutorRuralCadastro.Domain.Interfaces;

namespace ProdutorRuralCadastro.Tests.Services;

public class PropriedadeServiceTests
{
    private readonly Mock<IPropriedadeRepository> _repoMock;
    private readonly PropriedadeService _service;

    public PropriedadeServiceTests()
    {
        _repoMock = new Mock<IPropriedadeRepository>();
        _service = new PropriedadeService(_repoMock.Object);
    }

    private static Propriedade CriarPropriedade(Guid? id = null, Guid? produtorId = null)
    {
        return new Propriedade
        {
            Id = id ?? Guid.NewGuid(),
            ProdutorId = produtorId ?? Guid.NewGuid(),
            Nome = "Fazenda Teste",
            Endereco = "Rua 1",
            Cidade = "São Paulo",
            Estado = "SP",
            CEP = "01000-000",
            Latitude = -23.5m,
            Longitude = -46.6m,
            AreaTotalHa = 100m,
            Ativo = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarPropriedadesAtivas()
    {
        var propriedades = new List<Propriedade> { CriarPropriedade(), CriarPropriedade() };
        _repoMock.Setup(r => r.GetAllAtivosAsync()).ReturnsAsync(propriedades);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByProdutorIdAsync_DeveRetornarPropriedadesDoProdutorId()
    {
        var produtorId = Guid.NewGuid();
        var propriedades = new List<Propriedade> { CriarPropriedade(produtorId: produtorId) };
        _repoMock.Setup(r => r.GetByProdutorIdAsync(produtorId)).ReturnsAsync(propriedades);

        var result = await _service.GetByProdutorIdAsync(produtorId);

        result.Should().HaveCount(1);
        result.First().ProdutorId.Should().Be(produtorId);
    }

    [Fact]
    public async Task GetByIdAsync_QuandoExiste_DeveRetornarPropriedade()
    {
        var id = Guid.NewGuid();
        var propriedade = CriarPropriedade(id: id);
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(propriedade);

        var result = await _service.GetByIdAsync(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Nome.Should().Be("Fazenda Teste");
    }

    [Fact]
    public async Task GetByIdAsync_QuandoNaoExiste_DeveRetornarNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Propriedade?)null);

        var result = await _service.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdWithTalhoesAsync_DeveRetornarApenasTalhoesAtivos()
    {
        var propriedade = CriarPropriedade();
        propriedade.Talhoes = new List<Talhao>
        {
            new() { Id = Guid.NewGuid(), Nome = "Ativo", Ativo = true, Status = 0, Cultura = new Cultura { Nome = "Soja" } },
            new() { Id = Guid.NewGuid(), Nome = "Inativo", Ativo = false, Status = 0 }
        };
        _repoMock.Setup(r => r.GetByIdWithTalhoesAsync(propriedade.Id)).ReturnsAsync(propriedade);

        var result = await _service.GetByIdWithTalhoesAsync(propriedade.Id);

        result.Should().NotBeNull();
        result!.Talhoes.Should().HaveCount(1);
        result.Talhoes.First().Nome.Should().Be("Ativo");
    }

    [Fact]
    public async Task CreateAsync_DeveCriarPropriedadeComAtivoTrue()
    {
        var request = new PropriedadeCreateRequest(
            Guid.NewGuid(), "Nova Fazenda", "Rua 1", "SP", "SP", "01000-000", -23.5m, -46.6m, 50m);

        _repoMock.Setup(r => r.AddAsync(It.IsAny<Propriedade>()))
            .ReturnsAsync((Propriedade p) => p);

        var result = await _service.CreateAsync(request);

        result.Should().NotBeNull();
        result.Nome.Should().Be("Nova Fazenda");
        result.Ativo.Should().BeTrue();
        _repoMock.Verify(r => r.AddAsync(It.Is<Propriedade>(p => p.Ativo == true)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_QuandoExiste_DeveAtualizarERetornar()
    {
        var id = Guid.NewGuid();
        var propriedade = CriarPropriedade(id: id);
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(propriedade);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Propriedade>())).Returns(Task.CompletedTask);

        var request = new PropriedadeUpdateRequest("Nome Atualizado", "End", "City", "RJ", "20000-000", -22m, -43m, 200m, true);

        var result = await _service.UpdateAsync(id, request);

        result.Should().NotBeNull();
        result!.Nome.Should().Be("Nome Atualizado");
    }

    [Fact]
    public async Task UpdateAsync_QuandoNaoExiste_DeveRetornarNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Propriedade?)null);

        var request = new PropriedadeUpdateRequest("Nome", null, null, null, null, null, null, null, true);
        var result = await _service.UpdateAsync(Guid.NewGuid(), request);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_QuandoExiste_DeveRetornarTrue()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(CriarPropriedade(id: id));
        _repoMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

        var result = await _service.DeleteAsync(id);

        result.Should().BeTrue();
        _repoMock.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_QuandoNaoExiste_DeveRetornarFalse()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Propriedade?)null);

        var result = await _service.DeleteAsync(Guid.NewGuid());

        result.Should().BeFalse();
    }
}
