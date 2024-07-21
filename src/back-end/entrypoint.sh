#!/bin/sh
set -e

if [ "$APPLY_MIGRATION" = "true" ]
    then
        echo "Starting Bundled Migration"
        ./efbundle --connection "${CONNECTION_STR}"
        echo "Finished Bundled Migration"
    else
        echo "Skipping Bundled Migration"
fi