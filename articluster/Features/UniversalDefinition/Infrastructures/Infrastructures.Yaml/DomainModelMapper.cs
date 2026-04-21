using System;
using System.Collections.Generic;
using System.Linq;

using ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model;
using ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model.MidiMessages;
using ArtiCluster.Shared.Domain.Articulation.Model;
using ArtiCluster.Shared.Domain.Articulation.Model.Values;
using ArtiCluster.Shared.Domain.MidiMessages.Model;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml;

internal sealed class DomainModelMapper
{
    public Articulation Map( RootModel source )
    {
        if( source == null )
        {
            throw new ArgumentNullException( nameof( source ) );
        }

        return new Articulation(
            id: source.Id,
            author: new Author( source.Author ),
            manufacturerName: new ManufacturerName( source.ManufacturerName ),
            productName: new ProductName( source.ProductName ),
            patchName: new PatchName( source.PatchName ),
            description: new Description( source.Description ),
            assignments: MapAssignments( source.Assignments ),
            extra: new Dictionary<string, string>( source.Extra )
        );
    }

    private static List<Assignment> MapAssignments( IEnumerable<AssignmentModel> source )
    {
        return source
              .Select( model => new Assignment(
                           name: model.Name,
                           noteOn: MapMidiNoteOn( model.NoteOn ),
                           noteOff: MapMidiNoteOff( model.NoteOff ),
                           controlChange: MapMidiCc( model.ControlChange ),
                           programChange: MapMidiPc( model.ProgramChange ), extra: new Dictionary<string, string>( model.Extra )
                       )
               )
              .ToList();
    }

    private static List<TModel> MapMidiMessages<TSource, TModel>(
        IEnumerable<TSource> source,
        Func<TSource, TModel> selector )
        where TModel : IMidiMessage
        where TSource : IMidiMessageModel
    {
        return source
              .Select( selector )
              .ToList();
    }

    private static List<MidiNoteOnMessage> MapMidiNoteOn( IEnumerable<MidiNoteOnMessageModel> source )
    {
        return MapMidiMessages(
            source,
            x => new MidiNoteOnMessage(
                channel: x.Channel,
                noteNumber: x.Data1,
                velocity: x.Data2
            )
        );
    }

    private static List<MidiNoteOffMessage> MapMidiNoteOff( IEnumerable<MidiNoteOffMessageModel> source )
    {
        return MapMidiMessages(
            source,
            x => new MidiNoteOffMessage(
                channel: x.Channel,
                noteNumber: x.Data1,
                velocity: x.Data2
            )
        );
    }

    private static List<MidiControlChangeMessage> MapMidiCc( IEnumerable<MidiControlChangeMessageModel> source )
    {
        return MapMidiMessages(
            source,
            x => new MidiControlChangeMessage(
                channel: x.Status & 0x0F,
                controlNumber: x.Data1,
                controlValue: x.Data2
            )
        );
    }

    private static List<MidiProgramChangeMessage> MapMidiPc( IEnumerable<MidiProgramChangeMessageModel> source )
    {
        return MapMidiMessages(
            source,
            x => new MidiProgramChangeMessage(
                channel: x.Status & 0x0F,
                programNumber: x.Data1
            )
        );
    }
}
