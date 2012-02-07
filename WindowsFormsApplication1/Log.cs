using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class FrmLog : Form
    {
        public FrmLog()
        {
            InitializeComponent();
        }

        public void AddLine(string sLogLine)
        {
            LogTextBox.AppendText(sLogLine);
            LogTextBox.ScrollToCaret();
        }

        public void LogClear()
        {
            LogTextBox.Clear();
        }
    }
}
