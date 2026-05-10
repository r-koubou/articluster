#!/bin/sh

mkdir -p ./.out

uvx jsonschema-markdown --title "Universal Definition Schema" \
                        --examples-format yaml \
                        --no-footer \
                        ./universal-definition-schema.json > ./.out/generated.md
