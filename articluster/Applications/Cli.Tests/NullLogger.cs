using System;

using Microsoft.Extensions.Logging;

namespace ArtiCluster.Applications.Cli.Tests;

internal sealed class NullLogger : ILogger
{
    public void Log<TState>( LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter ) {}

    public bool IsEnabled( LogLevel logLevel )
        => true;

    public IDisposable? BeginScope<TState>( TState state ) where TState : notnull
        => null;
}

internal sealed class NullLogger<T> : ILogger<T>
{
    public void Log<TState>( LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter ) {}

    public bool IsEnabled( LogLevel logLevel )
        => true;

    public IDisposable? BeginScope<TState>( TState state ) where TState : notnull
        => null;
}

internal sealed class NullLoggerFactory : ILoggerFactory
{
    public void Dispose() {}

    public ILogger CreateLogger( string categoryName )
        => new NullLogger();

    public void AddProvider( ILoggerProvider provider ) {}
}
