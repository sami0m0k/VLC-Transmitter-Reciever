
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Ports;
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

namespace VLC.Apps.Receiver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort serialPort;
        private StringBuilder stringBuilder = new StringBuilder();
        public MainWindow()
        {
            InitializeComponent();
            serialPort = new SerialPort();
            cboDevices.ItemsSource = SerialPort.GetPortNames();
            btnConnect.Click += BtnConnect_Click;
            btnDisconnect.Click += BtnDisconnect_Click;
        }

        private void BtnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            Disconnect();
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            Connect();
        }

        private void Connect()
        {
            var portName = cboDevices.SelectedValue as string;
            if (portName == null)
            {
                return;
            }
            serialPort.PortName = portName; // Creating the new object.
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.BaudRate = 9600; // Setting baudrate.0
            serialPort.Open(); // Open the port for use.
            btnConnect.IsEnabled = false;
            btnDisconnect.IsEnabled = true;
            btnClear.IsEnabled = true;
        }

        private void Disconnect()
        {
            serialPort.DataReceived -= SerialPort_DataReceived;
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
            btnConnect.IsEnabled = true;
            btnDisconnect.IsEnabled = false;
            btnClear.IsEnabled = false;
        }
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                txtBox.Text += serialPort.ReadExisting();
            });
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            if (serialPort != null)
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }

                serialPort.Dispose();
                serialPort = null;
            }

        }



        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            txtBox.Text = "";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
