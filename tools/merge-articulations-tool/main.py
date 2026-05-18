import sys
import os.path

from ruamel.yaml import YAML

yaml = YAML()
yaml.indent(mapping=2, sequence=4, offset=2)


def load_yaml_file(file_path: str) -> dict:
    with open(file_path, "r") as f:
        return yaml.load(f)

def main(args: list[str]):
    input_base_dir = args[0]
    output_file_name = args[1]
    base_yaml_file = args[2]
    merge_yaml_files: list[str] = []

    for i in range(3, len(args)):
        merge_yaml_files.append(args[i])

    # Load Base YAML file
    base_yaml_data = load_yaml_file(os.path.join(input_base_dir, base_yaml_file))

    # Append Articulation in 'ArticulationGroups' section
    for merge_yaml_file in merge_yaml_files:
        merge_yaml_data = load_yaml_file(os.path.join(input_base_dir, merge_yaml_file))
        if "ArticulationGroups" in merge_yaml_data:
            if "ArticulationGroups" not in base_yaml_data:
                base_yaml_data["ArticulationGroups"] = []
            base_yaml_data["ArticulationGroups"].extend(merge_yaml_data["ArticulationGroups"])

    # Output Yaml to file
    with open(os.path.join(input_base_dir, output_file_name), "w") as f:
        yaml.dump(base_yaml_data, f)

if __name__ == "__main__":
    if len(sys.argv) < 4:
        print("Usage: python main.py <input_base_dir> <output_file_name> <base_yaml_file> <merge_yaml_file1> [<merge_yaml_file2> ...]")
        sys.exit(1)

    main(sys.argv[1:])
