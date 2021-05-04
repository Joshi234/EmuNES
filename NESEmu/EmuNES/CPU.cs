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
                    case 0x21:
                        cyclesLeft = 6;
                        AND(bus.Read(IZX()));
                        break;
                    case 0x25:
                        cyclesLeft = 3;
                        AND(bus.Read(ZeroPage()));
                        break;
                    case 0x29:
                        cyclesLeft = 2;
                        AND(bus.Read(Immediate()));
                        break;
                    case 0x2D:
                        cyclesLeft = 4;
                        AND(bus.Read(Absolute()));
                        break;
                    case 0x31:
                        cyclesLeft = 5;
                        AND(bus.Read(IZY()));
                        break;
                    case 0x35:
                        cyclesLeft = 4;
                        AND(bus.Read(ZeroPageX()));
                        break;
                    case 0x39:
                        cyclesLeft = 4;
                        AND(bus.Read(AbsoluteY()));
                        break;
                    case 0x3D:
                        cyclesLeft = 4;
                        AND(bus.Read(AbsoluteX()));
                        break;
                    case 0x4C:
                        cyclesLeft = 3;
                        JMP(Absolute());
                        break;
                    case 0x6C:
                        cyclesLeft = 5;
                        JMP(Indirect());
                        break;
                    case 0x81:
                        cyclesLeft = 6;
                        STA(IZX());
                        break;
                    case 0x84:
                        cyclesLeft = 3;
                        STY(ZeroPage());
                        break;
                    case 0x85:
                        cyclesLeft = 3;
                        STA(ZeroPage());
                        break;
                    case 0x86:
                        cyclesLeft = 3;
                        STX(ZeroPageY());
                        break;
                    case 0x8C:
                        cyclesLeft = 4;
                        STY(Absolute());
                        break;
                    case 0x8D:
                        cyclesLeft = 4;
                        STA(Absolute());
                        break;
                    case 0x8E:
                        cyclesLeft = 4;
                        STX(Absolute());
                        break;
                    case 0x91:
                        cyclesLeft = 6;
                        STA(IZY());
                        break;
                    case 0x94:
                        cyclesLeft = 4;
                        STY(ZeroPageX());
                        break;
                    case 0x95:
                        cyclesLeft = 4;
                        STA(ZeroPageX());
                        break;
                    case 0x96:
                        cyclesLeft = 4;
                        STX(ZeroPageY());
                        break;
                    case 0x99:
                        cyclesLeft = 5;
                        STA(AbsoluteY());
                        break;
                    case 0x9D:
                        cyclesLeft = 5;
                        STA(AbsoluteX());
                        break;
                    case 0xA0:
                        cyclesLeft = 2;
                        LDY(bus.Read(Immediate()));
                        break;
                    case 0xA1:
                        cyclesLeft = 3;
                        LDA(bus.Read(IZX()));
                        break;
                    case 0xA2:
                        cyclesLeft = 2;
                        LDX(bus.Read(Immediate()));
                        break;
                    case 0xA4:
                        cyclesLeft = 3;
                        LDX(bus.Read(ZeroPage()));
                        break;
                    case 0xA5:
                        cyclesLeft = 3;
                        LDA(bus.Read(ZeroPage()));
                        break;
                    case 0xA6:
                        cyclesLeft = 3;
                        LDX(bus.Read(Immediate()));
                        break;
                    case 0xA9:
                        cyclesLeft = 2;
                        LDA(bus.Read(Immediate()));
                        break;
                    case 0xAC:
                        cyclesLeft = 4;
                        LDY(bus.Read(Absolute()));
                        break;
                    case 0xAD:
                        cyclesLeft = 4;
                        LDA(bus.Read(Absolute()));
                        break;
                    case 0xAE:
                        cyclesLeft = 4;
                        LDX(bus.Read(Absolute()));
                        break;
                    case 0xB1:
                        cyclesLeft = 6;
                        LDA(bus.Read(IZY()));
                        break;
                    case 0xB4:
                        cyclesLeft = 4;
                        LDY(bus.Read(ZeroPageX()));
                        break;
                    case 0xB5:
                        cyclesLeft = 4;
                        LDA(bus.Read(ZeroPageX()));
                        break;
                    case 0xB6:
                        cyclesLeft = 4;
                        LDX(bus.Read(ZeroPageY()));
                        break;
                    case 0xB9:
                        cyclesLeft = 4;
                        LDA(bus.Read(AbsoluteY()));
                        break;
                    case 0xBC:
                        cyclesLeft = 4;
                        LDY(bus.Read(AbsoluteY()));
                        break;
                    case 0xBD:
                        cyclesLeft = 4;
                        LDA(bus.Read(AbsoluteX()));
                        break;
                    case 0xBE:
                        cyclesLeft = 4;
                        LDX(bus.Read(AbsoluteY()));
                        break;
                }
            }
        }

        #region Adressing Modes
        ushort Immediate()
        {
            return pc;
        }

        ushort ZeroPage()
        {
            byte adress = bus.Read(pc);
            return adress;
        }

        ushort ZeroPageX()
        {
            byte adress = bus.Read((ushort)(pc + x));
            return adress;
        }

        ushort ZeroPageY()
        {
            byte adress = bus.Read((ushort)(pc + y));
            return adress;
        }

        ushort Absolute()
        {
            byte pcl = bus.Read(pc);
            pc++;
            byte pch = bus.Read(pc);
            pc++;
            return BitConverter.ToUInt16(new byte[]{ pch ,pcl});
        }

        ushort AbsoluteX()
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

            return (ushort)(BitConverter.ToUInt16(new byte[] { pch, pcl }) + x);
        }

        ushort AbsoluteY()
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

            return (ushort)(BitConverter.ToUInt16(new byte[] { pch, pcl }) + y);
        }

        ushort Indirect()
        {
            ushort adress = (BitConverter.ToUInt16(new byte[] { bus.Read((ushort)(pc + 1)), bus.Read(pc) }));
            byte pcl = bus.Read(adress);
            byte pch = bus.Read((ushort)(adress + 1));

            return (ushort)(BitConverter.ToUInt16(new byte[] { pch, pcl }));
        }

        ushort IZX()
        {
            ushort t = bus.Read(pc);
            pc++;

            byte pcl = bus.Read((ushort)((t+x) & 0x00FF));

            int pageIndex = pc / 256;

            byte pch = bus.Read((ushort)((t + x + 1) & 0x00FF));
            ushort adress = BitConverter.ToUInt16(new byte[] { pch, pcl });

            return (ushort)(adress + y);
        }

        ushort IZY()
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

            return (ushort)(adress + y);
        }
        #endregion

        #region Instructions 

        void AND(byte value)
        {
            a = (byte)(a & value);
            p.Z = (a == 0);
        }

        /// <summary>
        /// Load to A(Accumalator)
        /// </summary>
        /// <param name="adress"></param>
        void LDA(byte value)
        {
            a = value;

            p.Z = (a==0);
            BitArray array = new BitArray(a); 
            if(array.Count > 7)
            {
                p.N = array[7];
            }
            else
            {
                p.N = false;
            }
        }

        void LDX(byte value)
        {
            x = value;

            p.Z = (x == 0);
            BitArray array = new BitArray(a);
            if (array.Count > 7)
            {
                p.N = array[7];
            }
            else
            {
                p.N = false;
            }
        }

        void LDY(byte value)
        {
            y = value;

            p.Z = (y == 0);
            BitArray array = new BitArray(a);
            if (array.Count > 7)
            {
                p.N = array[7];
            }
            else
            {
                p.N = false;
            }
        }

        void STX(ushort value)
        {
            bus.Write(value, x);
        }

        void STY(ushort value)
        {
            bus.Write(value, y);
        }

        void STA(ushort value)
        {
            bus.Write(value, a);
        }

        void JMP(ushort value)
        {
            pc = value;
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
