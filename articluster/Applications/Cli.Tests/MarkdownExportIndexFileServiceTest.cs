using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Exports;
using ArtiCluster.Applications.Services.Abstractions.Exports.MarkdownExports.Strategies;
using ArtiCluster.Applications.Services.Abstractions.Exports.Models;
using ArtiCluster.Applications.Services.Local.Exports.MarkdownExports.Services;
using ArtiCluster.Commons;
using ArtiCluster.Tests.Helpers;

using NUnit.Framework;

namespace ArtiCluster.Applications.Cli.Tests;

[TestFixture]
public class MarkdownExportIndexFileServiceTest
{
    [Test]
    public async Task ExportsAllGroupsUsingTheirMatchingLayoutStrategyAsync()
    {
        var cakewalkStrategy = new RecordingMarkdownDocumentLayoutStrategy(
            [ "Cakewalk" ],
            _ => Result<Unit, ExportFailureReason>.Success( Unit.Default )
        );

        var cubaseStrategy = new RecordingMarkdownDocumentLayoutStrategy(
            [ "Cubase" ],
            _ => Result<Unit, ExportFailureReason>.Success( Unit.Default )
        );

        var markdownContentRootDirectory = Path.Combine( TestUtility.TestDataDirectoryRoot, "markdown-root" );

        var service = new MarkdownExportIndexFileService( [ cakewalkStrategy, cubaseStrategy ] );

        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        var entries = new[]
        {
            CreateEntry( "Cakewalk", "Acme", "Lead A" ),
            CreateEntry( "Cakewalk", "Acme", "Lead B" ),
            CreateEntry( "Cubase", "Acme", "Pad A" ),
            CreateEntry( "Cubase", "Contoso", "Bass A" )
        };

        var result = await service.ExportAsync( markdownContentRootDirectory, entries, cancellationToken );

        Assert.That( result.IsSuccess, Is.True );

        Assert.Multiple( () =>
            {
                Assert.That( cakewalkStrategy.Calls, Has.Count.EqualTo( 1 ) );
                Assert.That( cubaseStrategy.Calls, Has.Count.EqualTo( 2 ) );

                var cakewalkCall = cakewalkStrategy.Calls.Single();
                Assert.That( cakewalkCall.MarkdownContentRootDirectory, Is.EqualTo( markdownContentRootDirectory ) );
                Assert.That( cakewalkCall.DawName, Is.EqualTo( "Cakewalk" ) );
                Assert.That( cakewalkCall.ManufacturerName, Is.EqualTo( "Acme" ) );
                Assert.That( cakewalkCall.Entries.Select( x => x.DisplayName ), Is.EquivalentTo( new[] { "Lead A", "Lead B" } ) );
                Assert.That( cakewalkCall.CancellationToken, Is.EqualTo( cancellationToken ) );

                var cubaseAcmeCall = cubaseStrategy.Calls.Single( x => x.ManufacturerName == "Acme" );
                Assert.That( cubaseAcmeCall.DawName, Is.EqualTo( "Cubase" ) );
                Assert.That( cubaseAcmeCall.Entries.Select( x => x.DisplayName ), Is.EquivalentTo( new[] { "Pad A" } ) );

                var cubaseContosoCall = cubaseStrategy.Calls.Single( x => x.ManufacturerName == "Contoso" );
                Assert.That( cubaseContosoCall.DawName, Is.EqualTo( "Cubase" ) );
                Assert.That( cubaseContosoCall.Entries.Select( x => x.DisplayName ), Is.EquivalentTo( new[] { "Bass A" } ) );
            }
        );
    }

    [Test]
    public async Task ReturnsSuccessWithoutInvokingAnyStrategyWhenThereAreNoEntriesAsync()
    {
        var strategy = new RecordingMarkdownDocumentLayoutStrategy(
            [ "Cakewalk" ],
            _ => Result<Unit, ExportFailureReason>.Success( Unit.Default )
        );

        var markdownContentRootDirectory = Path.Combine( TestUtility.TestDataDirectoryRoot, "markdown-root" );

        var service = new MarkdownExportIndexFileService( [ strategy ] );

        var result = await service.ExportAsync( markdownContentRootDirectory, Array.Empty<ExportedFileEntry>() );

        Assert.Multiple( () =>
            {
                Assert.That( result.IsSuccess, Is.True );
                Assert.That( strategy.Calls, Is.Empty );
            }
        );
    }

    [Test]
    public async Task ReturnsFailureWhenNoLayoutStrategyCanHandleAGroupAsync()
    {
        var strategy = new RecordingMarkdownDocumentLayoutStrategy(
            [ "Cubase" ],
            _ => Result<Unit, ExportFailureReason>.Success( Unit.Default )
        );

        var markdownContentRootDirectory = Path.Combine( TestUtility.TestDataDirectoryRoot, "markdown-root" );

        var service = new MarkdownExportIndexFileService( [ strategy ] );

        var result = await service.ExportAsync(
            markdownContentRootDirectory,
            [ CreateEntry( "Cakewalk", "Acme", "Lead A" ) ]
        );

        var (reason, error) = result.UnwrapError();

        Assert.Multiple( () =>
            {
                Assert.That( reason, Is.EqualTo( ExportFailureReason.OtherError ) );
                Assert.That( error, Is.TypeOf<KeyNotFoundException>() );
                Assert.That( error?.Message, Does.Contain( "No markdown layout strategy found for DAW: Cakewalk" ) );
                Assert.That( strategy.Calls, Is.Empty );
            }
        );
    }

    [Test]
    public async Task ReturnsFailureWhenMultipleLayoutStrategiesCanHandleTheSameGroupAsync()
    {
        var firstStrategy = new RecordingMarkdownDocumentLayoutStrategy(
            [ "Cakewalk" ],
            _ => Result<Unit, ExportFailureReason>.Success( Unit.Default )
        );

        var secondStrategy = new RecordingMarkdownDocumentLayoutStrategy(
            [ "Cakewalk" ],
            _ => Result<Unit, ExportFailureReason>.Success( Unit.Default )
        );

        var markdownContentRootDirectory = Path.Combine( TestUtility.TestDataDirectoryRoot, "markdown-root" );

        var service = new MarkdownExportIndexFileService( [ firstStrategy, secondStrategy ] );

        var result = await service.ExportAsync(
            markdownContentRootDirectory,
            [ CreateEntry( "Cakewalk", "Acme", "Lead A" ) ]
        );

        var (reason, error) = result.UnwrapError();

        Assert.Multiple( () =>
            {
                Assert.That( reason, Is.EqualTo( ExportFailureReason.OtherError ) );
                Assert.That( error, Is.TypeOf<InvalidOperationException>() );
                Assert.That( error?.Message, Does.Contain( "Multiple markdown layout strategies found for DAW: Cakewalk" ) );
                Assert.That( firstStrategy.Calls, Is.Empty );
                Assert.That( secondStrategy.Calls, Is.Empty );
            }
        );
    }

    [Test]
    public async Task ReturnsTheFailingStrategyResultAndSkipsRemainingGroupsAsync()
    {
        var expectedError = new InvalidOperationException( "boom" );

        var failingStrategy = new RecordingMarkdownDocumentLayoutStrategy(
            [ "Cakewalk" ],
            _ => Result<Unit, ExportFailureReason>.Failure( ExportFailureReason.OtherError, expectedError )
        );

        var succeedingStrategy = new RecordingMarkdownDocumentLayoutStrategy(
            [ "Cubase" ],
            _ => Result<Unit, ExportFailureReason>.Success( Unit.Default )
        );

        var markdownContentRootDirectory = Path.Combine( TestUtility.TestDataDirectoryRoot, "markdown-root" );

        var service = new MarkdownExportIndexFileService( [ failingStrategy, succeedingStrategy ] );

        var result = await service.ExportAsync(
            markdownContentRootDirectory,
            [
                CreateEntry( "Cakewalk", "Acme", "Lead A" ),
                CreateEntry( "Cubase", "Contoso", "Bass A" )
            ]
        );

        var (reason, error) = result.UnwrapError();

        Assert.Multiple( () =>
            {
                Assert.That( reason, Is.EqualTo( ExportFailureReason.OtherError ) );
                Assert.That( error, Is.SameAs( expectedError ) );
                Assert.That( failingStrategy.Calls, Has.Count.EqualTo( 1 ) );
                Assert.That( succeedingStrategy.Calls, Is.Empty );
            }
        );
    }

    private static ExportedFileEntry CreateEntry( string dawName, string manufacturerName, string displayName )
    {
        return new ExportedFileEntry(
            dawName,
            manufacturerName,
            Path.Combine( TestUtility.TestDataDirectoryRoot, "/output" ),
            $"{displayName}.md",
            displayName
        );
    }

    private sealed record ExportCall(
        string MarkdownContentRootDirectory,
        string DawName,
        string ManufacturerName,
        IReadOnlyCollection<ExportedFileEntry> Entries,
        CancellationToken CancellationToken
    );

    private sealed class RecordingMarkdownDocumentLayoutStrategy : IMarkdownDocumentLayoutStrategy
    {
        private readonly Func<ExportCall, Result<Unit, ExportFailureReason>> resultFactory;
        private readonly HashSet<string> supportedDawNames;

        public RecordingMarkdownDocumentLayoutStrategy(
            IEnumerable<string> supportedDawNames,
            Func<ExportCall, Result<Unit, ExportFailureReason>> resultFactory )
        {
            this.supportedDawNames = supportedDawNames.ToHashSet();
            this.resultFactory     = resultFactory;
        }

        public List<ExportCall> Calls { get; } = [ ];

        public bool CanHandle( string dawName )
            => supportedDawNames.Contains( dawName );

        public Task<Result<Unit, ExportFailureReason>> ExportAsync(
            string markdownContentRootDirectory,
            string dawName,
            string manufacturerName,
            IReadOnlyCollection<ExportedFileEntry> entries,
            CancellationToken cancellationToken = default )
        {
            var call = new ExportCall(
                markdownContentRootDirectory,
                dawName,
                manufacturerName,
                entries.ToArray(),
                cancellationToken
            );

            Calls.Add( call );

            return Task.FromResult( resultFactory( call ) );
        }
    }
}
