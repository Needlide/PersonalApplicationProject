FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.sln ./
COPY Directory.Packages.props ./
COPY PersonalApplicationProject.API/*.csproj ./PersonalApplicationProject.API/
COPY PersonalApplicationProject.BLL/*.csproj ./PersonalApplicationProject.BLL/
COPY PersonalApplicationProject.DAL/*.csproj ./PersonalApplicationProject.DAL/

RUN dotnet restore

COPY . ./

RUN dotnet tool install -g dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

RUN dotnet ef migrations bundle --self-contained -r linux-x64 --project PersonalApplicationProject.DAL --startup-project PersonalApplicationProject.API

WORKDIR /src/PersonalApplicationProject.API
RUN dotnet publish -c Release -o /app/out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "PersonalApplicationProject.API.dll"]
