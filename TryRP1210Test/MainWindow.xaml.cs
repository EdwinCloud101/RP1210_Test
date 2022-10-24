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
using Peak.RP1210C;
using RP1210_Test;
using RP1210_Test.HelpClasses;
using forms = System.Windows.Forms;

namespace TryRP1210Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private short _clientId;
        private PEAKRP32Plus m_PEAKRP32;

        private bool m_UsingOldPCANRP64;
        private bool m_AppIsPacking;
        private int m_Channel;
        private RP1210_ProtocolInfo m_Protocol;
        private RP1210_Baudrate m_Baudrate;
        private RP1210_ImplementationInformation m_Implementation;
        RP1210_DeviceInfo m_Device;


        public MainWindow()
        {
            InitializeComponent();




            this.m_Protocol = new RP1210_ProtocolInfo();
            m_Device = new RP1210_DeviceInfo();
            this.m_Implementation = new RP1210_ImplementationInformation();
            this.m_Baudrate = RP1210_Baudrate.Standard;
            this.m_AppIsPacking = false;
        }

        private void Log(string message)
        {
            txt1.Text = txt1.Text.Insert(0, $"{message}---{DateTime.Now}{Environment.NewLine}");
        }


        private void LoadVansco()
        {
            var reader = new RP1210_InformationReader("VE121032");
            m_Implementation = reader.Implementation;
            Log("Vansco Loaded, VE121032");
        }

        private void LoadPeak()
        {
            var reader = new RP1210_InformationReader("PEAKRP32");
            m_Implementation = reader.Implementation;
            Log("Peak Loaded, PEAKRP32");
        }

        private void LoadDla2()
        {
            var reader = new RP1210_InformationReader("DLA20R32");
            m_Implementation = reader.Implementation;
            Log("DLA2.0 Loaded, DLA20R32");
        }


        private void Disconnect()
        {
            var disconnectReturn = this.m_PEAKRP32.ClientDisconnect(_clientId);

            if (disconnectReturn >= 0 && disconnectReturn <= 127)
            {
                Log($"{m_Implementation.Name} successfully disconnected");
                SetState(false);
            }
            else
            {
                Log($"{m_Implementation.Name} failed to disconnect, error code {disconnectReturn}");
            }
        }

        private void Connect()
        {
            //this.m_Client = this.m_PEAKRP32.ClientConnect(0, Convert.ToUInt16(this.m_Device.DeviceID), this.GetProtocolString(), 0, 0, this.m_AppIsPacking ? (short)1 : (short)0);


            m_PEAKRP32 = new PEAKRP32Plus(m_Implementation.Name);

            string dongleSupports = $"This dongle supports:";

            ushort deviceId = 1;
            string protocol = "CAN";
            _clientId = m_PEAKRP32.ClientConnect(0, deviceId, protocol, 0, 0, 0);

            if (_clientId >= 0 && _clientId <= 127)
            {
                Log($"{m_Implementation.Name} successfully connected, clientId={_clientId}");

                Log($"RP1210A is {(m_PEAKRP32.RP1210_A_IsSupported ? "" : "not ")}supported");
                Log($"RP1210B is {(m_PEAKRP32.RP1210_B_IsSupported ? "" : "not ")}supported");
                Log($"RP1210C is {(m_PEAKRP32.RP1210_C_IsSupported ? "" : "not ")}supported");

                SetState(true);
            }
            else
            {
                Log($"{m_Implementation.Name} failed to connect, error code {_clientId}");
            }


        }




        private void btnConfig_Click(object sender, RoutedEventArgs e)
        {
            //this.ConfigureConnection();
        }

        private void btnVansco_Click(object sender, RoutedEventArgs e)
        {
            LoadVansco();
        }

        private void btnPeak_Click(object sender, RoutedEventArgs e)
        {
            LoadPeak();
        }

        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            Disconnect();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Connect();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            SetState(false);
        }

        private void SetState(bool enableConnectedState)
        {
            //return;

            var greenColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFC8F5C6");
            var redColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFF0C3C3");
            var grayColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDDDDDD");


            btnConnect.IsEnabled = !enableConnectedState;
            btnConnect.Background = enableConnectedState ? grayColor : greenColor;

            btnDisconnect.IsEnabled = enableConnectedState;
            btnDisconnect.Background = enableConnectedState ? redColor : grayColor;
        }

        private void btnDla2_Click(object sender, RoutedEventArgs e)
        {
            LoadDla2();
        }

        private void btnBaud_Click(object sender, RoutedEventArgs e)
        {
            byte[] numArray = new byte[8];
            //var speed = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_GET_PROTOCOL_CONNECTION_SPEED, this._clientId, numArray);
            var speed = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_GET_PROTOCOL_CONNECTION_SPEED, this._clientId, numArray);

            if (speed == 0)
            {
                string str = Encoding.Default.GetString(numArray).TrimEnd(new char[1]);
                txtChangeBaudRateFrom.Text = str;
                Log($"Baud Rate={str}");
            }
            else
            {
                Log($"Error reading Baud Rate, error code={speed}");
            }


            /*byte[] numArray = new byte[8];
                        if (this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_GET_PROTOCOL_CONNECTION_SPEED, this.m_Client, numArray) != (short)0)
                            return false;
                        string str = Encoding.Default.GetString(numArray).TrimEnd(new char[1]);
                        this.tsslSpeed.BackColor = str != "0" ? Color.Honeydew : Color.Bisque;
                        this.tsslSpeed.ForeColor = str != "0" ? Color.Green : Color.Red;
                        this.tsslSpeed.Text = str;
                        return str != "0";*/
        }

        private byte GetBaudrateCode(uint newBaudrate)
        {
            switch (newBaudrate)
            {
                case 125000:
                    return 4;
                case 250000:
                    return 5;
                case 500000:
                    return 6;
                case 1000000:
                    return 7;
                default:
                    return 0;
            }
        }

        private void btnBaud_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (txtChangeBaudRateFrom.Text.Trim() == "" || txtChangeBaudRateTo.Text.Trim() == "")
                return;

            uint newBaudrate = Convert.ToUInt32(txtChangeBaudRateTo.Text.Trim());

            byte[] fpchClientCommand = new byte[2]
            {
                (byte) 0,
                this.GetBaudrateCode(newBaudrate)
            };

            var returnCode = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_SET_CAN_BAUD, this._clientId, fpchClientCommand);

            if (returnCode == 0)
            {
                Log("Baud Rate has been changed, run Red-Baud-Rate again to confirm");
            }
            else
            {
                Log($"Error trying to change baud rate, errorCode={returnCode}");
            }

            txtChangeBaudRateFrom.Text = "";
            txtChangeBaudRateTo.Text = "";

            /*
            byte[] fpchClientCommand = new byte[2]
            {
        changeNow ? (byte) 0 : (byte) 1,
        this.GetBaudrateCode(newBaudrate)
            };
            short num1 = this.m_Protocol.Index != 10 ? this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_SET_J1939_BAUD, this.m_Client, fpchClientCommand) : this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_SET_CAN_BAUD, this.m_Client, fpchClientCommand);
            if (num1 != (short)0)
            {
                int num2 = (int)MessageBox.Show("CHANGE_BAUDRATE could not be done! Error: " + num1.ToString(), "SendCommand Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
                this.tsslSpeed.BackColor = Color.Bisque;
             */

        }

        private void btnSetAllFiltersToPass_Click(object sender, RoutedEventArgs e)
        {
            short num1 = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_SET_ALL_FILTERS_STATES_TO_PASS, _clientId, new byte[0]);
            if (num1 == 0)
            {
                Log($"Successful call. RPCMD_SET_ALL_FILTERS_STATES_TO_PASS the call returns={num1}");
            }
            else
            {
                Log($"Failed call. RPCMD_SET_ALL_FILTERS_STATES_TO_PASS error code={num1}");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //this.HandleMessage((object)new RP1210_MsgCanRx(SubArray.GetBytes(this.m_ReadBuffer, 0, (int)num), this.m_Configuration.ReceivingEcho));
        }
    }
}
