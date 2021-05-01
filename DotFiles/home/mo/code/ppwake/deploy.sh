#!/bin/bash

dotnet build
rsync -rv bin/Debug/net5.0/ mo@192.168.0.6:/home/mo/ppwake