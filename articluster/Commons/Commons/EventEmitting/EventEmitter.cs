using System;
using System.Collections.Generic;

namespace ArtiCluster.Commons.EventEmitting;

/// <summary>
/// Default implementation of <see cref="IEventEmitter"/>.
/// </summary>
public sealed class EventEmitter : IEventEmitter
{
    // object : 任意のIObservable<T>型を出し入れしたいため
    private readonly IDictionary<Type, object> registered = new Dictionary<Type, object>();

    /// <inheritdoc />
    public IDisposable Subscribe<TEvent>( IObserver<TEvent> observer ) where TEvent : IEvent
    {
        return AsObservable<TEvent>().Subscribe( observer );
    }

    /// <inheritdoc />
    public void Emit<TEvent>( TEvent evt ) where TEvent : IEvent
    {
        if( TryGetObservable( out IObservable<TEvent> observable ) )
        {
            ( (EventObservable<TEvent>)observable ).Publish( evt );
        }
    }

    /// <inheritdoc />
    public IObservable<TEvent> AsObservable<TEvent>() where TEvent : IEvent
    {
        if( TryGetObservable( out IObservable<TEvent> result ) )
        {
            return result;
        }

        result = new EventObservable<TEvent>();

        if( !registered.TryAdd( typeof( TEvent ), result ) )
        {
            throw new InvalidOperationException( $"Failed to register {typeof( TEvent ).Name}." );
        }

        return result;
    }

    public void Dispose()
    {
        registered.Clear();
    }

    private bool TryGetObservable<TEvent>( out IObservable<TEvent> result ) where TEvent : IEvent
    {
        var type = typeof( TEvent );

        result = NullObservable<TEvent>.Instance;

        if( !registered.TryGetValue( type, out var value ) )
        {
            return false;
        }

        result = (IObservable<TEvent>)value;

        return true;
    }

    #region IObservable Implementation

    #region Null Observable

    private class NullObservable<TEvent> : IObservable<TEvent> where TEvent : IEvent
    {
        public static readonly IObservable<TEvent> Instance = new NullObservable<TEvent>();

        private NullObservable() {}

        /// <inheritdoc />
        public IDisposable Subscribe( IObserver<TEvent> observer )
        {
            return new NullDisposer();
        }

        private class NullDisposer : IDisposable
        {
            public void Dispose() {}
        }
    }

    #endregion ~Null Observable

    private class EventObservable<TEvent> : IObservable<TEvent> where TEvent : IEvent
    {
        private readonly List<IObserver<TEvent>> subscribers = [];

        #region IObservable

        /// <inheritdoc />
        public IDisposable Subscribe( IObserver<TEvent> observer )
        {
            subscribers.Add( observer );

            return new AnonymousDisposer( () =>
                {
                    subscribers.Remove( observer );
                }
            );
        }

        #endregion ~IObservable

        public void Publish( TEvent evt )
        {
            var snapshot = new List<IObserver<TEvent>>( subscribers );

            foreach( var subscriber in snapshot )
            {
                try
                {
                    subscriber.OnNext( evt );
                }
                catch( Exception e )
                {
                    subscriber.OnError( e );
                }
            }
        }
    }
    #endregion ~IObservable Implementation

    #region Disposer

    private class AnonymousDisposer : IDisposable
    {
        private readonly Action dispose;

        // ReSharper disable once ConvertToPrimaryConstructor
        public AnonymousDisposer( Action dispose )
            => this.dispose = dispose;

        public void Dispose()
            => dispose();
    }

    #endregion ~Disposer
}
