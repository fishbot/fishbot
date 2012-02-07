namespace WindowsFormsApplication1
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtKey = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.txtSens = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.txtLurk = new System.Windows.Forms.ToolStripTextBox();
            this.ScreenPath = new System.Windows.Forms.SaveFileDialog();
            this.trackSens = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackSens)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripLabel1,
            this.txtKey,
            this.toolStripLabel2,
            this.txtSens,
            this.toolStripLabel3,
            this.txtLurk});
            this.toolStrip1.Location = new System.Drawing.Point(0, 376);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(556, 29);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 26);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(70, 26);
            this.toolStripLabel1.Text = "Fish Button:";
            // 
            // txtKey
            // 
            this.txtKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKey.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtKey.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtKey.MaxLength = 1;
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(25, 29);
            this.txtKey.Text = "t";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(63, 26);
            this.toolStripLabel2.Text = "Sensitivity:";
            // 
            // txtSens
            // 
            this.txtSens.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSens.Name = "txtSens";
            this.txtSens.Size = new System.Drawing.Size(50, 29);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(72, 26);
            this.toolStripLabel3.Text = "Lurk Button:";
            // 
            // txtLurk
            // 
            this.txtLurk.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLurk.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtLurk.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtLurk.MaxLength = 1;
            this.txtLurk.Name = "txtLurk";
            this.txtLurk.Size = new System.Drawing.Size(25, 29);
            this.txtLurk.Text = "q";
            // 
            // ScreenPath
            // 
            this.ScreenPath.CreatePrompt = true;
            // 
            // trackSens
            // 
            this.trackSens.BackColor = System.Drawing.Color.White;
            this.trackSens.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.trackSens.LargeChange = 1;
            this.trackSens.Location = new System.Drawing.Point(0, 331);
            this.trackSens.Maximum = 40;
            this.trackSens.Minimum = 1;
            this.trackSens.Name = "trackSens";
            this.trackSens.Size = new System.Drawing.Size(556, 45);
            this.trackSens.TabIndex = 2;
            this.trackSens.Value = 20;
            this.trackSens.ValueChanged += new System.EventHandler(this.trackSens_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(435, 366);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 39);
            this.label1.TabIndex = 3;
            this.label1.Text = "F10 - increase sensivity\r\nF11 - decrease sensivity\r\nF12 - start/stop fishing";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lime;
            this.ClientSize = new System.Drawing.Size(556, 405);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackSens);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.Text = "BT";
            this.TransparencyKey = System.Drawing.Color.Lime;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackSens)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.SaveFileDialog ScreenPath;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox txtKey;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.TrackBar trackSens;
        private System.Windows.Forms.ToolStripTextBox txtSens;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox txtLurk;
        private System.Windows.Forms.Label label1;
    }
}

