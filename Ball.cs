using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace делаем_норм_скалодром__2_
{
    class Ball
    {
        public Vector position = new Vector();//, velocity = new Vector();
        Brush color;
        public double radius, coeff;
        public int pwindowheight, pwindowwidth;//, number;//, life = 1, score = 0;
        //public Point pos;
        public Ellipse ell = new Ellipse();
        public Ball(Brush _color) { color = _color; }

        public Ball(double _radius, Brush _color, int _windowheight, int _windowwidth, double _coeff)
        {
           /* switch (_leftright)
            {
               // case ("left")

            }

            if (_leftright == "left") number = 1;
            else if (_leftright == "right") number = 2;
            else number = 0;*/
            radius = _radius;
            color = _color;
            coeff = _coeff;
            pwindowheight = _windowheight;
            pwindowwidth = _windowwidth;
            ell.Width = ell.Height = 2 * _radius * _coeff;
            ell.Fill = _color;

        }
        public void rand(double maxposition, double maxvelocity)//в метрах
        {
            Random randomposition = new Random();
            position.X = (double)pwindowwidth / 2 /coeff + maxposition * (1 - 2 * randomposition.NextDouble()) * Math.Cos(randomposition.NextDouble() * 2 * Math.PI);
            position.Y = (double)pwindowheight / 2 /coeff - maxposition * (1 - 2 * randomposition.NextDouble()) * Math.Sin(randomposition.NextDouble() * 2 * Math.PI);

           /* Random randomvelocity = new Random(); Random randomangle = new Random();
            velocity.X = maxvelocity * (1 - 2 * randomvelocity.NextDouble()) * Math.Cos(randomangle.NextDouble() * 2 * Math.PI);
            velocity.Y = maxvelocity * (1 - 2 * randomvelocity.NextDouble()) * Math.Sin(randomangle.NextDouble() * 2 * Math.PI);
       */ }
        public void setpos(double _x, double _y)//, double _Vx, double _Vy)
        {
            position.X = _x;
            position.Y = _y;
            Canvas.SetLeft(ell, pwindowwidth / 2 + position.X * coeff - ell.Width / 2);
            Canvas.SetTop(ell, pwindowheight / 2 - position.Y * coeff - ell.Height / 2);
            // velocity.X = _Vx;
            // velocity.Y = _Vy;
        }

        public void setpospix(int _px, int _py)//, double _Vx, double _Vy)
        {
            position.X = (-pwindowwidth/2 + _px) / coeff;
            position.Y = (pwindowheight / 2 - _py) / coeff;
        }
        public void paint()//coeff - перевод из метров в пиксели
        {
            //pos.X = position.X * coeff;
            //pos.Y = position.Y * coeff;

            Canvas.SetLeft(ell, pwindowwidth / 2 + position.X * coeff - ell.Width/2);
            Canvas.SetTop(ell, pwindowheight / 2 - position.Y * coeff - ell.Height/2);
        }
        public Vector changeposition( int _fps,Vector _velocity)
        {
            double dt = 1.0 / (double)_fps;
            //проверочка на стенки, простые отражения
            if (Math.Abs((position + _velocity * dt).X) > (double)pwindowwidth / 2 / coeff - radius) 
                _velocity.X *= -1;
           
            if (Math.Abs((position + _velocity * dt).Y) > (double)pwindowheight / 2 / coeff - radius)
                _velocity.Y *= -1;

            position += _velocity * dt;
            return _velocity;
        }

        public Vector VSchangeposition(int _fps, Vector _velocity, out string _border)
        {
            double dt = 1.0 / (double)_fps;
            //проверочка на стенки, простые отражения
            if ((position + _velocity * dt).X > (double)pwindowwidth / 2 / coeff - radius)
            {
                _border = "right";
                return _velocity;
            }
            if ((position + _velocity * dt).X < -((double)pwindowwidth / 2 / coeff - radius))
            {
                _border = "left";
                return _velocity;
            }

            if (Math.Abs((position + _velocity * dt).Y) > (double)pwindowheight / 2 / coeff - radius)
                _velocity.Y *= -1;

            position += _velocity * dt;
            _border = "null";
            return _velocity;
        }

        //проверочка на столкновения с игроком
        public Vector collision(double _x, double _y, Vector _velocity)
        {
            Vector dist = new Vector(_x - position.X, _y - position.Y);
            double distance = dist.Length,
             mindistance = 2 * radius;
            if (distance <= mindistance)
            {
                double absvelocity = _velocity.Length;
                dist.Normalize();
                _velocity.Normalize();
                _velocity -= 2 * dist;
                _velocity.Normalize();
                _velocity *= absvelocity;
            }
            return _velocity;
        }
    }
}
