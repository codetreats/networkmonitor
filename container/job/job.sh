#!/bin/bash
###############################################
RUNSTEP=/pipeline/src/run_step.sh
###############################################
set -e
STATUS=$1

# Enter your pipeline steps below
# Syntax: $RUNSTEP $STATUS <DESCRIPTION> <COMMAND>
$RUNSTEP $STATUS "Update Monitor" "/job/monitor.sh"
$RUNSTEP $STATUS "Wait $SLEEPTIME seconds" "/job/wait.sh"
