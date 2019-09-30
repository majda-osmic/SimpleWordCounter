using Microsoft.Win32;
using SimpleWordCounter.Model;
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

        public string SavedOrDefaultInitialDirectory
        {
            get
            {
                var lastPath = PathConfiguration.Default.LastDir;
                if (!string.IsNullOrEmpty(lastPath) && Directory.Exists(lastPath))
                    return lastPath;
                return Environment.SpecialFolder.Recent.ToString();
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && Directory.Exists(value))
                {
                    PathConfiguration.Default.LastDir = value;
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
                InitialDirectory = SavedOrDefaultInitialDirectory,
                Filter = "txt files(*.txt) | *.txt"
            };

            if (openFileDialog.ShowDialog() ?? false)
            {
                LoadFileRequested?.Invoke(openFileDialog.FileName);
                SavedOrDefaultInitialDirectory = Path.GetDirectoryName(openFileDialog.FileName);
            }
        }
    }
}
