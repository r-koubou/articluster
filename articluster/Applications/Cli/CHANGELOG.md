# Changelog

## Version 1.1.0

- Added Generating markdown files feature for `Static Site Generator` from converted data.
- Added `ArticulationGroups` field to Universal Definition specification.
- Modified `FormatVersion` field in Universal Definition specification to simple integer.

## Version 1.0.4

- Modified `FormatVersion` field in Universal Definition specification to Semantic Versioning.

## Version 1.0.3

- Added `FormatVersion` field to Universal Definition specification to allow for future format changes and backward compatibility.

## Version 1.0.2

- Added logging for failures during file loading and conversion processes.
- Added 'Idle' articulation when run `"new command"`
- Fixed an issue where file extensions are no longer required when specifying a file path for the `"new command"` argument.

## Version 1.0.1

- Modified StudioOne's output directory from `<output-dir>/StudioOne/<manufacturer>/<product>/product.keyswitch` to `<output-dir>/StudioOne/<manufacturer>/<product>.keyswitch`

- Added `-o / --overwrite` option to `convert` command to allow overwriting existing files in the output directory.

## Version 1.0.0

Initial release.
