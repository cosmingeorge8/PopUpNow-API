#!/bin/bash

set -e
run_cmd="dotnet run ASPNETCORE_URLS=http://*:7557 ASPNETCORE_ENVIRONMENT=Development"

dotnet tool install --global dotnet-ef --version 5

echo "EF installed"

export PATH="$PATH:/root/.dotnet/tools"

until dotnet ef database update; do
>&2 echo "SQL Server is starting up"
sleep 1
done

>&2 echo "SQL Server is up - executing command"
exec $run_cmd