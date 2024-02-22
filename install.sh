#!/bin/bash
source $CODETREATS_BASHUTILS_DIR/docker-utils.sh
export MONITOR_SRC=pipeline.$ORGANISATION.$REPO

remove_container networkmonitor
prune_images

export DATA_DIR=$DATA_DIR
export CONFIG_DIR=$CONFIG_DIR
export LOGS_DIR=$LOGS_DIR

assert_var "DATA_DIR not set" $DATA_DIR
assert_var "CONFIG_DIR not set" $CONFIG_DIR
assert_var "LOGS_DIR not set" $LOGS_DIR

build_and_up networkmonitor:master