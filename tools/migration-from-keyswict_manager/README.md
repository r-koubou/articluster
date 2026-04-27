Migration to Articluster format from Keyswitch Manager DB file
==============================================================

The converter tool of [Keyswitch Manager](https://github.com/r-koubou/KeySwitchManager) DB file is provided to convert the existing [Keyswitch Manager DB file (yaml)](https://github.com/r-koubou/ArticulationMappingFiles/blob/main/KeySwitches.db.yaml) to the articluster format.

## Required Software

- [uv](https://github.com/astral-sh/uv)

## Setup

```bash
uv sync
```

## Preparation

- Convert Keyswitch Manager DB file (yaml) to JSON

### ⚠️ If `universal-definition-schema.json` updated

- [universal-definition-schema.json](../../articluster/Features/UniversalDefinitions/Documents/schema/universal-definition-schema.json) is here.
    - `=/articluster/Features/UniversalDefinitions/Documents/schema/universal-definition-schema.json`

- Update the `target.py` with https://app.quicktype.io/

### Modify order of keys in to_dict method at `target.py`

To preserve the order of keys in the Articluster YAML file, you will need to manually modify the code as follows.

1. Coordinate.to_dict

```python
def to_dict(self) -> dict:
    result: dict = {}
    result["Id"] = str(self.id)
    result["Author"] = from_str(self.author)
    result["ManufacturerName"] = from_str(self.manufacturer_name)
    result["ProductName"] =  from_str(self.product_name)
    result["PatchName"] = from_str(self.patch_name)
    result["Description"] = LiteralScalarString(self.description)
    result["Articulations"] = from_list(lambda x: to_class(Articulation, x), self.articulations)
    if self.extra is not None:
        result["Extra"] = from_union([lambda x: from_dict(lambda x: x, x), from_none], self.extra)
    return result
```

2. Articulation.to_dict

```python
def to_dict(self) -> dict:
    result: dict = {}
    result["Name"] = from_str(self.name)
    result["MidiMessages"] = from_list(lambda x: to_class(MIDIMessage, x), self.midi_messages)
    if self.extra is not None:
        result["Extra"] = from_union([lambda x: from_dict(lambda x: x, x), from_none], self.extra)
    return result
```

3. MIDIMessage.to_dict

```python
def to_dict(self) -> dict:
    result: dict = {}
    result["Status"] = from_int(self.status)
    if self.data1 is not None:
        result["Data1"] = from_union([from_int, from_none], self.data1)
    if self.data2 is not None:
        result["Data2"] = from_union([from_int, from_none], self.data2)
    return result
```

## Convert

```sh
uv run python main.py <keyswitch_manager_db_file>

arguments:
  keyswitch_manager_db_file  Keyswitch Manager DB file (json converted from yaml)
```

The output files will be generated in the `THIS_DIRECTORY/out` directory.
