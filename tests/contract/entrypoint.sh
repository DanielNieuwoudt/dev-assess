#!/bin/sh
set -e

START=$(date +%s)

i=0
WEBAPISTATUS=1

echo Waiting for WebApi...

while [ $i -lt 30 ] && [ "${WEBAPISTATUS:-9}" -ne 0 ] ; do
	i=$((i+1))
  set +e
	curl -s "http://backend:5000/health/dependency" | grep -q '\"Status\": \"Healthy'
	WEBAPISTATUS=$?
	set -e
	echo "WebApi status: $WEBAPISTATUS (attempt $i)"
	sleep 5
done

if [ "${WEBAPISTATUS:-9}" -ne 0 ]; then 
 	echo "WebApi took more than 150 seconds to start up or one or more databases are not in an ONLINE state"
	exit 1
fi

END=$(date +%s)

echo "Finished initialisation successfully. Elapsed Time: $((END-START)) seconds"

npm run test:env