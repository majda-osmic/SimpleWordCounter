using System.ComponentModel;
using System.Configuration;
using System.Text;

namespace SimpleWordCounter.Model
{
    public class PathConfiguration : ApplicationSettingsBase
    {
        static PathConfiguration _defaultInstance = (PathConfiguration)Synchronized(new PathConfiguration());
        public static PathConfiguration Default { get => _defaultInstance; }

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Save();
            base.OnPropertyChanged(sender, e);
        }

        [UserScopedSetting]
        public string LastDir
        {
            get { return (string)this[nameof(LastDir)]; }
            set { this[nameof(LastDir)] = value; }
        }


    }
}
