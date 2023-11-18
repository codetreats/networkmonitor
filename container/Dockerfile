FROM codetreats/simple-pipeline:latest
RUN apt-get update && apt-get install -q -y mono-complete
RUN mkdir -p /bin /var/www/html/data/mail /var/www/html/data/data /var/www/html/data/config /var/www/html/data/logs
RUN touch /var/www/html/data/logs/monitor.log
COPY www/ /var/www/html/
COPY monitor-src/ /monitor-src
COPY pipeline/ /pipeline/src
COPY job/ /job
COPY default.cfg /var/www/html/data/
RUN rm /var/www/html/index.html
RUN echo "MONITOR=>/index.html" >> /var/www/html/pipeline/menu.txt
RUN echo "DATA=>/data/index.php" >> /var/www/html/pipeline/menu.txt
RUN echo "*/15 * * * * root /trigger.sh" > /etc/cron.d/cronjob
CMD ["/start.sh"]

