namespace ArtiCluster.Applications.Services.Abstractions.Imports;

public enum ImportFailureReason
{
    UnsupportedFormatVersion,
    DeserializationError,
    IoError,
    OtherError
}
