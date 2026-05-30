using Xunit;
using FluentAssertions;

namespace ControlEquiposElectronicos.Tests;

public class EquipoServiceTests
{
    [Fact]
    public void PruebaInicial_DeberiaPasar()
    {
        // Arrange
        var resultado = 2 + 2;

        // Assert
        resultado.Should().Be(4);
    }
}
