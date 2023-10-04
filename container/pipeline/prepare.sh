#!/bin/bash
/pipeline/src/compile.sh
echo "<?php" > /var/www/html/update/inc/timezone.inc.php
echo "date_default_timezone_set('$TZ');" >> /var/www/html/update/inc/timezone.inc.php
echo "?>" >> /var/www/html/update/inc/timezone.inc.php
/pipeline/src/trigger.sh