# Sweeper RPG

## Commands to run

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
