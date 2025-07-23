using CSV;
using ParallelProcessing.Models;
using ParallelProcessing_advanced;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParallelProcessing
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string rootPath = @"C:\Users\user\Desktop\ParallelData\";
            CSVHelper csvHelper = new CSVHelper(rootPath);

            #region 非批次讀取寫入
            //int TOTAL_COUNT = 1_0000000;
            //Stopwatch stopwatch = new Stopwatch();
            //string fileName = Path.Combine("Input", $"MOCK_DATA_{TOTAL_COUNT}.csv");
            //stopwatch.Start();
            //var datas = csvHelper.ReadCSV<Person>(fileName);
            ////string filePattern = "*.csv";
            //stopwatch.Stop();

            //Console.WriteLine($"  - 讀取了 {datas.Count} 列資料");
            //Console.WriteLine($"  - 處理時間: {stopwatch.Elapsed.TotalSeconds} 秒");
            //double totalTime = stopwatch.Elapsed.TotalSeconds;
            //stopwatch.Reset();
            //stopwatch.Start();

            //string outputFileName = Path.Combine("Output", $"MOCK_DATA_{TOTAL_COUNT}.csv");
            //csvHelper.WriteToCSV<Person>(outputFileName, datas);
            //stopwatch.Stop();
            //Console.WriteLine($"  - 寫入了 {datas.Count} 列資料");
            //Console.WriteLine($"  - 處理時間: {stopwatch.Elapsed.TotalSeconds} 秒");
            //totalTime += stopwatch.Elapsed.TotalSeconds;
            //Console.WriteLine($"  - 處理時間共: {totalTime} 秒");
            #endregion

            #region 批示讀取寫入
            int TOTAL_COUNT = 20000000;
            int batchCounts = 2500000;
            string outputFileName = Path.Combine("Output", $"MOCK_DATA_{TOTAL_COUNT}.csv");
            string outputDircName = Path.Combine(rootPath,"Output");
            if(Directory.Exists(outputDircName))
            {
               Directory.Delete(outputDircName, true);  
               Directory.CreateDirectory(outputDircName);
            }
            Stopwatch stopwatch = new Stopwatch();
            string fileName = Path.Combine("Input", $"MOCK_DATA_{TOTAL_COUNT}.csv");

            stopwatch.Start();
            var readTimes = new List<double>();
            var writeTimes = new List<double>();
            List<Task> tasks = new List<Task>();    
            for (int i = 0; i < (TOTAL_COUNT / batchCounts); i++)
            {
                int count = i;
                var task = Task.Run(() =>
                {
                    var batchStopwatch = new Stopwatch();
                    batchStopwatch.Start();
                    var datas = csvHelper.OptimizedReadCSV<Person>(fileName, count * batchCounts, batchCounts);
                    batchStopwatch.Stop();
                    readTimes.Add(batchStopwatch.Elapsed.TotalSeconds);
                    batchStopwatch.Reset();
                    batchStopwatch.Start();
                    outputFileName = Path.Combine("Output", $"MOCK_DATA_{TOTAL_COUNT}_{count}.csv");
                    csvHelper.OptimizedWriteToCSV<Person>($"{outputFileName}", datas);
                    batchStopwatch.Stop();
                    writeTimes.Add(batchStopwatch.Elapsed.TotalSeconds);
                    datas.Clear();
                    datas = null;
                    GC.Collect();
                });
                
                tasks.Add(task);    
            }
            await Task.WhenAll(tasks);
            stopwatch.Stop();
            Console.WriteLine($"讀取總時間: {readTimes.Median()} 秒 | 寫入總時間: {writeTimes.Median()} 秒 | 處理時間共: {stopwatch.Elapsed.TotalSeconds} 秒");
            //Console.WriteLine($"讀取總時間: {readTotalTime} 秒 | 處理時間共: {writeTotalTime} 秒");

            #endregion

            Console.ReadKey();
        }
    }
}
