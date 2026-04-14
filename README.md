# Sweeper RPG

## Usefull tools

- Developer Powershell for VS

## Commands to run

## Restore deps

```sh
dotnet restore
```

## Install dotnet tools

```sh
dotnet tool restore
```

## Restore MSBuild obj configuration

```sh
msbuild -t:restore
```

## First build with MSBuild

```bash
msbuild /p:Configuration=Debug /p:Platform="Any CPU"
```
