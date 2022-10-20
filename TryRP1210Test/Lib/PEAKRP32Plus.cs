using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Peak.RP1210C
{
    public class PEAKRP32Plus : IDisposable
    {
        public bool RP1210_A_IsSupported;
        public bool RP1210_B_IsSupported;
        public bool RP1210_C_IsSupported;


        public const string MODULE_NAME = "PEAKRP32";
        public const string INI_NAME = "PEAKRP32.ini";
        public const string DLL_NAME = "PEAKRP32.dll";
        public const string VENDOR_SECTION = "VendorInformation";
        public const string DEV_INF_SECTION = "DeviceInformation";
        public const string PTC_INF_SECTION = "ProtocolInformation";
        public const string CAN_STRING = "CAN";
        public const string J1939_STRING = "J1939";
        public const string ISO1576_STRING = "ISO15765";
        public const int NULL_WINDOW = 0;
        public const byte BLOCKING_IO = 1;
        public const byte NON_BLOCKING_IO = 0;
        public const byte FILTER_INCLUSIVE = 0;
        public const byte FILTER_EXCLUSIVE = 1;
        public const byte ECHO_ON = 1;
        public const byte ECHO_OFF = 0;
        public const byte RECEIVE_ON = 1;
        public const byte RECEIVE_OFF = 0;
        public const byte STANDARD_CAN = 0;
        public const byte EXTENDED_CAN = 1;
        public const byte BUSOFF_RESET_ON = 1;
        public const byte BUSOFF_RESET_OFF = 0;
        public const byte BLOCK_UNTIL_DONE = 0;
        public const byte RETURN_BEFORE_COMPLETION = 2;
        public const byte CHANGE_BAUD_NOW = 0;
        public const byte MSG_FIRST_CHANGE_BAUD = 1;
        public const byte RP1210_BAUD_125k = 4;
        public const byte RP1210_BAUD_250k = 5;
        public const byte RP1210_BAUD_500k = 6;
        public const byte RP1210_BAUD_1000k = 7;
        public const byte ADD_LIST = 1;
        public const byte VIEW_B_LIST = 2;
        public const byte DESTROY_LIST = 3;
        public const byte REMOVE_ENTRY = 4;
        public const byte LIST_LENGTH = 5;
        public const byte J1939_NULL_ADDR = 254;
        public const byte J1939_GLOBAL_ADDR = 255;
        public const byte J1939_NULL_BYTE = 255;
        public const byte J1939_PACKS_UNLIMITED = 255;
        public const byte J1939_CM_RTS = 16;
        public const byte J1939_CM_CTS = 17;
        public const byte J1939_CM_EOM_ACK = 19;
        public const byte J1939_CM_CA = 255;
        public const byte J1939_CM_BAM = 32;
        public const ushort J1939_PGN_CONN_MGMENT = 60416;
        public const ushort J1939_PGN_DATA_TRANSFER = 60160;
        public const ushort J1939_PGN_REQUEST = 59904;
        public const ushort J1939_PGN_ADDR_CLAIM = 60928;
        public const ushort J1939_PGN_PROPRIETARY_A = 61184;
        private bool m_WasLoaded;
        private IntPtr m_hModule;
        private string m_LibName;
        private PEAKRP32Plus.ImplementationLoadLevel m_Implementation;
        private PEAKRP32Plus.ConnectDelegate m_PtrConnect;
        private PEAKRP32Plus.DisconnectDelegate m_PtrDisconnect;
        private PEAKRP32Plus.SendMsgDelegate m_PtrSendMsg;
        private PEAKRP32Plus.ReadMsgeDelegate m_PtrReadMsg;
        private PEAKRP32Plus.SendCmdDelegate m_PtrSendCmd;
        private PEAKRP32Plus.ReadVersionDelegate m_PtrReadVer;
        private PEAKRP32Plus.GetErrorMsgDelegate m_PtrGetErrMsg;
        private PEAKRP32Plus.GetHwStsDelegate m_PtrGetHwSts;
        private PEAKRP32Plus.GetLastErrorMsgDelegate m_PtrGetLastErrMsg;
        private PEAKRP32Plus.ReadVersionExDelegate m_PtrReadVerX;
        private PEAKRP32Plus.IoctlDelegate m_PtrIoctl;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        public PEAKRP32Plus()
          : this(Environment.Is64BitProcess ? "PCANRP64" : "PCANRP32")
        {
        }

        public PEAKRP32Plus(string implementationName)
        {
            this.m_Implementation = PEAKRP32Plus.ImplementationLoadLevel.None;
            this.m_hModule = IntPtr.Zero;
            this.m_WasLoaded = false;
            this.LoadApi(implementationName);
        }

        ~PEAKRP32Plus() => this.Dispose(false);

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            int num = disposing ? 1 : 0;
            this.UnloadApi();
        }

        private void LoadApi(string libName)
        {
            this.UnloadApi();
            this.m_LibName = libName;
            this.m_WasLoaded = this.LoadDllHandle() && this.LoadFunctions();


        }

        private bool LoadFunctions()
        {
            this.m_Implementation = this.LoadFunctionsVersionA() ? PEAKRP32Plus.ImplementationLoadLevel.A : PEAKRP32Plus.ImplementationLoadLevel.None;
            this.m_Implementation |= this.LoadFunctionsVersionB() ? PEAKRP32Plus.ImplementationLoadLevel.B : PEAKRP32Plus.ImplementationLoadLevel.None;
            this.m_Implementation |= this.LoadFunctionsVersionC() ? PEAKRP32Plus.ImplementationLoadLevel.C : PEAKRP32Plus.ImplementationLoadLevel.None;

            return (this.m_Implementation & PEAKRP32Plus.ImplementationLoadLevel.A) == PEAKRP32Plus.ImplementationLoadLevel.A;
        }

        private bool LoadFunctionsVersionC()
        {
            try
            {
                this.m_PtrIoctl = (PEAKRP32Plus.IoctlDelegate)this.GetFunction("RP1210_Ioctl", typeof(PEAKRP32Plus.IoctlDelegate));
                RP1210_C_IsSupported = this.m_PtrIoctl != null;
                return RP1210_C_IsSupported;
            }
            catch
            {
                return false;
            }
        }

        private bool LoadFunctionsVersionB()
        {
            try
            {
                this.m_PtrGetLastErrMsg = (PEAKRP32Plus.GetLastErrorMsgDelegate)this.GetFunction("RP1210_GetLastErrorMsg", typeof(PEAKRP32Plus.GetLastErrorMsgDelegate));
                this.m_PtrReadVerX = (PEAKRP32Plus.ReadVersionExDelegate)this.GetFunction("RP1210_ReadDetailedVersion", typeof(PEAKRP32Plus.ReadVersionExDelegate));

                RP1210_B_IsSupported = this.m_PtrGetLastErrMsg != null && this.m_PtrReadVerX != null;

                return RP1210_B_IsSupported;
            }
            catch
            {
                return false;
            }
        }

        private bool LoadFunctionsVersionA()
        {
            try
            {
                //this.m_PtrConnect = (PEAKRP32Plus.ConnectDelegate) this.GetFunction("RP1210_ClientConnect", typeof (PEAKRP32Plus.ConnectDelegate));

                PEAKRP32Plus.ConnectDelegate clientConnectDelegate = (PEAKRP32Plus.ConnectDelegate)this.GetFunction("RP1210_ClientConnect", typeof(PEAKRP32Plus.ConnectDelegate));

                this.m_PtrConnect = clientConnectDelegate;


                this.m_PtrDisconnect = (PEAKRP32Plus.DisconnectDelegate)this.GetFunction("RP1210_ClientDisconnect", typeof(PEAKRP32Plus.DisconnectDelegate));
                this.m_PtrSendMsg = (PEAKRP32Plus.SendMsgDelegate)this.GetFunction("RP1210_SendMessage", typeof(PEAKRP32Plus.SendMsgDelegate));
                this.m_PtrReadMsg = (PEAKRP32Plus.ReadMsgeDelegate)this.GetFunction("RP1210_ReadMessage", typeof(PEAKRP32Plus.ReadMsgeDelegate));
                this.m_PtrSendCmd = (PEAKRP32Plus.SendCmdDelegate)this.GetFunction("RP1210_SendCommand", typeof(PEAKRP32Plus.SendCmdDelegate));
                this.m_PtrReadVer = (PEAKRP32Plus.ReadVersionDelegate)this.GetFunction("RP1210_ReadVersion", typeof(PEAKRP32Plus.ReadVersionDelegate));
                this.m_PtrGetErrMsg = (PEAKRP32Plus.GetErrorMsgDelegate)this.GetFunction("RP1210_GetErrorMsg", typeof(PEAKRP32Plus.GetErrorMsgDelegate));
                this.m_PtrGetHwSts = (PEAKRP32Plus.GetHwStsDelegate)this.GetFunction("RP1210_GetHardwareStatus", typeof(PEAKRP32Plus.GetHwStsDelegate));

                RP1210_A_IsSupported = this.m_PtrConnect != null && this.m_PtrDisconnect != null && (this.m_PtrSendMsg != null && this.m_PtrReadMsg != null) && (this.m_PtrSendCmd != null && this.m_PtrReadVer != null && this.m_PtrGetErrMsg != null) && this.m_PtrGetHwSts != null;

                return RP1210_A_IsSupported;
            }
            catch
            {
                return false;
            }
        }

        private Delegate GetFunction(string functionName, Type delegateType)
        {
            IntPtr procAddress = PEAKRP32Plus.GetProcAddress(this.m_hModule, functionName);
            return !(procAddress != IntPtr.Zero) ? (Delegate)null : Marshal.GetDelegateForFunctionPointer(procAddress, delegateType);
        }

        private void UnloadApi()
        {
            if (this.m_hModule != IntPtr.Zero)
                PEAKRP32Plus.FreeLibrary(this.m_hModule);
            this.m_hModule = IntPtr.Zero;
            this.m_WasLoaded = false;
            this.m_Implementation = PEAKRP32Plus.ImplementationLoadLevel.None;
        }

        private bool LoadDllHandle()
        {
            if (this.m_WasLoaded)
                return true;
            this.m_hModule = PEAKRP32Plus.LoadLibrary(this.m_LibName);
            return this.m_hModule != IntPtr.Zero;
        }

        public bool LoadImplementation(string implementationName)
        {
            this.UnloadApi();
            this.LoadApi(implementationName);
            return this.m_WasLoaded;
        }

        public short ClientConnect(
          int hwndClient,
          ushort nDeviceID,
          string fpchProtocol,
          int lTxBufferSize,
          int lRcvBufferSize,
          short nIsAppPacketizingIncomingMsgs)
        {
            if (!this.m_WasLoaded || (this.m_Implementation & PEAKRP32Plus.ImplementationLoadLevel.A) !=
                PEAKRP32Plus.ImplementationLoadLevel.A)
                return (short)128;
            else
                return this.m_PtrConnect(hwndClient, nDeviceID, fpchProtocol, lTxBufferSize, lRcvBufferSize,
                    nIsAppPacketizingIncomingMsgs);
        }

        public short ClientDisconnect(short nClientID) => !this.m_WasLoaded || (this.m_Implementation & PEAKRP32Plus.ImplementationLoadLevel.A) != PEAKRP32Plus.ImplementationLoadLevel.A ? (short)128 : this.m_PtrDisconnect(nClientID);

        public short SendMessage(
          short nClientID,
          byte[] fpchClientMessage,
          short nNotifyStatusOnTx,
          short nBlockOnSend)
        {
            return !this.m_WasLoaded || (this.m_Implementation & PEAKRP32Plus.ImplementationLoadLevel.A) != PEAKRP32Plus.ImplementationLoadLevel.A ? (short)128 : this.m_PtrSendMsg(nClientID, fpchClientMessage, fpchClientMessage != null ? (short)fpchClientMessage.Length : (short)0, nNotifyStatusOnTx, nBlockOnSend);
        }

        public short ReadMessage(short nClientID, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] fpchAPIMessage, short nBlockOnRead) => !this.m_WasLoaded || (this.m_Implementation & PEAKRP32Plus.ImplementationLoadLevel.A) != PEAKRP32Plus.ImplementationLoadLevel.A ? (short)128 : this.m_PtrReadMsg(nClientID, fpchAPIMessage, fpchAPIMessage != null ? (short)fpchAPIMessage.Length : (short)0, nBlockOnRead);

        public short SendCommand(RP1210CCommand nCommandNumber, short nClientID, byte[] fpchClientCommand)
        {
            //return !this.m_WasLoaded || (this.m_Implementation & PEAKRP32Plus.ImplementationLoadLevel.A) != PEAKRP32Plus.ImplementationLoadLevel.A ? (short)128 : this.m_PtrSendCmd(nCommandNumber, nClientID, fpchClientCommand, fpchClientCommand != null ? (short)fpchClientCommand.Length : (short)0);


            if (!this.m_WasLoaded || (this.m_Implementation & PEAKRP32Plus.ImplementationLoadLevel.A) != PEAKRP32Plus.ImplementationLoadLevel.A)
            {
                return (short)128;
            }
            else
            {
                var hasCommandData = fpchClientCommand != null;
                var commandReturn = this.m_PtrSendCmd(nCommandNumber
                    , nClientID, fpchClientCommand, hasCommandData ? (short)fpchClientCommand.Length : (short)0);

                return commandReturn;
            }
        }

        public void ReadVersion(
          StringBuilder fpchDLLMajorVersion,
          StringBuilder fpchDLLMinorVersion,
          StringBuilder fpchAPIMajorVersion,
          StringBuilder fpchAPIMinorVersion)
        {
            if (!this.m_WasLoaded || (this.m_Implementation & PEAKRP32Plus.ImplementationLoadLevel.A) != PEAKRP32Plus.ImplementationLoadLevel.A)
                return;
            int num = (int)this.m_PtrReadVer(fpchDLLMajorVersion, fpchDLLMinorVersion, fpchAPIMajorVersion, fpchAPIMinorVersion);
        }

        public short GetErrorMsg(RP1210CError ErrorCode, StringBuilder fpchDescription) => !this.m_WasLoaded || (this.m_Implementation & PEAKRP32Plus.ImplementationLoadLevel.A) != PEAKRP32Plus.ImplementationLoadLevel.A ? (short)128 : this.m_PtrGetErrMsg(ErrorCode, fpchDescription);

        public short GetHardwareStatus(short nClientID, byte[] fpchClientInfo, short nBlockOnRequest) => !this.m_WasLoaded || (this.m_Implementation & PEAKRP32Plus.ImplementationLoadLevel.A) != PEAKRP32Plus.ImplementationLoadLevel.A ? (short)128 : this.m_PtrGetHwSts(nClientID, fpchClientInfo, fpchClientInfo != null ? (short)fpchClientInfo.Length : (short)0, nBlockOnRequest);

        public short GetLastErrorMsg(
          short ErrorCode,
          ref int SubErrorCode,
          StringBuilder fpchDescription,
          short nClientID)
        {
            if (!this.m_WasLoaded)
                return 128;
            return this.m_Implementation <= PEAKRP32Plus.ImplementationLoadLevel.A ? (short)144 : this.m_PtrGetLastErrMsg(ErrorCode, ref SubErrorCode, fpchDescription, nClientID);
        }

        public short ReadDetailedVersion(
          short nClientID,
          StringBuilder fpchAPIVersionInfo,
          StringBuilder fpchDLLVersionInfo,
          StringBuilder fpchFWVersionInfo)
        {
            if (!this.m_WasLoaded)
                return 128;
            return this.m_Implementation <= PEAKRP32Plus.ImplementationLoadLevel.A ? (short)144 : this.m_PtrReadVerX(nClientID, fpchAPIVersionInfo, fpchDLLVersionInfo, fpchFWVersionInfo);
        }

        public short Ioctl(short nClientID, int nIoctlID, IntPtr pInput, IntPtr pOutput)
        {
            if (!this.m_WasLoaded)
                return 128;
            return this.m_Implementation != PEAKRP32Plus.ImplementationLoadLevel.C ? (short)144 : this.m_PtrIoctl(nClientID, nIoctlID, pInput, pOutput);
        }

        public bool Loaded => this.m_WasLoaded;

        public string ImplementationName
        {
            get => this.m_LibName;
            set => this.LoadApi(value);
        }

        public PEAKRP32Plus.ImplementationLoadLevel ImplementationLevel => this.m_Implementation;

        private delegate short ConnectDelegate(
          int hwndClient,
          ushort nDeviceID,
          string fpchProtocol,
          int lTxBufferSize,
          int lRcvBufferSize,
          short nIsAppPacketizingIncomingMsgs);

        private delegate short DisconnectDelegate(short nClientID);

        private delegate short SendMsgDelegate(
          short nClientID,
          [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] fpchClientMessage,
          short nMessageSize,
          short nNotifyStatusOnTx,
          short nBlockOnSend);

        private delegate short ReadMsgeDelegate(
          short nClientID,
          [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] fpchAPIMessage,
          short nBufferSize,
          short nBlockOnRead);

        private delegate short SendCmdDelegate(
          [MarshalAs(UnmanagedType.I2)] RP1210CCommand nCommandNumber,
          short nClientID,
          [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3), In, Out] byte[] fpchClientCommand,
          short nMessageSize);

        private delegate short ReadVersionDelegate(
          [MarshalAs(UnmanagedType.LPStr)] StringBuilder fpchDLLMajorVersion,
          [MarshalAs(UnmanagedType.LPStr)] StringBuilder fpchDLLMinorVersion,
          [MarshalAs(UnmanagedType.LPStr)] StringBuilder fpchAPIMajorVersion,
          [MarshalAs(UnmanagedType.LPStr)] StringBuilder fpchAPIMinorVersion);

        private delegate short GetErrorMsgDelegate(
          [MarshalAs(UnmanagedType.U2)] RP1210CError ErrorCode,
          [MarshalAs(UnmanagedType.LPStr)] StringBuilder fpchDescription);

        private delegate short GetHwStsDelegate(
          short nClientID,
          [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] fpchClientInfo,
          short nInfoSize,
          short nBlockOnRequest);

        private delegate short GetLastErrorMsgDelegate(
          short ErrorCode,
          ref int SubErrorCode,
          [MarshalAs(UnmanagedType.LPStr)] StringBuilder fpchDescription,
          short nClientID);

        private delegate short ReadVersionExDelegate(
          short nClientID,
          [MarshalAs(UnmanagedType.LPStr)] StringBuilder fpchAPIVersionInfo,
          [MarshalAs(UnmanagedType.LPStr)] StringBuilder fpchDLLVersionInfo,
          [MarshalAs(UnmanagedType.LPStr)] StringBuilder fpchFWVersionInfo);

        private delegate short IoctlDelegate(
          short nClientID,
          int nIoctlID,
          IntPtr pInput,
          IntPtr pOutput);

        [Flags]
        public enum ImplementationLoadLevel
        {
            None = 0,
            A = 1,
            B = 2,
            C = 4,
        }
    }
}
