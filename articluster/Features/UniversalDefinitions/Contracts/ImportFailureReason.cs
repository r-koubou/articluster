namespace ArtiCluster.Features.UniversalDefinitions.Contracts;

public enum ImportFailureReason
{
    UnsupportedFormatVersion,
    DeserializationError,
    IoError,
    OtherError
}
