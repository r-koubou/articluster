#!/bin/bash

if [ -z "$1" ]; then
  echo "Usage: $0 <FeatureName>"
  exit 1
fi

SCRIPT_DIR=$(cd $(dirname "$0"); pwd)

set -euo pipefail
pushd "$SCRIPT_DIR" > /dev/null

./module.sh Features.$1.Contracts --no-test
./module.sh Features.$1.Models --no-test
./module.sh Features.$1.Facades --no-test
./module.sh Features.$1.Mappers --no-test
./module.sh Features.$1.Imports --no-test
./module.sh Features.$1.Exports --no-test
./test.sh Features.$1

# Setup output directory (Create subdirectories if '.' is included in the feature name)
feature_dir="out/Features/$1"
feature_dir="${feature_dir//.//}"

mkdir -p "$feature_dir"

# rename directories : remove $1 from $1.xxx
mv out/Features.$1.Contracts $feature_dir/Contracts
mv out/Features.$1.Models $feature_dir/Models
mv out/Features.$1.Facades $feature_dir/Facades
mv out/Features.$1.Mappers $feature_dir/Mappers
mv out/Features.$1.Imports $feature_dir/Imports
mv out/Features.$1.Exports $feature_dir/Exports
mv out/Features.$1.Tests $feature_dir/Tests


# rename *.csproj : remove Features. from Features.$1.xxx.csproj
mv $feature_dir/Contracts/Features.$1.Contracts.csproj $feature_dir/Contracts/$1.Contracts.csproj
mv $feature_dir/Models/Features.$1.Models.csproj $feature_dir/Models/$1.Models.csproj
mv $feature_dir/Facades/Features.$1.Facades.csproj $feature_dir/Facades/$1.Facades.csproj
mv $feature_dir/Mappers/Features.$1.Mappers.csproj $feature_dir/Mappers/$1.Mappers.csproj
mv $feature_dir/Imports/Features.$1.Imports.csproj $feature_dir/Imports/$1.Imports.csproj
mv $feature_dir/Exports/Features.$1.Exports.csproj $feature_dir/Exports/$1.Exports.csproj
mv $feature_dir/Tests/Features.$1.Tests.csproj $feature_dir/Tests/$1.Tests.csproj

popd > /dev/null
