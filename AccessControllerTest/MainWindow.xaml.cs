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
       
        string strFrame, strCmd, swipeDate, strRunDetail, ipAddr, strMac,
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

            }
            else
            {
                outputTxtBlc.Text = "Success to connect";
                strRunDetail = "";
                strRunDetail = strRunDetail + "\r\n" + "Device Series No.S/N: " + "\t" + wudp.GetSNFromRunInfo(strFrame);
                strRunDetail = strRunDetail + "\r\n" + "Device Clock:      " + "\t" + wudp.GetClockTimeFromRunInfo(strFrame);
                strRunDetail = strRunDetail + "\r\n" + "Valid card records:    " + "\t" + wudp.GetCardRecordCountFromRunInfo(strFrame);
                strRunDetail = strRunDetail + "\r\n" + "Number of permissions:        " + "\t" + wudp.GetPrivilegeNumFromRunInfo(strFrame);
                strRunDetail = strRunDetail + "\r\n" + "\r\n" + "A recent brush Card record: " + "\t";
                swipeDate = wudp.GetSwipeDateFromRunInfo(strFrame, ref cardId, ref status);

                if (swipeDate != "") {
                    strRunDetail = strRunDetail + "\r\n" +
                                    "Card No.: " + cardId + "\t" +
                                    " Status:" + "\t" + status + "(" + wudp.NumToStrHex(status, 1)
                                    + ")" + "\t" + "Time:" + "\t" + swipeDate;

                }
                strRunDetail = strRunDetail + "\r\n";
                //Door Sensor Status
                //Bit      7         6        5          4        3          2         1        0
                //Explain Sensor4   Sensor3   Sensor2   Sensor1   Sensor4   Sensor3   Sensor2   Sensor1
                strRunDetail = strRunDetail + "\r\n" + "Sensor status  Door 1 sensor  Door 2 sensor Door 3sensor  door 4 sensor";
                strRunDetail = strRunDetail + "\r\n";
                strRunDetail = strRunDetail + "          ";

                if (wudp.GetDoorStatusFromRunInfo(strFrame, 1) == 1)
                {
                    strRunDetail = strRunDetail + "   Open   ";
                }
                else
                {
                    strRunDetail = strRunDetail + "   Close   ";
                }
                if (wudp.GetDoorStatusFromRunInfo(strFrame, 2) == 1)
                {
                    strRunDetail = strRunDetail + "   Open   ";
                }
                else
                {
                    strRunDetail = strRunDetail + "   Close   ";
                }
                if (wudp.GetDoorStatusFromRunInfo(strFrame, 3) == 1)
                {
                    strRunDetail = strRunDetail + "   Open   ";
                }
                else
                {
                    strRunDetail = strRunDetail + "   Close   ";
                }
                if (wudp.GetDoorStatusFromRunInfo(strFrame, 4) == 1)
                {
                    strRunDetail = strRunDetail + "   Open   ";
                }
                else
                {
                    strRunDetail = strRunDetail + "   Close   ";
                }
                strRunDetail = strRunDetail + "\r\n";
                strRunDetail = strRunDetail + "button status  No.1 button  No.2 buton  No.3 button No.4 button";
                strRunDetail = strRunDetail + "\r\n";
                strRunDetail = strRunDetail + "          ";

                if (wudp.GetButtonStatusFromRunInfo(strFrame, 1) == 1)
                {
                    strRunDetail = strRunDetail + " Release   ";
                }
                else
                {
                    strRunDetail = strRunDetail + " Press   ";
                }
                if (wudp.GetButtonStatusFromRunInfo(strFrame, 2) == 1)
                {
                    strRunDetail = strRunDetail + " Release   ";
                }
                else
                {
                    strRunDetail = strRunDetail + " Press   ";
                }
                if (wudp.GetButtonStatusFromRunInfo(strFrame, 3) == 1)
                {
                    strRunDetail = strRunDetail + " Release   ";
                }
                else
                {
                    strRunDetail = strRunDetail + " Press   ";
                }
                if (wudp.GetButtonStatusFromRunInfo(strFrame, 4) == 1)
                {
                    strRunDetail = strRunDetail + " Release   ";
                }
                else
                {
                    strRunDetail = strRunDetail + " Press   ";
                }
                strRunDetail = strRunDetail + "\r\n" + "Fault state:" + "\r\n";
                int errorNo;
                errorNo = (int)wudp.GetErrorNoFromRunInfo(strFrame);
                if (errorNo == 0)
                {
                    strRunDetail = strRunDetail + " no fault   ";
                }
                else
                {
                    strRunDetail = strRunDetail + " have fault  ";
                    if ((errorNo << 1)!=0)
                    {
                        strRunDetail = strRunDetail + "\r\n" + "        " + "\t" + "system fault 1";
                    }
                    if ((errorNo << 2)!=0)
                    {
                        strRunDetail = strRunDetail + "\r\n" + "        " + "\t" + "system fault 2";
                    }
                    if ((errorNo << 4) != 0)
                    {
                        strRunDetail = strRunDetail + "\r\n" + "        " + "\t" + "Fault 3 [device clock is faulty], please correct the clock processing";
                    }
                    if ((errorNo << 8) != 0)
                    {
                        strRunDetail = strRunDetail + "\r\n" + "        " + "\t" + "system fault 4";
                    }
                    outputTxtBlc.Text = strRunDetail;
                }
            }
        }
    }
}
