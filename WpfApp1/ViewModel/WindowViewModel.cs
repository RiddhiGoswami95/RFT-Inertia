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
using System.IO;

namespace WpfApp1
{
    public class WindowViewModel : BaseViewModel
    {
        #region Private Properties

        private ObservableCollection<Rebars>? user_int;
        private Window mWindow;
        private bool _option;
        private ComboBoxItem _mySelectedItem;
        private string _image = "Images/full.jpg";
        private float _radius;
        private float _diameter;
        const float PI = 3.14f;

        #endregion

        #region Public Properties/Methods

        public float Ax = 0;
        public float Ay = 0;
        private static float _ccenterX = 0;
        private static float _ccenterY = 0;

        public string Image
        {
            get => _image;
            set { _image = value; OnPropertyChanged(nameof(Image)); }
        }
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

        public float Radius // set for diameter too in this
        {
            get => _radius;
            set
            {
                _radius = value; OnPropertyChanged(nameof(Radius));
                Diameter = _radius * 2; OnPropertyChanged(nameof(Diameter));
            }
        }

        public float Diameter
        {
            get => _diameter;
            set
            {
                _diameter = value; OnPropertyChanged(nameof(Diameter));
                _radius = _diameter / 2; OnPropertyChanged(nameof(Radius));
            }
        }

        public float Centroid
        {
            get => _ccenterX;
            set
            {
                _ccenterX = value; OnPropertyChanged(nameof(Centroid));
            }
        }

        public void Circular_Centroid()
        {
            if (_radius != 0 && user_int is not null)
            {
                
                foreach (var item in user_int)
                {
                    var radius_Rebar = item.Dia / 2; 
                    var slice = (2 * Math.PI) / item.Num;

                    for (int i = 0; i < item.Num; i++)
                    {
                        var angle = slice * i;
                        var x = (_radius - item.DeltaY) * Math.Cos(angle);
                        var y = (_radius - item.DeltaY) * Math.Sin(angle);
                        Ax += PI * (float) Math.Pow(radius_Rebar,2) * (float)x;
                        Ay += PI * (float)Math.Pow(radius_Rebar, 2) * (float)y;
                    }
                    
                }
            }

            float A = PI * (float)Math.Pow(_radius, 2);
            _ccenterX = Ax / A;
            _ccenterY = Ay/A;
        }



        #endregion



        #region Constructor
        public WindowViewModel(Window window)
        {
            mWindow = window;

            MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() =>
            {
                if (mWindow.WindowState != WindowState.Maximized)
                {
                    Image = "Images/norm.jpg";
                    mWindow.WindowState = WindowState.Maximized;
                }
                else
                {
                    Image = "Images/full.jpg";
                    mWindow.WindowState = WindowState.Normal;
                }
            });
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
