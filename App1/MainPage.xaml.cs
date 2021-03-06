using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        static string PortNumber = "1001";

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //this.StartServer();
            this.StartClient();
        }

        private async void StartClient()
        {
            try
            {
                // Create the StreamSocket and establish a connection to the echo server.
                using (var streamSocket = new Windows.Networking.Sockets.StreamSocket())
                {
                    // The server hostname that we will be establishing a connection to. In this example, the server and client are in the same process.
                    var hostName = new Windows.Networking.HostName("192.168.0.100");

                    this.clientListBox.Items.Add("client is trying to connect...");

                    await streamSocket.ConnectAsync(hostName, MainPage.PortNumber);

                    this.clientListBox.Items.Add("client connected");

                    // Send a request to the echo server.
                    //string request = "Hello, World!";
                    //using (Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
                    //{
                    //    using (var streamWriter = new StreamWriter(outputStream))
                    //    {
                    //        await streamWriter.WriteLineAsync(request);
                    //        await streamWriter.FlushAsync();
                    //    }
                    //}

                    var dataWriter = new DataWriter();

                    // First byte is always the report id
                    dataWriter.WriteByte(0x01);
                    dataWriter.WriteByte(0x01);

                    await streamSocket.OutputStream.WriteAsync(dataWriter.DetachBuffer());

                    this.clientListBox.Items.Add("data sent");

                    // Read data from the echo server.
                    //string response;
                    //using (Stream inputStream = streamSocket.InputStream.AsStreamForRead())
                    //{
                    //    using (StreamReader streamReader = new StreamReader(inputStream))
                    //    {
                    //        response = await streamReader.ReadLineAsync();
                    //    }
                    //}

                    //this.clientListBox.Items.Add(string.Format("client received the response: \"{0}\" ", response));
                }

                this.clientListBox.Items.Add("client closed its socket");
            }
            catch (Exception ex)
            {
                Windows.Networking.Sockets.SocketErrorStatus webErrorStatus = Windows.Networking.Sockets.SocketError.GetStatus(ex.GetBaseException().HResult);
                this.clientListBox.Items.Add(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);
            }
        }
    }
}
