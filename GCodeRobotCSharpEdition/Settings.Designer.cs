
namespace GCodeRobotCSharpEdition
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NextLayerDelay = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.NextLayerBD = new System.Windows.Forms.CheckBox();
            this.Save = new System.Windows.Forms.Button();
            this.Drop = new System.Windows.Forms.Button();
            this.Loading = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NextLayerDelay
            // 
            this.NextLayerDelay.AutoSize = true;
            this.NextLayerDelay.Location = new System.Drawing.Point(23, 34);
            this.NextLayerDelay.Name = "NextLayerDelay";
            this.NextLayerDelay.Size = new System.Drawing.Size(209, 24);
            this.NextLayerDelay.TabIndex = 0;
            this.NextLayerDelay.Text = "Таймер следующего слоя";
            this.NextLayerDelay.UseVisualStyleBackColor = true;
            this.NextLayerDelay.CheckedChanged += new System.EventHandler(this.NextLayerDelay_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(289, 31);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(125, 27);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "60";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(289, 62);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(125, 27);
            this.textBox2.TabIndex = 3;
            this.textBox2.Text = "5";
            // 
            // NextLayerBD
            // 
            this.NextLayerBD.AutoSize = true;
            this.NextLayerBD.Location = new System.Drawing.Point(23, 64);
            this.NextLayerBD.Name = "NextLayerBD";
            this.NextLayerBD.Size = new System.Drawing.Size(251, 24);
            this.NextLayerBD.TabIndex = 2;
            this.NextLayerBD.Text = "Время отмены нажатия кнопки";
            this.NextLayerBD.UseVisualStyleBackColor = true;
            this.NextLayerBD.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(23, 166);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(94, 29);
            this.Save.TabIndex = 4;
            this.Save.Text = "Сохранить";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Drop
            // 
            this.Drop.Location = new System.Drawing.Point(164, 166);
            this.Drop.Name = "Drop";
            this.Drop.Size = new System.Drawing.Size(101, 29);
            this.Drop.TabIndex = 5;
            this.Drop.Text = "Сброс";
            this.Drop.UseVisualStyleBackColor = true;
            this.Drop.Click += new System.EventHandler(this.Drop_Click);
            // 
            // Loading
            // 
            this.Loading.Location = new System.Drawing.Point(320, 166);
            this.Loading.Name = "Loading";
            this.Loading.Size = new System.Drawing.Size(94, 29);
            this.Loading.TabIndex = 6;
            this.Loading.Text = "Загрузить";
            this.Loading.UseVisualStyleBackColor = true;
            this.Loading.Click += new System.EventHandler(this.Load_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 245);
            this.Controls.Add(this.Loading);
            this.Controls.Add(this.Drop);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.NextLayerBD);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.NextLayerDelay);
            this.Name = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox NextLayerDelay;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.CheckBox NextLayerBD;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button Drop;
        private System.Windows.Forms.Button Loading;
    }
}