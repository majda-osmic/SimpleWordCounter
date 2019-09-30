using FileParser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utilities;

namespace SimpleWordCounter.Model
{

    public enum Status
    {
        NotLoaded,
        Loading,
        Loaded,
        Canceled
    }

    public interface IFile : INotifyPropertyChanged
    {
        Status CurrentStatus { get; }
        int? WordCount { get; }
        int? UniqueWordCount { get; }
        string Path { get; }
    }

    public class File : NotificationObject, IFile
    {
        public string Path { get; }

        public long Size { get; }

        private int? _uniqueWordCount;
        public int? UniqueWordCount
        {
            get { return _uniqueWordCount; }
            set { SetField(ref _uniqueWordCount, value); }
        }

        private int? _wordCount;
        public int? WordCount
        {
            get { return _wordCount; }
            set { SetField(ref _wordCount, value); }
        }

        private Status _currentStatus = Status.NotLoaded;
        public Status CurrentStatus
        {
            get { return _currentStatus; }
            set { SetField(ref _currentStatus, value); }
        }

        private Dictionary<string, int> _loadedData;
        public Dictionary<string, int> LoadedData
        {
            get { return _loadedData; }
            set { SetField(ref _loadedData, value); }
        }

        public File(string path)
        {
            Path = path;
            Size = new FileInfo(path).Length;
        }

        public async Task LoadAsync(CancellationToken token, IProgress<long> progress)
        {
            CurrentStatus = Status.Loading;
            LoadedData = await (new Parser(progress)).ParseAsync(Path, token);
            WordCount = _loadedData?.Values.Sum() ?? null;
            UniqueWordCount = _loadedData?.Keys.Count ?? null;
            CurrentStatus = token.IsCancellationRequested ? Status.Canceled : Status.Loaded;
        }
    }
}
