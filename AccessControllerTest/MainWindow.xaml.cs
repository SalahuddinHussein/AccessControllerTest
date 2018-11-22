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
using WG3000_COMM.Core;
using System.ComponentModel;

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
        public const int WARNBIT_FORCE = 0;	            //Coercion
        public const int WARNBIT_DOOROPENTOOLONG = 1;	//Door opening time is too long
        public const int WARNBIT_DOORINVALIDOPEN = 2;   // The door is not swiped open
        public const int WARNBIT_FORCECLOSE = 3;	    //Emergency double closure
        public const int WARNBIT_DOORINVALIDREAD = 4;	//Can not open the door to swipe [illegal credit card]
        public const int WARNBIT_FIRELINK = 5;	        //Fire linkage
        public const int WARNBIT_ALARM = 6;	            //Used to identify anti-theft alarms (alarm source is 0, 1, 2)

        wgMjController wgMjController1 = new wgMjController();  //The controller used by this form

                   //Used to identify anti-theft alarms (alarm source is 0, 1, 2)
        public static string WarnDetail(int warnNo)
        {
            string retStr = "";
            if ((warnNo & (1 << WARNBIT_FORCE)) > 0)
            {
                retStr += "-" + "Duress alarm"; //"Threate Code"
            }
            if ((warnNo & (1 << WARNBIT_DOOROPENTOOLONG)) > 0)
            {
                retStr += "-" + "Normal opening time is too long"; //"Open Too Long"
            }
            if ((warnNo & (1 << WARNBIT_DOORINVALIDOPEN)) > 0)
            {
                retStr += "-" + "Forced intrusion alarm"; //"Forced Open"
            }
            if ((warnNo & (1 << WARNBIT_FORCECLOSE)) > 0)
            {
                retStr += "-" + "Forced lock"; //"Forced Lock"
            }
            if ((warnNo & (1 << WARNBIT_DOORINVALIDREAD)) > 0)
            {
                retStr += "-" + "Invalid Card Swiping"; //"Invalid Card Swiping"
            }
            if ((warnNo & (1 << WARNBIT_FIRELINK)) > 0)
            {
                retStr += "-" + "Fire Alarm"; //"Fire Alarm"
            }

            if ((warnNo & (1 << WARNBIT_ALARM)) > 0)
            {
                retStr += "-" + "Anti-theft";//"ARM"
            }
            return retStr;
        }

        //appError Detailed description of the fault
        public const int ERRBIT_PARAM = 0;  //Parameters Table
        public const int ERRBIT_DATAFLASH = 1;  //DataFlash
        public const int ERRBIT_REALCLOCK = 2;  //clock 2010-3-31 09:47:33
        public static string ErrorDetail(int errNo)
        {
            string retStr = "";
            if ((errNo & (1 << ERRBIT_PARAM)) > 0)
            {
                retStr += "Parameters Table"; // "Param"
            }
            if ((errNo & (1 << ERRBIT_DATAFLASH)) > 0)
            {
                retStr += " " + "storage"; // "DFLASH"
            }
            if ((errNo & (1 << ERRBIT_REALCLOCK)) > 0)
            {
                retStr += " " + "clock"; //"RealClock"
            }
            return retStr;
        }



        private void connectBTN_clicked(object sender, RoutedEventArgs e)
        {

            if (wgMjController1.GetMjControllerRunInformationIP() > 0)
            {
                outputTxtBlc.AppendText("Success\r\n");
                outputTxtBlc.AppendText(string.Format("Computer time:  \t{0}\r\n",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "[week " + DateTime.Now.DayOfWeek.ToString() + "]"));
                outputTxtBlc.AppendText(string.Format("Controller SN:  \t{0}\r\n", wgMjController1.RunInfo.CurrentControllerSN.ToString()));
                outputTxtBlc.AppendText(string.Format("Driver version:  \t{0}\r\n", wgMjController1.RunInfo.driverVersion));
                outputTxtBlc.AppendText(string.Format("Controller real time clock: \t{0}\r\n",
                    wgMjController1.RunInfo.dtNow.ToString("yyyy-MM-dd HH:mm:ss") + "[Week " + wgMjController1.RunInfo.weekday.ToString() + "]"));

                outputTxtBlc.AppendText(string.Format("Fault number: {0:d}\r\n", wgMjController1.RunInfo.appError));
                if (wgMjController1.RunInfo.appError > 0)
                {
                    outputTxtBlc.AppendText(string.Format("malfunction: {0}\r\n", ErrorDetail(wgMjController1.RunInfo.appError)));
                }
                outputTxtBlc.AppendText(string.Format("Alarm code - Gate -1: {0:d}\r\n", wgMjController1.RunInfo.WarnInfo(1)));
                if (wgMjController1.RunInfo.WarnInfo(1) > 0)
                {
                    outputTxtBlc.AppendText(string.Format("Call the police: {0}\r\n", WarnDetail(wgMjController1.RunInfo.WarnInfo(1))));
                }
                outputTxtBlc.AppendText(string.Format("Alarm code - Gate -2:{0:d}\r\n", wgMjController1.RunInfo.WarnInfo(2)));
                if (wgMjController1.RunInfo.WarnInfo(2) > 0)
                {
                    outputTxtBlc.AppendText(string.Format("Call the police: {0}\r\n", WarnDetail(wgMjController1.RunInfo.WarnInfo(2))));
                }
                outputTxtBlc.AppendText(string.Format("Alarm code - Gate -3: {0:d}\r\n", wgMjController1.RunInfo.WarnInfo(3)));
                if (wgMjController1.RunInfo.WarnInfo(3) > 0)
                {
                    outputTxtBlc.AppendText(string.Format("Call the police: {0}\r\n", WarnDetail(wgMjController1.RunInfo.WarnInfo(3))));
                }
                outputTxtBlc.AppendText(string.Format("Alarm code - Gate -4: {0:d}\r\n", wgMjController1.RunInfo.WarnInfo(4)));
                if (wgMjController1.RunInfo.WarnInfo(4) > 0)
                {
                    outputTxtBlc.AppendText(string.Format("Call the police: {0}\r\n", WarnDetail(wgMjController1.RunInfo.WarnInfo(4))));
                }

                outputTxtBlc.AppendText(string.Format("Number of records not extracted: {0:d}\r\n", wgMjController1.RunInfo.newRecordsNum));

                outputTxtBlc.AppendText(string.Format("Valid number of registered cards: {0}\r\n", wgMjController1.RunInfo.registerCardNum));

                outputTxtBlc.AppendText("\r\n");

                //door magnet
                outputTxtBlc.AppendText(string.Format("No. 1 magnetic state: {0}\r\n", (wgMjController1.RunInfo.IsOpen(1)) ? "     Door open" : "Door closed"));
                outputTxtBlc.AppendText(string.Format("No. 2 magnetic state: {0}\r\n", (wgMjController1.RunInfo.IsOpen(2)) ? "     Door open" : "Door closed"));
                outputTxtBlc.AppendText(string.Format("No. 3 magnetic state: {0}\r\n", (wgMjController1.RunInfo.IsOpen(3)) ? "     Door open" : "Door closed"));
                outputTxtBlc.AppendText(string.Format("No. 4 magnetic state: {0}\r\n", (wgMjController1.RunInfo.IsOpen(4)) ? "     Door open" : "Door closed"));

                outputTxtBlc.AppendText("\r\n");

                //Button
                outputTxtBlc.AppendText(string.Format("Button 1: {0}\r\n", (wgMjController1.RunInfo.PushButtonActive(1)) ? "     action" : "No acting"));
                outputTxtBlc.AppendText(string.Format("Button 2: {0}\r\n", (wgMjController1.RunInfo.PushButtonActive(2)) ? "     action" : "No acting"));
                outputTxtBlc.AppendText(string.Format("Button 3: {0}\r\n", (wgMjController1.RunInfo.PushButtonActive(3)) ? "     action" : "No acting"));
                outputTxtBlc.AppendText(string.Format("Button 4: {0}\r\n", (wgMjController1.RunInfo.PushButtonActive(4)) ? "     action" : "No acting"));

                outputTxtBlc.AppendText("\r\n");

                //Lock relay action
                outputTxtBlc.AppendText(string.Format("Door lock 1: {0}\r\n", (wgMjController1.RunInfo.LockRelayActive(1)) ? "     action" : "No acting"));
                outputTxtBlc.AppendText(string.Format("Door lock 2: {0}\r\n", (wgMjController1.RunInfo.LockRelayActive(2)) ? "     action" : "No acting"));
                outputTxtBlc.AppendText(string.Format("Door lock 3: {0}\r\n", (wgMjController1.RunInfo.LockRelayActive(3)) ? "     action" : "No acting"));
                outputTxtBlc.AppendText(string.Format("Door lock 4: {0}\r\n", (wgMjController1.RunInfo.LockRelayActive(4)) ? "     action" : "No acting"));

                outputTxtBlc.AppendText("\r\n");

                //Forced lock door
                outputTxtBlc.AppendText(string.Format("Forced lock: \t{0}\r\n", (wgMjController1.RunInfo.ForceLockIsActive) ? "     action" : "No acting"));
                outputTxtBlc.AppendText(string.Format("Fire alarm: \t{0}\r\n", (wgMjController1.RunInfo.FireIsActive) ? "     action" : "No acting"));
            }
            else
            {
                outputTxtBlc.AppendText("failed\r\n");
            }


        }

    }
    //appWarn Detailed description of the alarm

   
}
