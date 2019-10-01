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
            get => _statusBar;
            set => SetField(ref _statusBar, value);
        }

        private ToolBarViewModel _toolBar = new ToolBarViewModel();
        public ToolBarViewModel ToolBar
        {
            get => _toolBar;
            set { SetField(ref _toolBar, value); }
        }

        private IContentViewModel _content = new NoContentViewModel(); //C#8
        public IContentViewModel Content
        {
            get => _content;
            set => SetField(ref _content, value);
        }

        public MainWindowViewModel() => ToolBar.LoadFileRequested += OnLoadFile;

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
                else //C#8
                {
                    StatusBar.File = new EmptyFile(); 
                    Content = new NoContentViewModel();
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
