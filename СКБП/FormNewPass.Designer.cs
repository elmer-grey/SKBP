namespace СКБП
{
    partial class FormNewPass
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            buttonCancel = new Button();
            buttonOk = new Button();
            label2 = new Label();
            label3 = new Label();
            OldPassTextBox = new TextBox();
            button1 = new Button();
            NewPassTextBox = new TextBox();
            textBox1 = new TextBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = SystemColors.AppWorkspace;
            groupBox1.Controls.Add(buttonCancel);
            groupBox1.Controls.Add(buttonOk);
            groupBox1.Location = new Point(27, 108);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(426, 100);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            // 
            // buttonCancel
            // 
            buttonCancel.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            buttonCancel.Location = new Point(283, 22);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(137, 59);
            buttonCancel.TabIndex = 4;
            buttonCancel.Text = "Отмена";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonOk
            // 
            buttonOk.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            buttonOk.Location = new Point(6, 22);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(137, 59);
            buttonOk.TabIndex = 3;
            buttonOk.Text = "Подтвердить";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Cursor = Cursors.IBeam;
            label2.Font = new Font("Times New Roman", 14.25F);
            label2.Location = new Point(27, 20);
            label2.Name = "label2";
            label2.Size = new Size(203, 21);
            label2.TabIndex = 2;
            label2.Text = "Введите старый пароль:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Cursor = Cursors.IBeam;
            label3.Font = new Font("Times New Roman", 14.25F);
            label3.Location = new Point(27, 55);
            label3.Name = "label3";
            label3.Size = new Size(197, 21);
            label3.TabIndex = 3;
            label3.Text = "Введите новый пароль:";
            // 
            // OldPassTextBox
            // 
            OldPassTextBox.Cursor = Cursors.IBeam;
            OldPassTextBox.Font = new Font("Times New Roman", 14.25F);
            OldPassTextBox.Location = new Point(265, 17);
            OldPassTextBox.Name = "OldPassTextBox";
            OldPassTextBox.Size = new Size(192, 29);
            OldPassTextBox.TabIndex = 1;
            // 
            // button1
            // 
            button1.BackgroundImageLayout = ImageLayout.None;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = SystemColors.Control;
            button1.Image = Properties.Resources.Скрыто;
            button1.Location = new Point(427, 54);
            button1.Name = "button1";
            button1.Size = new Size(28, 23);
            button1.TabIndex = 12;
            button1.TabStop = false;
            button1.TextImageRelation = TextImageRelation.TextAboveImage;
            button1.UseVisualStyleBackColor = true;
            button1.MouseDown += button1_MouseDown;
            button1.MouseUp += button1_MouseUp;
            // 
            // NewPassTextBox
            // 
            NewPassTextBox.BorderStyle = BorderStyle.None;
            NewPassTextBox.Cursor = Cursors.IBeam;
            NewPassTextBox.Font = new Font("Times New Roman", 14.25F);
            NewPassTextBox.Location = new Point(267, 55);
            NewPassTextBox.Name = "NewPassTextBox";
            NewPassTextBox.PasswordChar = '*';
            NewPassTextBox.Size = new Size(161, 22);
            NewPassTextBox.TabIndex = 2;
            NewPassTextBox.WordWrap = false;
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Cursor = Cursors.IBeam;
            textBox1.Font = new Font("Times New Roman", 14.25F);
            textBox1.Location = new Point(265, 51);
            textBox1.Name = "textBox1";
            textBox1.PasswordChar = '*';
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(192, 29);
            textBox1.TabIndex = 13;
            textBox1.WordWrap = false;
            // 
            // FormNewPass
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(485, 218);
            Controls.Add(button1);
            Controls.Add(NewPassTextBox);
            Controls.Add(textBox1);
            Controls.Add(OldPassTextBox);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(groupBox1);
            Name = "FormNewPass";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Задание нового пароля";
            Load += FormNewPass_Load;
            Paint += FormNewPass_Paint;
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonOk;
        private Label label2;
        private Label label3;
        private Button buttonCancel;
        private TextBox OldPassTextBox;
        private GroupBox groupBox1;
        private Button button1;
        private TextBox NewPassTextBox;
        private TextBox textBox1;
    }
}
