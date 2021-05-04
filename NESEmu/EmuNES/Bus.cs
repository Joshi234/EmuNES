using System;
using System.Collections.Generic;
using System.Text;
using NESEmu.EmuNES.Mappers;
namespace NESEmu.EmuNES
{
    public class Bus
    {
        public CPU cpu;
        PPU ppu;
        APU apu;
        public byte[] rom = new byte[32000];
        public byte[] ram = new byte[2048];
        public byte[] io1 = new byte[8];
        public byte[] io2 = new byte[32];
        public byte[] expensionsRom = new byte[8160];
        public byte[] sram = new byte[8192];
        Mapper mapper = new Mapper0();

        public void Init()
        {
            ram[0] = 0x6C;
            ram[1] = 0x00;
            ram[2] = 0x03;
            ram[4] = 0xFF;
            mapper.rom = rom;
            cpu = new CPU(this);
        }

        public void Clock()
        {
            cpu.clock();
        }

        public byte Read(ushort adress)
        {
            System.Console.WriteLine("Read at: " + adress.ToString());
            if (adress <= 0x1FFF){
                System.Console.WriteLine("Read at: " + adress.ToString() + " Value: " + ram[adress % 0x0800]) ;
                return ram[adress%0x0800];
            }
            else if(adress <= 0x3FFF)
            {
                return io1[(adress-0x2000) % 0x8];
            }
            else if(adress <= 0x401F)
            {
                return io2[adress - 0x4000];
            }
            else if(adress <= 0x5FFF)
            {
                return expensionsRom[adress - 0x4020];
            }
            else if(adress <= 0x7FFF)
            {
                return sram[adress - 0x6000];
            }
            else
            {
                System.Console.WriteLine("Read at: " + adress.ToString() + " Value: " + mapper.Read(adress));
                return mapper.Read(adress);
            }
        }

        public void Write(ushort adress, byte value)
        {
            System.Console.WriteLine("Wrote at: " + adress.ToString() + " Value: " +value.ToString());
            if (adress <= 0x1FFF)
            {
                ram[adress % 0x0800] = value;
            }
            else if (adress <= 0x3FFF)
            {
                 io1[(adress - 0x2000) % 0x8] = value ;
            }
            else if (adress <= 0x401F)
            {
                 io2[adress - 0x4000]=value;
            }
            else if (adress <= 0x5FFF)
            {
                expensionsRom[adress - 0x4020] = value;
            }
            else if (adress <= 0x7FFF)
            {
                sram[adress - 0x6000] = value;
            }
            else
            {
                mapper.Write(adress, value);
            }
        }
    }
}
