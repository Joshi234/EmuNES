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

namespace NESEmu
{
    public partial class Form1 : Form
    {
        public Console console = new Console();
        BufferedGraphicsContext context;
        BufferedGraphics myBuffer;
        Thread consoleThread;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            context = BufferedGraphicsManager.Current;
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
            
            console.Init();
            consoleThread = new Thread(console.StartConsole);
            consoleThread.Start();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            console.Init();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            myBuffer.Graphics.DrawImage(new Bitmap(500, 500), new Point(0, 0));
            myBuffer.Render();
            console.bus.Clock();
            aText.Text = "A:" + console.bus.cpu.a.ToString();
            xText.Text = "X:" + console.bus.cpu.x.ToString();
            yText.Text = "Y:" + console.bus.cpu.y.ToString();
        }
    }
}
