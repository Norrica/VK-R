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
using VkNet.Exception;

namespace VK_R
{
    /// <summary>
    /// Interaction logic for AuthorizationControl.xaml
    /// </summary>
    public partial class AuthorizationControl : UserControl
    {
        private ApiBase Api = ApiBase.GetInstance();
        public AuthorizationControl()
        {
            InitializeComponent();

        }

        private void Authorize_Click(object sender, RoutedEventArgs e)
        {
            //handle captchaneededexception
            try
            {
#warning uncomment after debug
                // Api.Authorize(LoginBox.Text, PasswordBox.Password);
                
            }
            catch (CaptchaNeededException ex)
            {
                throw;
            }
            if (Api.VkApi.IsAuthorized)
            {
                MessageBox.Show(Api.VkApi.IsAuthorized.ToString());
                Authorized();
            }
            else
            {
#warning temporary as fuck
                MessageBox.Show("Fail. Try Again");
            }
        }
        public delegate void Method();
        public event Method Authorized;
    }
}
