using Xunit;

namespace MauiApp1.Tests
{
    public class PlayerValidatorTests
    {
        [Theory]
        [InlineData("Lionel Messi", "Delantero", "PSG", true)]
        [InlineData("", "Delantero", "PSG", false)]
        [InlineData("Lionel Messi", "", "PSG", false)]
        [InlineData("Lionel Messi", "Delantero", "", false)]
        public void ValidateAll_VariousInputs_ReturnsExpected(string nombre, string posicion, string equipo, bool expected)
        {
            // Act
            var result = MauiApp1.Utils.PlayerValidator.ValidateAll(nombre, posicion, equipo, out var error);

            // Assert
            Assert.Equal(expected, result);
            if (!result)
            {
                Assert.False(string.IsNullOrEmpty(error));
            }
        }
    }
}
