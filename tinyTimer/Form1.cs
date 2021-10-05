using System;
using System.Threading;
using System.Windows.Forms;

namespace tinyTimer {

    public partial class Form1 : Form {
        static int cnt = 0;
        private static System.Threading.Timer tm;
        static Form1 Instance;
        DateTime beforeTime;
        TimeSpan span = new TimeSpan();

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            Instance = this;

        }

        delegate void delegate1(String text1);
        public void addMsg(string msg) {
            Invoke(new delegate1(FormAddMsg), msg);
        }
        public void FormAddMsg(string msg) {
            label1.Text = msg;
            textBox1.AppendText(msg + "\r\n");
            var hhmmss = span.ToString(@"hh\:mm\:ss");
            label2.Text = hhmmss + " sec";
            span = span.Add(new TimeSpan(0, 0, (int)numericUpDown1.Value));
        }

        // 指定秒数間隔で呼び出される処理
        TimerCallback callback = state => {
            DateTime newTime = DateTime.Now;
            TimeSpan diff = newTime - Instance.beforeTime;

            if ((int)diff.TotalSeconds != (int)Instance.numericUpDown1.Value) {
                Instance.addMsg($"[{++cnt}] " + newTime + " *" + (int)diff.TotalSeconds);
            } else {
                Instance.addMsg($"[{++cnt}] " + newTime);
            }
            Instance.beforeTime = newTime;
        };

        private void button1_Click(object sender, EventArgs e) {
            this.Text = "[START] tinyTimer";
            button1.Enabled = false;
            numericUpDown1.Enabled = false;
            button2.Enabled = true;
            beforeTime = DateTime.Now;
            beforeTime = beforeTime.AddSeconds(-(int)numericUpDown1.Value);
            // タイマー起動
            tm = new System.Threading.Timer(callback, null, 0, (int)numericUpDown1.Value * 1000);
        }

        private void button2_Click(object sender, EventArgs e) {
            this.Text = "[STOP] tinyTimer";
            tm.Dispose();
            button2.Enabled = false;
            numericUpDown1.Enabled = true;
            button1.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e) {
            cnt = 0;
            textBox1.Text = "";
            TimeSpan span = new TimeSpan();

        }
    }
}
