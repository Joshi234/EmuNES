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
        Thread consoleThread;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
            console.bus.Clock();
            aText.Text = "A:" + console.bus.cpu.a.ToString();
        }
    }
}
