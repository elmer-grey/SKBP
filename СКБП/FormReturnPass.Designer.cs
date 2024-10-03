namespace СКБП
{
    partial class FormReturnPass
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
            NameTextBox = new TextBox();
            label1 = new Label();
            groupBox1 = new GroupBox();
            buttonCancel = new Button();
            buttonOk = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // NameTextBox
            // 
            NameTextBox.CharacterCasing = CharacterCasing.Lower;
            NameTextBox.Cursor = Cursors.IBeam;
            NameTextBox.Font = new Font("Times New Roman", 14.25F);
            NameTextBox.Location = new Point(269, 22);
            NameTextBox.Name = "NameTextBox";
            NameTextBox.Size = new Size(186, 29);
            NameTextBox.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Cursor = Cursors.IBeam;
            label1.Font = new Font("Times New Roman", 14.25F);
            label1.Location = new Point(29, 25);
            label1.Name = "label1";
            label1.Size = new Size(227, 21);
            label1.TabIndex = 6;
            label1.Text = "Введите имя пользователя:";
            // 
            // groupBox1
            // 
            groupBox1.BackColor = SystemColors.AppWorkspace;
            groupBox1.Controls.Add(buttonCancel);
            groupBox1.Controls.Add(buttonOk);
            groupBox1.Location = new Point(29, 76);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(426, 100);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            // 
            // buttonCancel
            // 
            buttonCancel.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            buttonCancel.Location = new Point(283, 22);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(137, 59);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "Отмена";
            buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            buttonOk.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            buttonOk.Location = new Point(6, 22);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(137, 59);
            buttonOk.TabIndex = 2;
            buttonOk.Text = "Подтвердить";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // FormReturnPass
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(485, 194);
            Controls.Add(NameTextBox);
            Controls.Add(label1);
            Controls.Add(groupBox1);
            Name = "FormReturnPass";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Форма сброса пароля";
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox NameTextBox;
        private Label label1;
        private GroupBox groupBox1;
        private Button buttonCancel;
        private Button buttonOk;
    }
}