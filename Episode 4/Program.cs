using System.IO.Ports;
using System.Text;

SerialPort sp = new SerialPort (@"/dev/ttyAMA0");
sp.Encoding = Encoding.UTF8;
sp.BaudRate = 921600;
sp.Handshake = Handshake.None;
sp.ReadTimeout = 1000;
sp.WriteTimeout = 1000;
sp.Open();

int State = 0;
int SystemTime = 0;
int Distance = 0;
int Status = 0;
int Value = 0;
int Checksum = 0;
int SystemTimeShift = 0;
int DistanceShift = 0;
string DebugHex = "";

while (true) {
    if (State == 0)
        DebugHex = "";

    try {
        Value = sp.ReadByte ();
        DebugHex = DebugHex + Value.ToString ("X2") + " ";
    }
    catch (TimeoutException) {

    }

    // https://www.waveshare.com/wiki/TOF_Laser_Range_Sensor
    // Example: 57 00 ff 00 9e 8f 00 00 ad 08 00 00 03 00 ff 3a

    switch (State++) {
        case 0: // Frame Header - Waiting for 0x57
            if (Value != 0x57)
                State = 0;
            else {
                SystemTime = 0;
                Distance = 0;
                Status = 0;
                SystemTimeShift = 0;
                DistanceShift = 0;
                Checksum = 0;
            }
            break;
        case 1: // Function Mark - Should be 0x00, if not reset state machine
            if (Value != 0x00)
                State = 0;
            break;
        case 2: // Reserved - Should be 0xFF
            if (Value != 0xFF)
                State = 0;
            break;
        case 3: // ID - default 0
             break;
        case 4: // System Time - (4 bytes), eg 9e 8f 00 00 = 36766ms = 00 00 8F 9E
        case 5:
        case 6:
        case 7:
            SystemTime += Value << SystemTimeShift;
            SystemTimeShift += 8;
            break;
        case 8: // Distance - (3 bytes), ad 08 00 = 2221mm = 00 08 AD
        case 9:
        case 10:
            Distance += Value << DistanceShift;
            DistanceShift += 8;
            break;
        case 11: // Status
            Status = Value;
            break;
        case 12: // Signal Strength (2 bytes)
        case 13:
            break;
        case 14: // Reserved
            break;
        case 15: // Sum Check - Checksum (Previous bytes added)
            if ((Checksum & 0xFF) == Value) {   // Good checksum ?
                if (Status == 0) {              // Have a reading ?
                    Console.WriteLine ($"{DebugHex} - Distance is {Distance:D4}");
                }
            }
            State = 0;
            break;

        default:
            break;
    }
    Checksum += Value;  // Keep this running..
}