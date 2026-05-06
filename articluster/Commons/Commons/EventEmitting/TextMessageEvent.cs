using System;

namespace ArtiCluster.Commons.EventEmitting;

public readonly record struct TextMessageEvent(
    string Message,
    Exception? Error = null
) : IEvent<string>;
