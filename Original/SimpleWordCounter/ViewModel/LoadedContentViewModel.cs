using System.Collections.Generic;
using System.ComponentModel;
using Utilities;

namespace SimpleWordCounter.ViewModel
{
    
    public class LoadedContentViewModel : NotificationObject, IContentViewModel
    {
        private Dictionary<string, int> _loadedData;
        public Dictionary<string, int> LoadedData
        {
            get { return _loadedData; }
            set { SetField(ref _loadedData, value); }
        }
    }



}
