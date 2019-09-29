using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Utilities;

namespace SimpleWordCounter.ViewModel
{
    public class FileLoadingProgressViewModel : NotificationObject, IContentViewModel
    {
        private readonly long _fileSize;

        public ICommand CancelCommand { get; }

        public IProgress<long> Progress { get; }

        private int _current = 0;
        public int Current
        {
            get { return _current; }
            set { SetField(ref _current, value); }
        }

        public FileLoadingProgressViewModel(long fileSize, CancellationTokenSource cancellationSource)
        {
            _fileSize = fileSize;
            Progress = new Progress<long>(OnProgressChanged);
            CancelCommand = new SimpleCommand(arg => cancellationSource.Cancel());
        }

        private void OnProgressChanged(long obj)
        {
            Application.Current.Dispatcher.Invoke(() => Current = (int)((obj * 100) / _fileSize));
        }
    }
}
