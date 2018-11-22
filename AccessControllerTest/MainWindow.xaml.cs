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
using WComm_UDP;

namespace AccessControllerTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
       
        string strFrame, strCmd, swipDate, strRunDetail, ipAddr, strMac,
               strHexNewIP, strHexMax, strHexGateway;
        long ret;
        long controllerSN;
        long cardId;
        long status;

        private void connectBTN_clicked(object sender, RoutedEventArgs e)
        {
            WComm_UDP.WComm_Operate wudp = new WComm_Operate();
            outputTxtBlc.Text = "Trying to conect";
            controllerSN = System.Convert.ToInt64(123237201);
            ipAddr = "";
            strCmd = wudp.CreateBstrCommand(controllerSN, "8110" + (wudp.NumToStrHex(0, 3)));
            strFrame = wudp.udp_comm(strCmd, ipAddr, 60000);
            if ((wudp.ErrCode != 0) || (string.IsNullOrEmpty(strFrame)))
            {
                wudp = null;
                outputTxtBlc.Text = "Failure to Connect";
                strCmd = "";
            }
            else
            {
                outputTxtBlc.Text = "Success to connect";

            }
        }
    }
}
