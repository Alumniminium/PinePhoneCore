#!/bin/bash

dotnet build
scp -r bin/Debug/net5/ mo@192.168.0.6:/home/mo/pplauncher