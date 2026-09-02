using Domain;
using Xunit;

namespace Domain.Tests;

public class AdditionBfTests
{
    [Theory]
    [InlineData(2, 3, 5)]
    [InlineData(0, 0, 0)]
    [InlineData(10, 15, 25)]
    [InlineData(100, 50, 150)]
    public void Addition_TwoBytes_ReturnsCorrectSum(byte a, byte b, byte expected)
    {
        // Arrange
        byte[] input = new byte[] { a, b };

        // Act & Assert
        byte[] output = BfInterpreter.Execute(BfPrograms.Addition, input);

        Assert.Single(output);
        Assert.Equal(expected, output[0]);
    }
}