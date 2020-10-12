FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env

# Install NPM
RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs

WORKDIR /Docker
# Copy CORE project csproj and restore as distinct layers
COPY ./ProfilerLite.Core/ProfilerLite.Core.csproj ./ProfilerLite.Core/ProfilerLite.Core.csproj
WORKDIR /Docker/ProfilerLite.Core
RUN dotnet restore
COPY ./ProfilerLite.Core ./
## Copy csproj and restore as distinct layers
WORKDIR /Docker/ProfilerLite
COPY ./ProfilerLite/*.csproj ./
RUN dotnet restore
## Copy everything else and build
COPY ./ProfilerLite/ ./
RUN dotnet publish -c Release -o out
## Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 as runtime

ENV ConnectionStrings__SqlLogDb="Data Source=host.docker.internal;Initial Catalog=SqlDbLog;Persist Security Info=True;User ID=sa;Password=tra1n1ng!;MultipleActiveResultSets=True"

WORKDIR /app
COPY --from=build-env /Docker/ProfilerLite/out .
ENTRYPOINT ["dotnet", "ProfilerLite.dll", "--no-launch-profile"]