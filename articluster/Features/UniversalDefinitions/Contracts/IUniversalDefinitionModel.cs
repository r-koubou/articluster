namespace ArtiCluster.Features.UniversalDefinitions.Contracts;

public interface IUniversalDefinitionModel
{
    const string FormatVersionFieldName = "FormatVersion";
    string FormatVersion { get; }
}
