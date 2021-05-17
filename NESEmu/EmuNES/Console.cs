using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;

namespace NESEmu.EmuNES
{
    public class Console
    {
        public Bus bus;
        public bool paused = false;
        int cyclecount = 0;
        public void Init(ref FastBitmapLib.FastBitmap frameBuffer)
        {
            bus = new Bus();
            bus.Init(ref frameBuffer);
            LoadRom(@"C:\Users\JS\Downloads\mariobros.nes");
            //LoadRom(@"G:\roms\nes\Mega Man (E) [!].nes");
            
            //bus.cpu.pc = 0x8000;
        }

        public void StartConsole()
        {

            while (paused == false) {
                cyclecount++;
                //bus.cpu.pc = 0;
                bus.Clock();
                if (cyclecount % 100000 == 0)
                {
                    System.Console.WriteLine("Cycles: " + cyclecount.ToString());
                }
            }
        }

        public void LoadRom(string Location)
        {
            byte[] rom = File.ReadAllBytes(Location);
            int prgRomSize = 16384 * rom[4];
            int chrRomSize = 8192 * rom[5];


  
            System.Console.WriteLine("Mapper: " + (rom[6] & 0b00001111).ToString());
            bus.rom = rom.Skip(16).Take(prgRomSize).ToArray();
            bus.ppu.chr = rom.Skip(16+prgRomSize).Take(chrRomSize).ToArray();
        }
    }
}
