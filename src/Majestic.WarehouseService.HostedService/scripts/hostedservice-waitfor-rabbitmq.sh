#!/bin/bash
# hostedservice-waitfor-rabbitmq.sh

set -e

host="$1"
shift
port="$1"
shift
cmd="$@"

until bash -c ">/dev/tcp/$host/$port" 2>/dev/null; do
  >&2 echo "$host:$port is unavailable - sleeping"
  sleep 1
done

>&2 echo "$host:$port is up - executing command"
exec $cmd
