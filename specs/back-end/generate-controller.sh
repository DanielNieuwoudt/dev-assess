#!/bin/sh
export MSYS_NO_PATHCONV=1
set -e
cd "$(dirname "$0")"

# Temporary file for code generation
cp ../todoitems.openapi.yaml ./webapi.openapi.yaml
# Parameters
contractId="webapi.openapi"
#needs absolute path
containerMountVolume="$(pwd)/../../src/back-end/TodoList.Api/Generated"

echo "Building Docker $contractId"
docker build --progress plain -t webapinswaggenerator -f ./Dockerfile .

echo "Running Docker $contractId"
docker run --rm -v "${containerMountVolume}:/tmp/output" webapinswaggenerator

# Temporary file for code generation
rm ./webapi.openapi.yaml

popd