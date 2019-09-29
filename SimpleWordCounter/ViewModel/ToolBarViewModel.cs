using Microsoft.Win32;
using System;
using System.IO;
using Utilities;

namespace SimpleWordCounter.ViewModel
{
    public class ToolBarViewModel : NotificationObject
    {
        public SimpleCommand OpenCommand { get; }

        private bool _loadingEnabled = true;
        public bool LoadingEnabled
        {
            get { return _loadingEnabled; }
            set
            {
                if (SetField(ref _loadingEnabled, value))
                    OpenCommand?.RaiseCanExecuteChanged();
            }
        }

        public string SavedInitialDirectory
        {
            get
            {
                if (!string.IsNullOrEmpty(Path.Default.LastFilePath) && Directory.Exists(Path.Default.LastFilePath))
                    return Path.Default.LastFilePath;
                return Environment.SpecialFolder.Recent.ToString();
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && Directory.Exists(value))
                {
                    Path.Default.LastFilePath = value;
                    Path.Default.Save();
                }
            }
        }

        public delegate void LoadFileHandler(string filePath);
        public event LoadFileHandler LoadFileRequested;

        public ToolBarViewModel()
        {
            OpenCommand = new SimpleCommand(OpenFile, obj => LoadingEnabled);
        }


        private void OpenFile(object obj)
        {
            var openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = SavedInitialDirectory,
                Filter = "txt files(*.txt) | *.txt"
            };

            if (openFileDialog.ShowDialog() ?? false)
            {
                LoadFileRequested?.Invoke(openFileDialog.FileName);
            }
        }
    }
}
