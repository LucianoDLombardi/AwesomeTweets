FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 50964
EXPOSE 44317

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["AwesomeTweets/AwesomeTweets.csproj", "AwesomeTweets/"]
RUN dotnet restore "AwesomeTweets/AwesomeTweets.csproj"
COPY . .
WORKDIR "/src/AwesomeTweets"
RUN dotnet build "AwesomeTweets.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AwesomeTweets.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AwesomeTweets.dll"]