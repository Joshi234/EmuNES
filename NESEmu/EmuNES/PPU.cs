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
        public PPU(Bus _bus,ref FastBitmap buffer)
        {
            bus = _bus;
            frameBuffer = buffer;
        }

        public void LoadSprites()
        {
            frameBuffer.Lock();
            for (int i = 256; i < chr.Length/16; i++)
            {
                for (int ie = 0; ie < 8; ie++)
                {
                    
                    BitArray bitArray = new BitArray(new byte[] { chr[(i * 16) + ie] });
                    for(int ir = 0; ir< bitArray.Count; ir++)
                    {
                        bool bit = bitArray[ir];
                        if ((i * 16)-4096 < frameBuffer.Width) {
                            
                            if (bit) {
                                System.Console.WriteLine("i: " + (i * 16).ToString() + " ie: " + ie.ToString());
                                frameBuffer.SetPixel(((i * 16)-4096)+ir, ie, Color.Red);
                            }
                        }   
                    }
                }
            }
            frameBuffer.Unlock();
        }
    }
}
