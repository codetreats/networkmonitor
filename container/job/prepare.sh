#!/bin/bash
/job/compile.sh
echo "<?php" > /var/www/html/update/inc/timezone.inc.php
echo "date_default_timezone_set('$TZ');" >> /var/www/html/update/inc/timezone.inc.php
echo "?>" >> /var/www/html/update/inc/timezone.inc.php
/trigger.sh