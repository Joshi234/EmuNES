using System;
using System.Collections.Generic;
using System.Text;

namespace NESEmu.EmuNES.Mappers
{
    abstract class Mapper
    {
        public byte[] rom;

        public abstract byte Read(ulong adress);


        public abstract void Write(ulong adress, byte value);

    }
}
