# Converted from schema by https://app.quicktype.io/

from typing import Any, List, Optional, Dict, TypeVar, Callable, Type, cast
from datetime import datetime
from uuid import UUID
import dateutil.parser


T = TypeVar("T")


def from_int(x: Any) -> int:
    assert isinstance(x, int) and not isinstance(x, bool)
    return x


def from_str(x: Any) -> str:
    assert isinstance(x, str)
    return x


def from_list(f: Callable[[Any], T], x: Any) -> List[T]:
    assert isinstance(x, list)
    return [f(y) for y in x]


def from_none(x: Any) -> Any:
    assert x is None
    return x


def from_union(fs, x):
    for f in fs:
        try:
            return f(x)
        except:
            pass
    assert False


def to_class(c: Type[T], x: Any) -> dict:
    assert isinstance(x, c)
    return cast(Any, x).to_dict()


def from_dict(f: Callable[[Any], T], x: Any) -> Dict[str, T]:
    assert isinstance(x, dict)
    return { k: f(v) for (k, v) in x.items() }


def from_datetime(x: Any) -> datetime:
    return dateutil.parser.parse(x)


class ControlChange:
    channel: int
    control_number: int
    data: int

    def __init__(self, channel: int, control_number: int, data: int) -> None:
        self.channel = channel
        self.control_number = control_number
        self.data = data

    @staticmethod
    def from_dict(obj: Any) -> 'ControlChange':
        assert isinstance(obj, dict)
        channel = from_int(obj.get("Channel"))
        control_number = from_int(obj.get("ControlNumber"))
        data = from_int(obj.get("Data"))
        return ControlChange(channel, control_number, data)

    def to_dict(self) -> dict:
        result: dict = {}
        result["Channel"] = from_int(self.channel)
        result["ControlNumber"] = from_int(self.control_number)
        result["Data"] = from_int(self.data)
        return result


class NoteOn:
    channel: int
    note: str
    velocity: int

    def __init__(self, channel: int, note: str, velocity: int) -> None:
        self.channel = channel
        self.note = note
        self.velocity = velocity

    @staticmethod
    def from_dict(obj: Any) -> 'NoteOn':
        assert isinstance(obj, dict)
        channel = from_int(obj.get("Channel"))
        note = from_str(obj.get("Note"))
        velocity = from_int(obj.get("Velocity"))
        return NoteOn(channel, note, velocity)

    def to_dict(self) -> dict:
        result: dict = {}
        result["Channel"] = from_int(self.channel)
        result["Note"] = from_str(self.note)
        result["Velocity"] = from_int(self.velocity)
        return result


class ProgramChange:
    channel: int
    program_number: int

    def __init__(self, channel: int, program_number: int) -> None:
        self.channel = channel
        self.program_number = program_number

    @staticmethod
    def from_dict(obj: Any) -> 'ProgramChange':
        assert isinstance(obj, dict)
        channel = from_int(obj.get("Channel"))
        program_number = from_int(obj.get("ProgramNumber"))
        return ProgramChange(channel, program_number)

    def to_dict(self) -> dict:
        result: dict = {}
        result["Channel"] = from_int(self.channel)
        result["ProgramNumber"] = from_int(self.program_number)
        return result


class MIDIMessage:
    control_change: Optional[List[ControlChange]]
    note_on: Optional[List[NoteOn]]
    program_change: Optional[List[ProgramChange]]

    def __init__(self, control_change: Optional[List[ControlChange]], note_on: Optional[List[NoteOn]], program_change: Optional[List[ProgramChange]]) -> None:
        self.control_change = control_change
        self.note_on = note_on
        self.program_change = program_change

    @staticmethod
    def from_dict(obj: Any) -> 'MIDIMessage':
        assert isinstance(obj, dict)
        control_change = from_union([lambda x: from_list(ControlChange.from_dict, x), from_none], obj.get("ControlChange"))
        note_on = from_union([lambda x: from_list(NoteOn.from_dict, x), from_none], obj.get("NoteOn"))
        program_change = from_union([lambda x: from_list(ProgramChange.from_dict, x), from_none], obj.get("ProgramChange"))
        return MIDIMessage(control_change, note_on, program_change)

    def to_dict(self) -> dict:
        result: dict = {}
        if self.control_change is not None:
            result["ControlChange"] = from_union([lambda x: from_list(lambda x: to_class(ControlChange, x), x), from_none], self.control_change)
        if self.note_on is not None:
            result["NoteOn"] = from_union([lambda x: from_list(lambda x: to_class(NoteOn, x), x), from_none], self.note_on)
        if self.program_change is not None:
            result["ProgramChange"] = from_union([lambda x: from_list(lambda x: to_class(ProgramChange, x), x), from_none], self.program_change)
        return result


class Articulations:
    extra_data: Optional[Dict[str, Any]]
    midi_message: Optional[MIDIMessage]
    name: str

    def __init__(self, extra_data: Optional[Dict[str, Any]], midi_message: Optional[MIDIMessage], name: str) -> None:
        self.extra_data = extra_data
        self.midi_message = midi_message
        self.name = name

    @staticmethod
    def from_dict(obj: Any) -> 'Articulations':
        assert isinstance(obj, dict)
        extra_data = from_union([lambda x: from_dict(lambda x: x, x), from_none], obj.get("ExtraData"))
        midi_message = from_union([MIDIMessage.from_dict, from_none], obj.get("MidiMessage"))
        name = from_str(obj.get("Name"))
        return Articulations(extra_data, midi_message, name)

    def to_dict(self) -> dict:
        result: dict = {}
        if self.extra_data is not None:
            result["ExtraData"] = from_union([lambda x: from_dict(lambda x: x, x), from_none], self.extra_data)
        if self.midi_message is not None:
            result["MidiMessage"] = from_union([lambda x: to_class(MIDIMessage, x), from_none], self.midi_message)
        result["Name"] = from_str(self.name)
        return result


class KeySwitches:
    articulations: Optional[List[Articulations]]
    author: str
    created: datetime
    description: str
    developer_name: str
    extra_data: Optional[Dict[str, Any]]
    id: UUID
    instrument_name: str
    last_updated: datetime
    product_name: str

    def __init__(self, articulations: Optional[List[Articulations]], author: str, created: datetime, description: str, developer_name: str, extra_data: Optional[Dict[str, Any]], id: UUID, instrument_name: str, last_updated: datetime, product_name: str) -> None:
        self.articulations = articulations
        self.author = author
        self.created = created
        self.description = description
        self.developer_name = developer_name
        self.extra_data = extra_data
        self.id = id
        self.instrument_name = instrument_name
        self.last_updated = last_updated
        self.product_name = product_name

    @staticmethod
    def from_dict(obj: Any) -> 'KeySwitches':
        assert isinstance(obj, dict)
        articulations = from_union([lambda x: from_list(Articulations.from_dict, x), from_none], obj.get("Articulations"))
        author = from_str(obj.get("Author"))
        created = from_datetime(obj.get("Created"))
        description = from_str(obj.get("Description"))
        developer_name = from_str(obj.get("DeveloperName"))
        extra_data = from_union([lambda x: from_dict(lambda x: x, x), from_none], obj.get("ExtraData"))
        id = UUID(obj.get("Id"))
        instrument_name = from_str(obj.get("InstrumentName"))
        last_updated = from_datetime(obj.get("LastUpdated"))
        product_name = from_str(obj.get("ProductName"))
        return KeySwitches(articulations, author, created, description, developer_name, extra_data, id, instrument_name, last_updated, product_name)

    def to_dict(self) -> dict:
        result: dict = {}
        if self.articulations is not None:
            result["Articulations"] = from_union([lambda x: from_list(lambda x: to_class(Articulations, x), x), from_none], self.articulations)
        result["Author"] = from_str(self.author)
        result["Created"] = self.created.isoformat()
        result["Description"] = from_str(self.description)
        result["DeveloperName"] = from_str(self.developer_name)
        if self.extra_data is not None:
            result["ExtraData"] = from_union([lambda x: from_dict(lambda x: x, x), from_none], self.extra_data)
        result["Id"] = str(self.id)
        result["InstrumentName"] = from_str(self.instrument_name)
        result["LastUpdated"] = self.last_updated.isoformat()
        result["ProductName"] = from_str(self.product_name)
        return result


class Coordinate:
    key_switches: List[KeySwitches]

    def __init__(self, key_switches: List[KeySwitches]) -> None:
        self.key_switches = key_switches

    @staticmethod
    def from_dict(obj: Any) -> 'Coordinate':
        assert isinstance(obj, dict)
        key_switches = from_list(KeySwitches.from_dict, obj.get("KeySwitches"))
        return Coordinate(key_switches)

    def to_dict(self) -> dict:
        result: dict = {}
        result["KeySwitches"] = from_list(lambda x: to_class(KeySwitches, x), self.key_switches)
        return result


def coordinate_from_dict(s: Any) -> Coordinate:
    return Coordinate.from_dict(s)


def coordinate_to_dict(x: Coordinate) -> Any:
    return to_class(Coordinate, x)
