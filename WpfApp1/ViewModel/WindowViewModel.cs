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
using System.Windows.Controls;

namespace WpfApp1
{
    public class WindowViewModel : BaseViewModel
    {
        private ObservableCollection<Rebars>? user_int;
        private Window mWindow;
        private bool _option;
        private ComboBoxItem _mySelectedItem;

        public ObservableCollection<Rebars>? Entries
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


        public bool Option
        {
            get { return _option; }
            set { _option = value; OnPropertyChanged(nameof(Option)); }
        }

        public ComboBoxItem MySelectedItem
        {
            get { return _mySelectedItem; }
            set
            {
                _mySelectedItem = value;
                switch(_mySelectedItem.Content.ToString())
                {
                    case ("Rectangular Column"):
                    case ("Rectangular Beam"):
                        { Option = true; break; }
                    case ("Circular Column"):
                        { Option = false; break; }
                }
            }
        }


        #region Constructor
        public WindowViewModel(Window window)
        {
            mWindow = window;

            MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized); //xor
            CloseCommand = new RelayCommand(() => mWindow.Close());

            Entries = new ObservableCollection<Rebars>();
            Entries.CollectionChanged += OnEntriesCollectionChanged;

        }

        private void OnEntriesCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in Entries)
            {
                item.Count = Entries.IndexOf(item) + 1;
            }
        }

        #endregion

    }

    public class Rebars: BaseViewModel
    {
        private float _dia;
        private int _num;
        private float _delta;
        private int _count;


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

        public int Count
        {
            get => _count;
            set { 
                _count = value;
                OnPropertyChanged(nameof(Count));
            }
        }

        public float DeltaY
        {
            get { return _delta; }
            set { _delta = value; }
        }

        public Rebars()
        {
        }
    }


}
