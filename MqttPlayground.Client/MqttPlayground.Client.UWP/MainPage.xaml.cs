namespace MqttPlayground.Client.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new Client.App());
        }
    }
}