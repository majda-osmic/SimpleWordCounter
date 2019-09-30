using SimpleWordCounter.Model;
using SimpleWordCounter.ViewModel;
using System;
using System.Threading;
using System.Windows;
using Utilities;

namespace SimpleWordCounter
{
    public class MainWindowViewModel : NotificationObject
    {

        public string Title => "Simple Word Counter";


        private StatusBarViewModel _statusBar = new StatusBarViewModel();
        public StatusBarViewModel StatusBar
        {
            get { return _statusBar; }
            set { SetField(ref _statusBar, value); }
        }

        private ToolBarViewModel _toolBar = new ToolBarViewModel();
        public ToolBarViewModel ToolBar
        {
            get { return _toolBar; }
            set { SetField(ref _toolBar, value); }
        }

        private IContentViewModel _content;
        public IContentViewModel Content
        {
            get { return _content; }
            set { SetField(ref _content, value); }
        }

        public MainWindowViewModel()
        {
            ToolBar.LoadFileRequested += OnLoadFile;
        }

        private async void OnLoadFile(string filePath)
        {
            try
            {
                ToolBar.LoadingEnabled = false;

                var file = new File(filePath);

                StatusBar.File = file;

                var cancellationSource = new CancellationTokenSource();
                var progressVM = new FileLoadingProgressViewModel(file.Size, cancellationSource);
                Content = progressVM;

                await file.LoadAsync(cancellationSource.Token, progressVM.Progress);

                if (file.CurrentStatus != Status.Canceled)
                {
                    Content = new LoadedContentViewModel() { LoadedData = file.LoadedData };
                }
                else
                {
                    StatusBar.File = null; 
                    Content = null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Cannot open file {filePath}:\n{e.Message}");
            }
            finally
            {
                ToolBar.LoadingEnabled = true;
            }
        }
    }
}
