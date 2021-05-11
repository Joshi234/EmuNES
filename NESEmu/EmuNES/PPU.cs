using FastBitmapLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;

namespace NESEmu.EmuNES
{
    public class PPU
    {
        FastBitmap frameBuffer;
        Bus bus;
        public byte[] chr;
        public PPU(Bus _bus,FastBitmap buffer)
        {
            bus = _bus;
            frameBuffer = buffer;
        }

        public void LoadSprites()
        {
            frameBuffer.Lock();
            for (int i = 0; i < chr.Length/16; i++)
            {
                for (int ie = 0; ie < 8; ie++)
                {
                    
                    BitArray bitArray = new BitArray(chr[(i * 16)+ie]);
                    bitArray.Length = 8;
                    foreach(bool bit in bitArray)
                    {
                        if (i * 16 < frameBuffer.Width) { 
                        
                        if (bit) {
                                frameBuffer.SetPixel(i * 16, ie, Color.Red);
                            }
                        }   
                    }
                }
            }
            frameBuffer.Unlock();
        }
    }
}
