using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using NESEmu.EmuNES;
using Console = NESEmu.EmuNES.Console;
using System.Threading;
using FastBitmapLib;

namespace NESEmu
{
    public partial class Form1 : Form
    {
        public Console console = new Console();
        BufferedGraphicsContext context;
        BufferedGraphics myBuffer;
        Thread consoleThread;
        Bitmap bitmap = new Bitmap(512, 512);
        FastBitmap frameBuffer ;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            context = BufferedGraphicsManager.Current;
            frameBuffer = new FastBitmap(bitmap);
            // Creates a BufferedGraphics instance associated with Form1, and with
            // dimensions the same size as the drawing surface of Form1.
            myBuffer = context.Allocate(this.CreateGraphics(),
               this.DisplayRectangle);
            AllocConsole();
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private void button1_Click(object sender, EventArgs e)
        {
            
            console.Init(ref frameBuffer);
            consoleThread = new Thread(console.StartConsole);
            consoleThread.Start();

        }

        private void button2_Click(object sender, EventArgs e)
        {

            console.Init(ref frameBuffer);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            console.bus.Clock();
            frameBuffer.Lock();
            //frameBuffer.SetPixel(0, 0, Color.Red);
            frameBuffer.Unlock();
            myBuffer.Graphics.DrawImage(bitmap, 0,0,2048,2048);
            myBuffer.Render();
            aText.Text = "A:" + console.bus.cpu.a.ToString();
            xText.Text = "X:" + console.bus.cpu.x.ToString();
            yText.Text = "Y:" + console.bus.cpu.y.ToString();
        }
    }
}
