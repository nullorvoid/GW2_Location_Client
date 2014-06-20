using GW2_Location_Client.Models;
using GW2_Location_Client.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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

namespace GW2_Location_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool getdata;
        public static bool transmit;
        public static int pollrate;
        public static string apiurl;

        public MainWindow()
        {
            InitializeComponent();
            InitializeUI();
            InitializeMumbleLink();
            InitializeApi();
            InitializeTransmission();
        }

        private void InitializeUI()
        {
            this.Closing += Window_Closing;
            lbl_Msg.Content = string.Empty;
            lbl_debug.Content = string.Empty;
        }

        private void InitializeTransmission()
        {
            transmit = false;
        }

        private void InitializeMumbleLink()
        {
            getdata = true;
            pollrate = 100;
            Task.Run(new Action(UpdateCords));
        }

        private void InitializeApi()
        {
            apiurl = ConfigurationManager.AppSettings["apiurl"];
        }

        private void Transmit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Api_Key.Text) && !transmit)
            {
                lbl_Msg.Content = "Please enter API Key before starting transmission";
                return;
            }
            lbl_Msg.Content = string.Empty;

            if (transmit)
                StopTransmission();
            else
                StartTransmission();
        }

        private void StartTransmission()
        {
            transmit = true;
            txt_Api_Key.IsEnabled = false;
            lbl_Msg.Content = "Location Data Now Transmitting";
            Transmit.Content = "Stop Transmission";
        }

        private void StopTransmission()
        {
            transmit = false;
            txt_Api_Key.IsEnabled = true;
            lbl_Msg.Content = string.Empty;
            Transmit.Content = "Start Transmission";
        }

        private void UpdateCords()
        {
            PlayerPosition playerpos = new PlayerPosition();

            while (getdata)
            {
                Player player = playerpos.GetPlayerData();

                if (transmit)
                {
                    Api_Update data = new Api_Update();
                    txt_Api_Key.Dispatcher.Invoke(new Action(() => data.Api_Key = txt_Api_Key.Text));
                    data.Player.Name = player.Info.Name;
                    data.Player.Commander = player.Info.Commander;
                    data.Player.Profession = player.Info.Profession;
                    data.Location.World_id = player.Location.World_id;
                    data.Location.Map_id = player.Location.Map_id;
                    data.Location.X = player.Location.X;
                    data.Location.Y = player.Location.Y;
                    data.Location.Z = player.Location.Z;

                    if (!SendCords(data))
                    {
                        lbl_Msg.Dispatcher.Invoke(new Action(() => lbl_Msg.Content = "There was an error sending the data"));
                    }
                }

                lbl_debug.Dispatcher.Invoke(new Action(() => lbl_debug.Content = player.Location.World_id + ", " + player.Location.Map_id + ", X:" + player.Location.X + ", Y:" + player.Location.Y + ", Z:" + player.Location.Z));

                Thread.Sleep(pollrate);
            }
        }

        private bool SendCords(Api_Update data)
        {
            if (string.IsNullOrEmpty(apiurl))
                return false;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(apiurl);

            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json"; // your content  type (change accordingly)

            // Send HttpContent
            HttpWebResponse response;
            try
            {
                using (System.IO.Stream s = httpWebRequest.GetRequestStream())
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
                        sw.Write(JsonConvert.SerializeObject(data));
                }

                // Get Response
                response = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch
            {
                return false;
            }

            return (response.StatusCode == HttpStatusCode.OK);
        }

        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            transmit = false;
            getdata = false;
            Thread.Sleep(pollrate + 10);
        }
    }
}
