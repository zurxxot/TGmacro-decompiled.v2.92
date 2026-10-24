# TGmacro-decompiled.v2.92

Decompiled C# source of **TGM / TGMacro v2.92** (a Windows gaming macro recorder by [trksyln](https://github.com/trksyln/TGMacro)).

The author's public repo only contains an old v2.01 snapshot, so this was produced by decompiling the released `TGMacro.exe` 2.92 with [ILSpy](https://github.com/icsharpcode/ILSpy) (`ilspycmd` 9.1). It is a reconstruction: compiler-generated code (async methods, lambdas) reads oddly, and the original comments and formatting are gone. The exe was not obfuscated, so class, method and form names are intact.

## Building

Needs the .NET SDK (8 or newer) and Windows.

```
dotnet build -c Release -o bin
```

`libs/CSInputs.dll` is the author's input library, copied unchanged from the release. Its source is at <https://github.com/trksyln/CSInputs>.

## Changes from the raw decompiler output

- `TargetFramework` changed from `net45` to `net48`.
- Added `GenerateResourceUsePreserializedResources` and a `System.Resources.Extensions` package reference so the form resources build.
- `CSInputs.dll` reference points at `libs\CSInputs.dll`.

No source files were edited after decompiling.

## License

The original project is MIT licensed, Copyright (c) 2021 trksyln. See [LICENSE](LICENSE). This repository is not affiliated with the author.