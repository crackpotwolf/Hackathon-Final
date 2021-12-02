FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["Parse_Documents/Parse_Documents.csproj", "Parse_Documents/"]

COPY ["Data/Data.csproj", "Data/"]
COPY ["Search_Data/Search_Data.csproj", "Search_Data/"]
COPY ["Data_Path/Data_Path.csproj", "Data_Path/"]

RUN dotnet restore "Parse_Documents/Parse_Documents.csproj"

COPY . .
RUN ls
WORKDIR "/src/Parse_Documents"
RUN dotnet build "Parse_Documents.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Parse_Documents.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Parse_Documents.dll"]