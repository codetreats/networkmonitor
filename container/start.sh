#!/bin/bash

chmod +x /*.sh
./compile.sh
service apache2 start

while true; do
  /monitor.sh
  sleep 30  
done