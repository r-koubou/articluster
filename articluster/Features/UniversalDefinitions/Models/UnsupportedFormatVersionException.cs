using System;

namespace ArtiCluster.Features.UniversalDefinitions.Models;

public class UnsupportedFormatVersionException( int formatVersion )
    : Exception( $"Unsupported format version: {formatVersion}" )
{
    public int FormatVersion => formatVersion;
}
