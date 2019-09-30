using SimpleWordCounter.Model;
using System;
using System.ComponentModel;
using Utilities;

namespace SimpleWordCounter.ViewModel
{
    public class StatusBarViewModel : NotificationObject
    {
        private string _status;
        public string Status
        {
            get { return _status; }
            set { SetField(ref _status, value); }
        }

        public string TotalWordCount => (File?.WordCount.HasValue ?? false) ? File.WordCount.ToString() : null;
        public string UniqeWordCount => (File?.UniqueWordCount.HasValue ?? false) ? File.UniqueWordCount.ToString() : null;

        private IFile _file;
        public IFile File
        {
            get { return _file; }
            set
            {
                if (_file != null)
                    _file.PropertyChanged -= OnFilePropertyChanged;
                _file = value;
                if (_file != null)
                    _file.PropertyChanged += OnFilePropertyChanged;
                HandleCurrentFileStatus();
            }
        }

        public StatusBarViewModel()
        {
            HandleCurrentFileStatus();
        }

        private void OnFilePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            HandleCurrentFileStatus();
        }

        private void HandleCurrentFileStatus()
        {
            RaisePropertyChnaged(nameof(TotalWordCount));
            RaisePropertyChnaged(nameof(UniqeWordCount));

            if (File == null)
            {
                Status = "Ready!";
                return;
            }

            switch (File?.CurrentStatus)
            {
                case Model.Status.Loaded:
                case Model.Status.Loading:
                    Status = File.Path;
                    break;

                default:
                    Status = string.Empty;
                    break;
            }
        }

    }
}
