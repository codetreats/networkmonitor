#!/bin/bash
WAIT_DURATION=1800

chmod +x /*.sh
./compile.sh
service apache2 start

LAST_EXECUTION=0
while true; do    
    CURRENT_TIME=$(date +%s)
    DIFF=$((CURRENT_TIME - LAST_EXECUTION))
    if [[ $DIFF -ge $WAIT_DURATION ]]; then
        echo "$(date) TIMER" >> /monitor/logs/run.log
        /monitor.sh
        LAST_EXECUTION=$CURRENT_TIME
    fi
    CHANGED=$(find /monitor/data -mtime -0.005 | wc -l)
    if [[ $CHANGED -gt 0 ]]; then
        echo "$(date) MODIFIED" >> /monitor/logs/run.log
        /monitor.sh
        LAST_EXECUTION=$CURRENT_TIME
    fi
    echo "$(date) SLEEP - DIFF: $DIFF, CHANGED: $CHANGED" >> /monitor/logs/run.log
    sleep 30
done