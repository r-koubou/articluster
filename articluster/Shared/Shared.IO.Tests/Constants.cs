using System.IO;

using NUnit.Framework;

namespace ArtiCluster.Shared.IO.Local.Tests;

public static class Constants
{
    public static readonly string TestDataDirectoryRoot = Path.Combine( TestContext.CurrentContext.TestDirectory, "TestData" );
}
