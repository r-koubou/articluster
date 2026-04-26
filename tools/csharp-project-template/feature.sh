#!/bin/bash

if [ -z "$1" ]; then
  echo "Usage: $0 <FeatureName>"
  exit 1
fi

SCRIPT_DIR=$(cd $(dirname "$0"); pwd)

set -euo pipefail
pushd "$SCRIPT_DIR" > /dev/null


./module.sh Features.$1.Domain --no-test
./module.sh Features.$1.Facades --no-test
./module.sh Features.$1.Gateways --no-test
./module.sh Features.$1.Infrastructures
./module.sh Features.$1.UseCases --no-test

# Setup output directory (Create subdirectories if '.' is included in the feature name)
feature_dir="out/Features/$1"
feature_dir="${feature_dir//.//}"

mkdir -p "$feature_dir"

# rename directories : remove $1 from $1.xxx
mv out/Features.$1.Domain $feature_dir/Domain
mv out/Features.$1.Facades $feature_dir/Facades
mv out/Features.$1.Gateways $feature_dir/Gateways
mv out/Features.$1.Infrastructures $feature_dir/Infrastructures
mv out/Features.$1.Infrastructures.Tests $feature_dir/Infrastructures.Tests
mv out/Features.$1.UseCases $feature_dir/UseCases

# rename *.csproj : remove Features. from Features.$1.xxx.csproj
mv $feature_dir/Domain/Features.$1.Domain.csproj $feature_dir/Domain/$1.Domain.csproj
mv $feature_dir/Facades/Features.$1.Facades.csproj $feature_dir/Facades/$1.Facades.csproj
mv $feature_dir/Gateways/Features.$1.Gateways.csproj $feature_dir/Gateways/$1.Gateways.csproj
mv $feature_dir/Infrastructures/Features.$1.Infrastructures.csproj $feature_dir/Infrastructures/$1.Infrastructures.csproj
mv $feature_dir/Infrastructures.Tests/Features.$1.Infrastructures.Tests.csproj $feature_dir/Infrastructures.Tests/$1.Infrastructures.Tests.csproj
mv $feature_dir/UseCases/Features.$1.UseCases.csproj $feature_dir/UseCases/$1.UseCases.csproj

popd > /dev/null
