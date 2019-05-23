using NBench;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TaskManager.NBench
{
    public class NBenchTest
    {
        private const string URL = "https://localhost:44371/api/tasks";
        private const string ContentType = "application/json";

        [PerfBenchmark(NumberOfIterations = 500, RunMode = RunMode.Throughput, TestMode = TestMode.Test,
            SkipWarmups = true, RunTimeMilliseconds = 1000)]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 60000)]
        public void BenchmarkMethod_ElapsedTime(BenchmarkContext context)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(URL);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(ContentType));

            var response = httpClient.GetAsync("").Result;
        }

        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Measurement)]
        [GcMeasurement(GcMetric.TotalCollections, GcGeneration.AllGc)]
        public void BenchmarkMethod_GC(BenchmarkContext context)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(URL);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(ContentType));

            var response = httpClient.GetAsync("").Result;
        }

        [PerfBenchmark(NumberOfIterations = 5, RunMode = RunMode.Throughput, TestMode = TestMode.Test,
            RunTimeMilliseconds = 2500)]
        [MemoryAssertion(MemoryMetric.TotalBytesAllocated, MustBe.LessThanOrEqualTo, 10485760)]
        public void BenchmarkMethod_Memory(BenchmarkContext context)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(URL);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(ContentType));

            var response = httpClient.GetAsync("").Result;
        }
    }
}