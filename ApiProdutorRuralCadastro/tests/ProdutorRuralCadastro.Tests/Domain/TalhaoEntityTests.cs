using FluentAssertions;
using ProdutorRuralCadastro.Domain.Entities;

namespace ProdutorRuralCadastro.Tests.Domain;

public class TalhaoEntityTests
{
    [Theory]
    [InlineData(0, "Normal")]
    [InlineData(1, "Alerta")]
    [InlineData(2, "Crítico")]
    [InlineData(3, "Desconhecido")]
    [InlineData(-1, "Desconhecido")]
    [InlineData(99, "Desconhecido")]
    public void GetStatusDescricao_DeveRetornarDescricaoCorreta(int status, string esperado)
    {
        var talhao = new Talhao { Status = status };

        var resultado = talhao.GetStatusDescricao();

        resultado.Should().Be(esperado);
    }

    [Fact]
    public void Talhao_DeveIniciarComValoresPadrao()
    {
        var talhao = new Talhao();

        talhao.Status.Should().Be(0);
        talhao.Ativo.Should().BeTrue();
        talhao.Nome.Should().BeEmpty();
    }
}
