using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NESEmu.EmuNES
{
    public class CPU
    {
        //Instructions based on http://www.obelisk.me.uk/6502/reference.html
        public byte a; //Accumulator
        public byte x; //X Register
        public byte y; //Y Register
        public ushort pc; // Program Counter
        public byte s; //Stack Pointer
        public Flags p = new Flags(); //Processor Status
        public Bus bus;
        public int cyclesLeft;

        public CPU(Bus _bus)
        {
            bus = _bus;
        }

        public void clock()
        {
            if (cyclesLeft != 0)
            {
                cyclesLeft--;
            }
            else
            {
                byte opcode = bus.Read(pc);
                pc++;
                switch (opcode)
                {
                    case 0xA1:
                        cyclesLeft = 6;
                        LDA(IZY());
                        break;
                    case 0xA5:
                        cyclesLeft = 3;
                        LDA(ZeroPage());
                        break;
                    case 0xA9:
                        cyclesLeft = 2;
                        LDA(Immediate());
                        break;
                    case 0xAD:
                        cyclesLeft = 4;
                        LDA(Absolute());
                        break;
                    case 0xB5:
                        cyclesLeft = 4;
                        LDA(ZeroPageX());
                        break;
                    case 0xB9:
                        cyclesLeft = 4;
                        LDA(AbsoluteY());
                        break;
                    case 0xBD:
                        cyclesLeft = 4;
                        LDA(AbsoluteX());
                        break;
                }
            }
        }

        #region Adressing Modes
        byte Immediate()
        {
            return bus.Read(pc);
        }

        byte ZeroPage()
        {
            byte adress = bus.Read(pc);
            return bus.Read(adress);
        }

        byte ZeroPageX()
        {
            byte adress = bus.Read((ushort)(pc + x));
            return bus.Read(adress);
        }

        byte Absolute()
        {
            byte pcl = bus.Read(pc);
            pc++;
            byte pch = bus.Read(pc);
            pc++;
            return bus.Read(BitConverter.ToUInt16(new byte[]{ pch ,pcl}));
        }

        byte AbsoluteX()
        {
            byte pcl = bus.Read(pc);
            int pageIndex = pc / 256;
            pc++;
            byte pch = bus.Read(pc);
            pc++;
            if(pc/256 != pageIndex)
            {
                cyclesLeft++;
            }

            return bus.Read((ushort)(BitConverter.ToUInt16(new byte[] { pch, pcl }) + x));
        }

        byte AbsoluteY()
        {
            byte pcl = bus.Read(pc);
            int pageIndex = pc / 256;
            pc++;
            byte pch = bus.Read(pc);
            pc++;
            if (pc / 256 != pageIndex)
            {
                cyclesLeft++;
            }

            return bus.Read((ushort)(BitConverter.ToUInt16(new byte[] { pch, pcl }) + y));
        }

        byte IZY()
        {
            ushort t = bus.Read(pc);
            pc++;

            byte pcl = bus.Read((ushort)(t & 0x00FF));

            int pageIndex = pc / 256;

            byte pch = bus.Read((ushort)((t + 1) & 0x00FF));
            ushort adress = BitConverter.ToUInt16(new byte[] { pch, pcl });
            if (adress / 256 != (adress + y)/256)
            {
                cyclesLeft++;
            }
            
            return bus.Read((ushort)(adress  + y));
        }
        #endregion

        #region Instructions
        /// <summary>
        /// Load to A(Accumalator)
        /// </summary>
        /// <param name="adress"></param>
        void LDA(byte value)
        {
            a = value;

            p.Z = (a==0);
            p.N = new BitArray(a)[7];
        }
        #endregion
    }



    public struct Flags
    {
        public bool N { get; set; }//Negative Flag
        public bool V { get; set; } //Overflow Flag
        public bool B { get; set; }//Break Flag
        public bool D { get; set; } //Decimal Mode Flag
        public bool I { get; set; } //Interrupt disable flag
        public bool Z { get; set; } //Zero Flag
        public bool C { get; set; } //Carry Flag
    }
}
