using System.Collections.Generic;
using System.Linq;

using ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model;
using ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model.MidiMessages;
using ArtiCluster.Shared.Domain.Articulation.Model;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml;

internal class YamlModelMapper
{
    public RootModel Map( Articulation source )
    {
        return new RootModel()
        {
            Id               = source.Id,
            Author           = source.Author.Value,
            ManufacturerName = source.ManufacturerName.Value,
            ProductName      = source.ProductName.Value,
            PatchName        = source.PatchName.Value,
            Description      = source.Description.Value,
            Assignments      = MapAssignment( source.Assignments ),
            Extra            = new Dictionary<string, string>( source.Extra )
        };
    }

    private static List<AssignmentModel> MapAssignment( IEnumerable<Assignment> source )
    {
        return source
              .Select( assignment => new AssignmentModel
                   {
                       Name          = assignment.Name.Value,
                       NoteOn        = MapMidiNoteOn( assignment ),
                       NoteOff       = MapMidiNoteOff( assignment ),
                       ControlChange = MapMidiCc( assignment ),
                       ProgramChange = MapMidiPc( assignment )
                   }
               )
              .ToList();
    }

    private static List<TModel> MapMidiMessages<TSource, TModel>(
        IEnumerable<TSource> source,
        System.Func<TSource, TModel> selector )
        where TModel : IMidiMessageModel
    {
        return source
              .Select( selector )
              .ToList();
    }

    private static List<MidiNoteOnMessageModel> MapMidiNoteOn( Assignment source )
    {
        return MapMidiMessages(
            source.MidiNoteOn,
            x => new MidiNoteOnMessageModel
            {
                Channel = x.Channel.Value,
                Data1   = x.DataByte1.Value,
                Data2   = x.DataByte2.Value,
            }
        );
    }

    private static List<MidiNoteOffMessageModel> MapMidiNoteOff( Assignment source )
    {
        return MapMidiMessages(
            source.MidiNoteOff,
            x => new MidiNoteOffMessageModel
            {
                Channel = x.Channel.Value,
                Data1   = x.DataByte1.Value,
                Data2   = x.DataByte2.Value,
            }
        );
    }

    private static List<MidiControlChangeMessageModel> MapMidiCc( Assignment source )
    {
        return MapMidiMessages(
            source.MidiNoteOn,
            x => new MidiControlChangeMessageModel
            {
                Channel = x.Channel.Value,
                Data1   = x.DataByte1.Value,
                Data2   = x.DataByte2.Value,
            }
        );
    }

    private static List<MidiProgramChangeMessageModel> MapMidiPc( Assignment source )
    {
        return MapMidiMessages(
            source.MidiNoteOn,
            x => new MidiProgramChangeMessageModel
            {
                Channel = x.Channel.Value,
                Data1   = x.DataByte1.Value,
            }
        );
    }
}
