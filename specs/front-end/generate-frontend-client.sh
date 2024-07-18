#!/bin/bash

export MSYS_NO_PATHCONV=1
set -e

scriptPath=$(dirname "$0")

echo "starting current path is $(pwd)"
echo "value of scriptPath is ${scriptPath}"

pushd $scriptPath/

echo "current path is $(pwd)"

# Parameters
openApiVersion="v7.0.0"
generatorName="typescript-axios"
apiSpecFile="webapi.openapi.yaml"
packageName=""
apiSpecFileInput="$(pwd)" #static spec for the meanwhile here
clientOutput="$(pwd)/../../src/front-end/src/services/generated"
additionalProperties="typescriptThreePlus=true,supportsES6=true,withInterfaces=true"

# Temporary file for code generation
cp ../todoitems.openapi.yaml ./webapi.openapi.yaml

docker pull openapitools/openapi-generator-cli:$openApiVersion

echo "Generating Open API client"

rm -rf $clientOutput/${packageName:+$packageName/}**

docker run --rm \
    -v "$apiSpecFileInput:/specifications" \
    -v "$clientOutput:/tmp${packageName:+/src}" \
    "openapitools/openapi-generator-cli:$openApiVersion" generate \
    -i "/specifications/$apiSpecFile" \
    --additional-properties $additionalProperties \
    --generator-name $generatorName \
    -o /tmp

# Temporary file for code generation
rm ./webapi.openapi.yaml

popd