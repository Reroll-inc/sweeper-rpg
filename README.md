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

```sh
msbuild /p:Configuration=Debug /p:Platform="Any CPU"
```

## Generate UML

- VSCode: execute related task.

```sh
dotnet puml-gen .\\EngineGDI\\Src .\\uml -dir -allInOne -createAssociation
```
