using System.IO;

using NUnit.Framework;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Tests;

public static class Constants
{
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly string TestDataDirectoryRoot = Path.Combine( TestContext.CurrentContext.TestDirectory, "TestData" );

    public static string MakeTestDataPath( string relativePath )
        => Path.Combine( TestDataDirectoryRoot, relativePath );
}
