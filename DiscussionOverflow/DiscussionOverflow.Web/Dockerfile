FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
RUN apt-get update && apt-get install -y nodejs
COPY ["DiscussionOverflow.Web/*.csproj", "DiscussionOverflow.Web/"]
COPY ["DiscussionOverflow.Application/*.csproj", "DiscussionOverflow.Application/"]
COPY ["DiscussionOverflow.Domain/*.csproj", "DiscussionOverflow.Domain/"]
COPY ["DiscussionOverflow.Infrastructure/*.csproj", "DiscussionOverflow.Infrastructure/"]
RUN dotnet restore "DiscussionOverflow.Web/DiscussionOverflow.Web.csproj"
COPY . .
WORKDIR "/src/DiscussionOverflow.Web"
RUN dotnet build "DiscussionOverflow.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DiscussionOverflow.Web.csproj" -c Release -o /app

FROM build AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 80
ENTRYPOINT ["dotnet", "DiscussionOverflow.Web.dll"]