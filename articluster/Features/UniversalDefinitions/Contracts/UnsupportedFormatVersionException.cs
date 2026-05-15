using System;

namespace ArtiCluster.Features.UniversalDefinitions.Contracts;

public class UnsupportedFormatVersionException( string formatVersion )
    : Exception( $"Unsupported format version: {formatVersion}" )
{
    public string FormatVersion => formatVersion;
}
