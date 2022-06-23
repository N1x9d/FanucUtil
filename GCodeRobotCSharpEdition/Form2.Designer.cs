
namespace GCodeRobotCSharpEdition
{
    partial class Form2
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
            this.Print = new System.Windows.Forms.Button();
            this.Await_layer = new System.Windows.Forms.CheckBox();
            this.RobotState = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.MakeTP = new System.Windows.Forms.Button();
            this.StartPrint = new System.Windows.Forms.Button();
            this.Collection = new System.Windows.Forms.ComboBox();
            this.Drop = new System.Windows.Forms.Button();
            this.Next = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.Prev = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.TpAll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Print
            // 
            this.Print.Location = new System.Drawing.Point(273, 69);
            this.Print.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Print.Name = "Print";
            this.Print.Size = new System.Drawing.Size(117, 31);
            this.Print.TabIndex = 0;
            this.Print.Text = " Выбрать папку";
            this.Print.UseVisualStyleBackColor = true;
            this.Print.Click += new System.EventHandler(this.Print_Click);
            // 
            // Await_layer
            // 
            this.Await_layer.AutoSize = true;
            this.Await_layer.Location = new System.Drawing.Point(273, 113);
            this.Await_layer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Await_layer.Name = "Await_layer";
            this.Await_layer.Size = new System.Drawing.Size(105, 24);
            this.Await_layer.TabIndex = 4;
            this.Await_layer.Text = "Await layer";
            this.Await_layer.UseVisualStyleBackColor = true;
            // 
            // RobotState
            // 
            this.RobotState.AutoSize = true;
            this.RobotState.Location = new System.Drawing.Point(31, 33);
            this.RobotState.Name = "RobotState";
            this.RobotState.Size = new System.Drawing.Size(50, 20);
            this.RobotState.TabIndex = 5;
            this.RobotState.Text = "label1";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(31, 69);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(235, 27);
            this.textBox1.TabIndex = 6;
            // 
            // MakeTP
            // 
            this.MakeTP.Location = new System.Drawing.Point(31, 241);
            this.MakeTP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MakeTP.Name = "MakeTP";
            this.MakeTP.Size = new System.Drawing.Size(151, 33);
            this.MakeTP.TabIndex = 8;
            this.MakeTP.Text = "Экспорт в TP";
            this.MakeTP.UseVisualStyleBackColor = true;
            this.MakeTP.Click += new System.EventHandler(this.MakeTP_Click);
            // 
            // StartPrint
            // 
            this.StartPrint.Enabled = false;
            this.StartPrint.Location = new System.Drawing.Point(31, 201);
            this.StartPrint.Name = "StartPrint";
            this.StartPrint.Size = new System.Drawing.Size(151, 33);
            this.StartPrint.TabIndex = 7;
            this.StartPrint.Text = "Начать печать";
            this.StartPrint.UseVisualStyleBackColor = true;
            this.StartPrint.Click += new System.EventHandler(this.button1_Click);
            // 
            // Collection
            // 
            this.Collection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Collection.FormattingEnabled = true;
            this.Collection.Location = new System.Drawing.Point(31, 108);
            this.Collection.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Collection.Name = "Collection";
            this.Collection.Size = new System.Drawing.Size(235, 28);
            this.Collection.TabIndex = 9;
            // 
            // Drop
            // 
            this.Drop.Location = new System.Drawing.Point(31, 288);
            this.Drop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Drop.Name = "Drop";
            this.Drop.Size = new System.Drawing.Size(359, 31);
            this.Drop.TabIndex = 12;
            this.Drop.Text = "Сброс";
            this.Drop.UseVisualStyleBackColor = true;
            this.Drop.Click += new System.EventHandler(this.Drop_Click);
            // 
            // Next
            // 
            this.Next.Location = new System.Drawing.Point(225, 147);
            this.Next.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Next.Name = "Next";
            this.Next.Size = new System.Drawing.Size(41, 31);
            this.Next.TabIndex = 13;
            this.Next.Text = ">>";
            this.Next.UseVisualStyleBackColor = true;
            this.Next.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(109, 147);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(85, 31);
            this.button3.TabIndex = 14;
            this.button3.Text = "Повтор";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Prev
            // 
            this.Prev.Location = new System.Drawing.Point(31, 147);
            this.Prev.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Prev.Name = "Prev";
            this.Prev.Size = new System.Drawing.Size(41, 31);
            this.Prev.TabIndex = 15;
            this.Prev.Text = "<<";
            this.Prev.UseVisualStyleBackColor = true;
            this.Prev.Click += new System.EventHandler(this.Prev_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(405, 23);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(313, 295);
            this.textBox2.TabIndex = 16;
            // 
            // TpAll
            // 
            this.TpAll.AutoSize = true;
            this.TpAll.Location = new System.Drawing.Point(214, 247);
            this.TpAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TpAll.Name = "TpAll";
            this.TpAll.Size = new System.Drawing.Size(183, 24);
            this.TpAll.TabIndex = 17;
            this.TpAll.Text = "Экспортировать один";
            this.TpAll.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 331);
            this.Controls.Add(this.TpAll);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.Prev);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.Next);
            this.Controls.Add(this.Drop);
            this.Controls.Add(this.Collection);
            this.Controls.Add(this.StartPrint);
            this.Controls.Add(this.MakeTP);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.RobotState);
            this.Controls.Add(this.Await_layer);
            this.Controls.Add(this.Print);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form2";
            this.Text = "x";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Print;
        private System.Windows.Forms.CheckBox Await_layer;
        private System.Windows.Forms.Label RobotState;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button MakeTP;
        private System.Windows.Forms.Button StartPrint;
        private System.Windows.Forms.ComboBox Collection;
        private System.Windows.Forms.Button Drop;
        private System.Windows.Forms.Button Next;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button Prev;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.CheckBox TpAll;
    }
}