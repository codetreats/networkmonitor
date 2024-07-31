#!/bin/bash
cd /monitor-src
SRC=$(ls -1 | grep .cs)
mcs -out:/bin/monitor.exe $SRC