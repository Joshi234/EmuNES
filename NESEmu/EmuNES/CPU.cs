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
        public byte s = 0xFF; //Stack Pointer
        public Flags p = new Flags(); //Processor Status
        public Bus bus;
        public int cyclesLeft;


        public CPU(Bus _bus)
        {
            bus = _bus;
        }
        #region opcodes and clock
        public void clock()
        {
            if (cyclesLeft != 0)
            {
                cyclesLeft--;
            }
            else
            {
                byte opcode = bus.Read(pc);


                //Opcode Table implemented as switch because speed
                switch (opcode)
                {
                    case 0x00:
                        cyclesLeft = 7;
                        BRK();
                        break;
                    case 0x01:
                        cyclesLeft = 6;
                        ORA(bus.Read(IZX()));
                        break;
                    case 0x06:
                        cyclesLeft = 5;
                        ASL(ZeroPage());
                        break;
                    case 0x08:
                        cyclesLeft = 3;
                        PHP();
                        break;
                    case 0x09:
                        cyclesLeft = 2;
                        ORA(bus.Read(Immediate()));
                        break;
                    case 0x0A:
                        cyclesLeft = 2;
                        ASL();
                        break;
                    case 0x0D:
                        cyclesLeft = 4;
                        ORA(bus.Read(Absolute()));
                        break;
                    case 0x0E:
                        cyclesLeft = 6;
                        ASL(Absolute());
                        break;
                    case 0x10:
                        cyclesLeft = 2;
                        BPL((sbyte)Relative());
                        break;
                    case 0x11:
                        cyclesLeft = 5;
                        ORA(bus.Read(IZY()));
                        break;
                    case 0x15:
                        cyclesLeft = 4;
                        ORA(bus.Read(Absolute()));
                        break;
                    case 0x16:
                        cyclesLeft = 6;
                        ASL(ZeroPageX());
                        break;
                    case 0x18:
                        cyclesLeft = 2;
                        CLC();
                        break;
                    case 0x19:
                        cyclesLeft = 4;
                        ORA(bus.Read(AbsoluteY()));
                        break;
                    case 0x1D:
                        cyclesLeft = 4;
                        ORA(bus.Read(AbsoluteX()));
                        break;
                    case 0x1E:
                        cyclesLeft = 7;
                        ASL(AbsoluteX());
                        break;
                    case 0x20:
                        cyclesLeft = 6;
                        JSR(Absolute());
                        break;
                    case 0x21:
                        cyclesLeft = 6;
                        AND(bus.Read(IZX()));
                        break;
                    case 0X24:
                        cyclesLeft = 3;
                        BIT(ZeroPage());
                        break;
                    case 0x25:
                        cyclesLeft = 3;
                        AND(bus.Read(ZeroPage()));
                        break;
                    case 0x26:
                        cyclesLeft = 5;
                        ROL(ZeroPage());
                        break;
                    case 0x28:
                        cyclesLeft = 4;
                        PLP();
                        break;
                    case 0x29:
                        cyclesLeft = 2;
                        AND(bus.Read(Immediate()));
                        break;
                    case 0x2A:
                        cyclesLeft = 2;
                        ROL();
                        break;
                    case 0x2C:
                        cyclesLeft = 4;
                        BIT(Absolute());
                        break;
                    case 0x2D:
                        cyclesLeft = 4;
                        AND(bus.Read(Absolute()));
                        break;
                    case 0x2E:
                        cyclesLeft = 6;
                        ROL(Absolute());
                        break;
                    case 0x30:
                        cyclesLeft = 2;
                        BMI((sbyte)Relative());
                        break;
                    case 0x31:
                        cyclesLeft = 5;
                        AND(bus.Read(IZY()));
                        break;
                    case 0x35:
                        cyclesLeft = 4;
                        AND(bus.Read(ZeroPageX()));
                        break;
                    case 0x36:
                        cyclesLeft = 6;
                        ROL(ZeroPage());
                        break;
                    case 0x38:
                        cyclesLeft = 2;
                        SEC();
                        break;
                    case 0x39:
                        cyclesLeft = 4;
                        AND(bus.Read(AbsoluteY()));
                        break;
                    case 0x3D:
                        cyclesLeft = 4;
                        AND(bus.Read(AbsoluteX()));
                        break;
                    case 0x3E:
                        cyclesLeft = 7;
                        ROL(AbsoluteX());
                        break;
                    case 0x40:
                        cyclesLeft = 6;
                        RTI();
                        break;
                    case 0x41:
                        cyclesLeft = 6;
                        EOR(bus.Read(IZY()));
                        break;
                    case 0x45:
                        cyclesLeft = 3;
                        EOR(bus.Read(ZeroPage()));
                        break;
                    case 0x46:
                        cyclesLeft = 5;
                        LSR(ZeroPage());
                        break;
                    case 0x48:
                        cyclesLeft = 3;
                        PHA();
                        break;
                    case 0x49:
                        cyclesLeft = 2;
                        EOR(bus.Read(Immediate()));
                        break;
                    case 0x4A:
                        cyclesLeft = 2;
                        LSR();
                        break;
                    case 0x4C:
                        cyclesLeft = 3;
                        JMP(Absolute());
                        break;
                    case 0x4D:
                        cyclesLeft = 4;
                        EOR(bus.Read(Absolute()));
                        break;
                    case 0x4E:
                        cyclesLeft = 6;
                        LSR(Absolute());
                        break;
                    case 0x50:
                        cyclesLeft = 2;
                        BVC((sbyte)Relative());
                        break;
                    case 0x51:
                        cyclesLeft = 5;
                        EOR(bus.Read(IZY()));
                        break;
                    case 0x55:
                        cyclesLeft = 4;
                        EOR(bus.Read(ZeroPageX()));
                        break;
                    case 0x56:
                        cyclesLeft = 6;
                        LSR(ZeroPageX());
                        break;
                    case 0x58:
                        cyclesLeft = 2;
                        CLI();
                        break;
                    case 0x59:
                        cyclesLeft = 4;
                        EOR(bus.Read(AbsoluteY()));
                        break;
                    case 0x5D:
                        cyclesLeft = 4;
                        EOR(bus.Read(AbsoluteX()));
                        break;
                    case 0x5E:
                        cyclesLeft = 7;
                        LSR(AbsoluteX());
                        break;
                    case 0x60:
                        cyclesLeft = 6;
                        RTS();
                        break;
                    case 0x61:
                        cyclesLeft = 6;
                        ADC(bus.Read(IZX()));
                        break;
                    case 0x65:
                        cyclesLeft = 3;
                        ADC(bus.Read(ZeroPage()));
                        break;
                    case 0x66:
                        cyclesLeft = 5;
                        ROR(ZeroPage());
                        break;
                    case 0x68:
                        cyclesLeft = 4;
                        PLA();
                        break;
                    case 0x69:
                        cyclesLeft = 2;
                        ADC(bus.Read(Immediate()));
                        break;
                    case 0x6A:
                        cyclesLeft = 2;
                        ROR();
                        break;
                    case 0x6C:
                        cyclesLeft = 5;
                        JMP(Indirect());
                        break;
                    case 0x6D:
                        cyclesLeft = 4;
                        ADC(bus.Read(Absolute()));
                        break;
                    case 0x6E:
                        cyclesLeft = 6;
                        ROR(Absolute());
                        break;
                    case 0x70:
                        cyclesLeft = 2;
                        BVS((sbyte)Relative());
                        break;
                    case 0x71:
                        cyclesLeft = 5;
                        ADC(bus.Read(IZY()));
                        break;
                    case 0x75:
                        cyclesLeft = 4;
                        ADC(bus.Read(ZeroPageX()));
                        break;
                    case 0x76:
                        cyclesLeft = 6;
                        ROR(ZeroPageX());
                        break;
                    case 0x78:
                        cyclesLeft = 2;
                        SEI();
                        break;
                    case 0x79:
                        cyclesLeft = 4;
                        ADC(bus.Read(AbsoluteY()));
                        break;
                    case 0x7D:
                        cyclesLeft = 4;
                        ADC(bus.Read(AbsoluteX()));
                        break;
                    case 0x7E:
                        cyclesLeft = 7;
                        ROR(AbsoluteX());
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
                        STX(ZeroPage());
                        break;
                    case 0x88:
                        cyclesLeft = 2;
                        DEY();
                        break;
                    case 0x8A:
                        cyclesLeft = 2;
                        TXA();
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
                    case 0x90:
                        cyclesLeft = 2;
                        BCC((sbyte)Relative());
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
                    case 0x98:
                        cyclesLeft = 2;
                        TYA();
                        break;
                    case 0x99:
                        cyclesLeft = 5;
                        STA(AbsoluteY());
                        break;
                    case 0x9A:
                        cyclesLeft = 2;
                        TXS();
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
                    case 0xA8:
                        cyclesLeft = 2;
                        TAY();
                        break;
                    case 0xA9:
                        cyclesLeft = 2;
                        LDA(bus.Read(Immediate()));
                        break;
                    case 0xAA:
                        cyclesLeft = 2;
                        TAX();
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
                    case 0xB0:
                        cyclesLeft = 2;
                        BCS((sbyte)Relative());
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
                    case 0xB8:
                        cyclesLeft = 2;
                        CLV();
                        break;
                    case 0xB9:
                        cyclesLeft = 4;
                        LDA(bus.Read(AbsoluteY()));
                        break;
                    case 0xBA:
                        cyclesLeft = 2;
                        TSX();
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
                    case 0xC0:
                        cyclesLeft = 2;
                        CPY(bus.Read(Immediate()));
                        break;
                    case 0xC1:
                        cyclesLeft = 6;
                        CMP(bus.Read(IZX()));
                        break;
                    case 0xC4:
                        cyclesLeft = 3;
                        CPY(bus.Read(ZeroPage()));
                        break;
                    case 0xC5:
                        cyclesLeft = 3;
                        CMP(bus.Read(ZeroPage()));
                        break;
                    case 0xC6:
                        cyclesLeft = 5;
                        DEC(ZeroPage());
                        break;
                    case 0xC8:
                        cyclesLeft = 2;
                        INY();
                        break;
                    case 0xC9:
                        cyclesLeft = 2;
                        CMP(bus.Read(Immediate()));
                        break;
                    case 0xCA:
                        cyclesLeft = 2;
                        DEX();
                        break;
                    case 0xCC:
                        cyclesLeft = 4;
                        CPY(bus.Read(Absolute()));
                        break;
                    case 0xCD:
                        cyclesLeft = 4;
                        CMP(bus.Read(Absolute()));
                        break;
                    case 0xCE:
                        cyclesLeft = 6;
                        DEC(Absolute());
                        break;
                    case 0xD0:
                        cyclesLeft = 2;
                        BNE((sbyte)Relative());
                        break;
                    case 0xD1:
                        cyclesLeft = 5;
                        CMP(bus.Read(IZY()));
                        break;
                    case 0xD5:
                        cyclesLeft = 4;
                        CMP(bus.Read(ZeroPageX()));
                        break;
                    case 0xD6:
                        cyclesLeft = 6;
                        DEC(ZeroPageX());
                        break;
                    case 0xD8:
                        cyclesLeft = 2;
                        CLD();
                        break;
                    case 0xD9:
                        cyclesLeft = 4;
                        CMP(bus.Read(AbsoluteY()));
                        break;
                    case 0xDD:
                        cyclesLeft = 4;
                        CMP(bus.Read(AbsoluteX()));
                        break;
                    case 0xDE:
                        cyclesLeft = 7;
                        DEC(AbsoluteX());
                        break;
                    case 0xE0:
                        cyclesLeft = 2;
                        CPX(bus.Read(ZeroPage()));
                        break;
                    case 0xE1:
                        cyclesLeft = 6;
                        SBC(bus.Read(IZX()));
                        break;
                    case 0xE4:
                        cyclesLeft = 3;
                        CPX(bus.Read(ZeroPage()));
                        break;
                    case 0xE5:
                        cyclesLeft = 3;
                        SBC(ZeroPage());
                        break;
                    case 0xE6:
                        cyclesLeft = 5;
                        INC(ZeroPage());
                        break;
                    case 0xE8:
                        cyclesLeft = 2;
                        INX();
                        break;
                    case 0xE9:
                        cyclesLeft = 2;
                        SBC(bus.Read(Immediate()));
                        break;
                    case 0xEC:
                        cyclesLeft = 4;
                        CPX(bus.Read(Absolute()));
                        break;
                    case 0xEA:
                        cyclesLeft = 2;
                        //NOP
                        break;
                    case 0xED:
                        cyclesLeft = 4;
                        SBC(bus.Read(Absolute()));
                        break;
                    case 0xEE:
                        cyclesLeft = 6;
                        INC(Absolute());
                        break;
                    case 0xF0:
                        cyclesLeft = 2;
                        BEQ((sbyte)Relative());
                        break;
                    case 0xF1:
                        cyclesLeft = 5;
                        SBC(bus.Read(IZY()));
                        break;
                    case 0xF5:
                        cyclesLeft = 4;
                        SBC(bus.Read(ZeroPageX()));
                        break;
                    case 0xF6:
                        cyclesLeft = 6;
                        INC(ZeroPageX());
                        break;
                    case 0xF8:
                        cyclesLeft = 2;
                        SED();
                        break;
                    case 0xF9:
                        cyclesLeft = 4;
                        SBC(bus.Read(AbsoluteY()));
                        break;
                    case 0xFD:
                        cyclesLeft = 4;
                        SBC(bus.Read(AbsoluteX()));
                        break;
                    case 0xFE:
                        cyclesLeft = 7;
                        INC(AbsoluteX());
                        break;
                    case 0xFF:
                        break;
                    default:

                        throw new Exception("OPCODE " + opcode + " NOT IMPLEMENTED");
                        break;
                }
                pc++;
            }
        }
        #endregion

        #region Interrupts
        //not implemented yet
        #endregion

        #region AdressModes

        //Implied needs no operand thus it needs no implementation

        ushort Immediate()
        {
            pc++;
            return pc;
        }

        ushort ZeroPage()
        {
            pc++;
            byte adress = bus.Read(pc);
            return adress;
        }

        ushort ZeroPageX()
        {
            pc++;
            byte adress = bus.Read((ushort)(pc + x));
            return adress;
        }

        ushort ZeroPageY()
        {
            pc++;
            byte adress = bus.Read((ushort)(pc + y));
            return adress;
        }

        ushort Absolute()
        {
            pc++;
            byte pcl = bus.Read(pc);
            pc++;
            byte pch = bus.Read(pc);
            return BitConverter.ToUInt16(new byte[] { pcl, pch });
        }

        ushort AbsoluteX()
        {
            pc++;
            byte pcl = bus.Read(pc);
            int pageIndex = pc / 256;
            pc++;
            byte pch = bus.Read(pc);
            if (pc / 256 != pageIndex)
            {
                cyclesLeft++;
            }

            return (ushort)(BitConverter.ToUInt16(new byte[] { pcl, pch }) + x);
        }

        byte Relative()
        {
            pc++;
            byte addr = bus.Read(pc);

            return addr;
        }

        ushort AbsoluteY()
        {
            pc++;
            byte pcl = bus.Read(pc);
            int pageIndex = pc / 256;
            pc++;
            byte pch = bus.Read(pc);
            if (pc / 256 != pageIndex)
            {
                cyclesLeft++;
            }

            return (ushort)(BitConverter.ToUInt16(new byte[] { pcl, pch }) + y);
        }

        ushort Indirect()
        {
            pc++;
            ushort adress = (BitConverter.ToUInt16(new byte[] { bus.Read((ushort)(pc + 1)), bus.Read(pc) }));
            byte pcl = bus.Read(adress);
            byte pch = bus.Read((ushort)(adress + 1));

            return (ushort)(BitConverter.ToUInt16(new byte[] { pcl, pch }));
        }

        ushort IZX()
        {
            pc++;
            ushort t = bus.Read(pc);
            pc++;

            byte pcl = bus.Read((ushort)((t + x) & 0x00FF));

            int pageIndex = pc / 256;

            byte pch = bus.Read((ushort)((t + x + 1) & 0x00FF));
            ushort adress = BitConverter.ToUInt16(new byte[] { pcl, pch });

            return (ushort)(adress + y);
        }

        ushort IZY()
        {
            pc++;
            ushort t = bus.Read(pc);

            byte pcl = bus.Read((ushort)(t & 0x00FF));


            byte pch = bus.Read((ushort)((t + 1) & 0x00FF));
            ushort adress = BitConverter.ToUInt16(new byte[] { pcl, pch });
            if (adress / 256 != (adress + y) / 256)
            {
                cyclesLeft++;
            }

            return (ushort)(adress + y);
        }
        #endregion

        #region Instructions 
        //Source for ADC and SBC: https://github.com/OneLoneCoder/olcNES/blob/master/Part%232%20-%20CPU/olc6502.cpp
        void ADC(byte value)
        {

            // Add is performed in 16-bit domain for emulation to capture any
            // carry bit, which will exist in bit 8 of the 16-bit word
            ushort temp = (ushort)((ushort)a + (ushort)value + (ushort)(p.C ? 1 : 0));

            p.C = temp > 255;

            // The Zero flag is set if the result is 0
            p.Z = ((temp & 0x00FF) == 0);

            // The signed Overflow flag is set based on all that up there! :D
            p.V = (~(a ^ value) & (a ^ temp) & 0x0080) == 1;

            // The negative flag is set to the most significant bit of the result
            p.N = (temp & 0x80) == 1;

            // Load the result into the accumulator (it's 8-bit dont forget!)
            a = (byte)(temp & 0x00FF);

            // This instruction has the potential to require an additional clock cycle
        }

        void SBC(ushort value)
        {


            // We can invert the bottom 8 bits with bitwise xor
            ushort valueXor = (ushort)(((ushort)value) ^ 0x00FF);

            // Notice this is exactly the same as addition from here!
            ushort temp = (ushort)((ushort)a + valueXor + (ushort)(p.C ? 1 : 0));
            p.C = (temp & 0xFF00) == 1;
            p.Z = (temp & 0x00FF) == 0;
            p.V = ((temp ^ (ushort)a) & (temp ^ valueXor) & 0x0080) == 1;
            p.N = (temp & 0x0080) == 1;
            a = (byte)(temp & 0x00FF);
        }

        void AND(ushort value)
        {
            a = (byte)(a & value);
            p.Z = (a == 0);
        }
        
        //Uses Accumalator
        void ASL()
        {
            BitArray bitArray = new BitArray(new byte[] { a });
            p.C = bitArray[7];

            for (int i = 0; i < 7; i++)
            {
                bitArray[i + 1] = bitArray[i];
            }
            byte[] temp = new byte[0];
            bitArray.CopyTo(temp, 0);
            a = temp[0];

            p.N = bitArray[7];
            p.Z = (a == 0);

        }

        //Manipulates memory Content
        void ASL(ushort adress)
        {
            byte[] temp = new byte[1];

            temp[0] = bus.Read(adress);

            BitArray bitArray = new BitArray(new byte[] { temp[0] });
            p.C = bitArray[7];

            for(int i = 0;i < 7; i++)
            {
                bitArray[i+1] = bitArray[i];
            }

            bitArray.CopyTo(temp, 0);

            p.N = bitArray[7];
            p.Z = (temp[0] == 0);

            bus.Write(adress, temp[0]);
        }

        void BCC(sbyte value)
        {
            if (!p.C)
            {
                cyclesLeft++;

                ushort addr_temp = (ushort)(pc + value);

                if ((addr_temp / 256) != (pc / 256))
                {
                    cyclesLeft++;
                }

                pc = addr_temp;
            }
        }

        void BCS(sbyte value)
        {
            if (p.C)
            {
                cyclesLeft++;

                ushort addr_temp = (ushort)(pc + value);

                if ((addr_temp / 256) != (pc / 256))
                {
                    cyclesLeft++;
                }

                pc = addr_temp;
            }
        }

        void BEQ(sbyte value)
        {
            if (p.Z)
            {
                cyclesLeft++;

                ushort addr_temp = (ushort)(pc + value);

                if ((addr_temp / 256) != (pc / 256))
                {
                    cyclesLeft++;
                }

                pc = addr_temp;
            }
        }

        void BIT(ushort adress)
        {
            byte value = bus.Read(adress);

            byte temp = (byte)(a & value);
            BitArray tempBitArray = new BitArray(new byte[] { temp });
            p.Z = (temp == 0);
            p.N = tempBitArray[7];
            p.V = tempBitArray[6];
        }

        void BMI(sbyte value)
        {
            if (p.N)
            {
                cyclesLeft++;

                ushort addr_temp = (ushort)(pc + value);

                if ((addr_temp / 256) != (pc / 256))
                {
                    cyclesLeft++;
                }

                pc = addr_temp;
            }
        }

        void BNE(sbyte value)
        {
            if (!p.Z)
            {
                cyclesLeft++;

                ushort addr_temp = (ushort)(pc + value);

                if ((addr_temp / 256) != (pc / 256))
                {
                    cyclesLeft++;
                }

                pc = addr_temp;
            }
        }

        void BPL(sbyte value)
        {
            if (!p.N)
            {
                cyclesLeft++;

                ushort addr_temp = (ushort)(pc + value);

                if ((addr_temp / 256) != (pc / 256))
                {
                    cyclesLeft++;
                }

                pc = addr_temp;
            }
        }

        /// <summary>
        /// Load to A(Accumalator)
        /// </summary>
        /// <param name="adress"></param>
        void LDA(byte value)
        {
            a = value;

            p.Z = (a==0);
            BitArray array = new BitArray(new byte[] { a });
            array.Length = 8;
            if(array.Count > 7)
            {
                p.N = array[7];
            }
            else
            {
                p.N = false;
            }
        }

        void BRK()
        {
            pc++;

            p.I = true;
            byte[] temp = BitConverter.GetBytes(pc);
            bus.Write((ushort)(0x0100 + s), temp[0]);
            s--;
            bus.Write((ushort)(0x0100 + s), temp[1]);
            s--;

            p.B = true;
            bus.Write((ushort)(0x0100 + s), ConvertFlagsToByte(true));
            s--;
            p.B = false;

            byte pcl = bus.Read(0xFFFE);
            byte pch = bus.Read(0xFFFF);

            pc = BitConverter.ToUInt16(new byte[] { pcl, pch });
        }

        void BVC(sbyte value)
        {
            if (!p.V)
            {
                cyclesLeft++;

                ushort addr_temp = (ushort)(pc + value);

                if ((addr_temp / 256) != (pc / 256))
                {
                    cyclesLeft++;
                }

                pc = addr_temp;
            }
        }

        void BVS(sbyte value) 
        {
            if (p.V)
            {
                cyclesLeft++;

                ushort addr_temp = (ushort)(pc + value);

                if ((addr_temp / 256) != (pc / 256))
                {
                    cyclesLeft++;
                }

                pc = addr_temp;
            }
        }

        void CLC()
        {
            p.C = false;
        }

        void CLD()
        {
            p.D = false;
        }

        void CLI()
        {
            p.I = false;
        }

        void CLV()
        {
            p.V = false;
        }

        void CMP(byte value)
        {
            p.C = a >= value;
            value = (byte)(a - value);
            p.Z = a == value;

            BitArray temp = new BitArray(new byte[] { value });
            p.N = temp[7];
        }

        void CPX(byte value)
        {
            p.C = x >= value;
            value = (byte)(x - value);
            p.Z = x == value;

            BitArray temp = new BitArray(new byte[] { value });
            p.N = temp[7];
        }

        void CPY(byte value)
        {
            p.C = y >= value;
            value = (byte)(y - value);
            p.Z = y == value;

            BitArray temp = new BitArray(new byte[] { value });
            temp.Length = 8;
            p.N = temp[7];
        }

        void DEC(ushort adress)
        {
            byte result = (byte)(bus.Read(adress) - 1);
            p.Z = (result == 0);

            BitArray temp = new BitArray(new byte[] { result });
            p.N = temp[7];

            bus.Write(adress, result);
        }

        void DEX()
        {
            x--;

            p.Z = (x == 0);

            BitArray temp = new BitArray(new byte[] { x });
            p.N = temp[7];
        }

        void DEY()
        {
            y--;

            p.Z = (y == 0);

            BitArray temp = new BitArray(new byte[] { y });
            p.N = temp[7];
        }

        void EOR(byte value)
        {
            a = (byte)(a ^ value);

            p.Z = a == 0;

            BitArray temp = new BitArray(new byte[] { a });
            p.N = temp[7];
        }

        void INC(ushort adress)
        {
            byte temp = bus.Read(adress);
            temp++;
            p.Z = (temp == 0);

            BitArray tempBitArray = new BitArray(new byte[] { temp });
            p.N = tempBitArray[7];

            bus.Write(adress, temp);
        }

        void INX()
        {
            x++;
            p.Z = (x == 0);

            BitArray tempBitArray = new BitArray(new byte[] { x });
            p.N = tempBitArray[7];
        }

        void INY()
        {
            y++;
            p.Z = (y == 0);

            BitArray tempBitArray = new BitArray(new byte[] { y });
            p.N = tempBitArray[7];
        }

        /// <summary>
        /// Changes the program counter to the supplied Operand
        /// </summary>
        /// <param name="value"></param>
        void JMP(ushort value)
        {
            pc = value;
        }

        void JSR(ushort adress)
        {
            pc--;

            byte[] temp = BitConverter.GetBytes(pc);
            bus.Write((ushort)(0x0100 + s), temp[0]);
            s--;
            bus.Write((ushort)(0x0100 + s), temp[1]);
            s--;

            pc = adress;

        }

        void LDX(byte value)
        {
            x = value;

            p.Z = (x == 0);
            BitArray array = new BitArray(new byte[] { a });
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
            BitArray array = new BitArray(new byte[] { a });
            if (array.Count > 7)
            {
                p.N = array[7];
            }
            else
            {
                p.N = false;
            }
        }

        void LSR(ushort adress)
        {
            byte[] temp = new byte[0];
            temp[0] = bus.Read(adress);
            BitArray bitArray = new BitArray(new byte[] { temp[0] });
            p.C = bitArray[0];

            temp[0] = (byte)(temp[0] >> 1);
            bitArray = new BitArray(new byte[] { temp[0] });
            p.N = bitArray[7];
            p.Z = (temp[0] == 0);

            bus.Write(adress, temp[0]);
        }

        void LSR()
        {
            byte[] temp = new byte[1];
            temp[0] = a;
            BitArray bitArray = new BitArray(new byte[] { temp[0] });
            p.C = bitArray[0];

            temp[0] = (byte)(temp[0] >> 1);
            bitArray = new BitArray(new byte[] { temp[0] });
            p.N = bitArray[7];
            p.Z = (temp[0] == 0);
            a = temp[0];
        }

        void ORA(byte value)
        {
            a = (byte)(a | value);

            p.Z = a == 0;

            BitArray bitArray = new BitArray(new byte[] { a });
            p.N = bitArray[7];
        }

        void PHA()
        {
            bus.Write((ushort)(0x0100 + s), a);
            s--;
        }
        
        void PHP()
        {
            bus.Write((ushort)(0x0100 + s), ConvertFlagsToByte(true));
            s--;
        }

        void PLA()
        {
            s++;
            a = bus.Read(s);

            p.Z = a == 0;
            BitArray bitArray = new BitArray(new byte[] { a });
            p.N = bitArray[7];
        }

        void PLP()
        {
            s++;
            ConvertByteToFlags(bus.Read((ushort)(0x0100 + s)));
        }

        void ROL(ushort adress)
        {
            byte value = bus.Read(adress);
            BitArray bitArray = new BitArray(new byte[] { value });
            bool tempC = p.C;

            value = (byte)(value << 1);
            p.C = bitArray[7];
            bitArray = new BitArray(new byte[] { value });

            bitArray[0] = tempC;
            p.N = bitArray[7];
        }

        void ROL()
        {
            BitArray bitArray = new BitArray(new byte[] { a });
            bool tempC = p.C;

            a = (byte)(a << 1);
            p.C = bitArray[7];
            bitArray = new BitArray(new byte[] { a });

            bitArray[0] = tempC;
            p.N = bitArray[7];
        }

        void ROR()
        {
            BitArray bitArray = new BitArray(new byte[] { a });
            bool tempC = p.C;

            a = (byte)(a >> 1);
            p.C = bitArray[0];
            bitArray = new BitArray(new byte[] { a });

            bitArray[7] = tempC;
            p.N = bitArray[0];
        }

        void ROR(ushort adress)
        {
            byte value = bus.Read(adress);
            BitArray bitArray = new BitArray(new byte[] { value });
            bool tempC = p.C;

            value = (byte)(value >> 1);
            p.C = bitArray[0];
            bitArray = new BitArray(new byte[] { value });

            bitArray[7] = tempC;
            p.N = bitArray[0];
        }

        void RTI()
        {
            s++;
            ConvertByteToFlags(bus.Read((ushort)(0x0100 + s)));
            s++;
            byte pcl = bus.Read((ushort)(0x0100 + s));
            s++;
            byte pch = bus.Read((ushort)(0x0100 + s));
            pc = BitConverter.ToUInt16(new byte[] { pch, pcl });
        }

        void RTS()
        {
            s++;
            byte pcl = bus.Read((ushort)(0x0100 + s));
            s++;
            byte pch = bus.Read((ushort)(0x0100 + s));
            pc = BitConverter.ToUInt16(new byte[] { pch, pcl });

            pc++;
        }

        //STX,STY,STA All writes the register at the supplied adress
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

        /// <summary>
        /// Sets the carry Flag
        /// </summary>
        void SEC() 
        {
            p.C = true;
        }

        /// <summary>
        /// Sets the decimal Flag
        /// </summary>
        void SED()
        {
            p.D = true;
        }

        /// <summary>
        /// Sets the Interrupt Flag (Ignores all Interrupts when one gets triggered when this Flag is set)
        /// </summary>
        void SEI()
        {
            p.I = true;
        }

        void TAX()
        {
            x = a;
            p.Z = (x == 0);
            BitArray bitArray = new BitArray(new byte[] { x });
            p.N = bitArray[7];
        }

        void TAY()
        {
            y = a;
            p.Z = (y == 0);
            BitArray bitArray = new BitArray(new byte[] { y });
            p.N = bitArray[7];
        }

        void TSX()
        {
            x = s;
            p.Z = (x == 0);
            BitArray bitArray = new BitArray(new byte[] { x });
            p.N = bitArray[7];
        }

        void TXA()
        {
            a = x;
            p.Z = (a == 0);
            BitArray bitArray = new BitArray(new byte[] { a });
            p.N = bitArray[7];
        }

        void TXS()
        {
            s = x;
            p.Z = (s == 0);
            BitArray bitArray = new BitArray(new byte[] { s });
            p.N = bitArray[7];
        }

        void TYA()
        {
            a = y;
            p.Z = (a == 0);
            BitArray bitArray = new BitArray(new byte[] { a });
            p.N = bitArray[7];
        }
        #endregion

        byte ConvertFlagsToByte(bool bit4)
        {

            BitArray bitArray = new BitArray(8);
            bitArray[0] = p.C;
            bitArray[1] = p.Z;
            bitArray[2] = p.I;
            bitArray[3] = p.D;
            bitArray[4] = bit4;
            bitArray[5] = true;
            bitArray[6] = p.V;
            bitArray[7] = p.N;

            byte[] temp = new byte[1];
            bitArray.CopyTo(temp, 0);
            return temp[0];
        }

        void ConvertByteToFlags(byte value)
        {

            BitArray bitArray = new BitArray(new byte[] { value });
            bitArray.Length = 8;
            p.C = bitArray[0];
            p.Z = bitArray[1];
            p.I = bitArray[2];
            p.D = bitArray[3];
            p.V = bitArray[6];
            p.N = bitArray[7];

            byte[] temp = new byte[1];
            bitArray.CopyTo(temp, 0);

        }
    }



    public struct Flags
    {
        public bool N { get; set; } //Negative Flag
        public bool V { get; set; } //Overflow Flag
        public bool B { get; set; } //Break Flag
        public bool D { get; set; } //Decimal Mode Flag
        public bool I { get; set; } //Interrupt disable flag
        public bool Z { get; set; } //Zero Flag
        public bool C { get; set; } //Carry Flag
    }
}
