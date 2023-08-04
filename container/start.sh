#!/bin/bash
WAIT_DURATION=1800

chmod +x /*.sh
./compile.sh
service apache2 start

LAST_EXECUTION=0
while true; do
    sleep 30
    CURRENT_TIME=$(date +%s)
    DIFF=$((CURRENT_TIME - LAST_EXECUTION))
    if [[ $DIFF -ge $WAIT_DURATION ]]; then
        /monitor.sh
        LAST_EXECUTION=$CURRENT_TIME
    fi
    CHANGED=$(find /monitor/data -mtime -0.005)
    if [[ $CHANGED -gt 0 ]]; then
        /monitor.sh
        LAST_EXECUTION=$CURRENT_TIME
    fi
done