FROM mcr.microsoft.com/dotnet/sdk:8.0

RUN apt-get update && apt-get install --no-install-recommends -y jq

WORKDIR /etc/app/src

ENV PATH="$PATH:/root/.dotnet/tools"
ENV NUGET_PACKAGES="/etc/app/nuget"

RUN dotnet tool install --global dotnet-ef

COPY . .

RUN dotnet ef migrations bundle --configuration release --project TodoList.Infrastructure --startup-project TodoList.Api --context TodoList.Infrastructure.Persistence.TodoListDbContext --self-contained -o ./TodoList.Infrastructure/efbundle

WORKDIR /etc/app/src/TodoList.Infrastructure

COPY ../entrypoint.sh .

# Grant permissions for to our script to be executable and fix line endings
RUN tr -d '\015' < entrypoint.sh > ep2.sh && mv ep2.sh entrypoint.sh && chmod +x entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]