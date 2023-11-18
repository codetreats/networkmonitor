#!/bin/bash
set -e
##############################################################
LOCAL=/var/www/html/data
DATADIR=$LOCAL/data/
CONFDIR=$LOCAL/config/
LOGDIR=$LOCAL/logs
HTML=/var/www/html
mkdir -p $LOGDIR
mkdir -p $HTML

###################### Create default-configurations ########
chown -R www-data:www-data $LOCAL
chmod -R 777 $LOCAL
LEN=${#DATADIR}
for F in $(find $DATADIR -name "*.txt")
do
  F2=${F:$LEN}
  FILE=${F2/%txt/cfg}
  if [ ! -f $CONFDIR/$FILE ] ; then
    echo "create $CONFDIR/$FILE"
    cat $LOCAL/default.cfg > "$CONFDIR/$FILE"
  fi
done

###################### Generate output  ######################
echo "Run Monitor-Builder"
/usr/bin/mono /bin/monitor.exe $LOCAL $HTML $LOGDIR
/usr/bin/tail -n 1000 $LOGDIR/monitor.log | /usr/bin/tac | sed 's/$/<br>/' > $HTML/log.html

###################### Send mail  ############################
if [ ! -f $LOCAL/mail/subject.txt ] ; then
  exit 0;
fi

if [ ! -f $LOCAL/mail/message.txt ] ; then
  exit 0;
fi

SUBJECT=$(cat $LOCAL/mail/subject.txt)
TEXT=$(cat $LOCAL/mail/message.txt)

cat $LOCAL/mail/subject.txt > $LOCAL/status.txt
echo "" >> $LOCAL/status.txt
cat $LOCAL/mail/message.txt >> $LOCAL/status.txt

echo "Send Mail: $SUBJECT"
/usr/bin/php /pipeline/src/mail.php "$SUBJECT" "$TEXT"

echo "Done"