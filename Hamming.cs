using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST_diplom
{
    class Hamming
    {
        const int lengthFact = 5040; // = 7!
        const int lengthCode = 7;

        public static List<byte> To(List<byte> data)
        {
            int bitsLen = data.Count * 14;
            if (bitsLen % 8 != 0)
                bitsLen += 8 - bitsLen % 8;

            BitArray bitArray = new BitArray(bitsLen);

            int pointer = 0;
            // byte8 -> 2 byte7
            foreach (var x in data)
            {
                byte b7 = toHamming(x);
                for (int i = 0; i < 7; ++i)
                    bitArray.Set(pointer + i, (b7 & (1 << i)) != 0);
                pointer += 7;

                b7 = toHamming((byte)(x >> 4));
                for (int i = 0; i < 7; ++i)
                    bitArray.Set(pointer + i, (b7 & (1 << i)) != 0);
                pointer += 7;
            }

            List<byte> newData = new List<byte>(bitArray.Count / 8);
            for (int i = 0; i < bitArray.Count / 8; ++i)
            {
                byte b = 0;
                for (int j = 0; j < 8; ++j)
                    b |= (byte)((1 & (bitArray.Get(i * 8 + j) ? 1 : 0)) << j);
                newData.Add(b);
            }

            return newData;
        }

        public static List<byte> From(List<byte> data)
        {
            BitArray bitArray = new BitArray(data.Count * 8);

            int pointer = 0;
            foreach (var x in data)
            {
                for (int i = 0; i < 8; ++i)
                    bitArray.Set(pointer + i, (x & (1 << i)) != 0);
                pointer += 8;
            }

            List<byte> newData = new List<byte>(bitArray.Count / 14);

            // 2 byte7 -> byte8
            for (int i = 0; i < bitArray.Count / 14; ++i)
            {
                byte b = 0, resB = 0;
                for (int j = 0; j < 7; ++j)
                    b |= (byte)((1 & (bitArray.Get(i * 14 + j) ? 1 : 0)) << j);

                resB = fromHamming(b);

                b = 0;
                for (int j = 0; j < 7; ++j)
                    b |= (byte)((1 & (bitArray.Get(i * 14 + 7 + j) ? 1 : 0)) << j);

                resB |= (byte)(fromHamming(b) << 4);

                newData.Add(resB);
            }

            return newData;
        }

        // 0000XXXX -> 0YYYYYYY
        private static byte toHamming(byte byteMsg)
        {
            int info1 = (getBit(byteMsg, 0) + getBit(byteMsg, 1) + getBit(byteMsg, 3)) % 2;
            int info2 = (getBit(byteMsg, 0) + getBit(byteMsg, 2) + getBit(byteMsg, 3)) % 2;
            info2 = info2 << 1;
            int info3 = (getBit(byteMsg, 1) + getBit(byteMsg, 2) + getBit(byteMsg, 3)) % 2;
            info3 = info3 << 3;
            byte newMsg = (byte)(((byteMsg >> 1) << 4) + info1 + info2 + info3 + (getBit(byteMsg, 0) << 2));
            return newMsg;
        }

        // 0YYYYYYY -> 0000XXXX
        private static byte fromHamming(byte byteMsg)
        {
            int errSynd1 = (getBit(byteMsg, 0) + getBit(byteMsg, 2) + getBit(byteMsg, 4) + getBit(byteMsg, 6)) % 2;
            int errSynd2 = (getBit(byteMsg, 1) + getBit(byteMsg, 2) + getBit(byteMsg, 5) + getBit(byteMsg, 6)) % 2;
            int errSynd3 = (getBit(byteMsg, 3) + getBit(byteMsg, 4) + getBit(byteMsg, 5) + getBit(byteMsg, 6)) % 2;
            int errSynd = (errSynd1 << 2) + (errSynd2 << 1) + errSynd3;

            byte infoMsg = byteMsg;
            if (errSynd != 0)
            {
                byte xorByte = (byte)(1 << errSynd);
                infoMsg = (byte)(infoMsg ^ xorByte);
            }
            infoMsg = (byte)(getBit(infoMsg, 2) + (getBit(infoMsg, 4) << 1) + (getBit(infoMsg, 5) << 2) + (getBit(infoMsg, 6) << 3));
            return infoMsg;
        }

        private static int getBit(byte inByte, int pos)
        {
            return ((1 << pos) & inByte) != 0 ? 1 : 0;
        }
    }
}
