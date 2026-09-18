using System.Diagnostics;

namespace Bison.CLI.Tests;

public class EndToEndTests
{
    private static string RunBison(string workingDirectory, params string[] args)
    {
        string cliDll = Path.Combine(AppContext.BaseDirectory, "Bison.CLI.dll");

        var psi = new ProcessStartInfo("dotnet")
        {
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            UseShellExecute = false,
        };
        psi.ArgumentList.Add(cliDll);
        foreach (var arg in args)
        {
            psi.ArgumentList.Add(arg);
        }

        using var process = Process.Start(psi)!;
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return output;
    }

    [Fact]
    public void Read_WithSeededData_PrintsExpectedText()
    {
        var workDir = Directory.CreateTempSubdirectory();
        File.WriteAllText(Path.Combine(workDir.FullName, "bison_observe_cli_db.csv"),
            "Author,Observation,Timestamp,id,Location\r\nropf,A bird at DR Byen,1690891760,1,DR Byen\r\n");

        string output = RunBison(workDir.FullName, "read");

        Assert.Contains("ropf @ A bird at DR Byen: 08-01 12:09:20 (id: 1, Location: DR Byen)", output);
    }

    [Fact]
    public void Observe_Penguin_IsStoredWithCorrectValues()
    {
        var workDir = Directory.CreateTempSubdirectory();

        RunBison(workDir.FullName, "observe", "Penguin", "Antarctica");

        string csv = File.ReadAllText(Path.Combine(workDir.FullName, "bison_observe_cli_db.csv"));
        Assert.Contains("Penguin", csv);
        Assert.Contains(Environment.UserName, csv);
        Assert.Contains("Antarctica", csv);
    }

    [Fact]
    public void Location_PrintsOnlyMatchingObservations()
    {
        var workDir = Directory.CreateTempSubdirectory();
        File.WriteAllText(Path.Combine(workDir.FullName, "bison_observe_cli_db.csv"),
            "Author,Observation,Timestamp,id,Location\r\n" +
            "ropf,A bird at DR Byen,1690891760,1,DR Byen\r\n" +
            "adho,A duck at Fælledparken,1690891761,2,Fælledparken\r\n");

        string output = RunBison(workDir.FullName, "location", "DR Byen");

        Assert.Contains("ropf", output);
        Assert.DoesNotContain("adho", output);
    }
}