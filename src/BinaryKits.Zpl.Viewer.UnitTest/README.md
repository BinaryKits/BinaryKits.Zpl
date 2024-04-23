# Viewer Unit Test

This project (should) supports the Viewer project in multiple ways.

## Standard unit testing
Mostly testing the features of the virtual printer(s) and analyzers.

## Detect changes and regressions in png outputs
Given a set of known good png output files, we should be able to detect when new output changes.
This would be done by loading 2 pngs into skia bitmaps and xor-ing them. We can then count the amount of black pixels and output a new png showing the difference.

## Feature development
The `CustomTest.cs` exists for quick development. Load your zpl data in `Data/Zpl/custom.zpl2` and test. Changes to these files are not tracked under source control. Use `git update-index --no-skip-worktree custom.zpl2` and `git update-index --skip-worktree custom.zpl2` (see https://stackoverflow.com/a/39776107) to enable tracking changes to the files.

## Benchmarks
Performance isn't stellar yet. We can use features in Visual Studio, but other tools might be more suitable like [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet).

Note that a lot of time is spent inside 3rd party libraries like Skia, BarcodeLib and Zxing.net and Drawing.

## Todo
- [ ] Linux compatibility
- [ ] Benchmarking
- [ ] Xor diffing pngs