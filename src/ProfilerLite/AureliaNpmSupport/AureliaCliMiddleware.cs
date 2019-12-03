using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.Logging;

namespace ProfilerLite.AureliaNpmSupport
{
    internal static class AureliaCliMiddleware
    {
        private static TimeSpan RegexMatchTimeout = TimeSpan.FromSeconds(5.0);
        private const string LogCategoryName = "Microsoft.AspNetCore.SpaServices";

        public static void Attach(ISpaBuilder spaBuilder, string npmScriptName)
        {
            string sourcePath = spaBuilder.Options.SourcePath;
            if (string.IsNullOrEmpty(sourcePath))
                throw new ArgumentException("Cannot be null or empty", "sourcePath");
            if (string.IsNullOrEmpty(npmScriptName))
                throw new ArgumentException("Cannot be null or empty", nameof(npmScriptName));
            Task<Uri> targetUriTask = AureliaCliMiddleware.StartAureliaCliServerAsync(sourcePath, npmScriptName, null)
                .ContinueWith<Uri>((Func<Task<AureliaCliMiddleware.AureliaCliServerInfo>, Uri>) (task => new UriBuilder("http", "localhost", task.Result.Port).Uri));
            spaBuilder.UseProxyToSpaDevelopmentServer((Func<Task<Uri>>) (() =>
            {
                TimeSpan startupTimeout = spaBuilder.Options.StartupTimeout;
                return targetUriTask.WithTimeout<Uri>(startupTimeout,
                    "The Angular CLI process did not start listening for requests " + string.Format("within the timeout period of {0} seconds. ", (object) startupTimeout.Seconds) +
                    "Check the log output for error information.");
            }));
        }

        private static async Task<AureliaCliMiddleware.AureliaCliServerInfo> StartAureliaCliServerAsync(
            string sourcePath,
            string npmScriptName,
            ILogger logger)
        {
            int availablePort = TcpPortFinder.FindAvailablePort();
            //logger.LogInformation(string.Format("Starting @angular/cli on port {0}...", (object) availablePort));
            NpmScriptRunner npmScriptRunner =
                new NpmScriptRunner(sourcePath, npmScriptName, string.Format("--port {0}", (object) availablePort), (IDictionary<string, string>) null);
            Match match;
            using (EventedStreamStringReader stdErrReader = new EventedStreamStringReader(npmScriptRunner.StdErr))
            {
                try
                {
                    match = await npmScriptRunner.StdOut.WaitForMatch(new Regex("Project is running at (http\\S+)", RegexOptions.None, AureliaCliMiddleware.RegexMatchTimeout));
                }
                catch (EndOfStreamException ex)
                {
                    throw new InvalidOperationException(
                        "The NPM script '" + npmScriptName + "' exited without indicating that the Angular CLI was listening for requests. The error output was: " +
                        stdErrReader.ReadAsString(), (Exception) ex);
                }
            }
            Uri cliServerUri = new Uri(match.Groups[1].Value);
            var serverInfo = new AureliaCliMiddleware.AureliaCliServerInfo()
            {
                Port = cliServerUri.Port
            };
            await AureliaCliMiddleware.WaitForAureliaCliServerToAcceptRequests(cliServerUri);
            return serverInfo;
        }

        private static async Task WaitForAureliaCliServerToAcceptRequests(Uri cliServerUri)
        {
            int timeoutMilliseconds = 1000;
            using (HttpClient client = new HttpClient())
            {
                while (true)
                {
                    do
                    {
                        int num;
                        do
                        {
                            try
                            {
                                HttpResponseMessage httpResponseMessage =
                                    await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, cliServerUri), new CancellationTokenSource(timeoutMilliseconds).Token);
                                goto label_12;
                            }
                            catch (Exception)
                            {
                                num = 1;
                            }
                        } while (num != 1);
                        await Task.Delay(500);
                    } while (timeoutMilliseconds >= 10000);
                    timeoutMilliseconds += 3000;
                }
            }
            label_12: ;
        }

        private class AureliaCliServerInfo
        {
            public int Port { get; set; }
        }
    }
}