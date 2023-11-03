FROM codetreats/simple-pipeline:0.9.0
RUN apt-get update && apt-get install -q -y mono-complete
RUN mkdir -p /bin /monitor/mail /monitor/data /monitor/config /monitor/logs
RUN touch /monitor/logs/monitor.log
COPY www/ /var/www/html/
COPY monitor-src/ /monitor-src
COPY pipeline/ /pipeline/src
COPY job/ /job
COPY default.cfg /monitor/
RUN echo "*/15 * * * * root /trigger.sh" > /etc/cron.d/cronjob
CMD ["/start.sh"]

