#!/bin/bash

if [ -z "$1" ]; then
  echo "Usage: $0 <FeatureName>"
  exit 1
fi

./module.sh Features.$1.Domain --no-test
./module.sh Features.$1.Facades --no-test
./module.sh Features.$1.Gateways --no-test
./module.sh Features.$1.Infrastructures
./module.sh Features.$1.UseCases --no-test

# rename directories : remove $1 from $1.xxx
mkdir -p out/$1
mv out/Features.$1.Domain out/$1/Domain
mv out/Features.$1.Facades out/$1/Facades
mv out/Features.$1.Gateways out/$1/Gateways
mv out/Features.$1.Infrastructures out/$1/Infrastructures
mv out/Features.$1.Infrastructures.Tests out/$1/Infrastructures.Tests
mv out/Features.$1.UseCases out/$1/UseCases

# rename *.scproj : remove Features. from Features.$1.xxx.csproj
mv out/$1/Domain/Features.$1.Domain.csproj out/$1/Domain/$1.Domain.csproj
mv out/$1/Facades/Features.$1.Facades.csproj out/$1/Facades/$1.Facades.csproj
mv out/$1/Gateways/Features.$1.Gateways.csproj out/$1/Gateways/$1.Gateways.csproj
mv out/$1/Infrastructures/Features.$1.Infrastructures.csproj out/$1/Infrastructures/$1.Infrastructures.csproj
mv out/$1/Infrastructures.Tests/Features.$1.Infrastructures.Tests.csproj out/$1/Infrastructures.Tests/$1.Infrastructures.Tests.csproj
mv out/$1/UseCases/Features.$1.UseCases.csproj out/$1/UseCases/$1.UseCases.csproj
