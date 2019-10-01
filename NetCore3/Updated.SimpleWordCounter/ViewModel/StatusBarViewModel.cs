using SimpleWordCounter.Model;
using System;
using System.ComponentModel;
using Utilities;

namespace SimpleWordCounter.ViewModel
{
    public class StatusBarViewModel : NotificationObject
    {
        private string _status = String.Empty;
        public string Status
        {
            get { return _status; }
            set { SetField(ref _status, value); }
        }

        public string TotalWordCount => File.WordCount.ToString(); //C#8
        public string UniqeWordCount => File.UniqueWordCount.ToString(); //C#8

        private IFile _file = new EmptyFile();
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

        public StatusBarViewModel() => HandleCurrentFileStatus();

        private void OnFilePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            HandleCurrentFileStatus();
        }

        private void HandleCurrentFileStatus()
        {
            RaisePropertyChnaged(nameof(TotalWordCount));
            RaisePropertyChnaged(nameof(UniqeWordCount));

            switch (File.CurrentStatus) //C#8, no need to check for null :)
            {
                case Model.Status.Loaded:
                case Model.Status.Loading:
                    Status = File.Path;
                    break;

                default:
                    Status = "Ready!";
                    break;
            }
        }

    }
}
