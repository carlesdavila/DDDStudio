using BenchmarkDotNet.Running;
using DDDCanvasCreator.Benchmarks;

var summary = BenchmarkRunner.Run<BcBasicCreatorBenchmark>();