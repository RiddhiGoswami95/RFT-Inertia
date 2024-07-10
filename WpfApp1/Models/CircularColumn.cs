using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class CircularColumn
    {
        public void Inertia_Cal()
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
                        Ix += ((PI * Math.Pow(item.Dia, 4)) / 64) + (A_rebar * Math.Pow(x, 2));
                        Iy += ((PI * Math.Pow(item.Dia, 4)) / 64) + (A_rebar * Math.Pow(y, 2));
                    }

                }
            }

            RebarRx = Math.Round(Ix, 5);
            RebarRy = Math.Round(Iy, 2);

        }


    }
}
