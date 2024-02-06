using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace OilSpill
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
        private const double k = 941.176471;
        private const double b = 1.76476588;
        //  private const double xmin = 0.10437493620683597;
        private double timescroll = 10;
        private const double cmtopx = 37.938105;
        public double temp; //y
        public static double vel;//x
        public DispatcherTimer updateTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            SetTimer();
          
        }


        private void SetTimer()
        {
            updateTimer = new DispatcherTimer(DispatcherPriority.SystemIdle);
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Interval = TimeSpan.FromMilliseconds(10);
           // updateTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            Update();
        }

        public  void Update()
        {
            Droplet.Width += vel;
            Droplet.Height += vel;
            Radius.Content = $"R = {Math.Round((Droplet.Width / cmtopx ), 4)}cm";
        }
        private void ResetDroplet()
        {
            Droplet.Width = 200;
            Droplet.Height = 200;
        }

        private void Modeling_Click(object sender, RoutedEventArgs e)
        {
           updateTimer.Stop();
            ResetDroplet();
            if (double.TryParse(Velocity.Text, out vel))
            {
                if (double.TryParse(Temp.Text, out temp) && temp <= 100 && temp >= 7.9)
                {
                    //vel = (temp - b) / k;
                    vel = (Math.Log(temp / 3.5) - 0.805) / 42;   // 
                    Velocity.Text = vel.ToString();
                    temp = (k * vel) + b;
                    if (double.TryParse(TimesScroll.Text,out timescroll) && timescroll > 0 && timescroll <= 50)
                    {
                        //Получение timescroll ...
                        //
                        //Рассчёт
                        vel = ((vel * timescroll)*cmtopx)/100;
                        Radius.Content = $"R = {Math.Round((Droplet.Width / cmtopx * 2), 4)}cm";
                        updateTimer.Start();
                    }
                    else
                    {
                        MessageBox.Show($"1<=TimeScroll<=50!");
                        vel = ((vel) * cmtopx) / 100;
                        Radius.Content = $"R = {Math.Round((Droplet.Width / cmtopx * 2), 4)}cm";
                        updateTimer.Start();
                    }
                }
                else
                {
                    MessageBox.Show("Неправльно Введена температура");
                }
            }
            else
            {
                MessageBox.Show("Неправильно Введена скорость");
            }

        }
    }
}
