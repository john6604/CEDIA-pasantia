FROM mcr.microsoft.com/dotnet/runtime:8.0

WORKDIR /opt/app
VOLUME /opt/jobs

COPY entrypoint.sh /usr/local/bin/entrypoint.sh
RUN chmod +x /usr/local/bin/entrypoint.sh

ENTRYPOINT ["/usr/local/bin/entrypoint.sh"]
