FROM mono:6.12.0.182
RUN apt-get update && apt-get install -q -y apache2 php libapache2-mod-php php-cli php-common nano
RUN mkdir -p /bin /monitor/mail /monitor/data /monitor/config /monitor/logs
RUN touch /monitor/logs/monitor.log
RUN echo "Europe/Berlin" > /etc/timezone
COPY www/ /var/www/html/
COPY src/ /src
COPY default.cfg /monitor/
COPY compile.sh /compile.sh
COPY monitor.sh /monitor.sh
COPY start.sh /start.sh
COPY mail.php /mail.php

CMD ["/start.sh"]
