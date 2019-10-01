using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileParser
{
    public class Parser
    {
        private static char[] _wordSplitters = new char[2] { ' ', '\t' };

        private System.Timers.Timer _timer = new System.Timers.Timer();
        private long _bytesRead = 0;

        public double ProgressReportInveralMs { get; }
        public Encoding Encoding { get; }
        public IProgress<long> Progres { get; }

        public Parser(IProgress<long> progress, double progressReportIntervalMs = 50) : this(Encoding.UTF8, progress, progressReportIntervalMs)
        {

        }

        public Parser(Encoding encoding, IProgress<long> progress, double progressReportIntervalMs)
        {
            Encoding = encoding;
            ProgressReportInveralMs = progressReportIntervalMs;
            Progres = progress;

            if (Progres != null)
            {
                _timer = new System.Timers.Timer(ProgressReportInveralMs)
                {
                    AutoReset = true,
                    Enabled = true
                };
                _timer.Elapsed += (obj, e) => Progres.Report(_bytesRead);
            }
        }

        public Task<Dictionary<string, int>> ParseAsync(string filePath)
        {
            return ParseAsync(filePath, new CancellationToken());
        }

        public Task<Dictionary<string, int>> ParseAsync(string filePath, CancellationToken cancelationToken)
        {
            return Task.Run(() =>
            {
                var result = new Dictionary<string, int>();
                _timer?.Start();
                using var reader = new StreamReader(filePath, Encoding); //C#8
                _bytesRead = 0;
                var line = String.Empty;
                try
                {
                    do
                    {
                        line = reader.ReadLine();
                        MapUniqueWords(line, result);
                        _bytesRead += Encoding.GetByteCount(line);
                        if (cancelationToken.IsCancellationRequested)
                        {
                            return result;
                        }

                    } while (!reader.EndOfStream);

                    return result;
                }
                finally
                {
                    _bytesRead = 0;
                    _timer?.Stop();
                }
            });

        }

        private static void MapUniqueWords(string line, Dictionary<string, int> mapping)
        {
            var data = line.Split(_wordSplitters);

            foreach (var uniqueValue in data)
            {
                if (!mapping.ContainsKey(uniqueValue))
                    mapping[uniqueValue] = 1;
                else
                    mapping[uniqueValue]++;
            }
        }
    }
}
