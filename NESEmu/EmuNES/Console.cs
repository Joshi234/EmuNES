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
        public void Init()
        {
            bus = new Bus();
            //LoadRom(@"C:\Users\Joshua\source\repos\NESEmu\NESEmu\rom_singles\08-ind_y.nes");
            bus.Init();
        }

        public void StartConsole()
        {
            while (paused == false) { 
                bus.Clock();
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
