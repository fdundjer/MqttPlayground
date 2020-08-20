using System;
using System.Net;
using System.Threading;
using System.Windows.Input;
using MQTTnet;
using MQTTnet.Client.Options;
using Xamarin.Forms;

namespace Feature.MqttClient.ViewModels
{
    public class ClientInfoViewModel : BindableBase
    {
        private readonly MqttFactory _mqttFactory;
        private string _errorText;
        private bool _isInputEnabled = true;

        private string _serverIpAddress;
        private string _serverPort;

        public ClientInfoViewModel()
        {
            _mqttFactory = new MqttFactory();
            ConnectToServerCommand = new Command(ConnectToServerCommandHandler, o => IsInputEnabled);
        }

        public string ServerIpAddress
        {
            get => _serverIpAddress;
            set => SetProperty(ref _serverIpAddress, value);
        }

        public string ServerPort
        {
            get => _serverPort;
            set => SetProperty(ref _serverPort, value);
        }

        public bool IsInputEnabled
        {
            get => _isInputEnabled;
            set => SetProperty(ref _isInputEnabled, value);
        }

        public string ErrorText
        {
            get => _errorText;
            set => SetProperty(ref _errorText, value);
        }

        public ICommand ConnectToServerCommand { get; }

        private async void ConnectToServerCommandHandler(object obj)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IsInputEnabled = false;
                ErrorText = string.Empty;
            });

            try
            {
                if (!Uri.TryCreate($"http://{ServerIpAddress}:{ServerPort}", UriKind.Absolute, out var uri) ||
                    !IPAddress.TryParse(uri.Host, out _))
                {
                    Device.BeginInvokeOnMainThread(() => ErrorText = "Invalid server address.");
                    return;
                }

                var clientOptions = new MqttClientOptionsBuilder()
                    .WithClientId(Guid.NewGuid().ToString())
                    .WithCredentials(string.Empty, string.Empty)
                    .WithTcpServer(uri.Host, uri.Port)
                    .Build();

                var client = _mqttFactory.CreateMqttClient();

                using (var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
                {
                    await client.ConnectAsync(clientOptions, cancellationToken.Token).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(() => ErrorText = e.Message);
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() => IsInputEnabled = true);
            }
        }
    }
}