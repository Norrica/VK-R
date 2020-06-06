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
using VkNet.Model;

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
                Api.Authorize("a.kolpakov2010@mail.ru", "ZxcAsdQwe123");
            }
            catch (CaptchaNeededException ex)
            {
                var cpt = new System.Windows.Controls.Image 
                { 
                    Source = new BitmapImage(ex.Img) 
                };
                CaptchaWindow cptchaWindow = new CaptchaWindow(cpt);

                if (cptchaWindow.ShowDialog() == true)
                {
                    Api.VkApi.Authorize(new ApiAuthParams
                    {
                        Login = LoginBox.Text,
                        Password = PasswordBox.Password,
                        CaptchaSid = ex.Sid,
                        CaptchaKey = cptchaWindow.SolvedCaptcha.Text

                    }) ;
                }
                              
                

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
        //#error now u obligated to handle captcha
        //https://ru.stackoverflow.com/questions/581470/Как-обрабатывать-капчу-с-помощью-библиотеки-vknet
    }
}
