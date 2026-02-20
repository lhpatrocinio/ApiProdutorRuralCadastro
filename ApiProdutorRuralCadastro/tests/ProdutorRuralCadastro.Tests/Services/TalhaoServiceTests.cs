using FluentAssertions;
using Moq;
using ProdutorRuralCadastro.Application.DTOs.Talhao;
using ProdutorRuralCadastro.Application.Services;
using ProdutorRuralCadastro.Domain.Entities;
using ProdutorRuralCadastro.Domain.Interfaces;

namespace ProdutorRuralCadastro.Tests.Services;

public class TalhaoServiceTests
{
    private readonly Mock<ITalhaoRepository> _talhaoRepoMock;
    private readonly Mock<IPropriedadeRepository> _propriedadeRepoMock;
    private readonly Mock<ICulturaRepository> _culturaRepoMock;
    private readonly TalhaoService _service;

    public TalhaoServiceTests()
    {
        _talhaoRepoMock = new Mock<ITalhaoRepository>();
        _propriedadeRepoMock = new Mock<IPropriedadeRepository>();
        _culturaRepoMock = new Mock<ICulturaRepository>();
        _service = new TalhaoService(_talhaoRepoMock.Object, _propriedadeRepoMock.Object, _culturaRepoMock.Object);
    }

    private static Talhao CriarTalhao(Guid? id = null, int status = 0) => new()
    {
        Id = id ?? Guid.NewGuid(),
        PropriedadeId = Guid.NewGuid(),
        CulturaId = Guid.NewGuid(),
        Nome = "Talhão 1",
        Codigo = "T001",
        AreaHa = 10m,
        Status = status,
        Latitude = -23m,
        Longitude = -46m,
        Ativo = true,
        CreatedAt = DateTime.UtcNow
    };

    [Fact]
    public async Task GetAllAsync_DeveRetornarTalhoesAtivos()
    {
        var talhoes = new List<Talhao> { CriarTalhao(), CriarTalhao() };
        _talhaoRepoMock.Setup(r => r.GetAllAtivosAsync()).ReturnsAsync(talhoes);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_QuandoExiste_DeveRetornarTalhao()
    {
        var id = Guid.NewGuid();
        var talhao = CriarTalhao(id: id);
        _talhaoRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(talhao);

        var result = await _service.GetByIdAsync(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
    }

    [Fact]
    public async Task GetByIdAsync_QuandoNaoExiste_DeveRetornarNull()
    {
        _talhaoRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Talhao?)null);

        var result = await _service.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_QuandoPropriedadeExisteECulturaExiste_DeveCriarTalhao()
    {
        var propId = Guid.NewGuid();
        var cultId = Guid.NewGuid();
        _propriedadeRepoMock.Setup(r => r.GetByIdAsync(propId)).ReturnsAsync(new Propriedade { Id = propId });
        _culturaRepoMock.Setup(r => r.GetByIdAsync(cultId)).ReturnsAsync(new Cultura { Id = cultId });
        _talhaoRepoMock.Setup(r => r.AddAsync(It.IsAny<Talhao>())).ReturnsAsync((Talhao t) => t);

        var request = new TalhaoCreateRequest(propId, cultId, "Novo Talhão", "T002", 5m, -23m, -46m, null, null);
        var result = await _service.CreateAsync(request);

        result.Should().NotBeNull();
        result.Nome.Should().Be("Novo Talhão");
        result.Status.Should().Be(0);
    }

    [Fact]
    public async Task CreateAsync_QuandoPropriedadeNaoExiste_DeveLancarArgumentException()
    {
        _propriedadeRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Propriedade?)null);

        var request = new TalhaoCreateRequest(Guid.NewGuid(), Guid.NewGuid(), "Teste", null, null, null, null, null, null);

        var act = async () => await _service.CreateAsync(request);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Propriedade não encontrada");
    }

    [Fact]
    public async Task CreateAsync_QuandoCulturaNaoExiste_DeveLancarArgumentException()
    {
        var propId = Guid.NewGuid();
        _propriedadeRepoMock.Setup(r => r.GetByIdAsync(propId)).ReturnsAsync(new Propriedade { Id = propId });
        _culturaRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Cultura?)null);

        var request = new TalhaoCreateRequest(propId, Guid.NewGuid(), "Teste", null, null, null, null, null, null);

        var act = async () => await _service.CreateAsync(request);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Cultura não encontrada");
    }

    [Fact]
    public async Task UpdateAsync_QuandoExiste_DeveAtualizarERetornar()
    {
        var id = Guid.NewGuid();
        var cultId = Guid.NewGuid();
        var talhao = CriarTalhao(id: id);
        _talhaoRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(talhao);
        _culturaRepoMock.Setup(r => r.GetByIdAsync(cultId)).ReturnsAsync(new Cultura { Id = cultId });
        _talhaoRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Talhao>())).Returns(Task.CompletedTask);

        var request = new TalhaoUpdateRequest(cultId, "Atualizado", "T003", 20m, -22m, -45m, null, null, true);
        var result = await _service.UpdateAsync(id, request);

        result.Should().NotBeNull();
        result!.Nome.Should().Be("Atualizado");
    }

    [Fact]
    public async Task UpdateAsync_QuandoNaoExiste_DeveRetornarNull()
    {
        _talhaoRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Talhao?)null);

        var request = new TalhaoUpdateRequest(Guid.NewGuid(), "Nome", null, null, null, null, null, null, true);
        var result = await _service.UpdateAsync(Guid.NewGuid(), request);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_QuandoCulturaNaoExiste_DeveLancarArgumentException()
    {
        var id = Guid.NewGuid();
        _talhaoRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(CriarTalhao(id: id));
        _culturaRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Cultura?)null);

        var request = new TalhaoUpdateRequest(Guid.NewGuid(), "Nome", null, null, null, null, null, null, true);

        var act = async () => await _service.UpdateAsync(id, request);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Cultura não encontrada");
    }

    [Fact]
    public async Task UpdateStatusAsync_QuandoExiste_DeveRetornarTrue()
    {
        var id = Guid.NewGuid();
        _talhaoRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(CriarTalhao(id: id));
        _talhaoRepoMock.Setup(r => r.UpdateStatusAsync(id, 1, "Alerta")).Returns(Task.CompletedTask);

        var request = new TalhaoUpdateStatusRequest(1, "Alerta");
        var result = await _service.UpdateStatusAsync(id, request);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateStatusAsync_QuandoNaoExiste_DeveRetornarFalse()
    {
        _talhaoRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Talhao?)null);

        var request = new TalhaoUpdateStatusRequest(1, null);
        var result = await _service.UpdateStatusAsync(Guid.NewGuid(), request);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_QuandoExiste_DeveRetornarTrue()
    {
        var id = Guid.NewGuid();
        _talhaoRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(CriarTalhao(id: id));
        _talhaoRepoMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

        var result = await _service.DeleteAsync(id);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_QuandoNaoExiste_DeveRetornarFalse()
    {
        _talhaoRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Talhao?)null);

        var result = await _service.DeleteAsync(Guid.NewGuid());

        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdWithDetailsAsync_QuandoExiste_DeveRetornarComDetalhes()
    {
        var id = Guid.NewGuid();
        var talhao = CriarTalhao(id: id);
        talhao.Propriedade = new Propriedade { Id = Guid.NewGuid(), Nome = "Fazenda", Cidade = "SP", Estado = "SP", AreaTotalHa = 100 };
        talhao.Cultura = new Cultura { Id = Guid.NewGuid(), Nome = "Soja" };
        _talhaoRepoMock.Setup(r => r.GetByIdWithDetailsAsync(id)).ReturnsAsync(talhao);

        var result = await _service.GetByIdWithDetailsAsync(id);

        result.Should().NotBeNull();
        result!.Propriedade.Should().NotBeNull();
        result.Propriedade!.Nome.Should().Be("Fazenda");
        result.Cultura.Should().NotBeNull();
        result.Cultura!.Nome.Should().Be("Soja");
    }
}
