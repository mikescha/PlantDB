using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace PlantDB
{
    class PageLocationViewModel : INotifyPropertyChanged
    {
        public PageLocationViewModel()
        {
            ShowCritterArea = false;
            ShowSizeArea = false;
        }

        private bool showCritterArea;
        public bool ShowCritterArea
        {
            set
            {
                if (showCritterArea != value)
                {
                    showCritterArea = value;
                    OnPropertyChanged();
                }
            }
            get { return showCritterArea; }
        }


        private bool showSizeArea;
        public bool ShowSizeArea
        {
            set
            {
                if (showSizeArea != value)
                {
                    showSizeArea = value;
                    OnPropertyChanged();
                }
            }
            get { return showSizeArea; }
        }




        #region INPC

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INPC
    }
}
