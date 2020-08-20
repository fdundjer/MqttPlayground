using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Feature.MqttClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientInfoView : ContentView
    {
        public ClientInfoView()
        {
            InitializeComponent();
        }
    }
}