using System.IO;

using NUnit.Framework;

namespace ArtiCluster.Features.StudioOne.KeySwitchDefinitions.Infrastructures.Tests;

internal static class TestUtility
{
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly string TestDataDirectoryRoot = Path.Combine( TestContext.CurrentContext.TestDirectory, "TestData" );

    public static string MakeTestDataPath( string relativePath )
        => Path.Combine( TestDataDirectoryRoot, relativePath );
}
