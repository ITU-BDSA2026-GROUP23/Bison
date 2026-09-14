namespace Bison.CLI.Tests;

public class UserInterfaceTests
{
    [Fact]
    public void FormatTimestamp_KnownValue_ReturnsExpectedText()
    {
        String result = UserInterface.FormatTimestamp(1690891760);

        Assert.Equal("08-01 12:09:20", result);
    }
}