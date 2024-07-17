FROM mcr.microsoft.com/dotnet/sdk:8.0

RUN apt-get update && apt-get install --no-install-recommends -y jq

WORKDIR /etc/app/src

ENV PATH="$PATH:/root/.dotnet/tools"
ENV NUGET_PACKAGES="/etc/app/nuget"

RUN dotnet tool install --global dotnet-ef

COPY . .
RUN dotnet ef migrations bundle --configuration release --project TodoList.Infrastructure --startup-project TodoList.Api --context TodoList.Infrastructure.Persistence.TodoListDbContext --self-contained -o ./TodoList.Infrastructure/efbundle

WORKDIR /etc/app/src/TodoList.Infrastructure

# Set the entry point directly in the Dockerfile
ENTRYPOINT ["sh", "-c", "set -e; \
    if [ \"$APPLY_MIGRATION\" = \"true\" ]; then \
        CONNECTION_STR=\"${CONNECTION_STR}\"; \
        echo \"Starting Bundled Migration.\"; \
        ./efbundle --connection \"${CONNECTION_STR}\"; \
        echo \"Completed Bundled Migration.\"; \
    else \
        echo \"Skipping Bundled Migration\"; \
    fi"]