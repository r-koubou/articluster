#!/bin/bash

uv run python main.py templates/module.csproj $1

if [ "$2" == "--no-test" ]; then
    exit 0
fi

uv run python main.py templates/test.csproj $1.Tests
