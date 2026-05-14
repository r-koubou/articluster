using System;

namespace ArtiCluster.Features.UniversalDefinitions.v1.Models;

public class UnsupportedFormatVersionException( int formatVersion )
    : Exception( $"Unsupported format version: {formatVersion}" )
{
    public int FormatVersion => formatVersion;
}
