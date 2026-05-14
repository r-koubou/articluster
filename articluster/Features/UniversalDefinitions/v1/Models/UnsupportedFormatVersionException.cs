using System;

namespace ArtiCluster.Features.UniversalDefinitions.v1.Models;

public class UnsupportedFormatVersionException( string formatVersion )
    : Exception( $"Unsupported format version: {formatVersion}" )
{
    public string FormatVersion => formatVersion;
}
