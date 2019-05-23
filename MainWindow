using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Kinect;

class _const {
    public const int fps = 100,
         VSmaxscore = 3,
        pwidth = 640, pheight = 640;//480;//pixel proportions
    public const double Step = 12.5;
    public const double width = 4, height = 3, coeff = pwidth / width,//real proportions
        minvelocity = 1.5, maxvelocity = 2.5, radius = 0.1;
}

namespace делаем_норм_скалодром__2_
{
    class Player : Ball
    {
        public int score, win;
        public string orientation;
        public Player(double _radius, Brush _color, int _windowheight, int _windowwidth, double _coeff, string _orientation)
            : base(_radius, _color, _windowheight, _windowwidth, _coeff)
        { win = 0; score = 0; orientation = _orientation; }
    }
    public partial class Versus : Window
    {
        private bool[,] Zacep = new bool[,]  //
        {   { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false} };

        private KinectSensor _sensor;

        private async Task SensorInit()
        {
            _sensor = KinectSensor.KinectSensors.FirstOrDefault();
            if (_sensor == null)
            {
                MessageBox.Show($"{nameof(_sensor)}==Кинект не подключен");
                return;
            }

            _sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            _sensor.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
            _sensor.SkeletonStream.Enable();

            _sensor.SkeletonFrameReady += Sensor_SkeletonFrameReady;

            _sensor.Start();

            // MessageBox.Show("Сенсор перезапущен!");
        }

        private double Arounding(double x, double santim)
        {
            double around;
            x *= 100;
            if (x % santim < (double)santim / 2) {
                around = (x - x % santim) / 100;
                return around;
            }
            else {
                around = (x - x % santim + santim) / 100;
                return around;
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //time.Start();
            await SensorInit();
        }

        private void Sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame sFrame = e.OpenSkeletonFrame();

            if (sFrame == null)
                return;

            Skeleton[] skeletons = new Skeleton[sFrame.SkeletonArrayLength];

            sFrame.CopySkeletonDataTo(skeletons);
            var activeSkeleton = skeletons.Where(x => x.TrackingState == SkeletonTrackingState.Tracked).ToList();
            
            NewCanva.Children.Clear();

            for (int i = 0; i < activeSkeleton.Count; i++)
            {
                Ball[] PointArray = new Ball[20];               
                foreach (Joint point in activeSkeleton[i].Joints)
                {                   
                    if ((activeSkeleton[i].Position.X>0) && (point.Position.Z > 0.15))
                    {                    
                        PointArray[(int)point.JointType] = new Ball(_const.radius, Brushes.MediumSlateBlue, pHeight, pWidth, Coeff);
                        PointArray[(int)point.JointType].setpos(Arounding(point.Position.X * 3.7 * 4 / 3 / _const.coeff / point.Position.Z, _const.Step), Arounding(point.Position.Y * 3.7 / point.Position.Z, _const.Step));
                        if (Zacep[(int)(PointArray[(int)point.JointType].position.X * 100 / _const.Step),(int) PointArray[(int)point.JointType].position.Y])
                        ballvelocity = ball.collision(PointArray[(int)point.JointType].position.X, PointArray[(int)point.JointType].position.Y, ballvelocity);
                        PointArray[(int)point.JointType].paint();
                        NewCanva.Children.Add(PointArray[(int)point.JointType].ell);
                    }
                    else if (point.Position.Z > 0.15)
                    {
                        PointArray[(int)point.JointType] = new Ball(_const.radius, Brushes.Coral, pHeight, pWidth, Coeff);
                        PointArray[(int)point.JointType].setpos(Arounding(point.Position.X * 3.7 * 4 / 3 / point.Position.Z, _const.Step), Arounding(point.Position.Y * 3.7 / point.Position.Z, _const.Step));
                        ballvelocity = ball.collision(PointArray[(int)point.JointType].position.X, PointArray[(int)point.JointType].position.Y, ballvelocity);
                        PointArray[(int)point.JointType].paint();
                        NewCanva.Children.Add(PointArray[(int)point.JointType].ell);
                    }
                }
                sFrame.Dispose();
            }
            activeSkeleton.Clear();
        }

        DispatcherTimer Starttimer = new DispatcherTimer(), VStimer = new DispatcherTimer(), Continuetimer = new DispatcherTimer();
        Ball ball;
        Player player1, player2;
        Vector ballvelocity;
        public int pWidth, pHeight, Seconds, VSmaxscore, VSmaxwins;// * _const.fps;
        double Coeff;
        string border = "null";
        //Rectangle leftborder1 = new Rectangle();
        
        public Versus()
        {
            Seconds = 3;
            Time = TimeSpan.FromSeconds(Seconds);
            pWidth = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            pHeight = (int)System.Windows.SystemParameters.PrimaryScreenHeight;
            Coeff = (double)pWidth / _const.width;
            //WindowStyle = WindowStyle.None;
            //WindowState = WindowState.Maximized;
            InitializeComponent();

            ball = new Ball(_const.radius, Brushes.AliceBlue, pHeight, pWidth, Coeff);
            player1 = new Player(_const.radius, Brushes.Coral, pHeight, pWidth, Coeff,"left");
            player2 = new Player(_const.radius, Brushes.MediumSlateBlue, pHeight, pWidth, Coeff, "right");
            ballvelocity = new Vector();
            ball.setpos(0, 0);
            player1.setpos(-10, -10);
            player2.setpos(-10, -12);
            ballvelocity = randomvelocity(_const.minvelocity, _const.maxvelocity);

            NewCanva.Width = pWidth;
            NewCanva.Height = pHeight;
            Canva.Width = Grid.Width = pWidth;
            Canva.Height = Grid.Height = pHeight;
            Canvas.SetTop(countdownText, (pHeight-countdownText.FontSize)/2);
            Canvas.SetLeft(countdownText, pWidth/2- countdownText.FontSize);
            //countdownText.Foreground = Brushes.White; 
            countdownText.Text = "Ready...";
            leftborder.Margin = new Thickness(0, 0, pWidth / 2, 0);
            rightborder.Margin = new Thickness(pWidth / 2, 0, 0, 0);
            leftwinScore.Margin = new Thickness(pWidth / 4, 0, 0, 0);
            rightwinScore.Margin = new Thickness(0, 0, pWidth / 4, 0);
            leftborder.Visibility = rightborder.Visibility = Visibility.Hidden;
            //
            /* centerborder.Visibility = Visibility.Hidden;
             centerborder.Height = pHeight;
             Canvas.SetTop(centerborder, 0);
             Canvas.SetLeft(centerborder, (pWidth - rightborder.Width)/2);
             */
            VStimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / _const.fps);   /*you can change speed of the snake here */
            Starttimer.Interval = Continuetimer.Interval = TimeSpan.FromSeconds(1d);
            VStimer.Tick += VStime_Tick;
            Continuetimer.Tick += continue_Tick;
            Starttimer.Tick += start_Tick;
            Starttimer.Start();
        }

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
            "Time", typeof(TimeSpan), typeof(Versus), new PropertyMetadata(default(TimeSpan)));
        
        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        private async void Kinect_Reboot(object sender, RoutedEventArgs e)
        {
            if (_sensor != null)
                MessageBox.Show("Уже запущен");
            else
                SensorInit();
        }

        void start_Tick(object sender, EventArgs e)
        {
            if ((player1.win == VSmaxwins || player2.win == VSmaxwins) && VSmaxwins != 0)
            {
                Seconds = -2;
                if (player1.win == VSmaxwins) countdownText.Text = "Red won\nthe game!";
                else countdownText.Text = "Blue won\nthe game!";
                //player2.win = player1.win = 0;
                //return;
            }
            if (Seconds > 0) countdownText.Text = "" + Seconds.ToString();
            switch (Seconds)
            {
                case 0:
                    {
                        countdownText.Text = "Start!";
                        leftScore.Text = rightScore.Text = "0";
                        leftScore.Visibility = rightScore.Visibility = Visibility.Visible;
                        break;
                    }
                case -1:
                    {
                        //centerborder.Visibility = Visibility.Visible;
                        countdownText.Text = "";//countdownText.Visibility = Visibility.Hidden;//.Text = "";
                        leftborder.Visibility = rightborder.Visibility = Visibility.Visible;
                        var timer = (DispatcherTimer)sender;
                        timer.Stop();
                        VStimer.Start();
                        ball.setpos(0, 0);
                        ballvelocity = randomvelocity(_const.minvelocity, _const.maxvelocity);
                        
                        break;
                    }
                case -4:
                    {
                        Menu menu = new Menu();
                        menu.VSmaxscore.Text = VSmaxscore.ToString();
                        menu.VSmaxwin.Text = VSmaxwins.ToString();
                        menu.Show();
                        Close();
                        break;
                    }
            }
            Seconds--;
        }

        void continue_Tick(object sender, EventArgs e)
        {
            //countdownText.Text = "   " + Seconds.ToString();
            switch (Seconds)
            {
                case 1:
                    {
                        countdownText.Text = "set";
                        break;
                    }
                case 0:
                    {
                        countdownText.Text = "GO!";
                        break;
                    }
                case -1:
                    {
                       // centerborder.Visibility = Visibility.Visible;
                        countdownText.Text = "";//countdownText.Visibility = Visibility.Hidden;//.Text = "";
                        leftborder.Visibility = rightborder.Visibility = Visibility.Visible;
                        var timer = (DispatcherTimer)sender;
                        timer.Stop();
                        VStimer.Start();                        
                        break;
                    }
            }
            Seconds--;
        }

        void VStime_Tick(object sender, EventArgs e)//работа игры без действий
        {
            
            Canva.Children.Remove(ball.ell);
            Canva.Children.Remove(player1.ell);
            Canva.Children.Remove(player2.ell);
            ballvelocity = ball.VSchangeposition(_const.fps, ballvelocity, out border);
            if (border != "null")
            {
                if (border == "right")
                    leftScore.Text = (++player1.score).ToString();
                else
                    rightScore.Text = (++player2.score).ToString();

                if (player1.score == VSmaxscore)
                {
                    leftborder.Visibility = rightborder.Visibility = Visibility.Hidden;
                    leftScore.Visibility = rightScore.Visibility = Visibility.Hidden;
                    leftwinScore.Text = (++player1.win).ToString();
                    countdownText.Text = "Red wins!";
                    Seconds = 3;
                    player2.score = player1.score = 0;

                    Starttimer.Start();
                    VStimer.Stop();
                    return;
                }
                if (player2.score == VSmaxscore)
                {
                    leftborder.Visibility = rightborder.Visibility = Visibility.Hidden;
                    leftScore.Visibility = rightScore.Visibility = Visibility.Hidden;
                    rightwinScore.Text = (++player2.win).ToString();
                    countdownText.Text = "Blue wins!";
                    Seconds = 3;
                    player2.score = player1.score = 0;

                    Starttimer.Start();
                    VStimer.Stop();
                    return;
                }
                ball.setpos(0, 0);
                ballvelocity = randomvelocity(_const.minvelocity, _const.maxvelocity);

                leftborder.Visibility = rightborder.Visibility = Visibility.Hidden;
                Seconds = 1;
                countdownText.Text = "Ready...";
                //countdownText.Visibility = Visibility.Visible;
                //Starttimer.Tick -= start_Tick;
                //Continuetimer.Tick += continue_Tick;
                Continuetimer.Start();
                VStimer.Stop();
                return;
            }

            ballvelocity = ball.collision(player1.position.X, player1.position.Y, ballvelocity);
            ballvelocity = ball.collision(player2.position.X, player2.position.Y, ballvelocity);
            ball.paint();
            Canva.Children.Add(ball.ell);
            Canva.Children.Add(player1.ell); Canva.Children.Add(player2.ell);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point mousepos = Mouse.GetPosition(Canva);
                player1.setpos((-pWidth / 2 + mousepos.X) / Coeff, (pHeight / 2 - mousepos.Y) / Coeff);
              
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
               
                Point mousepos = Mouse.GetPosition(Canva);
                player2.setpos((-pWidth / 2 + mousepos.X) / Coeff, (pHeight / 2 - mousepos.Y) / Coeff);
            }
        }
        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    //Starttimer.Start();
        //    //VStimer.Start();
        //   // VStimer.Stop();
        //}

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Q)
            {
                if (_sensor != null)
                    _sensor.Stop();
                Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Menu menu= new Menu();
            menu.VSmaxscore.Text = VSmaxscore.ToString();
            menu.VSmaxwin.Text = VSmaxwins.ToString();
            menu.Show();
            if (_sensor != null)
                _sensor.Stop();
            Close();
        }
        public Vector randomvelocity(double _minvelocity, double _maxvelocity)
        {
            Vector _vel = new Vector();
            Random rd = new Random(); Random rda = new Random();
            double angle = rda.NextDouble() * 2 * Math.PI;
            double velocity = Math.Min(_minvelocity, _maxvelocity) + Math.Abs(_maxvelocity - _minvelocity) * rd.NextDouble();
            _vel.X = velocity * Math.Cos(angle);
            _vel.Y = velocity * Math.Sin(angle);
            return _vel;
        }
    }
}
