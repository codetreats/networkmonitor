#!/bin/bash
cd /src
SRC=$(ls -1 | grep .cs)
mcs -out:/bin/monitor.exe $SRC