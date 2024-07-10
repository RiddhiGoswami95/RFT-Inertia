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

        private static float _ccenterX = 0;
        private static float _ccenterY = 0;
        private static double _rebarIx = 0;
        private static double _rebarIy = 0;
        private static double _rebarRx = 0;
        private static double _rebarRy = 0;

        #endregion

        #region Public Properties/Methods

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
        public ICommand Calculate { get; set; }

        public double RebarIx
        {
            get => _rebarIx;
            set
            {
                _rebarIx = value; OnPropertyChanged(nameof(RebarIx));
            }
        }
        public double RebarIy
        {
            get => _rebarIy;
            set
            {
                _rebarIy = value; OnPropertyChanged(nameof(RebarIy));
            }
        }
        public double RebarRx
        {
            get => _rebarRx;
            set
            {
                _rebarRx = value; OnPropertyChanged(nameof(RebarRx));
            }
        }
        public double RebarRy
        {
            get => _rebarRy;
            set
            {
                _rebarRy = value; OnPropertyChanged(nameof(RebarRy));
            }
        }
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
        public float Radius
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
            float A_rebar_x = 0;
            float A_rebar_y = 0;

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
                        A_rebar_x += PI * (float) Math.Pow(radius_Rebar,2) * (float)x;
                        A_rebar_y += PI * (float)Math.Pow(radius_Rebar, 2) * (float)y;
                    }
                    
                }
            }

            float A = PI * (float)Math.Pow(_radius, 2);
            Centroid = A_rebar_x / A;
            _ccenterY = A_rebar_y / A;
        }
        public void Inerta_Cal()
        {
            double A_rebar = 0;
            double Ix = 0;
            double Iy = 0;

            if (_radius != 0 && user_int is not null)
            { 
                foreach (var item in user_int)
                {
                    double radius_Rebar = item.Dia / 2;
                    A_rebar = PI * Math.Pow(radius_Rebar, 2);
                    double slice = (2 * Math.PI) / item.Num;

                    for (int i = 0; i < item.Num; i++)
                    {
                        double angle = slice * i;
                        double x = (_radius - item.DeltaY) * Math.Cos(angle);
                        double y = (_radius - item.DeltaY) * Math.Sin(angle);
                        Ix += ((PI * Math.Pow(item.Dia, 4)) / 64) + (A_rebar * Math.Pow(x,2));
                        Iy += ((PI * Math.Pow(item.Dia, 4)) / 64) + (A_rebar * Math.Pow(y, 2));
                    }

                }
            }

            RebarRx = Math.Round(Ix, 5);
            RebarRy = Math.Round(Iy, 2); 

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

            Calculate = new RelayCommand(() => 
            { 
                Circular_Centroid();
                Inerta_Cal();
            });

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
