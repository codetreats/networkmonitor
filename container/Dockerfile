FROM codetreats/simple-pipeline:latest
RUN apt-get update && apt-get install -q -y mono-complete
RUN mkdir -p /bin /var/www/html/data/mail /var/www/html/data/data /var/www/html/data/config /var/www/html/data/logs
RUN touch /var/www/html/data/logs/monitor.log
COPY www/ /var/www/html/
COPY monitor-src/ /monitor-src
COPY job/ /job
COPY default.cfg /var/www/html/data/
RUN rm /var/www/html/index.html
RUN bash menu "MONITOR" "/index.html"
RUN bash menu "DATA" "/data/index.php"
RUN crontrigger "*/15 * * * *"
CMD ["/start.sh"]

