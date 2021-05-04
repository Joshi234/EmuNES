using System;
using System.Collections.Generic;
using System.Text;

namespace NESEmu.EmuNES.Mappers
{
    class Mapper0 : Mapper
    {
        public override byte Read(ulong adress)
        {
            if(adress >= 0x8000 && adress <= 0xFFFF)
            {
                return rom[adress - 0x8000];
            }
            else
            {
                return 0x0;
            }
        }

        public override void Write(ulong adress, byte value)
        {
            throw new AccessViolationException();
            //Nothing can be writen!
        }
    }
}
