#!/bin/sh

mkdir -p ./.out
uvx jsonschema2md --header-level 2 \
                  --show-examples all \
                  --examples-as-yaml  \
                  ./universal-definition-schema.json \
                  ./.out/generated.md
