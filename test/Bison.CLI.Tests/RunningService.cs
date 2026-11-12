using System.Diagnostics;

namespace Bison.CLI.Tests;

public sealed class RunningService : IDisposable
{
    public HttpClient Client { get; } = new HttpClient { BaseAddress = new Uri("http://localhost:5090") };
    private readonly Process process;

    public RunningService(string dataDirectory)
    {
        string serviceDirectory = Directory.CreateDirectory(Path.Combine(dataDirectory, "service", "run")).FullName;
        string serviceDll = Path.Combine(AppContext.BaseDirectory, "Bison.CSVDBService.dll");

        var psi = new ProcessStartInfo("dotnet")
        {
            WorkingDirectory = serviceDirectory,
            UseShellExecute = false,
        };
        psi.ArgumentList.Add(serviceDll);
        psi.ArgumentList.Add("--urls");
        psi.ArgumentList.Add("http://localhost:5090");

        process = Process.Start(psi)!;
        WaitUntilReady();
    }

    private void WaitUntilReady()
    {
        for (int attempt = 0; attempt < 50; attempt++)
        {
            try
            {
                var response = Client.GetAsync("/observations").Result;
                if (response.IsSuccessStatusCode)
                {
                    return;
                }
            }
            catch (AggregateException)
            {
               
            }
            Thread.Sleep(200);
        }
        Dispose();
        throw new Exception("Bison.CSVDBService did not start within 10 seconds.");
    }

    public void Dispose()
    {
        Client.Dispose();
        if (!process.HasExited)
        {
            process.Kill(entireProcessTree: true);
            process.WaitForExit();
        }
    }
}