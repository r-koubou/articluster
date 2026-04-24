# JSON Schema

## Definitions

- <a id="definitions/Articulation"></a>**`Articulation`** *(object)*: The root of articulation. Cannot contain additional properties.
  - <a id="definitions/Articulation/properties/Id"></a>**`Id`** *(string, format: uuid, required)*
  - <a id="definitions/Articulation/properties/Author"></a>**`Author`** *(string, required)*
  - <a id="definitions/Articulation/properties/ManufacturerName"></a>**`ManufacturerName`** *(string, required)*
  - <a id="definitions/Articulation/properties/ProductName"></a>**`ProductName`** *(string, required)*
  - <a id="definitions/Articulation/properties/PatchName"></a>**`PatchName`** *(string, required)*
  - <a id="definitions/Articulation/properties/Description"></a>**`Description`** *(string, required)*
  - <a id="definitions/Articulation/properties/Assignments"></a>**`Assignments`** *(array, required)*
    - <a id="definitions/Articulation/properties/Assignments/items"></a>**Items**: Refer to *[#/definitions/Assignment](#definitions/Assignment)*.
  - <a id="definitions/Articulation/properties/Extra"></a>**`Extra`**: Refer to *[#/definitions/Extra](#definitions/Extra)*.

  Examples:
  ```yaml
  Id: 34023d3f-30b0-4646-90e8-5d2978d09d43
  Author: r-koubou
  ManufacturerName: r-koubou
  ProductName: My Guitar
  PatchName: My Guitar 1
  Description: This is my guitar articulation.
  Assignments:
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

- <a id="definitions/Assignment"></a>**`Assignment`** *(object)*: An assignment within the articulation. Cannot contain additional properties.
  - <a id="definitions/Assignment/properties/Name"></a>**`Name`** *(string, required)*
  - <a id="definitions/Assignment/properties/MidiMessages"></a>**`MidiMessages`** *(array, required)*
    - <a id="definitions/Assignment/properties/MidiMessages/items"></a>**Items**: Refer to *[#/definitions/MidiMessage](#definitions/MidiMessage)*.
  - <a id="definitions/Assignment/properties/Extra"></a>**`Extra`**: Refer to *[#/definitions/Extra](#definitions/Extra)*.

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

- <a id="definitions/MidiMessage"></a>**`MidiMessage`** *(object)*: A MIDI message within the assignment. Cannot contain additional properties.
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

