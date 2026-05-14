namespace ArtiCluster.Applications.Services.Abstractions;

public enum ImportFailureReason
{
    UnsupportedFormatVersion,
    DeserializationError,
    IoError,
    OtherError
}
