
// Type: RP1210_Test.Form1




using Peak.Classes;
using Peak.RP1210C;
using RP1210_Test.HelpClasses;
using RP1210_Test.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RP1210_Test
{
    public class Form1 : Form
    {
        private IniFileReader m_Ini;
        private RP1210_ImplementationInformation m_Implementation;
        private RP1210_DeviceInfo m_Device;
        private RP1210_ProtocolInfo m_Protocol;
        private int m_Channel;
        private RP1210_Baudrate m_Baudrate;
        private RP1210J1939Network m_J19239Network;
        private RP1210_Configuration m_Configuration;
        private List<ListViewMessageStatus> m_ReceiveMsgList;
        private List<ListViewMessageSend> m_SendMsgList;
        private List<RP1210ISO15765FlowControl> m_ISO15765FlowControls;
        private List<object> m_MsgTransmitList;
        private bool m_AppIsPacking;
        private bool m_UsingOldPCANRP64;
        private bool m_Connected;
        private short m_Client;
        private byte[] m_ReadBuffer;
        private PEAKRP32Plus m_PEAKRP32;
        private Form1.ReadDelegateHandler m_ReadDelegate;
        private Thread m_ReadThread;
        private Stopwatch m_Chronometer;
        private IContainer components;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel tsslConnection;
        private ToolStrip toolStrip1;
        private ToolStripButton tsbConnect;
        private ToolStripButton tsbRefresh;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton tsbConfigure;
        private ToolStripButton tsbExit;
        private ToolStripButton tsbAbout;
        private SplitContainer spcContainer;
        private Panel panel4;
        private ListView lstMessages;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox pictureBox1;
        private Panel panel5;
        private RichTextBox rtbCharOutput;
        private Panel panel2;
        private TableLayoutPanel tableLayoutPanel2;
        private PictureBox pictureBox2;
        private ToolStripStatusLabel tsslSpeed;
        private ToolStripStatusLabel tsslProtocol;
        private System.Windows.Forms.Timer tmrRead;
        private ToolStripButton tsbMsgConfig;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripStatusLabel tsslFilterSts;
        private ToolStripStatusLabel tsslEcho;
        private ToolStripStatusLabel tsslReceiving;
        private ToolStripButton tsbNetworkConfiguration;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ListView lstTxMessages;
        private ContextMenuStrip cmsNewTxMsg;
        private ToolStripMenuItem tsmiAdd;
        private ToolStripMenuItem tsmiEdit;
        private ToolStripSeparator toolStripMenuItem1;
        private Button button1;
        private ToolStripMenuItem tsmiDelete;
        private Button button2;
        private Button button4;
        private Button button3;
        private Button button5;
        private ToolStripMenuItem tsmiClear;
        private ToolStripButton tsbBroadcastConfig;
        private ToolStripStatusLabel tsslBroadcasting;

        public Form1()
        {
            this.InitializeComponent();
            this.m_PEAKRP32 = new PEAKRP32Plus();
            this.m_ReadBuffer = new byte[4110];
            this.m_ReceiveMsgList = new List<ListViewMessageStatus>();
            this.m_SendMsgList = new List<ListViewMessageSend>();
            this.m_MsgTransmitList = new List<object>();
            this.m_ReadDelegate = new Form1.ReadDelegateHandler(this.DoRead);
            this.m_Configuration = new RP1210_Configuration();
            this.m_ReceiveMsgList = new List<ListViewMessageStatus>();
            this.m_SendMsgList = new List<ListViewMessageSend>();
            this.m_ISO15765FlowControls = new List<RP1210ISO15765FlowControl>();
            this.m_Implementation = new RP1210_ImplementationInformation();
            this.m_Device = new RP1210_DeviceInfo();
            this.m_Protocol = new RP1210_ProtocolInfo();
            this.m_Baudrate = RP1210_Baudrate.Standard;
            this.m_J19239Network = new RP1210J1939Network((byte)249);
            this.m_Client = (short)0;
            this.m_AppIsPacking = false;
            this.SetConnectionStatus(false);
            this.m_Chronometer = new Stopwatch();
        }

        private void SetConnectionStatus(bool connected)
        {
            this.m_Connected = connected;
            this.tsslConnection.Text = connected ? this.m_Device.ToString() : "Disconnected";
            this.tsslProtocol.Text = connected ? this.m_Protocol.ProtocolString : "       ";
            this.tsslSpeed.Text = "       ";
            if (!connected)
            {
                this.tsslSpeed.BackColor = Color.FromKnownColor(KnownColor.ButtonFace);
                this.m_J19239Network = new RP1210J1939Network((byte)249);
            }
            this.tsbRefresh.Enabled = connected;
            this.tsbConfigure.Enabled = !connected;
            this.tsbMsgConfig.Enabled = connected;
            this.tsbBroadcastConfig.Enabled = connected && (this.m_PEAKRP32.ImplementationLevel & PEAKRP32Plus.ImplementationLoadLevel.C) == PEAKRP32Plus.ImplementationLoadLevel.C;
            this.tsbConnect.Image = connected ? (Image)Resources.flash_delete : (Image)Resources.flash;
            this.tsbNetworkConfiguration.Enabled = connected && (this.m_Protocol.Index == 20 || this.m_Protocol.Index == 30);
            switch ((RP1210CProtocol)this.m_Protocol.Index)
            {
                case RP1210CProtocol.CAN:
                    this.tsbNetworkConfiguration.Text = "";
                    break;
                case RP1210CProtocol.J1939:
                    this.tsbNetworkConfiguration.Text = "J1939 address claiming";
                    break;
                case RP1210CProtocol.ISO15765:
                    this.tsbNetworkConfiguration.Text = "ISO15765 flow control";
                    break;
            }
            this.ShowConfigurationStatus(connected);
        }

        private void ShowConfigurationStatus(bool connected)
        {
            this.tsslFilterSts.Text = connected ? string.Format("Filter: {0}", (object)this.m_Configuration.FilterStatus) : "       ";
            this.tsslEcho.Text = connected ? string.Format("Echo: {0}", (object)this.m_Configuration.ReceivingEcho) : "       ";
            this.tsslReceiving.Text = connected ? string.Format("Receiving: {0}", (object)this.m_Configuration.ReceivingMessages) : "       ";
            this.tsslBroadcasting.Text = connected ? string.Format("Broadcasting: {0}", (object)this.m_Configuration.BroadcastMessagesCount) : "       ";
            this.tsslReceiving.BackColor = !connected || this.m_Configuration.ReceivingMessages ? Color.FromKnownColor(KnownColor.ButtonFace) : Color.Bisque;
            this.tsslFilterSts.BackColor = !connected || this.m_Configuration.FilterStatus == RP1210C_FilterStatus.Open ? Color.FromKnownColor(KnownColor.ButtonFace) : Color.Bisque;
            this.tsslBroadcasting.BackColor = !connected || this.m_Configuration.BroadcastMessagesCount == 0 ? Color.FromKnownColor(KnownColor.ButtonFace) : Color.Bisque;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.m_Ini = new IniFileReader("C:\\Windows\\PEAKRP32.ini", "DeviceInformation1");
            string result;
            if (this.m_Ini.ReadString("DeviceName", out result))
            {
                int num1 = (int)MessageBox.Show(result);
            }
            else
            {
                int num2 = (int)MessageBox.Show("NOP!");
            }
            if (IniFileReader.ReadString("C:\\Windows\\RP121032.ini", "RP1210Support", "APIImplementations", out result))
            {
                int num3 = (int)MessageBox.Show(result);
            }
            else
            {
                int num4 = (int)MessageBox.Show("NOP!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RP1210_InformationReader informationReader = new RP1210_InformationReader("PEAKRP32");
            RP1210_ImplementationInformation implementation = informationReader.Implementation;
            // ISSUE: variable of a boxed type
            bool exists = implementation.Exists;
            implementation = informationReader.Implementation;
            // ISSUE: variable of a boxed type
            bool library32Found = implementation.Library32Found;
            implementation = informationReader.Implementation;
            // ISSUE: variable of a boxed type
            bool library64Found = implementation.Library64Found;
            int num = (int)MessageBox.Show(string.Format("Found: {0} - Win32: {1} Win64: {2}", (object)exists, (object)library32Found, (object)library64Found));
        }

        private void ConfigureConnection()
        {
            ConnectionForm connectionForm = new ConnectionForm(this.m_Implementation);
            connectionForm.Baudrate = this.m_Baudrate;
            connectionForm.Protocol = this.m_Protocol;
            connectionForm.Device = this.m_Device;
            connectionForm.ChannelNumber = this.m_Channel;
            connectionForm.AppIsDoingPacketizing = this.m_AppIsPacking;
            connectionForm.UsingOldPCANRP64 = this.m_UsingOldPCANRP64;
            if (connectionForm.ShowDialog() == DialogResult.OK)
            {
                PEAKRP32Plus peakrP32Plus;
                RP1210_ImplementationInformation implementation;
                if (connectionForm.UsePCANRP64File)
                {
                    peakrP32Plus = new PEAKRP32Plus("PCANRP64");
                }
                else
                {
                    implementation = connectionForm.Implementation;
                    peakrP32Plus = new PEAKRP32Plus(implementation.Name);
                }
                if (!peakrP32Plus.Loaded)
                {
                    implementation = connectionForm.Implementation;
                    int num = (int)MessageBox.Show(string.Format("Fail to load : {0}", (object)implementation.Name));
                }
                else
                {
                    if (connectionForm.Protocol.ProtocolDescription != this.m_Protocol.ProtocolDescription)
                        this.PerformClearMessages(Form1.MessagesToClear.Reception | Form1.MessagesToClear.Transmission);
                    this.m_PEAKRP32.ImplementationName = peakrP32Plus.ImplementationName;
                    this.m_Implementation = connectionForm.Implementation;
                    this.m_Device = connectionForm.Device;
                    this.m_Protocol = connectionForm.Protocol;
                    this.m_Channel = connectionForm.ChannelNumber;
                    this.m_Baudrate = connectionForm.Baudrate;
                    this.m_AppIsPacking = connectionForm.AppIsDoingPacketizing;
                    this.m_UsingOldPCANRP64 = connectionForm.UsingOldPCANRP64;
                    this.ConfigureIncomingListView();
                }
            }
            this.SetConnectionStatus(this.m_Connected);
        }

        private void ConfigureIncomingListView()
        {
            this.lstMessages.Columns.Clear();
            this.lstTxMessages.Columns.Clear();
            this.m_ReceiveMsgList.Clear();
            this.m_SendMsgList.Clear();
            switch ((RP1210CProtocol)this.m_Protocol.Index)
            {
                case RP1210CProtocol.CAN:
                    this.ConfigureRxColumns(RP1210CProtocol.CAN);
                    this.ConfigureTxColumns(RP1210CProtocol.CAN);
                    break;
                case RP1210CProtocol.J1939:
                    this.ConfigureRxColumns(RP1210CProtocol.J1939);
                    this.ConfigureTxColumns(RP1210CProtocol.J1939);
                    break;
                case RP1210CProtocol.ISO15765:
                    this.ConfigureRxColumns(RP1210CProtocol.ISO15765);
                    this.ConfigureTxColumns(RP1210CProtocol.ISO15765);
                    break;
            }
        }

        private bool ConfigureTxColumns(RP1210CProtocol protocol)
        {
            switch (protocol)
            {
                case RP1210CProtocol.CAN:
                    this.lstTxMessages.Columns.Add("Type", 70).Name = "clhType";
                    this.lstTxMessages.Columns.Add("ID", 90).Name = "clhID";
                    this.lstTxMessages.Columns.Add("Length", 50).Name = "clhLength";
                    this.lstTxMessages.Columns.Add("Data", 160).Name = "clhData";
                    this.lstTxMessages.Columns.Add("Count", 100).Name = "clhCount";
                    return true;
                case RP1210CProtocol.J1939:
                    this.lstTxMessages.Columns.Add("PGN", 50).Name = "clhPGN";
                    this.lstTxMessages.Columns.Add("Priority", 50).Name = "clhPriority";
                    this.lstTxMessages.Columns.Add("Originator", 65).Name = "clhOriginator";
                    this.lstTxMessages.Columns.Add("Responder", 65).Name = "clhResponder";
                    this.lstTxMessages.Columns.Add("Length", 50).Name = "clhLength";
                    this.lstTxMessages.Columns.Add("Count", 50).Name = "clhCount";
                    this.lstTxMessages.Columns.Add("Data", 500).Name = "clhData";
                    return true;
                case RP1210CProtocol.ISO15765:
                    this.lstTxMessages.Columns.Add("Message Type", 220).Name = "clhType";
                    this.lstTxMessages.Columns.Add("ID", 80).Name = "clhID";
                    this.lstTxMessages.Columns.Add("Ext. Addr.", 60).Name = "clhExtensionAddress";
                    this.lstTxMessages.Columns.Add("Length", 50).Name = "clhLength";
                    this.lstTxMessages.Columns.Add("Count", 60).Name = "clhCount";
                    this.lstTxMessages.Columns.Add("Data", 500).Name = "clhData";
                    return true;
                default:
                    return false;
            }
        }

        private bool ConfigureRxColumns(RP1210CProtocol protocol)
        {
            switch (protocol)
            {
                case RP1210CProtocol.CAN:
                    this.lstMessages.Columns.Add("Type", 70).Name = "clhType";
                    this.lstMessages.Columns.Add("ID", 90).Name = "clhID";
                    this.lstMessages.Columns.Add("Length", 50).Name = "clhLength";
                    this.lstMessages.Columns.Add("Data", 160).Name = "clhData";
                    this.lstMessages.Columns.Add("Count", 100).Name = "clhCount";
                    this.lstMessages.Columns.Add("Timestamp", 100).Name = "clhTimestamp";
                    return true;
                case RP1210CProtocol.J1939:
                    this.lstMessages.Columns.Add("PGN", 50).Name = "clhPGN";
                    this.lstMessages.Columns.Add("Priority", 50).Name = "clhPriority";
                    this.lstMessages.Columns.Add("Originator", 65).Name = "clhOriginator";
                    this.lstMessages.Columns.Add("Responder", 65).Name = "clhResponder";
                    this.lstMessages.Columns.Add("Length", 50).Name = "clhLength";
                    this.lstMessages.Columns.Add("Count", 50).Name = "clhCount";
                    this.lstMessages.Columns.Add("Timestamp", 80).Name = "clhTimestamp";
                    this.lstMessages.Columns.Add("Data", 500).Name = "clhData";
                    return true;
                case RP1210CProtocol.ISO15765:
                    this.lstMessages.Columns.Add("Kind", 80).Name = "clhKind";
                    this.lstMessages.Columns.Add("Message Type", 220).Name = "clhType";
                    this.lstMessages.Columns.Add("ID", 80).Name = "clhID";
                    this.lstMessages.Columns.Add("Ext. Addr.", 60).Name = "clhExtensionAddress";
                    this.lstMessages.Columns.Add("Count", 60).Name = "clhCount";
                    this.lstMessages.Columns.Add("Timestamp", 80).Name = "clhTimestamp";
                    this.lstMessages.Columns.Add("Length", 50).Name = "clhLength";
                    this.lstMessages.Columns.Add("Data", 500).Name = "clhData";
                    return true;
                default:
                    return false;
            }
        }

        private void ConfigureSession()
        {
            ConfigurationForm configurationForm = new ConfigurationForm((RP1210CProtocol)this.m_Protocol.Index, this.m_Configuration, this.m_PEAKRP32.ImplementationLevel);
            if (configurationForm.ShowDialog() != DialogResult.OK)
                return;
            this.CompileConfiguration(configurationForm.Configuration);
        }

        private void ConfigureBroadcast()
        {
            BroadcastsForm broadcastsForm = new BroadcastsForm((RP1210CProtocol)this.m_Protocol.Index, this.m_Configuration);
            if (broadcastsForm.ShowDialog() != DialogResult.OK)
                return;
            this.CompileConfiguration(broadcastsForm.Configuration);
        }

        private void ConfigureClaimAddr()
        {
            AddressClaimingForm addressClaimingForm = new AddressClaimingForm(this.m_J19239Network);
            if (addressClaimingForm.ShowDialog() != DialogResult.OK)
                return;
            this.DoAddressClaiming(addressClaimingForm.Network);
        }

        private void ConfigureFlowControls()
        {
            FlowControlsForm flowControlsForm = new FlowControlsForm(this.m_ISO15765FlowControls.ToArray());
            if (flowControlsForm.ShowDialog() != DialogResult.OK)
                return;
            this.DoFlowControlsConfiguration(flowControlsForm.FlowControls);
        }

        private void DoFlowControlsConfiguration(RP1210ISO15765FlowControl[] flowControls)
        {
            short num1 = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_CLEAR_ISO15765_FLOW_CONTROL, this.m_Client, (byte[])null);
            if (num1 == (short)0)
            {
                this.m_ISO15765FlowControls.Clear();
                if (flowControls.Length == 0)
                    return;
                List<byte> byteList = new List<byte>();
                foreach (RP1210ISO15765FlowControl flowControl in flowControls)
                    byteList.AddRange((IEnumerable<byte>)flowControl.ByteArray);
                short num2 = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_SET_ISO15765_FLOW_CONTROL, this.m_Client, byteList.ToArray());
                if (num2 == (short)0)
                {
                    this.m_ISO15765FlowControls.AddRange((IEnumerable<RP1210ISO15765FlowControl>)flowControls);
                }
                else
                {
                    int num3 = (int)MessageBox.Show(string.Format("The flow control list couldn't be configured. Error: {0}", (object)num2), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            else
            {
                int num4 = (int)MessageBox.Show(string.Format("The list of flow controls couldn't be cleared. Error: {0}", (object)num1), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void DoAddressClaiming(RP1210J1939Network addrConfig)
        {
            byte[] fpchClientCommand = new byte[10];
            if (this.m_J19239Network.Claimed)
            {
                short num1;
                if (this.m_PEAKRP32.ImplementationLevel == PEAKRP32Plus.ImplementationLoadLevel.A)
                {
                    fpchClientCommand[0] = byte.MaxValue;
                    num1 = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_PROTECT_J1939_ADDRESS, this.m_Client, fpchClientCommand);
                }
                else
                    num1 = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_RELEASE_J1939_ADDRESS, this.m_Client, new byte[1]
                    {
            this.m_J19239Network.Address
                    });
                if (num1 == (short)0)
                {
                    this.m_J19239Network.Claimed = false;
                }
                else
                {
                    int num2 = (int)MessageBox.Show("J1939 address couldn't be released (RPCMD_RELEASE_J1939_ADDRESS)! Error: " + num1.ToString(), "SendCommand Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            this.m_J19239Network.Address = addrConfig.Address;
            this.m_J19239Network.Name = addrConfig.Name;
            fpchClientCommand[0] = addrConfig.Address;
            Array.Copy((Array)addrConfig.Name.AsByteArray, 0, (Array)fpchClientCommand, 1, 8);
            fpchClientCommand[9] = (byte)0;
            short num3 = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_PROTECT_J1939_ADDRESS, this.m_Client, fpchClientCommand);
            this.m_J19239Network.Claimed = num3 == (short)0;
            if (this.m_J19239Network.Claimed)
                return;
            int num4 = (int)MessageBox.Show("Could not claim the J1939 address (RPCMD_PROTECT_J1939_ADDRESS)! Error: " + num3.ToString(), "SendCommand Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void ConfigureInterpacketTime(uint interpacketTime)
        {
            short num1 = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_SET_J1939_INTERPACKET_TIME, this.m_Client, BitConverter.GetBytes(interpacketTime));
            if (num1 == (short)0)
            {
                this.m_Configuration.InterpacketTime = interpacketTime;
            }
            else
            {
                int num2 = (int)MessageBox.Show("SET_J1939_INTERPACKET_TIME could not be Set! Error: " + num1.ToString(), "SendCommand Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void ConfigureEchoReception(bool receivingEcho)
        {
            short num1 = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_ECHO_TRANSMITTED_MESSAGES, this.m_Client, new byte[1]
            {
        Convert.ToByte(receivingEcho)
            });
            if (num1 == (short)0)
            {
                this.m_Configuration.ReceivingEcho = receivingEcho;
            }
            else
            {
                int num2 = (int)MessageBox.Show("ECHO_TRANSMITTED_MESSAGES could not be Set! Error: " + num1.ToString(), "SendCommand Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void ConfigureMessagesReception(bool receivingMessages)
        {
            short num1 = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_SET_MESSAGE_RECEIVE, this.m_Client, new byte[1]
            {
        Convert.ToByte(receivingMessages)
            });
            if (num1 == (short)0)
            {
                this.m_Configuration.ReceivingMessages = receivingMessages;
            }
            else
            {
                int num2 = (int)MessageBox.Show("SET_MESSAGE_RECEIVE could not be Set! Error: " + num1.ToString(), "SendCommand Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void ConfigureFilterType(RP1210C_FilterType filterType)
        {
            short num1;
            if (this.m_Protocol.Index == 10)
                num1 = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_SET_CAN_FILTER_TYPE, this.m_Client, new byte[1]
                {
          (byte) filterType
                });
            else if (this.m_Protocol.Index == 20)
                num1 = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_SET_J1939_FILTER_TYPE, this.m_Client, new byte[1]
                {
          (byte) filterType
                });
            else if (this.m_Protocol.Index == 30)
            {
                num1 = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_SET_ISO15765_FILTER_TYPE, this.m_Client, new byte[1]
                {
          (byte) filterType
                });
            }
            else
            {
                int num2 = (int)MessageBox.Show("ISO-15765 not supported!");
                return;
            }
            if (num1 == (short)0)
            {
                this.m_Configuration.FilterType = filterType;
            }
            else
            {
                int num3 = (int)MessageBox.Show("SET_FILTER_TYPE could not be Set! Error: " + num1.ToString(), "SendCommand Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void ConfigureFilter(RP1210_Configuration configuration)
        {
            switch (configuration.FilterStatus)
            {
                case RP1210C_FilterStatus.Close:
                    if (!this.CloseFilter())
                        break;
                    this.m_Configuration.FilterStatus = RP1210C_FilterStatus.Close;
                    break;
                case RP1210C_FilterStatus.Open:
                    if (!this.OpenFilter())
                        break;
                    this.m_Configuration.FilterStatus = RP1210C_FilterStatus.Open;
                    break;
                case RP1210C_FilterStatus.Custom:
                    if (!this.CloseFilter())
                        break;
                    this.m_Configuration.FilterStatus = RP1210C_FilterStatus.Close;
                    switch ((RP1210CProtocol)this.m_Protocol.Index)
                    {
                        case RP1210CProtocol.CAN:
                            if (configuration.CanFilters.Length != 0 && !this.ConfigureCustomFilter(configuration.CanFilters))
                                return;
                            this.m_Configuration.FilterStatus = RP1210C_FilterStatus.Custom;
                            return;
                        case RP1210CProtocol.J1939:
                            if (configuration.J1939Filters.Length != 0 && !this.ConfigureCustomFilter(configuration.J1939Filters))
                                return;
                            this.m_Configuration.FilterStatus = RP1210C_FilterStatus.Custom;
                            return;
                        case RP1210CProtocol.ISO15765:
                            if (configuration.Iso15765Filters.Length != 0 && !this.ConfigureCustomFilter(configuration.Iso15765Filters))
                                return;
                            this.m_Configuration.FilterStatus = RP1210C_FilterStatus.Custom;
                            return;
                        default:
                            return;
                    }
                default:
                    int num = (int)MessageBox.Show("Filter Status not supported!");
                    break;
            }
        }

        private void AddBroadcastMessage(RP1210_BroadcastMsg broadcast)
        {
            List<byte> byteList = new List<byte>();
            RP1210CCommand nCommandNumber;
            switch (broadcast.Protocol)
            {
                case RP1210CProtocol.CAN:
                    nCommandNumber = RP1210CCommand.RPCMD_SET_BROADCAST_FOR_CAN;
                    break;
                case RP1210CProtocol.J1939:
                    nCommandNumber = RP1210CCommand.RPCMD_SET_BROADCAST_FOR_J1939;
                    break;
                case RP1210CProtocol.ISO15765:
                    nCommandNumber = RP1210CCommand.RPCMD_SET_BROADCAST_FOR_ISO15765;
                    break;
                default:
                    int num1 = (int)MessageBox.Show("RPCMD_SET_BROADCAST_FOR PROTOCOL could not be performed! Wrong protocol used", "SendCommand Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
            }
            byteList.Add((byte)1);
            byteList.AddRange((IEnumerable<byte>)BitConverter.GetBytes(broadcast.Interval));
            byteList.AddRange((IEnumerable<byte>)BitConverter.GetBytes(broadcast.Msg.WriteBufferLength));
            byteList.AddRange((IEnumerable<byte>)broadcast.Msg.ToByteArray());
            byte[] array = byteList.ToArray();
            short num2 = this.m_PEAKRP32.SendCommand(nCommandNumber, this.m_Client, array);
            if (num2 == (short)0)
            {
                if (this.m_Configuration.AddBroadcastMessage(new RP1210_BroadcastMsg(broadcast.Msg, broadcast.Interval, array[0])))
                    return;
                int num3 = (int)MessageBox.Show("Adding the new added broadcast message internally caused an unknown error", nameof(AddBroadcastMessage), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                int num4 = (int)MessageBox.Show("SET_MESSAGE_RECEIVE could not be Set! Error: " + num2.ToString(), "SendCommand Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void RemoveBroadcastMessage(RP1210_BroadcastMsg broadcast)
        {
            RP1210CCommand nCommandNumber;
            switch (broadcast.Protocol)
            {
                case RP1210CProtocol.CAN:
                    nCommandNumber = RP1210CCommand.RPCMD_SET_BROADCAST_FOR_CAN;
                    break;
                case RP1210CProtocol.J1939:
                    nCommandNumber = RP1210CCommand.RPCMD_SET_BROADCAST_FOR_J1939;
                    break;
                case RP1210CProtocol.ISO15765:
                    nCommandNumber = RP1210CCommand.RPCMD_SET_BROADCAST_FOR_ISO15765;
                    break;
                default:
                    int num1 = (int)MessageBox.Show("RPCMD_SET_BROADCAST_FOR PROTOCOL could not be performed! Wrong protocol used", "SendCommand Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
            }
            byte[] fpchClientCommand = new byte[2]
            {
        (byte) 4,
        broadcast.EntryNumber
            };
            short num2 = this.m_PEAKRP32.SendCommand(nCommandNumber, this.m_Client, fpchClientCommand);
            if (num2 == (short)0)
            {
                if (this.m_Configuration.RemoveBroadcastMessage(broadcast))
                    return;
                int num3 = (int)MessageBox.Show("Removing the broadcast message internally caused an unknown error", nameof(RemoveBroadcastMessage), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                int num4 = (int)MessageBox.Show("SET_MESSAGE_RECEIVE could not be Set! Error: " + num2.ToString(), "SendCommand Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void ConfigureBroadcastEntries(RP1210_BroadcastMsg[] broadcastMsgs)
        {
            foreach (RP1210_BroadcastMsg broadcastMessage in this.m_Configuration.BroadcastMessages)
            {
                if (!((IEnumerable<RP1210_BroadcastMsg>)broadcastMsgs).Contains<RP1210_BroadcastMsg>(broadcastMessage))
                    this.RemoveBroadcastMessage(broadcastMessage);
            }
            foreach (RP1210_BroadcastMsg broadcastMsg in broadcastMsgs)
            {
                if (!((IEnumerable<RP1210_BroadcastMsg>)this.m_Configuration.BroadcastMessages).Contains<RP1210_BroadcastMsg>(broadcastMsg))
                    this.AddBroadcastMessage(broadcastMsg);
            }
        }

        private void CompileConfiguration(RP1210_Configuration configuration)
        {
            if ((int)configuration.InterpacketTime != (int)this.m_Configuration.InterpacketTime)
                this.ConfigureInterpacketTime(configuration.InterpacketTime);
            if (configuration.ReceivingEcho != this.m_Configuration.ReceivingEcho)
                this.ConfigureEchoReception(configuration.ReceivingEcho);
            if (configuration.ReceivingMessages != this.m_Configuration.ReceivingMessages)
                this.ConfigureMessagesReception(configuration.ReceivingMessages);
            if (configuration.FilterType != this.m_Configuration.FilterType)
                this.ConfigureFilterType(configuration.FilterType);
            if (configuration.FilterStatus == RP1210C_FilterStatus.Custom || configuration.FilterStatus != this.m_Configuration.FilterStatus)
                this.ConfigureFilter(configuration);
            this.ConfigureBroadcastEntries(configuration.BroadcastMessages);
        }

        private bool ConfigureCustomFilter(RP1210_FilterCan[] filter)
        {
            List<byte> byteList = new List<byte>();
            for (int index = 0; index < filter.Length; ++index)
                byteList.AddRange((IEnumerable<byte>)filter[index].ByteArray);
            short num1 = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_SET_MESSAGE_FILTERING_FOR_CAN, this.m_Client, byteList.ToArray());
            if (num1 != (short)0)
            {
                int num2 = (int)MessageBox.Show("SET_MESSAGE_FILTERING_FOR_CAN could not be executed! Error: " + num1.ToString(), "SendCommand Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return num1 == (short)0;
        }

        private bool ConfigureCustomFilter(RP1210_FilterJ1939[] filter)
        {
            List<byte> byteList = new List<byte>();
            for (int index = 0; index < filter.Length; ++index)
                byteList.AddRange((IEnumerable<byte>)filter[index].ByteArray);
            short num1 = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_SET_MESSAGE_FILTERING_FOR_J1939, this.m_Client, byteList.ToArray());
            if (num1 != (short)0)
            {
                int num2 = (int)MessageBox.Show("SET_MESSAGE_FILTERING_FOR_J1939 could not be executed! Error: " + num1.ToString(), "SendCommand Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return num1 == (short)0;
        }

        private bool ConfigureCustomFilter(RP1210_FilterIso15765[] filter)
        {
            List<byte> byteList = new List<byte>();
            for (int index = 0; index < filter.Length; ++index)
                byteList.AddRange((IEnumerable<byte>)filter[index].ByteArray);
            short num1 = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_SET_MESSAGE_FILTERING_FOR_ISO15765, this.m_Client, byteList.ToArray());
            if (num1 != (short)0)
            {
                int num2 = (int)MessageBox.Show("SET_MESSAGE_FILTERING_FOR_ISO15765 could not be executed! Error: " + num1.ToString(), "SendCommand Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return num1 == (short)0;
        }

        private bool OpenFilter()
        {
            short num1 = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_SET_ALL_FILTERS_STATES_TO_PASS, this.m_Client, new byte[0]);
            if (num1 != (short)0)
            {
                int num2 = (int)MessageBox.Show("SET_ALL_FILTERS_STATES_TO_PASS could not be executed! Error: " + num1.ToString(), "SendCommand Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return num1 == (short)0;
        }

        private bool CloseFilter()
        {
            short num1 = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_SET_ALL_FILTERS_STATES_TO_DISCARD, this.m_Client, new byte[0]);
            if (num1 != (short)0)
            {
                int num2 = (int)MessageBox.Show("SET_ALL_FILTERS_STATES_TO_DISCARD could not be executed! Error: " + num1.ToString(), "SendCommand Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return num1 == (short)0;
        }

        private void tsbExit_Click(object sender, EventArgs e) => this.Close();

        private void tsbConfigure_Click(object sender, EventArgs e) => this.ConfigureConnection();

        private void tsbConnect_Click(object sender, EventArgs e)
        {
            if (this.m_Connected)
                this.Disconnect();
            else
                this.Connect();
        }

        private string GetProtocolString()
        {
            string str = this.m_Protocol.ProtocolString;
            switch (this.m_Baudrate)
            {
                case RP1210_Baudrate.Standard:
                    if (this.m_Protocol.ImplementationName == "PEAKRP32")
                    {
                        str += ":";
                        break;
                    }
                    break;
                case RP1210_Baudrate.Auto:
                    str += ":Baud=Auto,";
                    break;
                case RP1210_Baudrate.All:
                    str += ":Baud=All,";
                    break;
                default:
                    str = str + ":Baud=" + ((int)this.m_Baudrate).ToString() + ",";
                    break;
            }
            return this.m_Protocol.ImplementationName == "PEAKRP32" ? string.Format("{0}Channel={1}", (object)str, (object)this.m_Channel) : this.m_Protocol.ProtocolString;
        }

        private void Connect()
        {
            StringBuilder fpchAPIVersionInfo = new StringBuilder(50);
            StringBuilder fpchDLLVersionInfo = new StringBuilder(50);
            StringBuilder fpchFWVersionInfo = new StringBuilder(50);
            if (this.m_Device.DeviceID == 0)
            {
                this.ConfigureConnection();
                if (this.m_Device.DeviceID == 0)
                    return;
            }
            //if (this.m_PEAKRP32.ImplementationName != "PEAKRP32" && this.m_PEAKRP32.ImplementationName != "PCANRP32" && this.m_PEAKRP32.ImplementationName != "PCANRP64")
            //{
            //    int num1 = (int)MessageBox.Show("Initialization not allowed. This is a demo application intended to be used as test application for devices of the company PEAK-System Technik GmbH", "Device is not allowed!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}
            //else
            {
                this.m_Chronometer.Reset();
                this.tsslSpeed.ToolTipText = this.m_Baudrate == RP1210_Baudrate.Auto ? "Detecting bitrate..." : "Connecting: " + this.GetProtocolString();
                this.m_Configuration = new RP1210_Configuration();
                this.m_Client = this.m_PEAKRP32.ClientConnect(0, Convert.ToUInt16(this.m_Device.DeviceID), this.GetProtocolString(), 0, 0, this.m_AppIsPacking ? (short)1 : (short)0);
                if (this.m_Baudrate == RP1210_Baudrate.Auto)
                    this.m_Chronometer.Start();
                this.m_Connected = this.m_Client < (short)128;
                this.SetConnectionStatus(this.m_Connected);
                this.tmrRead.Enabled = this.m_Connected;
                this.StartThread();
                if (!this.m_Connected)
                {
                    int num2 = (int)MessageBox.Show(string.Format("Error while connecting client: {0}", (object)(RP1210CError)this.m_Client), "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    if (this.m_Baudrate == RP1210_Baudrate.Auto)
                        return;
                    if (this.m_PEAKRP32.ImplementationName == "PEAKRP32")
                    {
                        short num3 = this.m_PEAKRP32.ReadDetailedVersion(this.m_Client, fpchAPIVersionInfo, fpchDLLVersionInfo, fpchFWVersionInfo);
                        if (num3 != (short)0)
                        {
                            int num4 = (int)MessageBox.Show(string.Format("Error on ReadDetailedVersion call: {0}", (object)(RP1210CError)num3), "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        else
                            this.Text = string.Format("RP1210 View (Versions API: {0} - DLL: {1} - Firmware: {2})", (object)fpchAPIVersionInfo.ToString(), (object)fpchDLLVersionInfo.ToString(), (object)fpchFWVersionInfo.ToString());
                    }
                    else
                    {
                        StringBuilder fpchDLLMajorVersion = new StringBuilder(2);
                        StringBuilder fpchDLLMinorVersion = new StringBuilder(2);
                        StringBuilder fpchAPIMajorVersion = new StringBuilder(2);
                        StringBuilder fpchAPIMinorVersion = new StringBuilder(2);
                        this.m_PEAKRP32.ReadVersion(fpchDLLMajorVersion, fpchDLLMinorVersion, fpchAPIMajorVersion, fpchAPIMinorVersion);
                        this.Text = string.Format("RP1210 View (Versions API: {2}.{3} - DLL: {0}.{1})", (object)fpchDLLMajorVersion, (object)fpchDLLMinorVersion, (object)fpchAPIMajorVersion, (object)fpchAPIMinorVersion);
                    }
                }
            }
        }

        private void StartThread()
        {
            if (this.m_ReadThread != null)
                return;
            this.m_ReadThread = new Thread(new ThreadStart(this.ThreadFunction));
            this.m_ReadThread.IsBackground = true;
            this.m_ReadThread.Start();
        }

        private void ThreadFunction()
        {
            while (this.m_Connected)
            {
                Thread.Sleep(50);
                this.Invoke((Delegate)this.m_ReadDelegate);
            }
        }

        private void StopTrhead()
        {
            if (this.m_ReadThread == null)
                return;
            this.m_ReadThread.Abort();
            this.m_ReadThread.Join();
            this.m_ReadThread = (Thread)null;
        }

        private void Disconnect()
        {
            if (!this.m_Connected)
                return;
            this.tmrRead.Enabled = false;
            this.StopTrhead();
            short num1 = this.m_PEAKRP32.ClientDisconnect(this.m_Client);
            if (num1 != (short)0)
            {
                int num2 = (int)MessageBox.Show(string.Format("Error while disconnecting client: {0}", (object)num1), "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            this.Text = "RP1210 View";
            this.m_Connected = false;
            this.m_ISO15765FlowControls.Clear();
            this.SetConnectionStatus(this.m_Connected);
        }

        private void ResetConnection()
        {
            if (!this.m_Connected)
                return;
            this.tmrRead.Enabled = false;
            short num1 = this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_RESET_DEVICE, this.m_Client, new byte[0]);
            if (num1 != (short)0)
            {
                int num2 = (int)MessageBox.Show(string.Format("Error while reseting client: {0}", (object)num1), "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                this.m_Connected = false;
                this.m_ISO15765FlowControls.Clear();
                this.SetConnectionStatus(this.m_Connected);
                this.Connect();
            }
        }

        private void tmrRead_Tick(object sender, EventArgs e)
        {
            if (!this.m_Connected)
                return;
            if (!this.CheckBitrate())
            {
                this.m_Chronometer.Stop();
                if (this.m_Baudrate == RP1210_Baudrate.Auto)
                {
                    this.tsslSpeed.ToolTipText = string.Format("Detection finished. Time taken {0:F5} seconds", (object)this.m_Chronometer.Elapsed.TotalSeconds);
                }
                else
                {
                    this.tsslSpeed.ToolTipText = string.Format("Connection string used: {0}", (object)this.GetProtocolString());
                    this.DoRead();
                }
            }
            else
                this.DoRead();
        }

        private void DoRead()
        {
            short num;
            while ((num = this.m_PEAKRP32.ReadMessage(this.m_Client, this.m_ReadBuffer, (short)0)) > (short)0)
            {
                if (this.m_Protocol.Index == 10)
                    this.HandleMessage((object)new RP1210_MsgCanRx(SubArray.GetBytes(this.m_ReadBuffer, 0, (int)num), this.m_Configuration.ReceivingEcho));
                if (this.m_Protocol.Index == 20)
                    this.HandleMessage((object)new RP1210_MsgJ1939Rx(SubArray.GetBytes(this.m_ReadBuffer, 0, (int)num), this.m_Configuration.ReceivingEcho));
                if (this.m_Protocol.Index == 30)
                    this.HandleMessage((object)new RP1210_MsgIso15765Rx(SubArray.GetBytes(this.m_ReadBuffer, 0, (int)num), this.m_Configuration.ReceivingEcho));
            }
        }

        private void HandleMessage(object msg)
        {
            if (msg == null)
                return;
            foreach (ListViewMessageStatus receiveMsg in this.m_ReceiveMsgList)
            {
                if (receiveMsg.SameMessage(msg))
                {
                    receiveMsg.Update(msg);
                    return;
                }
            }
            this.m_ReceiveMsgList.Add(new ListViewMessageStatus(this.lstMessages, msg));
        }

        private bool CheckBitrate()
        {
            byte[] numArray = new byte[8];
            if (this.m_PEAKRP32.SendCommand(RP1210CCommand.RPCMD_GET_PROTOCOL_CONNECTION_SPEED, this.m_Client, numArray) != (short)0)
                return false;
            string str = Encoding.Default.GetString(numArray).TrimEnd(new char[1]);
            this.tsslSpeed.BackColor = str != "0" ? Color.Honeydew : Color.Bisque;
            this.tsslSpeed.ForeColor = str != "0" ? Color.Green : Color.Red;
            this.tsslSpeed.Text = str;
            return str != "0";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) => this.Disconnect();

        private void tsbRefresh_Click(object sender, EventArgs e) => this.ResetConnection();

        private void Form1_Shown(object sender, EventArgs e) => this.ConfigureConnection();

        private void tsbMsgConfig_Click(object sender, EventArgs e)
        {
            this.ConfigureSession();
            this.ShowConfigurationStatus(true);
        }

        private void tsbBroadcastConfig_Click(object sender, EventArgs e)
        {
            this.ConfigureBroadcast();
            this.ShowConfigurationStatus(true);
        }

        private void tsbAddressClaim_Click(object sender, EventArgs e)
        {
            if (this.m_Protocol.Index == 20)
                this.ConfigureClaimAddr();
            else
                this.ConfigureFlowControls();
        }

        private void tsmiAdd_Click(object sender, EventArgs e) => this.PerformCreateMessage();

        private void tsmiEdit_Click(object sender, EventArgs e) => this.PerformEditMessage();

        private void tsmiDelete_Click(object sender, EventArgs e) => this.PerformDeleteMessage();

        private void tsmiClear_Click(object sender, EventArgs e) => this.PerformClearMessages(Form1.MessagesToClear.Transmission);

        private void CreateNewCanMessage()
        {
            FormTxMsgCan formTxMsgCan = new FormTxMsgCan();
            if (formTxMsgCan.ShowDialog() != DialogResult.OK)
                return;
            this.m_SendMsgList.Add(new ListViewMessageSend(this.lstTxMessages, (object)formTxMsgCan.Message));
        }

        private void CreateNewJ1939Message()
        {
            FormTxMsgJ1939 formTxMsgJ1939 = new FormTxMsgJ1939();
            formTxMsgJ1939.SA = this.m_J19239Network.Address;
            if (formTxMsgJ1939.ShowDialog() != DialogResult.OK)
                return;
            this.m_SendMsgList.Add(new ListViewMessageSend(this.lstTxMessages, (object)formTxMsgJ1939.Message));
        }

        private void CreateNewIso15765Message()
        {
            FormTxMsgISO15765 formTxMsgIsO15765 = new FormTxMsgISO15765();
            if (formTxMsgIsO15765.ShowDialog() != DialogResult.OK)
                return;
            this.m_SendMsgList.Add(new ListViewMessageSend(this.lstTxMessages, (object)formTxMsgIsO15765.Message));
        }

        private bool SendMessage(RP1210_MsgCan msg)
        {
            short num1 = this.m_PEAKRP32.SendMessage(this.m_Client, msg.ToByteArray(), (short)0, (short)0);
            if (num1 == (short)0)
                return true;
            int num2 = (int)MessageBox.Show(string.Format("Error while sending CAN msg: {0}", (object)num1), "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return false;
        }

        private bool SendMessage(RP1210_MsgJ1939 msg)
        {
            short num1 = this.m_PEAKRP32.SendMessage(this.m_Client, msg.ToByteArray(), (short)0, (short)0);
            if (num1 == (short)0)
                return true;
            int num2 = (int)MessageBox.Show(string.Format("Error while sending J1939 msg: {0}", (object)num1), "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return false;
        }

        private bool SendMessage(RP1210_MsgIso15765 msg)
        {
            short num1 = this.m_PEAKRP32.SendMessage(this.m_Client, msg.ToByteArray(), (short)0, (short)0);
            if (num1 == (short)0)
                return true;
            int num2 = (int)MessageBox.Show(string.Format("Error while sending ISO15765 msg: {0}", (object)num1), "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return false;
        }

        private void tsslSpeed_DoubleClick(object sender, EventArgs e)
        {
            if (!this.m_Connected)
                return;
            
            //FormChangeBaudrate formChangeBaudrate = new FormChangeBaudrate(uint.Parse(this.tsslSpeed.Text.Trim()));
            FormChangeBaudrate formChangeBaudrate = new FormChangeBaudrate(250000);

            if (formChangeBaudrate.ShowDialog() != DialogResult.OK)
                return;
            this.ChangeBaudrate(formChangeBaudrate.NewBaudrate, formChangeBaudrate.ChangeNow);
        }

        private void ChangeBaudrate(uint newBaudrate, bool changeNow)
        {
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

        private void statusStrip1_DoubleClick(object sender, EventArgs e) => this.tsslSpeed_DoubleClick((object)this, new EventArgs());

        private void lstMessages_DoubleClick(object sender, EventArgs e) => this.PerformClearMessages(Form1.MessagesToClear.Reception);

        private void tsbAbout_Click(object sender, EventArgs e)
        {
            string dllVersion;
            string apiVersion;
            if (this.m_Connected && this.m_PEAKRP32.ImplementationLevel > PEAKRP32Plus.ImplementationLoadLevel.A)
                this.ReadDetailedVersion(out dllVersion, out apiVersion);
            else
                this.ReadVersion(out dllVersion, out apiVersion);
            int num = (int)new AboutBox(this.m_Implementation.Name, dllVersion, apiVersion).ShowDialog();
        }

        private void ReadDetailedVersion(out string dllVersion, out string apiVersion)
        {
            StringBuilder fpchAPIVersionInfo = new StringBuilder(17);
            StringBuilder fpchDLLVersionInfo = new StringBuilder(17);
            StringBuilder fpchFWVersionInfo = new StringBuilder(17);
            dllVersion = apiVersion = string.Empty;
            if (this.m_PEAKRP32.ReadDetailedVersion(this.m_Client, fpchAPIVersionInfo, fpchDLLVersionInfo, fpchFWVersionInfo) != (short)0)
                return;
            apiVersion = fpchAPIVersionInfo.ToString();
            dllVersion = fpchDLLVersionInfo.ToString();
        }

        private void ReadVersion(out string dllVersion, out string apiVersion)
        {
            StringBuilder fpchDLLMajorVersion = new StringBuilder(5);
            StringBuilder fpchDLLMinorVersion = new StringBuilder(5);
            StringBuilder fpchAPIMinorVersion = new StringBuilder(5);
            StringBuilder fpchAPIMajorVersion = new StringBuilder(5);
            this.m_PEAKRP32.ReadVersion(fpchDLLMajorVersion, fpchDLLMinorVersion, fpchAPIMajorVersion, fpchAPIMinorVersion);
            dllVersion = string.Format("{0}.{1}", (object)fpchDLLMajorVersion, (object)fpchDLLMinorVersion);
            apiVersion = string.Format("{0}.{1}", (object)fpchAPIMinorVersion, (object)fpchAPIMajorVersion);
        }

        private void cmsNewTxMsg_Opening(object sender, CancelEventArgs e)
        {
            this.tsmiClear.Enabled = this.lstTxMessages.Items.Count > 0;
            this.tsmiDelete.Enabled = this.tsmiEdit.Enabled = this.lstTxMessages.SelectedIndices.Count == 1;
        }

        private void lstTxMessages_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.PageUp:
                    this.PerformEditMessage();
                    break;
                case Keys.Space:
                    this.PerformSendMessage();
                    break;
                case Keys.Insert:
                    this.PerformCreateMessage();
                    break;
                case Keys.Delete:
                    this.PerformDeleteMessage();
                    break;
            }
        }

        private void PerformCreateMessage()
        {
            switch ((RP1210CProtocol)this.m_Protocol.Index)
            {
                case RP1210CProtocol.CAN:
                    this.CreateNewCanMessage();
                    break;
                case RP1210CProtocol.J1939:
                    this.CreateNewJ1939Message();
                    break;
                case RP1210CProtocol.ISO15765:
                    this.CreateNewIso15765Message();
                    break;
            }
        }

        private void PerformEditMessage()
        {
            switch ((RP1210CProtocol)this.m_Protocol.Index)
            {
                case RP1210CProtocol.CAN:
                    this.EditCanMessage();
                    break;
                case RP1210CProtocol.J1939:
                    this.EditJ1939Message();
                    break;
                case RP1210CProtocol.ISO15765:
                    this.EditIso15765Message();
                    break;
            }
        }

        private void EditCanMessage()
        {
            if (this.lstTxMessages.SelectedItems.Count != 1)
                return;
            ListViewMessageSend sendMsg = this.m_SendMsgList[this.lstTxMessages.SelectedItems[0].Index];
            FormTxMsgCan formTxMsgCan = new FormTxMsgCan();
            formTxMsgCan.Message = sendMsg.CANMsg;
            if (formTxMsgCan.ShowDialog() != DialogResult.OK)
                return;
            sendMsg.Update((object)formTxMsgCan.Message);
        }

        private void EditJ1939Message()
        {
            if (this.lstTxMessages.SelectedItems.Count != 1)
                return;
            ListViewMessageSend sendMsg = this.m_SendMsgList[this.lstTxMessages.SelectedItems[0].Index];
            FormTxMsgJ1939 formTxMsgJ1939 = new FormTxMsgJ1939();
            formTxMsgJ1939.Message = sendMsg.J1939Msg;
            if (formTxMsgJ1939.ShowDialog() != DialogResult.OK)
                return;
            sendMsg.Update((object)formTxMsgJ1939.Message);
        }

        private void EditIso15765Message()
        {
            if (this.lstTxMessages.SelectedItems.Count != 1)
                return;
            ListViewMessageSend sendMsg = this.m_SendMsgList[this.lstTxMessages.SelectedItems[0].Index];
            FormTxMsgISO15765 formTxMsgIsO15765 = new FormTxMsgISO15765();
            formTxMsgIsO15765.Message = sendMsg.ISO15765Msg;
            if (formTxMsgIsO15765.ShowDialog() != DialogResult.OK)
                return;
            sendMsg.Update((object)formTxMsgIsO15765.Message);
        }

        private void PerformDeleteMessage()
        {
            if (this.lstTxMessages.SelectedItems.Count != 1)
                return;
            ListViewMessageSend sendMsg = this.m_SendMsgList[this.lstTxMessages.SelectedItems[0].Index];
            this.lstTxMessages.Items.RemoveAt(sendMsg.Index);
            this.m_SendMsgList.Remove(sendMsg);
            if (this.lstTxMessages.Items.Count <= 0)
                return;
            this.lstTxMessages.Items[0].Selected = true;
        }

        private void PerformClearMessages(Form1.MessagesToClear msgsToclear)
        {
            if ((msgsToclear & Form1.MessagesToClear.Transmission) == Form1.MessagesToClear.Transmission)
            {
                this.lstTxMessages.Items.Clear();
                this.m_SendMsgList.Clear();
            }
            if ((msgsToclear & Form1.MessagesToClear.Reception) != Form1.MessagesToClear.Reception)
                return;
            this.lstMessages.Items.Clear();
            this.m_ReceiveMsgList.Clear();
        }

        private void PerformSendMessage()
        {
            if (this.lstTxMessages.SelectedItems.Count != 1)
                return;
            ListViewMessageSend sendMsg = this.m_SendMsgList[this.lstTxMessages.SelectedItems[0].Index];
            switch (sendMsg.Protocol)
            {
                case RP1210CProtocol.CAN:
                    if (!this.SendMessage(sendMsg.CANMsg))
                        break;
                    sendMsg.UpdateCount();
                    break;
                case RP1210CProtocol.J1939:
                    if (!this.SendMessage(sendMsg.J1939Msg))
                        break;
                    sendMsg.UpdateCount();
                    break;
                case RP1210CProtocol.ISO15765:
                    if (!this.SendMessage(sendMsg.ISO15765Msg))
                        break;
                    sendMsg.UpdateCount();
                    break;
            }
        }

        private void lstTxMessages_DoubleClick(object sender, EventArgs e)
        {
            if (this.lstTxMessages.SelectedItems.Count != 1)
                return;
            this.PerformEditMessage();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            byte[] numArray = new byte[1000];
            new Random().NextBytes(numArray);
            this.SendMessage(new RP1210_MsgJ1939(0U, RP1210_MsgJ1939.TransportMode.BAM, (byte)7, this.m_J19239Network.Address, byte.MaxValue, numArray));
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (this.m_Protocol.Index == 20)
            {
                byte[] numArray = new byte[8];
                new Random().NextBytes(numArray);
                this.SendMessage(new RP1210_MsgJ1939(0U, RP1210_MsgJ1939.TransportMode.RTS_CTS, (byte)7, this.m_J19239Network.Address, (byte)248, numArray));
            }
            else
            {
                byte[] numArray = new byte[8];
                new Random().NextBytes(numArray);
                this.SendMessage(new RP1210_MsgCan(256U, true, numArray));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.m_Protocol.Index != 30)
                return;
            this.SendMessage(new RP1210_MsgIso15765(RP1210CISO15765MsgType.EXTENDED_CAN_ISO15765_EXTENDED, 32U, (byte)1, new byte[5]
            {
        (byte) 1,
        (byte) 2,
        (byte) 3,
        (byte) 4,
        (byte) 5
            }));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.m_Protocol.Index != 30)
                return;
            this.SendMessage(new RP1210_MsgIso15765(RP1210CISO15765MsgType.STANDARD_CAN_ISO15765_EXTENDED, 32U, (byte)1, new byte[22]
            {
        (byte) 1,
        (byte) 2,
        (byte) 3,
        (byte) 4,
        (byte) 5,
        (byte) 6,
        (byte) 7,
        (byte) 8,
        (byte) 9,
        (byte) 10,
        (byte) 11,
        (byte) 12,
        (byte) 13,
        (byte) 14,
        (byte) 15,
        (byte) 16,
        (byte) 17,
        (byte) 18,
        (byte) 19,
        (byte) 20,
        (byte) 21,
        (byte) 22
            }));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.m_Protocol.Index != 20)
                return;
            byte[] numArray = new byte[100];
            new Random().NextBytes(numArray);
            this.SendMessage(new RP1210_MsgJ1939(0U, RP1210_MsgJ1939.TransportMode.RTS_CTS, (byte)7, this.m_J19239Network.Address, (byte)248, numArray));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = (IContainer)new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Form1));
            this.statusStrip1 = new StatusStrip();
            this.tsslConnection = new ToolStripStatusLabel();
            this.tsslProtocol = new ToolStripStatusLabel();
            this.tsslSpeed = new ToolStripStatusLabel();
            this.tsslFilterSts = new ToolStripStatusLabel();
            this.tsslEcho = new ToolStripStatusLabel();
            this.tsslReceiving = new ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new ToolStripStatusLabel();
            this.tsslBroadcasting = new ToolStripStatusLabel();
            this.toolStrip1 = new ToolStrip();
            this.tsbConnect = new ToolStripButton();
            this.tsbRefresh = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.tsbMsgConfig = new ToolStripButton();
            this.tsbBroadcastConfig = new ToolStripButton();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.tsbConfigure = new ToolStripButton();
            this.tsbExit = new ToolStripButton();
            this.tsbAbout = new ToolStripButton();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.tsbNetworkConfiguration = new ToolStripButton();
            this.spcContainer = new SplitContainer();
            this.panel4 = new Panel();
            this.lstMessages = new ListView();
            this.panel1 = new Panel();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.pictureBox1 = new PictureBox();
            this.panel5 = new Panel();
            this.button5 = new Button();
            this.button4 = new Button();
            this.button3 = new Button();
            this.button2 = new Button();
            this.button1 = new Button();
            this.lstTxMessages = new ListView();
            this.cmsNewTxMsg = new ContextMenuStrip(this.components);
            this.tsmiAdd = new ToolStripMenuItem();
            this.tsmiEdit = new ToolStripMenuItem();
            this.toolStripMenuItem1 = new ToolStripSeparator();
            this.tsmiDelete = new ToolStripMenuItem();
            this.tsmiClear = new ToolStripMenuItem();
            this.rtbCharOutput = new RichTextBox();
            this.panel2 = new Panel();
            this.tableLayoutPanel2 = new TableLayoutPanel();
            this.pictureBox2 = new PictureBox();
            this.tmrRead = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.spcContainer.BeginInit();
            this.spcContainer.Panel1.SuspendLayout();
            this.spcContainer.Panel2.SuspendLayout();
            this.spcContainer.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            this.panel5.SuspendLayout();
            this.cmsNewTxMsg.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((ISupportInitialize)this.pictureBox2).BeginInit();
            this.SuspendLayout();
            this.statusStrip1.Items.AddRange(new ToolStripItem[8]
            {
        (ToolStripItem) this.tsslConnection,
        (ToolStripItem) this.tsslProtocol,
        (ToolStripItem) this.tsslSpeed,
        (ToolStripItem) this.tsslFilterSts,
        (ToolStripItem) this.tsslEcho,
        (ToolStripItem) this.tsslReceiving,
        (ToolStripItem) this.toolStripStatusLabel1,
        (ToolStripItem) this.tsslBroadcasting
            });
            this.statusStrip1.Location = new Point(0, 381);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.ShowItemToolTips = true;
            this.statusStrip1.Size = new Size(761, 24);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "Status";
            this.statusStrip1.DoubleClick += new EventHandler(this.statusStrip1_DoubleClick);
            this.tsslConnection.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.tsslConnection.BorderStyle = Border3DStyle.Etched;
            this.tsslConnection.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsslConnection.Name = "tsslConnection";
            this.tsslConnection.Size = new Size(90, 19);
            this.tsslConnection.Text = "Not connected";
            this.tsslConnection.TextAlign = ContentAlignment.MiddleLeft;
            this.tsslProtocol.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.tsslProtocol.Name = "tsslProtocol";
            this.tsslProtocol.Size = new Size(14, 19);
            this.tsslProtocol.Text = " ";
            this.tsslSpeed.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.tsslSpeed.BorderStyle = Border3DStyle.Etched;
            this.tsslSpeed.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsslSpeed.Name = "tsslSpeed";
            this.tsslSpeed.Size = new Size(14, 19);
            this.tsslSpeed.Text = " ";
            this.tsslSpeed.DoubleClick += new EventHandler(this.tsslSpeed_DoubleClick);
            this.tsslFilterSts.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.tsslFilterSts.Name = "tsslFilterSts";
            this.tsslFilterSts.Size = new Size(14, 19);
            this.tsslFilterSts.Text = " ";
            this.tsslEcho.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.tsslEcho.Name = "tsslEcho";
            this.tsslEcho.Size = new Size(14, 19);
            this.tsslEcho.Text = " ";
            this.tsslReceiving.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.tsslReceiving.Name = "tsslReceiving";
            this.tsslReceiving.Size = new Size(14, 19);
            this.tsslReceiving.Text = " ";
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new Size(0, 19);
            this.tsslBroadcasting.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.tsslBroadcasting.Name = "tsslBroadcasting";
            this.tsslBroadcasting.Size = new Size(14, 19);
            this.tsslBroadcasting.Text = " ";
            this.toolStrip1.ImageScalingSize = new Size(24, 24);
            this.toolStrip1.Items.AddRange(new ToolStripItem[11]
            {
        (ToolStripItem) this.tsbConnect,
        (ToolStripItem) this.tsbRefresh,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.tsbMsgConfig,
        (ToolStripItem) this.tsbBroadcastConfig,
        (ToolStripItem) this.toolStripSeparator3,
        (ToolStripItem) this.tsbConfigure,
        (ToolStripItem) this.tsbExit,
        (ToolStripItem) this.tsbAbout,
        (ToolStripItem) this.toolStripSeparator2,
        (ToolStripItem) this.tsbNetworkConfiguration
            });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(761, 31);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            this.tsbConnect.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbConnect.Image = (Image)Resources.flash;
            this.tsbConnect.ImageTransparentColor = Color.Magenta;
            this.tsbConnect.Name = "tsbConnect";
            this.tsbConnect.Size = new Size(28, 28);
            this.tsbConnect.Text = "Connect / Disconnect channel";
            this.tsbConnect.Click += new EventHandler(this.tsbConnect_Click);
            this.tsbRefresh.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbRefresh.Enabled = false;
            this.tsbRefresh.Image = (Image)Resources.refresh;
            this.tsbRefresh.ImageTransparentColor = Color.Magenta;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new Size(28, 28);
            this.tsbRefresh.Text = "Reset connection";
            this.tsbRefresh.Click += new EventHandler(this.tsbRefresh_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 31);
            this.tsbMsgConfig.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbMsgConfig.Enabled = false;
            this.tsbMsgConfig.Image = (Image)Resources.mail_exchange;
            this.tsbMsgConfig.ImageTransparentColor = Color.Magenta;
            this.tsbMsgConfig.Name = "tsbMsgConfig";
            this.tsbMsgConfig.Size = new Size(28, 28);
            this.tsbMsgConfig.Text = "Messaging options and configuration";
            this.tsbMsgConfig.Click += new EventHandler(this.tsbMsgConfig_Click);
            this.tsbBroadcastConfig.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbBroadcastConfig.Enabled = false;
            this.tsbBroadcastConfig.Image = (Image)Resources.history;
            this.tsbBroadcastConfig.ImageTransparentColor = Color.Magenta;
            this.tsbBroadcastConfig.Name = "tsbBroadcastConfig";
            this.tsbBroadcastConfig.Size = new Size(28, 28);
            this.tsbBroadcastConfig.Text = "Broadcast messaging configuration";
            this.tsbBroadcastConfig.Click += new EventHandler(this.tsbBroadcastConfig_Click);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(6, 31);
            this.tsbConfigure.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbConfigure.Image = (Image)Resources.gear;
            this.tsbConfigure.ImageTransparentColor = Color.Magenta;
            this.tsbConfigure.Name = "tsbConfigure";
            this.tsbConfigure.Size = new Size(28, 28);
            this.tsbConfigure.Text = "Configure device for communication";
            this.tsbConfigure.Click += new EventHandler(this.tsbConfigure_Click);
            this.tsbExit.Alignment = ToolStripItemAlignment.Right;
            this.tsbExit.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbExit.Image = (Image)Resources.exit;
            this.tsbExit.ImageTransparentColor = Color.Magenta;
            this.tsbExit.Name = "tsbExit";
            this.tsbExit.Size = new Size(28, 28);
            this.tsbExit.Text = "Exit application";
            this.tsbExit.Click += new EventHandler(this.tsbExit_Click);
            this.tsbAbout.Alignment = ToolStripItemAlignment.Right;
            this.tsbAbout.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbAbout.Image = (Image)Resources.about;
            this.tsbAbout.ImageTransparentColor = Color.Magenta;
            this.tsbAbout.Name = "tsbAbout";
            this.tsbAbout.Size = new Size(28, 28);
            this.tsbAbout.Text = "About this application";
            this.tsbAbout.Click += new EventHandler(this.tsbAbout_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 31);
            this.tsbNetworkConfiguration.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbNetworkConfiguration.Image = (Image)Resources.node;
            this.tsbNetworkConfiguration.ImageTransparentColor = Color.Magenta;
            this.tsbNetworkConfiguration.Name = "tsbNetworkConfiguration";
            this.tsbNetworkConfiguration.Size = new Size(28, 28);
            this.tsbNetworkConfiguration.Text = "J1939 address claiming";
            this.tsbNetworkConfiguration.Click += new EventHandler(this.tsbAddressClaim_Click);
            this.spcContainer.Dock = DockStyle.Fill;
            this.spcContainer.Location = new Point(0, 31);
            this.spcContainer.Name = "spcContainer";
            this.spcContainer.Orientation = Orientation.Horizontal;
            this.spcContainer.Panel1.Controls.Add((Control)this.panel4);
            this.spcContainer.Panel1.Controls.Add((Control)this.panel1);
            this.spcContainer.Panel1.RightToLeft = RightToLeft.No;
            this.spcContainer.Panel1MinSize = 100;
            this.spcContainer.Panel2.Controls.Add((Control)this.panel5);
            this.spcContainer.Panel2.Controls.Add((Control)this.panel2);
            this.spcContainer.Panel2.RightToLeft = RightToLeft.No;
            this.spcContainer.Panel2MinSize = 100;
            this.spcContainer.Size = new Size(761, 350);
            this.spcContainer.SplitterDistance = 166;
            this.spcContainer.TabIndex = 8;
            this.panel4.BorderStyle = BorderStyle.FixedSingle;
            this.panel4.Controls.Add((Control)this.lstMessages);
            this.panel4.Dock = DockStyle.Fill;
            this.panel4.Location = new Point(30, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(731, 166);
            this.panel4.TabIndex = 30;
            this.lstMessages.BorderStyle = BorderStyle.None;
            this.lstMessages.Dock = DockStyle.Fill;
            this.lstMessages.FullRowSelect = true;
            this.lstMessages.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.lstMessages.HideSelection = false;
            this.lstMessages.Location = new Point(0, 0);
            this.lstMessages.MultiSelect = false;
            this.lstMessages.Name = "lstMessages";
            this.lstMessages.Size = new Size(729, 164);
            this.lstMessages.TabIndex = 29;
            this.lstMessages.UseCompatibleStateImageBehavior = false;
            this.lstMessages.View = View.Details;
            this.lstMessages.DoubleClick += new EventHandler(this.lstMessages_DoubleClick);
            this.panel1.BackColor = SystemColors.Control;
            this.panel1.Controls.Add((Control)this.tableLayoutPanel1);
            this.panel1.Dock = DockStyle.Left;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(30, 166);
            this.panel1.TabIndex = 0;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.Controls.Add((Control)this.pictureBox1, 0, 1);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel1.Size = new Size(30, 166);
            this.tableLayoutPanel1.TabIndex = 1;
            this.pictureBox1.Location = new Point(3, 31);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(24, 103);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.panel5.BorderStyle = BorderStyle.FixedSingle;
            this.panel5.Controls.Add((Control)this.button5);
            this.panel5.Controls.Add((Control)this.button4);
            this.panel5.Controls.Add((Control)this.button3);
            this.panel5.Controls.Add((Control)this.button2);
            this.panel5.Controls.Add((Control)this.button1);
            this.panel5.Controls.Add((Control)this.lstTxMessages);
            this.panel5.Controls.Add((Control)this.rtbCharOutput);
            this.panel5.Dock = DockStyle.Fill;
            this.panel5.Location = new Point(30, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new Size(731, 180);
            this.panel5.TabIndex = 5;
            this.button5.Location = new Point(439, 143);
            this.button5.Name = "button5";
            this.button5.Size = new Size(75, 23);
            this.button5.TabIndex = 35;
            this.button5.Text = "Test J1939";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Visible = false;
            this.button5.Click += new EventHandler(this.button5_Click);
            this.button4.Location = new Point(124, 143);
            this.button4.Name = "button4";
            this.button4.Size = new Size(135, 23);
            this.button4.TabIndex = 34;
            this.button4.Text = "Send Large ISO15765";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new EventHandler(this.button4_Click);
            this.button3.Location = new Point(5, 143);
            this.button3.Name = "button3";
            this.button3.Size = new Size(113, 23);
            this.button3.TabIndex = 33;
            this.button3.Text = "Send SF ISO15765";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new EventHandler(this.button3_Click);
            this.button2.Location = new Point(265, 143);
            this.button2.Name = "button2";
            this.button2.Size = new Size(168, 23);
            this.button2.TabIndex = 32;
            this.button2.Text = "Send P2P (PGN 0 - 1000 bytes)";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new EventHandler(this.button2_Click_1);
            this.button1.Location = new Point(520, 143);
            this.button1.Name = "button1";
            this.button1.Size = new Size(87, 23);
            this.button1.TabIndex = 31;
            this.button1.Text = "Send BAM (PGN 0 - 1000 bytes)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new EventHandler(this.button1_Click_1);
            this.lstTxMessages.BorderStyle = BorderStyle.None;
            this.lstTxMessages.ContextMenuStrip = this.cmsNewTxMsg;
            this.lstTxMessages.Dock = DockStyle.Fill;
            this.lstTxMessages.FullRowSelect = true;
            this.lstTxMessages.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.lstTxMessages.HideSelection = false;
            this.lstTxMessages.Location = new Point(0, 0);
            this.lstTxMessages.MultiSelect = false;
            this.lstTxMessages.Name = "lstTxMessages";
            this.lstTxMessages.Size = new Size(729, 130);
            this.lstTxMessages.TabIndex = 30;
            this.lstTxMessages.UseCompatibleStateImageBehavior = false;
            this.lstTxMessages.View = View.Details;
            this.lstTxMessages.DoubleClick += new EventHandler(this.lstTxMessages_DoubleClick);
            this.lstTxMessages.KeyDown += new KeyEventHandler(this.lstTxMessages_KeyDown);
            this.cmsNewTxMsg.Items.AddRange(new ToolStripItem[5]
            {
        (ToolStripItem) this.tsmiAdd,
        (ToolStripItem) this.tsmiEdit,
        (ToolStripItem) this.toolStripMenuItem1,
        (ToolStripItem) this.tsmiDelete,
        (ToolStripItem) this.tsmiClear
            });
            this.cmsNewTxMsg.Name = "cmsNewTxMsg";
            this.cmsNewTxMsg.Size = new Size(187, 98);
            this.cmsNewTxMsg.Opening += new CancelEventHandler(this.cmsNewTxMsg_Opening);
            this.tsmiAdd.Name = "tsmiAdd";
            this.tsmiAdd.ShortcutKeyDisplayString = "Ins";
            this.tsmiAdd.ShortcutKeys = Keys.Insert;
            this.tsmiAdd.Size = new Size(186, 22);
            this.tsmiAdd.Text = "New Message...";
            this.tsmiAdd.Click += new EventHandler(this.tsmiAdd_Click);
            this.tsmiEdit.DoubleClickEnabled = true;
            this.tsmiEdit.Name = "tsmiEdit";
            this.tsmiEdit.ShortcutKeyDisplayString = "Enter";
            this.tsmiEdit.Size = new Size(186, 22);
            this.tsmiEdit.Text = "Edit Message...";
            this.tsmiEdit.Click += new EventHandler(this.tsmiEdit_Click);
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new Size(183, 6);
            this.tsmiDelete.Name = "tsmiDelete";
            this.tsmiDelete.ShortcutKeys = Keys.Delete;
            this.tsmiDelete.Size = new Size(186, 22);
            this.tsmiDelete.Text = "Delete Message";
            this.tsmiDelete.Click += new EventHandler(this.tsmiDelete_Click);
            this.tsmiClear.Name = "tsmiClear";
            this.tsmiClear.ShortcutKeyDisplayString = "Shift + Del";
            this.tsmiClear.Size = new Size(186, 22);
            this.tsmiClear.Text = "Clear All";
            this.tsmiClear.Click += new EventHandler(this.tsmiClear_Click);
            this.rtbCharOutput.AutoWordSelection = true;
            this.rtbCharOutput.BackColor = SystemColors.Info;
            this.rtbCharOutput.BorderStyle = BorderStyle.None;
            this.rtbCharOutput.Dock = DockStyle.Bottom;
            this.rtbCharOutput.Location = new Point(0, 130);
            this.rtbCharOutput.Name = "rtbCharOutput";
            this.rtbCharOutput.ReadOnly = true;
            this.rtbCharOutput.Size = new Size(729, 48);
            this.rtbCharOutput.TabIndex = 2;
            this.rtbCharOutput.Text = "";
            this.rtbCharOutput.Visible = false;
            this.panel2.BackColor = SystemColors.Control;
            this.panel2.Controls.Add((Control)this.tableLayoutPanel2);
            this.panel2.Dock = DockStyle.Left;
            this.panel2.Location = new Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(30, 180);
            this.panel2.TabIndex = 1;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel2.Controls.Add((Control)this.pictureBox2, 0, 1);
            this.tableLayoutPanel2.Dock = DockStyle.Fill;
            this.tableLayoutPanel2.Location = new Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50.00001f));
            this.tableLayoutPanel2.Size = new Size(30, 180);
            this.tableLayoutPanel2.TabIndex = 0;
            this.pictureBox2.Location = new Point(3, 38);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new Size(24, 103);
            this.pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            this.tmrRead.Interval = 5_000;
            this.tmrRead.Tick += new EventHandler(this.tmrRead_Tick);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(761, 405);
            this.Controls.Add((Control)this.spcContainer);
            this.Controls.Add((Control)this.toolStrip1);
            this.Controls.Add((Control)this.statusStrip1);
            this.DoubleBuffered = true;
            this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            this.MinimumSize = new Size(666, 444);
            this.Name = nameof(Form1);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "PCAN-RP1210 - Test Application";
            this.FormClosing += new FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new EventHandler(this.Form1_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.spcContainer.Panel1.ResumeLayout(false);
            this.spcContainer.Panel2.ResumeLayout(false);
            this.spcContainer.EndInit();
            this.spcContainer.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((ISupportInitialize)this.pictureBox1).EndInit();
            this.panel5.ResumeLayout(false);
            this.cmsNewTxMsg.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((ISupportInitialize)this.pictureBox2).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private delegate void ReadDelegateHandler();

        [System.Flags]
        private enum MessagesToClear
        {
            Reception = 1,
            Transmission = 2,
        }
    }
}
