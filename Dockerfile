FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
RUN apk update
RUN apk add curl gnupg 
#RUN curl -sL https://deb.nodesource.com/setup_20.x  | bash -
RUN apk add nodejs
WORKDIR /src
COPY ["app-colecta.csproj", "."]
RUN dotnet restore "./app-colecta.csproj"
COPY . .
RUN dotnet build "app-colecta.csproj" -c Release -o ./app/build
RUN dotnet publish "app-colecta.csproj" -c Release -o ./app/publish

FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS final
WORKDIR /app
COPY --from=build /src/app/publish .
ENV TZ=America/Santiago
CMD ["dotnet","app-colecta.dll","--environment","Production"]