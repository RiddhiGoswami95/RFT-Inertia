using WpfApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Fasetto.Word;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WpfApp1
{
    public class WindowViewModel : BaseViewModel
    {
        private ObservableCollection<Rebars> user_int;
        private Window mWindow;

        public ObservableCollection<Rebars> Entries
        {
            get { return user_int; }
            set
            {
                user_int = value;
                OnPropertyChanged(nameof(Entries));
            }
        }

        public ICommand MinimizeCommand { get; set; }
        public ICommand MaximizeCommand { get; set; }
        public ICommand CloseCommand { get; set; }

   
        #region Constructor
        public WindowViewModel(Window window)
        {
            mWindow = window;

            MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized); //xor
            CloseCommand = new RelayCommand(() => mWindow.Close());
            Entries = new ObservableCollection<Rebars>();
            //Entries.CollectionChanged += OnEntriesCollectionChanged;

        }

        /*
        private void OnEntriesCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();

            e.NewItems.Count //dynamically change 

        }
        */
        #endregion

    }

    public class Rebars
    {
        private float _dia;
        private static int _num;
        private float _delta;
        private int _count;

        static int Counter = 0;

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        public float Dia
        {
            get { return _dia; }
            set { _dia = value; }
        }

        public int Num
        {
            get { return _num; }
            set { _num = value; }
        }

        public float DeltaY
        {
            get { return _delta; }
            set { _delta = value; }
        }

        public Rebars()
        {
            _count = ++Counter;
        }
    }


}
