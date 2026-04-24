# JSON Schema

## Definitions

- <a id="definitions/UniversalDefinition"></a>**`UniversalDefinition`** *(object)*: The root of articulation. Cannot contain additional properties.
  - <a id="definitions/UniversalDefinition/properties/Id"></a>**`Id`** *(string, format: uuid, required)*
  - <a id="definitions/UniversalDefinition/properties/Author"></a>**`Author`** *(string, required)*
  - <a id="definitions/UniversalDefinition/properties/ManufacturerName"></a>**`ManufacturerName`** *(string, required)*
  - <a id="definitions/UniversalDefinition/properties/ProductName"></a>**`ProductName`** *(string, required)*
  - <a id="definitions/UniversalDefinition/properties/PatchName"></a>**`PatchName`** *(string, required)*
  - <a id="definitions/UniversalDefinition/properties/Description"></a>**`Description`** *(string, required)*
  - <a id="definitions/UniversalDefinition/properties/Articluations"></a>**`Articluations`** *(array, required)*
    - <a id="definitions/UniversalDefinition/properties/Articluations/items"></a>**Items**: Refer to *[#/definitions/Articluation](#definitions/Articluation)*.
  - <a id="definitions/UniversalDefinition/properties/Extra"></a>**`Extra`**: Refer to *[#/definitions/Extra](#definitions/Extra)*.

  Examples:
  ```yaml
  Id: 34023d3f-30b0-4646-90e8-5d2978d09d43
  Author: r-koubou
  ManufacturerName: r-koubou
  ProductName: My Guitar
  PatchName: My Guitar 1
  Description: This is my guitar articulation.
  Articluations:
  -   Name: Sustain
      MidiMessages:
      -   Status: 144
          Data1: 64
          Data2: 100
      Extra:
          Extra.Custom.Key: Custom Value
  -   Name: Bridge Mute
      MidiMessages:
      -   Status: 144
          Data1: 66
          Data2: 100
      Extra:
          Extra.Custom.Key: Custom Value
  ```

- <a id="definitions/Articluation"></a>**`Articluation`** *(object)*: Defines an articulation with its name and associated MIDI messages. Cannot contain additional properties.
  - <a id="definitions/Articluation/properties/Name"></a>**`Name`** *(string, required)*
  - <a id="definitions/Articluation/properties/MidiMessages"></a>**`MidiMessages`** *(array, required)*
    - <a id="definitions/Articluation/properties/MidiMessages/items"></a>**Items**: Refer to *[#/definitions/MidiMessage](#definitions/MidiMessage)*.
  - <a id="definitions/Articluation/properties/Extra"></a>**`Extra`**: Refer to *[#/definitions/Extra](#definitions/Extra)*.

  Examples:
  ```yaml
  Name: Sustain
  MidiMessages:
  -   Status: 144
      Data1: 64
      Data2: 100
  Extra:
      Extra.Custom.Key: Custom Value
  ```

- <a id="definitions/MidiMessage"></a>**`MidiMessage`** *(object)*: A MIDI message within the articulation, consisting of a status byte and two data bytes. Cannot contain additional properties.
  - <a id="definitions/MidiMessage/properties/Status"></a>**`Status`** *(integer, required)*
  - <a id="definitions/MidiMessage/properties/Data1"></a>**`Data1`** *(integer)*
  - <a id="definitions/MidiMessage/properties/Data2"></a>**`Data2`** *(integer)*

  Examples:
  ```yaml
  Status: 144
  Data1: 64
  Data2: 100
  ```

- <a id="definitions/Extra"></a>**`Extra`** *(object)*: Additional custom properties for the articulation and assignments. Can contain additional properties.

  Examples:
  ```yaml
  Extra.Custom.Key: Custom Value
  ```

