using System;

namespace ArtiCluster.Features.UniversalDefinitions.Contracts;

public class UnsupportedFormatVersionException : Exception
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public UnsupportedFormatVersionException( string formatVersion = "Unknown" )
        : base( formatVersion ) {}
}
