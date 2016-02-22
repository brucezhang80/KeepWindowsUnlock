using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MouseControl
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }
        [DllImport("User32")]
        public extern static bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll")]
        public static extern void SetCursorPos(int x, int y);

        [DllImport("USER32.DLL")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        bool running = true;
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            this.niMain.ShowBalloonTip(3000);
            StartThread();
        }

        delegate void del();
        void move()
        {
            //Point p = new Point();
            //GetCursorPos(ref p);
            //Console.WriteLine(p.X + ", " + p.Y);
            //SetCursorPos(p.X + 100, p.Y + 100);
            keybd_event(0x11, 0, 0, 0);
            keybd_event(0x11, 0, 2, 0);
        }

        private void StartThread()
        {
            Thread oThread = new Thread(delegate()
            {
                while (running)
                {
                    if (this.InvokeRequired)
                    {
                        del del = new del(move);
                        this.Invoke(del);
                    }
                    else
                    {
                        move();
                    }

                    int remaining = 3 * 60 * 1000;
                    while (running && remaining > 0)
                    {
                        Thread.Sleep(500);
                        remaining -= 500;
                    }
                }
            });
            oThread.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            running = false;
        }

        bool isQuitShown = false;
        private void niMain_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            if (me == null || me.Button != System.Windows.Forms.MouseButtons.Left)
            {
                return;
            }

            this.Show();
            if (isQuitShown)
            {
                return;
            }
            isQuitShown = true;
            if (MessageBox.Show("quit?", "sty works", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                this.Close();
            }
            isQuitShown = false;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        bool isSettingShown = false;
        private void tsmiSetting_Click(object sender, EventArgs e)
        {
            if (isSettingShown)
            {
                return;
            }

            MessageBox.Show("not support now!", "sty works");
            return;

            FrmSetting form = new FrmSetting();
            isSettingShown = true;
            
            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                //save();
            }
            isSettingShown = false;
        }
    }
}
