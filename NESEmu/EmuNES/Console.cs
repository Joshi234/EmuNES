using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
namespace NESEmu.EmuNES
{
    public class Console
    {
        public Bus bus;
        public bool paused = false;
        int cyclecount = 0;
        public void Init()
        {
            bus = new Bus();
            LoadRom(@"C:\Users\Joshua\source\repos\NESEmu\NESEmu\rom_singles\nestest.nes");
            bus.Init();
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

            bus.rom = rom.Skip(16).Take(prgRomSize).ToArray();
            
        }
    }
}
