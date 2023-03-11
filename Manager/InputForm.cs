using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manager.Page
{
    internal class InputForm : Form
    {
        private Label label1;
        private Button button1;
        private TextBox textBox1;

        public InputForm() 
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(70, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(167, 21);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "输入ID";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(162, 67);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "查询";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // InputForm
            // 
            this.ClientSize = new System.Drawing.Size(249, 102);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Name = "InputForm";
            this.Text = "查询窗口";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public int QueryId { get; private set; }

        private void button1_Click(object sender, EventArgs e)
        {
            int id = 0;
            if (string.IsNullOrWhiteSpace(textBox1.Text) || !int.TryParse(textBox1.Text, out id)) 
            {
                MessageBox.Show("输入错误");
            }

            QueryId = id;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
