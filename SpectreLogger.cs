using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace RipStainAPI;

public class SpectreConsoleLogger : ILogger
{
    private readonly string _name;
    private readonly LogLevel _minLogLevel;

    public SpectreConsoleLogger(string name, LogLevel minLogLevel)
    {
        _name = name;
        _minLogLevel = minLogLevel;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _minLogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        var logMessage = formatter(state, exception);

        switch (logLevel)
        {
            case LogLevel.Trace:
            case LogLevel.Debug:
                AnsiConsole.MarkupLine($"[blue][[DEBUG]] {logMessage}[/]");
                break;
            case LogLevel.Information:
                AnsiConsole.MarkupLine($"[green][[INFO]] {logMessage}[/]");
                break;
            case LogLevel.Warning:
                AnsiConsole.MarkupLine($"[yellow][[WARN]] {logMessage}[/]");
                break;
            case LogLevel.Error:
            case LogLevel.Critical:
                AnsiConsole.MarkupLine($"[red][[ERROR]] {logMessage}[/]");
                break;
            default:
                AnsiConsole.WriteLine(logMessage);
                break;
        }
    }
}

public class SpectreConsoleLoggerProvider : ILoggerProvider
{
    private readonly LogLevel _minLogLevel;

    public SpectreConsoleLoggerProvider(LogLevel minLogLevel)
    {
        _minLogLevel = minLogLevel;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new SpectreConsoleLogger(categoryName, _minLogLevel);
    }

    public void Dispose() { }
}
