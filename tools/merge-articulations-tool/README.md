Merge Articulations Tool
========================

This is a maintenance tool for merging articulation groups from multiple YAML files into a single YAML file, which is useful when you have split articulation groups into several parts and want to combine them back into one later.

## Usage

```bash
uv run python main.py <input_base_dir> <output_file_name> <base_yaml_file> <merge_yaml_file1> [<merge_yaml_file2> ...]
```

The output destination is `<input_base_dir>/<output_file_name>`.
