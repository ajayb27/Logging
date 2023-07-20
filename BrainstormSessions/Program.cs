using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;
using System;

namespace BrainstormSessions
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                //var inMemorySink = new InMemorySink();
                Log.Logger = new LoggerConfiguration()
                                .MinimumLevel.Debug()
                                .Enrich.FromLogContext()
                                .Filter.ByIncludingOnly(evt => evt.Timestamp >= DateTimeOffset.Now.AddDays(-1))
                                //WriteTo.Sink(inMemorySink, LogEventLevel.Debug).
                                .WriteTo.Console()
                                .WriteTo.File(new RenderedCompactJsonFormatter(), "C:\\Users\\Ajay_Batthula\\Downloads\\Logging\\BrainstormSessions\\log.ndjson")
                                .WriteTo.Log4Net("serilog", Serilog.Events.LogEventLevel.Debug, null, true)
                                .CreateLogger();

                #region "comments"
                /*
                //.WriteTo.Seq(Environment.GetEnvironmentVariable("SEQ_URL") ?? "http://localhost:5001")
                //.WriteTo.Email(new EmailConnectionInfo
                //{
                //    FromEmail = appConfigs.Logger.EmailSettings.FromAddress,
                //    ToEmail = appConfigs.Logger.EmailSettings.ToAddress,
                //    MailServer = "smtp.gmail.com",
                //    NetworkCredentials = new NetworkCredential
                //    {
                //        UserName = appConfigs.Logger.EmailSettings.Username,
                //        Password = appConfigs.Logger.EmailSettings.Password
                //    },
                //    EnableSsl = true,
                //    Port = 465,
                //    EmailSubject = appConfigs.Logger.EmailSettings.EmailSubject
                //},
                //    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}",
                //    batchPostingLimit: 10
                //    , restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error
                //)
                */
                #endregion

                Log.Information("Starting up the application");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to start the application");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    //public class InMemorySink : ILogEventSink
    //{
    //    private readonly List<LogEvent> _logEvents = new List<LogEvent>();

    //    public IReadOnlyList<LogEvent> LogEvents => _logEvents;

    //    public void Emit(LogEvent logEvent)
    //    {
    //        _logEvents.Add(logEvent);
    //    }
    //}
}
