# Clone Repository
FROM alpine AS git_client
MAINTAINER knight

RUN apk update
RUN apk add git
WORKDIR /repo
RUN git clone https://github.com/knight720/VoteIt.git

# Build Application
FROM microsoft/dotnet:2.2-sdk AS dotnet_sdk
WORKDIR /code

## copy csproj and restore as distinct layers
COPY --from=git_client /repo/VoteIt/src/VoteIt/*.sln .
COPY --from=git_client /repo/VoteIt/src/VoteIt/VoteIt/*.csproj ./VoteIt/
RUN dotnet restore

# copy everything else and build app
COPY --from=git_client /repo/VoteIt/src/VoteIt/. ./VoteIt/
WORKDIR /code/VoteIt
RUN dotnet publish -c Release -o code_publish

# Deploy Application
FROM microsoft/dotnet:2.2-aspnetcore-runtime AS dotnet_runtime

# Development, Staging, Production
ARG ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_ENVIRONMENT ${ASPNETCORE_ENVIRONMENT}

WORKDIR /app
COPY --from=dotnet_sdk /code/VoteIt/VoteIt/code_publish ./
COPY --from=git_client /repo/VoteIt/data/VoteItDB.sqlite /data/
COPY ./appsettings.Production.json ./
CMD ["dotnet", "VoteIt.dll"]
