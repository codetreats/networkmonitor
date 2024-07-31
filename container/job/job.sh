#!/bin/bash
###############################################
RUNSTEP=/pipeline/run_step.sh
###############################################
set -e
STATUS=$1

# Enter your pipeline steps below
# Syntax: $RUNSTEP $STATUS <DESCRIPTION> <COMMAND>
$RUNSTEP $STATUS "Update Monitor" "/job/monitor.sh"