#!/bin/bash
set -e
cd $(dirname "$0")
BASEDIR=$(pwd)

assert_var() {
     MSG=$1
     VAR=$2

     if [[ $VAR == "" ]]
     then
          echo $MSG
          exit 1
     fi
}

export MAIL_HOST=$MAIL_HOST
export MAIL_USER=$MAIL_USER
export MAIL_PASSWORD=$MAIL_PASSWORD
export MAIL_FROM=$MAIL_FROM
export MAIL_TO=$MAIL_TO

export DATA_DIR=$DATA_DIR
export CONFIG_DIR=$CONFIG_DIR
export LOGS_DIR=$LOGS_DIR

assert_var "DATA_DIR not set" $DATA_DIR
assert_var "CONFIG_DIR not set" $CONFIG_DIR
assert_var "LOGS_DIR not set" $LOGS_DIR

assert_var "MAIL_HOST not set" $MAIL_HOST
assert_var "MAIL_USER not set" $MAIL_USER
assert_var "MAIL_PASSWORD not set" $MAIL_PASSWORD
assert_var "MAIL_FROM not set" $MAIL_FROM
assert_var "MAIL_TO not set" $MAIL_TO

# remove old container
if [[ $(docker ps -q --filter "name=networkmonitor"  | wc -l) -gt 0 ]]
then
     echo "Remove networkmonitor"
     docker rm -f networkmonitor
fi

docker image prune -f

# build image
cd $BASEDIR/container
docker build -t networkmonitor:0.1.0 .

docker-compose up --detach
