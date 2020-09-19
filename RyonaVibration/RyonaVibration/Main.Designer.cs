namespace RyonaVibration
{
    partial class Main
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.btnConnect = new System.Windows.Forms.Button();
            this.rtbLogs = new System.Windows.Forms.RichTextBox();
            this.rbSC6 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbAMAZON = new System.Windows.Forms.RadioButton();
            this.rbRRXX = new System.Windows.Forms.RadioButton();
            this.btnEmergency = new System.Windows.Forms.Button();
            this.btnReadMemory = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbRight = new System.Windows.Forms.RadioButton();
            this.rbLeft = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(130, 92);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Pair Sextoy";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // rtbLogs
            // 
            this.rtbLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.rtbLogs.Location = new System.Drawing.Point(12, 195);
            this.rtbLogs.Name = "rtbLogs";
            this.rtbLogs.Size = new System.Drawing.Size(325, 160);
            this.rtbLogs.TabIndex = 1;
            this.rtbLogs.Text = "";
            this.rtbLogs.TextChanged += new System.EventHandler(this.rtbLogs_TextChanged);
            // 
            // rbSC6
            // 
            this.rbSC6.AutoSize = true;
            this.rbSC6.Location = new System.Drawing.Point(6, 19);
            this.rbSC6.Name = "rbSC6";
            this.rbSC6.Size = new System.Drawing.Size(90, 17);
            this.rbSC6.TabIndex = 3;
            this.rbSC6.Text = "Soul Calibur 6";
            this.rbSC6.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbAMAZON);
            this.groupBox1.Controls.Add(this.rbRRXX);
            this.groupBox1.Controls.Add(this.rbSC6);
            this.groupBox1.Location = new System.Drawing.Point(148, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 92);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Game";
            // 
            // rbAMAZON
            // 
            this.rbAMAZON.AutoSize = true;
            this.rbAMAZON.Location = new System.Drawing.Point(6, 65);
            this.rbAMAZON.Name = "rbAMAZON";
            this.rbAMAZON.Size = new System.Drawing.Size(139, 17);
            this.rbAMAZON.TabIndex = 5;
            this.rbAMAZON.Text = "Amazon Brawl Hardcore";
            this.rbAMAZON.UseVisualStyleBackColor = true;
            // 
            // rbRRXX
            // 
            this.rbRRXX.AutoSize = true;
            this.rbRRXX.Checked = true;
            this.rbRRXX.Location = new System.Drawing.Point(6, 42);
            this.rbRRXX.Name = "rbRRXX";
            this.rbRRXX.Size = new System.Drawing.Size(111, 17);
            this.rbRRXX.TabIndex = 4;
            this.rbRRXX.TabStop = true;
            this.rbRRXX.Text = "Rumble Roses XX";
            this.rbRRXX.UseVisualStyleBackColor = true;
            // 
            // btnEmergency
            // 
            this.btnEmergency.BackColor = System.Drawing.Color.Red;
            this.btnEmergency.Location = new System.Drawing.Point(12, 110);
            this.btnEmergency.Name = "btnEmergency";
            this.btnEmergency.Size = new System.Drawing.Size(130, 51);
            this.btnEmergency.TabIndex = 5;
            this.btnEmergency.Text = "Emergency Stop";
            this.btnEmergency.UseVisualStyleBackColor = false;
            this.btnEmergency.Click += new System.EventHandler(this.btnEmergency_Click);
            // 
            // btnReadMemory
            // 
            this.btnReadMemory.Location = new System.Drawing.Point(12, 166);
            this.btnReadMemory.Name = "btnReadMemory";
            this.btnReadMemory.Size = new System.Drawing.Size(130, 23);
            this.btnReadMemory.TabIndex = 6;
            this.btnReadMemory.Text = "Hook to Game";
            this.btnReadMemory.UseVisualStyleBackColor = true;
            this.btnReadMemory.Click += new System.EventHandler(this.btnReadMemory_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbRight);
            this.groupBox2.Controls.Add(this.rbLeft);
            this.groupBox2.Location = new System.Drawing.Point(154, 119);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 70);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Player Side";
            // 
            // rbRight
            // 
            this.rbRight.AutoSize = true;
            this.rbRight.Location = new System.Drawing.Point(6, 42);
            this.rbRight.Name = "rbRight";
            this.rbRight.Size = new System.Drawing.Size(50, 17);
            this.rbRight.TabIndex = 4;
            this.rbRight.Text = "Right";
            this.rbRight.UseVisualStyleBackColor = true;
            // 
            // rbLeft
            // 
            this.rbLeft.AutoSize = true;
            this.rbLeft.Checked = true;
            this.rbLeft.Location = new System.Drawing.Point(6, 19);
            this.rbLeft.Name = "rbLeft";
            this.rbLeft.Size = new System.Drawing.Size(43, 17);
            this.rbLeft.TabIndex = 3;
            this.rbLeft.TabStop = true;
            this.rbLeft.Text = "Left";
            this.rbLeft.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 360);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnReadMemory);
            this.Controls.Add(this.btnEmergency);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.rtbLogs);
            this.Controls.Add(this.btnConnect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "Ryona Vibrations";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.RichTextBox rtbLogs;
        private System.Windows.Forms.RadioButton rbSC6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbRRXX;
        private System.Windows.Forms.RadioButton rbAMAZON;
        private System.Windows.Forms.Button btnEmergency;
        private System.Windows.Forms.Button btnReadMemory;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbRight;
        private System.Windows.Forms.RadioButton rbLeft;
    }
}

