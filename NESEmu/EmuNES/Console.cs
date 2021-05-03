using System;
using System.Collections.Generic;
using System.Text;

namespace NESEmu.EmuNES
{
    public class Console
    {
        public Bus bus;
        public bool paused = false;
        public void Init()
        {
            bus = new Bus();
            bus.Init();
        }

        public void StartConsole()
        {
            while (paused == false) { 
                bus.Clock();
            }
        }
    }
}
