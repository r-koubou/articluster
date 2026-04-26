# Converted from schema by https://app.quicktype.io/

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

    data1: Optional[int]
    data2: Optional[int]
    status: int

    def __init__(self, data1: Optional[int], data2: Optional[int], status: int) -> None:
        self.data1 = data1
        self.data2 = data2
        self.status = status

    @staticmethod
    def from_dict(obj: Any) -> 'MIDIMessage':
        assert isinstance(obj, dict)
        data1 = from_union([from_int, from_none], obj.get("Data1"))
        data2 = from_union([from_int, from_none], obj.get("Data2"))
        status = from_int(obj.get("Status"))
        return MIDIMessage(data1, data2, status)

    def to_dict(self) -> dict:
        result: dict = {}
        result["Status"] = from_int(self.status)
        if self.data1 is not None:
            result["Data1"] = from_union([from_int, from_none], self.data1)
        if self.data2 is not None:
            result["Data2"] = from_union([from_int, from_none], self.data2)
        return result


class Articluation:
    """Defines an articulation with its name and associated MIDI messages"""

    extra: Optional[Dict[str, Any]]
    midi_messages: List[MIDIMessage]
    name: str

    def __init__(self, extra: Optional[Dict[str, Any]], midi_messages: List[MIDIMessage], name: str) -> None:
        self.extra = extra
        self.midi_messages = midi_messages
        self.name = name

    @staticmethod
    def from_dict(obj: Any) -> 'Articluation':
        assert isinstance(obj, dict)
        extra = from_union([lambda x: from_dict(lambda x: x, x), from_none], obj.get("Extra"))
        midi_messages = from_list(MIDIMessage.from_dict, obj.get("MidiMessages"))
        name = from_str(obj.get("Name"))
        return Articluation(extra, midi_messages, name)

    def to_dict(self) -> dict:
        result: dict = {}
        result["Name"] = from_str(self.name)
        result["MidiMessages"] = from_list(lambda x: to_class(MIDIMessage, x), self.midi_messages)
        if self.extra is not None:
            result["Extra"] = from_union([lambda x: from_dict(lambda x: x, x), from_none], self.extra)
        return result


class Coordinate:
    """The root of articulation"""

    id: UUID
    author: str
    manufacturer_name: str
    product_name: str
    patch_name: str
    articluations: List[Articluation]
    description: str
    extra: Optional[Dict[str, Any]]

    def __init__(self, articluations: List[Articluation], author: str, description: str, extra: Optional[Dict[str, Any]], id: UUID, manufacturer_name: str, patch_name: str, product_name: str) -> None:
        self.articluations = articluations
        self.author = author
        self.description = description
        self.extra = extra
        self.id = id
        self.manufacturer_name = manufacturer_name
        self.patch_name = patch_name
        self.product_name = product_name

    @staticmethod
    def from_dict(obj: Any) -> 'Coordinate':
        assert isinstance(obj, dict)
        articluations = from_list(Articluation.from_dict, obj.get("Articluations"))
        author = from_str(obj.get("Author"))
        description = from_str(obj.get("Description"))
        extra = from_union([lambda x: from_dict(lambda x: x, x), from_none], obj.get("Extra"))
        id = UUID(obj.get("Id"))
        manufacturer_name = from_str(obj.get("ManufacturerName"))
        patch_name = from_str(obj.get("PatchName"))
        product_name = from_str(obj.get("ProductName"))
        return Coordinate(articluations, author, description, extra, id, manufacturer_name, patch_name, product_name)

    def to_dict(self) -> dict:
        result: dict = {}
        result["Id"] = str(self.id)
        result["Author"] = from_str(self.author)
        result["ManufacturerName"] = from_str(self.manufacturer_name)
        result["ProductName"] =  from_str(self.product_name)
        result["PatchName"] = from_str(self.patch_name)
        result["Description"] = from_str(self.description)
        result["Articluations"] = from_list(lambda x: to_class(Articluation, x), self.articluations)
        if self.extra is not None:
            result["Extra"] = from_union([lambda x: from_dict(lambda x: x, x), from_none], self.extra)
        return result


def coordinate_from_dict(s: Any) -> Coordinate:
    return Coordinate.from_dict(s)


def coordinate_to_dict(x: Coordinate) -> Any:
    return to_class(Coordinate, x)
