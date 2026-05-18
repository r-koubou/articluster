namespace ArtiCluster.Features.UniversalDefinitions.Contracts;

public interface IUniversalDefinitionModel
{
    const string FormatVersionFieldName = "FormatVersion";
    int FormatVersion { get; }
}
