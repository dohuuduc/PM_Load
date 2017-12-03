using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
//https://stackoverflow.com/questions/1918158/how-do-i-show-a-loading-please-wait-message-in-winforms-for-a-long-loadi
namespace Load
{
    public partial class Waiting : Form
    {
        private readonly MethodInvoker method;
        private readonly DateTime TimeBatDau = DateTime.MinValue;
        System.Timers.Timer aTimer = new System.Timers.Timer();
        

        public Waiting(MethodInvoker action,string tieude=null)
        {
            InitializeComponent();
            method = action;
            if (tieude != null)
                lbltieude.Text = tieude;
            TimeBatDau = DateTime.Now;

            
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 1000;
            aTimer.Enabled = true;

        }

        // Specify what you want to happen when the Elapsed event is raised.
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            timer();
        }

        private void timer() {
            TimeSpan timeSpan = DateTime.Now - TimeBatDau;

            //Fixed window handle issue.
            if (!txtThoiGian.IsHandleCreated)
                txtThoiGian.CreateControl();
            //Update the label using delegate method
            txtThoiGian.Invoke((Action)delegate
            {
                txtThoiGian.Text = string.Concat(new string[] { timeSpan.Hours.ToString("D2"), ":", timeSpan.Minutes.ToString("D2"), ":", timeSpan.Seconds.ToString("D2") });
            });
        }
        private void Waiting_Load(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                method.Invoke();
                InvokeAction(this, Dispose);
            }).Start();

            
        }
        public static void InvokeAction(Control control, MethodInvoker action)
        {
            if (control.InvokeRequired)
            {
                 control.BeginInvoke(action);
                 Control.CheckForIllegalCrossThreadCalls = false;
            }
            else
            {
                action();
            }
        }

        private void Waiting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (aTimer != null)
                aTimer.Stop();
        }
    }
}
