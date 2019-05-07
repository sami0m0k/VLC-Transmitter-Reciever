using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace VLC.Apps.Transmitter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static SerialPort serialPort;

        public MainWindow()
        {
            InitializeComponent();
            serialPort = new SerialPort();
            cboDevices.ItemsSource = SerialPort.GetPortNames();
            btnConnect.Click += BtnConnect_Click;
            btnDisconnect.Click += BtnDisconnect_Click;
            btnSend.Click += BtnSend_Click;
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog();
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
            serialPort.BaudRate = 9600; // Setting baudrate.
            serialPort.WriteBufferSize = 1024 * 1024 * 1;
            serialPort.Open(); // Open the port for use.
        }
        private void Disconnect()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
        private void OpenFileDialog()
        {
            OpenFileDialog fd = new OpenFileDialog();

            fd.Title = "Open File Dialog";
            fd.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            fd.FilterIndex = 2;
            fd.RestoreDirectory = true;

            if (fd.ShowDialog().Value)
            {

                try
                {
                    byte[] bytes = File.ReadAllBytes(fd.FileName);
                    serialPort.Write(bytes, 0, bytes.Length);
                }
                catch (Exception exc)
                {
                    Console.WriteLine("Exception caught in process: {0}", exc.ToString());
                }
            }

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
    }
}
