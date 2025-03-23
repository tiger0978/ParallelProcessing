using CSV;
using ParallelProcessing.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelProcessing
{
    internal class Program
    {
        static void Main(string[] args)
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
            int TOTAL_COUNT = 5_000_000;
            int batchCounts = 500_000;
            string outputFileName = Path.Combine("Output", $"MOCK_DATA_{TOTAL_COUNT}.csv");
            if(File.Exists( outputFileName ))
            {
                File.Delete( outputFileName );
            }
            Stopwatch stopwatch = new Stopwatch();
            string fileName = Path.Combine("Input", $"MOCK_DATA_{TOTAL_COUNT}.csv");
            double readTotalTime = 0;
            double writeTotalTime = 0;

            for (int i = 0; i < (TOTAL_COUNT / batchCounts)+1; i++)
            {
                stopwatch.Start();
                var datas = csvHelper.ReadCSV<Person>(fileName, i*batchCounts, batchCounts);
                stopwatch.Stop();
                double readTime = stopwatch.Elapsed.TotalSeconds;
                readTotalTime += readTime;
                //stopwatch.Start();
                //csvHelper.WriteToCSV<Person>(outputFileName, datas);
                //stopwatch.Stop();
                //double writeTime = stopwatch.Elapsed.TotalSeconds;
                //writeTotalTime += writeTime;
                //Console.WriteLine($"處理 {i+1} 批 | 資料筆數: {datas.Count} | 讀取時間 {readTime} 秒 | 寫入時間: {writeTime} 秒 | 批次時間: {writeTime+readTime} 秒");
                Console.WriteLine($"處理 {i + 1} 批 | 資料筆數: {datas.Count} | 讀取時間 {readTime} 秒 | 批次時間: {readTime} 秒");
                stopwatch.Reset();
            }
            //Console.WriteLine($"讀取總時間: {readTotalTime} 秒 | 寫入總時間: {writeTotalTime} 秒 | 處理時間共: {readTotalTime + writeTotalTime} 秒");
            Console.WriteLine($"讀取總時間: {readTotalTime} 秒 | 處理時間共: {writeTotalTime} 秒");

            #endregion

            Console.ReadKey();
        }
    }
}
