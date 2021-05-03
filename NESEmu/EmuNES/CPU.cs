using System;
using System.Collections.Generic;
using System.Text;

namespace NESEmu.EmuNES
{
    public class CPU
    {
        public byte a; //Accumulator
        public byte x; //X Register
        public byte y; //Y Register
        public ushort pc; // Program Counter
        public byte s; //Stack Pointer
        public Status p = Status.O; //Processor Status
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
                }
                pc++;
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

            return bus.Read(BitConverter.ToUInt16(new byte[]{ pch ,pcl}));
        }
        #endregion

        #region Instructions
        /// <summary>
        /// Load to A(Accumalator
        /// </summary>
        /// <param name="adress"></param>
        void LDA(byte value)
        {
            a = value;
        }
        #endregion
    }



    public enum Status
    {
        N, //Negative Flag
        V, //Overflow Flag
        O, //=1 Unused Flag
        B, //Break Flag
        D, //Decimal Mode Flag
        I, //Interrupt disable flag
        Z, //Zero Flag
        C, //Carry Flag
        //Source = http://nesdev.com/6502_cpu.txt
    }
}
