# Converted from json schema by https://app.quicktype.io/
# [Settings]
# Name:; UniversalDefinition
# Launguage:; Python
#   - Transform property names to be Pythonic: on
#
# When updated, Sort properties for output in to_dict() method
#

# - UniversalDefinition
#   - FormatVersion
#   - Id
#   - Author
#   - ManufacturerName
#   - ProductName
#   - PatchName
#   - Description
#   - ArticulationGroups
#   - Extra
#
# - ArticulationGroups
#   - Name
#   - Articulations
#   - Extra
#
# - Articulation
#   - Name
#   - MidiMessages
#   - Extra
#
#  - MidiMessage
#    - Status
#    - Channel
#    - Data1
#    - Data2
#

from typing import Optional, Any, Dict, List, TypeVar, Callable, Type, cast
from uuid import UUID


T = TypeVar("T")


def from_int(x: Any) -> int:
    assert isinstance(x, int) and not isinstance(x, bool)
    return x


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


def from_dict(f: Callable[[Any], T], x: Any) -> Dict[str, T]:
    assert isinstance(x, dict)
    return { k: f(v) for (k, v) in x.items() }


def from_list(f: Callable[[Any], T], x: Any) -> List[T]:
    assert isinstance(x, list)
    return [f(y) for y in x]


def from_str(x: Any) -> str:
    assert isinstance(x, str)
    return x


def to_class(c: Type[T], x: Any) -> dict:
    assert isinstance(x, c)
    return cast(Any, x).to_dict()


class MIDIMessage:
    """A MIDI message within the articulation, consisting of a status byte and two data bytes"""

    channel: Optional[int]
    """MIDI Channel (0-15). If `-1` or `not defined`, it will be ignored in conversion"""

    data1: Optional[int]
    """MIDI Data Byte 1 (0-127). If `not defined`, it will be ignored in conversion"""

    data2: Optional[int]
    """MIDI Data Byte 2 (0-127). If `not defined`, it will be ignored in conversion"""

    status: int
    """MIDI Status Byte"""

    def __init__(self, channel: Optional[int], data1: Optional[int], data2: Optional[int], status: int) -> None:
        self.channel = channel
        self.data1 = data1
        self.data2 = data2
        self.status = status

    @staticmethod
    def from_dict(obj: Any) -> 'MIDIMessage':
        assert isinstance(obj, dict)
        channel = from_union([from_int, from_none], obj.get("Channel"))
        data1 = from_union([from_int, from_none], obj.get("Data1"))
        data2 = from_union([from_int, from_none], obj.get("Data2"))
        status = from_int(obj.get("Status"))
        return MIDIMessage(channel, data1, data2, status)

    def to_dict(self) -> dict:
        result: dict = {}
        result["Status"] = from_int(self.status)
        if self.channel is not None:
            result["Channel"] = from_union([from_int, from_none], self.channel)
        if self.data1 is not None:
            result["Data1"] = from_union([from_int, from_none], self.data1)
        if self.data2 is not None:
            result["Data2"] = from_union([from_int, from_none], self.data2)
        return result


class Articulation:
    """Defines an articulation with its name and associated MIDI messages"""

    extra: Optional[Dict[str, Any]]
    midi_messages: List[MIDIMessage]
    name: str
    """Name of the articulation"""

    def __init__(self, extra: Optional[Dict[str, Any]], midi_messages: List[MIDIMessage], name: str) -> None:
        self.extra = extra
        self.midi_messages = midi_messages
        self.name = name

    @staticmethod
    def from_dict(obj: Any) -> 'Articulation':
        assert isinstance(obj, dict)
        extra = from_union([lambda x: from_dict(lambda x: x, x), from_none], obj.get("Extra"))
        midi_messages = from_list(MIDIMessage.from_dict, obj.get("MidiMessages"))
        name = from_str(obj.get("Name"))
        return Articulation(extra, midi_messages, name)

    def to_dict(self) -> dict:
        result: dict = {}
        result["Name"] = from_str(self.name)
        result["MidiMessages"] = from_list(lambda x: to_class(MIDIMessage, x), self.midi_messages)
        if self.extra is not None:
            result["Extra"] = from_union([lambda x: from_dict(lambda x: x, x), from_none], self.extra)
        return result


class ArticulationGroup:
    """Defines an articulation group with its name and associated articulations"""

    articulations: List[Articulation]
    """List of articulations in this group"""

    extra: Optional[Dict[str, Any]]
    name: str
    """A Group name"""

    def __init__(self, articulations: List[Articulation], extra: Optional[Dict[str, Any]], name: str) -> None:
        self.articulations = articulations
        self.extra = extra
        self.name = name

    @staticmethod
    def from_dict(obj: Any) -> 'ArticulationGroup':
        assert isinstance(obj, dict)
        articulations = from_list(Articulation.from_dict, obj.get("Articulations"))
        extra = from_union([lambda x: from_dict(lambda x: x, x), from_none], obj.get("Extra"))
        name = from_str(obj.get("Name"))
        return ArticulationGroup(articulations, extra, name)

    def to_dict(self) -> dict:
        result: dict = {}
        result["Name"] = from_str(self.name)
        result["Articulations"] = from_list(lambda x: to_class(Articulation, x), self.articulations)
        if self.extra is not None:
            result["Extra"] = from_union([lambda x: from_dict(lambda x: x, x), from_none], self.extra)
        return result


class UniversalDefinition:
    """Definition of the Universal Definition v1.0.0 file format.

    The root of articulation
    """
    articulation_groups: List[ArticulationGroup]
    """List of articulation groups in this Universal Definition. In the case of DAWs that
    support grouping, this means exporting the articulations defined in this group into a
    single file. In the case of DAWs that do not support grouping, the groups are ignored,
    and a file is exported for each articulation within the group.
    """
    author: str
    """Author name of this Universal Definition"""

    description: Optional[str]
    """Description of this Universal Definition file"""

    extra: Optional[Dict[str, Any]]
    format_version: str
    """Universal File Format Version (<major>.<minor>.<patch>)"""

    id: UUID
    """Unique identifier for this Universal Definition"""

    manufacturer_name: str
    """Name of the manufacturer"""

    patch_name: str
    """Name of the patch"""

    product_name: str
    """Name of the product"""

    def __init__(self, articulation_groups: List[ArticulationGroup], author: str, description: Optional[str], extra: Optional[Dict[str, Any]], format_version: str, id: UUID, manufacturer_name: str, patch_name: str, product_name: str) -> None:
        self.articulation_groups = articulation_groups
        self.author = author
        self.description = description
        self.extra = extra
        self.format_version = format_version
        self.id = id
        self.manufacturer_name = manufacturer_name
        self.patch_name = patch_name
        self.product_name = product_name

    @staticmethod
    def from_dict(obj: Any) -> 'UniversalDefinition':
        assert isinstance(obj, dict)
        articulation_groups = from_list(ArticulationGroup.from_dict, obj.get("ArticulationGroups"))
        author = from_str(obj.get("Author"))
        description = from_union([from_str, from_none], obj.get("Description"))
        extra = from_union([lambda x: from_dict(lambda x: x, x), from_none], obj.get("Extra"))
        format_version = from_str(obj.get("FormatVersion"))
        id = UUID(obj.get("Id"))
        manufacturer_name = from_str(obj.get("ManufacturerName"))
        patch_name = from_str(obj.get("PatchName"))
        product_name = from_str(obj.get("ProductName"))
        return UniversalDefinition(articulation_groups, author, description, extra, format_version, id, manufacturer_name, patch_name, product_name)

    def to_dict(self) -> dict:
        result: dict = {}
        result["FormatVersion"] = from_str(self.format_version)
        result["Id"] = str(self.id)
        result["Author"] = from_str(self.author)
        result["ManufacturerName"] = from_str(self.manufacturer_name)
        result["ProductName"] = from_str(self.product_name)
        result["PatchName"] = from_str(self.patch_name)
        if self.description is not None:
            result["Description"] = from_union([from_str, from_none], self.description)
        result["ArticulationGroups"] = from_list(lambda x: to_class(ArticulationGroup, x), self.articulation_groups)
        if self.extra is not None:
            result["Extra"] = from_union([lambda x: from_dict(lambda x: x, x), from_none], self.extra)
        return result


def universal_definition_from_dict(s: Any) -> UniversalDefinition:
    return UniversalDefinition.from_dict(s)


def universal_definition_to_dict(x: UniversalDefinition) -> Any:
    return to_class(UniversalDefinition, x)
