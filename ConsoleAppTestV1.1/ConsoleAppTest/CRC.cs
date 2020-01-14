using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppTest
{
    public class CRC
    {
        public static void ModRTU_CRC(byte[] command_com, int length, out byte CRCHigh, out byte CRCLow)
        {
            ushort CRCFull = 0xFFFF;

            for (int pos = 0; pos < length; pos++)
            {
                CRCFull ^= (ushort)command_com[pos];

                for (int i = 8; i != 0; i--)
                {
                    if ((CRCFull & 0x0001) != 0)
                    {
                        CRCFull >>= 1;
                        CRCFull ^= 0xA001;
                    }
                    else
                        CRCFull >>= 1;
                }
            }
            CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRCLow = (byte)(CRCFull & 0xFF);
        }
    }
}
