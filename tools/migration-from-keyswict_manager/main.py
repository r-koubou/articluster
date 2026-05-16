import sys
import os
import os.path
import re
import json

import original
import target
import midinote

from ruamel.yaml import YAML

THIS_SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))
OUTPUT_DIR = os.path.join(THIS_SCRIPT_DIR, "out")

yaml = YAML()
yaml.indent(mapping=2, sequence=4, offset=2)


def convert(src: original.Coordinate):
    converted_count = 0

    for ks in src.key_switches:
        id = ks.id
        manufacturer = ks.developer_name
        product = ks.product_name
        patch = ks.instrument_name
        target_articulation_groups: list[target.ArticulationGroup] = []
        target_articulations: list[target.Articulation] = []
        global_extra = ks.extra_data

        print(f"🏃 Converting {manufacturer} {product}: {patch}")

        if ks.articulations is not None:
            for x in ks.articulations:
                name = x.name
                local_extra = x.extra_data
                midi_messages: list[target.MIDIMessage] = []

                if x.midi_message and x.midi_message.note_on:
                    for note_on in x.midi_message.note_on:
                        if note_on.note is None:
                            raise Exception("note_on.note is required")
                        if note_on.channel is None:
                            raise Exception("note_on.channel is required")
                        if note_on.velocity is None:
                            raise Exception("note_on.velocity is required")

                        midi_messages.append(
                            target.MIDIMessage(
                                channel=None,
                                status=0x90,
                                data1=midinote.MIDI_NOTES_MAP[note_on.note],
                                data2=note_on.velocity,
                            )
                        )

                if x.midi_message and x.midi_message.control_change:
                    for control_change in x.midi_message.control_change:
                        if control_change.control_number is None:
                            raise Exception("control_change.control_number is required")
                        if control_change.channel is None:
                            raise Exception("control_change.channel is required")
                        if control_change.data is None:
                            raise Exception("control_change.data is required")

                        midi_messages.append(
                            target.MIDIMessage(
                                channel=None,
                                status=0xB0,
                                data1=control_change.control_number,
                                data2=control_change.data,
                            )
                        )

                if x.midi_message and x.midi_message.program_change:

                    if not x.midi_message.program_change:
                        raise Exception("program_change is required")

                    for program_change in x.midi_message.program_change:

                        if program_change.channel is None:
                            raise Exception("program_change.channel is required")
                        if program_change.program_number is None:
                            raise Exception("program_change.program_number is required")

                        midi_messages.append(
                            target.MIDIMessage(
                                channel=None,
                                status=0xC0,
                                data1=program_change.program_number,
                                data2=0,
                            )
                        )

                if x.extra_data is not None and len(x.extra_data) == 0:
                    local_extra = None

                target_articulations.append(
                    target.Articulation(
                        name=name,
                        midi_messages=midi_messages,
                        extra=local_extra,
                    )
                )
            # ~for x in ks.articulations:

            target_articulation_groups.append(
                target.ArticulationGroup(
                    name=patch,
                    articulations=target_articulations,
                    extra=None
                )
            )

        # ~ if ks.articulations is not None:

        if global_extra is not None and len(global_extra) == 0:
            global_extra = None

        target_coordinate = target.UniversalDefinition(
            format_version='1.0.0',
            id=id,
            author="R-Koubou",
            manufacturer_name=manufacturer,
            product_name=product,
            patch_name=patch,
            articulation_groups=target_articulation_groups,
            extra=global_extra,
            description=f"{manufacturer} {product} - {patch}",
        )

        output_dir = os.path.join(OUTPUT_DIR, manufacturer, product)
        output_filename = f"{patch}.yaml"

        # Normalize filename by replacing invalid characters with underscores
        output_filename = re.sub(r'[\\/*?:"<>|]', "_", output_filename)

        output_path = os.path.join(output_dir, output_filename)

        os.makedirs(output_dir, exist_ok=True)

        with open(output_path, "w") as f:
            yaml.dump(target_coordinate.to_dict(), f)

        print(
            f"👍 Converted {manufacturer} {product}: {patch} -> {os.path.basename(output_path)}"
        )
        converted_count += 1

    print(f"✅ Successfully converted {converted_count} key switches!")


def main(args: list[str]):
    keyswich_manager_db_json_path = args[0]
    os.makedirs(OUTPUT_DIR, exist_ok=True)

    with open(keyswich_manager_db_json_path, "r") as f:
        original_data = json.load(f)

    original_data = original.coordinate_from_dict(original_data)
    convert(original_data)


if __name__ == "__main__":
    args = sys.argv[1:]
    if len(args) == 0:
        print("Usage: python main.py <path/to/KeySwitches.db.json>")
        sys.exit(1)

    main(args)
