﻿
// Type: Peak.RP1210C.RP1210CError




namespace Peak.RP1210C
{
  public enum RP1210CError : short
  {
    NO_ERRORS = 0,
    ERR_DLL_NOT_INITIALIZED = 128, // 0x0080
    ERR_INVALID_CLIENT_ID = 129, // 0x0081
    ERR_CLIENT_ALREADY_CONNECTED = 130, // 0x0082
    ERR_CLIENT_AREA_FULL = 131, // 0x0083
    ERR_FREE_MEMORY = 132, // 0x0084
    ERR_NOT_ENOUGH_MEMORY = 133, // 0x0085
    ERR_INVALID_DEVICE = 134, // 0x0086
    ERR_DEVICE_IN_USE = 135, // 0x0087
    ERR_INVALID_PROTOCOL = 136, // 0x0088
    ERR_TX_QUEUE_FULL = 137, // 0x0089
    ERR_TX_QUEUE_CORRUPT = 138, // 0x008A
    ERR_RX_QUEUE_FULL = 139, // 0x008B
    ERR_RX_QUEUE_CORRUPT = 140, // 0x008C
    ERR_MESSAGE_TOO_LONG = 141, // 0x008D
    ERR_HARDWARE_NOT_RESPONDING = 142, // 0x008E
    ERR_COMMAND_NOT_SUPPORTED = 143, // 0x008F
    ERR_INVALID_COMMAND = 144, // 0x0090
    ERR_TXMESSAGE_STATUS = 145, // 0x0091
    ERR_ADDRESS_CLAIM_FAILED = 146, // 0x0092
    ERR_CANNOT_SET_PRIORITY = 147, // 0x0093
    ERR_CLIENT_DISCONNECTED = 148, // 0x0094
    ERR_CONNECT_NOT_ALLOWED = 149, // 0x0095
    ERR_CHANGE_MODE_FAILED = 150, // 0x0096
    ERR_BUS_OFF = 151, // 0x0097
    ERR_COULD_NOT_TX_ADDRESS_CLAIMED = 152, // 0x0098
    ERR_ADDRESS_LOST = 153, // 0x0099
    ERR_CODE_NOT_FOUND = 154, // 0x009A
    ERR_BLOCK_NOT_ALLOWED = 155, // 0x009B
    ERR_MULTIPLE_CLIENTS_CONNECTED = 156, // 0x009C
    ERR_ADDRESS_NEVER_CLAIMED = 157, // 0x009D
    ERR_WINDOW_HANDLE_REQUIRED = 158, // 0x009E
    ERR_MESSAGE_NOT_SENT = 159, // 0x009F
    ERR_MAX_NOTIFY_EXCEEDED = 160, // 0x00A0
    ERR_MAX_FILTERS_EXCEEDED = 161, // 0x00A1
    ERR_HARDWARE_STATUS_CHANGE = 162, // 0x00A2
    ERR_INVALID_PARAMETER = 193, // 0x00C1
    ERR_INVALID_LICENCE = 194, // 0x00C2
    ERR_INI_FILE_NOT_IN_WIN_DIR = 202, // 0x00CA
    ERR_INI_SECTION_NOT_FOUND = 204, // 0x00CC
    ERR_INI_KEY_NOT_FOUND = 205, // 0x00CD
    ERR_INVALID_KEY_STRING = 206, // 0x00CE
    ERR_DEVICE_NOT_SUPPORTED = 207, // 0x00CF
    ERR_INVALID_PORT_PARAM = 208, // 0x00D0
    ERR_COMMAND_TIMED_OUT = 213, // 0x00D5
    ERR_OS_NOT_SUPPORTED = 220, // 0x00DC
    ERR_COMMAND_QUEUE_IS_FULL = 222, // 0x00DE
    ERR_CANNOT_SET_CAN_BAUDRATE = 224, // 0x00E0
    ERR_CANNOT_CLAIM_BROADCAST_ADDRESS = 225, // 0x00E1
    ERR_OUT_OF_ADDRESS_RESOURCES = 226, // 0x00E2
    ERR_ADDRESS_RELEASE_FAILED = 227, // 0x00E3
    ERR_COMM_DEVICE_IN_USE = 230, // 0x00E6
    ERR_DATA_LINK_CONFLICT = 441, // 0x01B9
    ERR_ADAPTER_NOT_RESPONDING = 453, // 0x01C5
    ERR_CAN_BAUD_SET_NONSTANDARD = 454, // 0x01C6
    ERR_MULTIPLE_CONNECTIONS_NOT_ALLOWED_NOW = 455, // 0x01C7
    ERR_J1708_BAUD_SET_NONSTANDARD = 456, // 0x01C8
    ERR_J1939_BAUD_SET_NONSTANDARD = 457, // 0x01C9
    ERR_ISO15765_BAUD_SET_NONSTANDARD = 458, // 0x01CA
  }
}
