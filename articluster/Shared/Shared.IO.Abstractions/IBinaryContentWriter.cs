using System;

namespace ArtiCluster.Shared.IO.Abstractions;

public interface IBinaryContentWriter : IContentWriter<ReadOnlyMemory<byte>>;
