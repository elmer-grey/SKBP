namespace СКБП
{
    partial class SaveFileAs
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
            CheckKoef0 = new CheckBox();
            CheckKoef1 = new CheckBox();
            CheckKoef2 = new CheckBox();
            CheckKoef3 = new CheckBox();
            label1 = new Label();
            groupBox1 = new GroupBox();
            AcceptButton = new Button();
            ReturnButton = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // CheckKoef0
            // 
            CheckKoef0.AutoSize = true;
            CheckKoef0.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            CheckKoef0.Location = new Point(33, 34);
            CheckKoef0.Margin = new Padding(4);
            CheckKoef0.Name = "CheckKoef0";
            CheckKoef0.Size = new Size(299, 25);
            CheckKoef0.TabIndex = 0;
            CheckKoef0.Text = "Коэффициент оборота по приему";
            CheckKoef0.UseVisualStyleBackColor = true;
            // 
            // CheckKoef1
            // 
            CheckKoef1.AutoSize = true;
            CheckKoef1.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            CheckKoef1.Location = new Point(18, 51);
            CheckKoef1.Margin = new Padding(4);
            CheckKoef1.Name = "CheckKoef1";
            CheckKoef1.Size = new Size(317, 25);
            CheckKoef1.TabIndex = 1;
            CheckKoef1.Text = "Коэффициент оборота по выбытию";
            CheckKoef1.UseVisualStyleBackColor = true;
            // 
            // CheckKoef2
            // 
            CheckKoef2.AutoSize = true;
            CheckKoef2.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            CheckKoef2.Location = new Point(18, 84);
            CheckKoef2.Margin = new Padding(4);
            CheckKoef2.Name = "CheckKoef2";
            CheckKoef2.Size = new Size(284, 25);
            CheckKoef2.TabIndex = 2;
            CheckKoef2.Text = "Коэффициент текучести кадров";
            CheckKoef2.UseVisualStyleBackColor = true;
            // 
            // CheckKoef3
            // 
            CheckKoef3.AutoSize = true;
            CheckKoef3.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            CheckKoef3.Location = new Point(18, 117);
            CheckKoef3.Margin = new Padding(4);
            CheckKoef3.Name = "CheckKoef3";
            CheckKoef3.Size = new Size(506, 25);
            CheckKoef3.TabIndex = 3;
            CheckKoef3.Text = "Коэффициент постоянства состава персонала предприятия";
            CheckKoef3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(28, 9);
            label1.Name = "label1";
            label1.Size = new Size(508, 21);
            label1.TabIndex = 4;
            label1.Text = "Отметьте те формулы, которые необходимо сохранить в файл";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(CheckKoef1);
            groupBox1.Controls.Add(CheckKoef3);
            groupBox1.Controls.Add(CheckKoef2);
            groupBox1.Location = new Point(15, 16);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(586, 149);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            // 
            // AcceptButton
            // 
            AcceptButton.Location = new Point(33, 171);
            AcceptButton.Name = "AcceptButton";
            AcceptButton.Size = new Size(145, 50);
            AcceptButton.TabIndex = 6;
            AcceptButton.Text = "Подтвердить выбор";
            AcceptButton.UseVisualStyleBackColor = true;
            AcceptButton.Click += AcceptButton_Click;
            // 
            // ReturnButton
            // 
            ReturnButton.Location = new Point(443, 171);
            ReturnButton.Name = "ReturnButton";
            ReturnButton.Size = new Size(145, 50);
            ReturnButton.TabIndex = 7;
            ReturnButton.Text = "Отменить выбор";
            ReturnButton.UseVisualStyleBackColor = true;
            ReturnButton.Click += ReturnButton_Click;
            // 
            // SaveFileAs
            // 
            AutoScaleDimensions = new SizeF(10F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(613, 228);
            Controls.Add(ReturnButton);
            Controls.Add(AcceptButton);
            Controls.Add(label1);
            Controls.Add(CheckKoef0);
            Controls.Add(groupBox1);
            Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            Margin = new Padding(4);
            Name = "SaveFileAs";
            Text = "Отметьте...";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox CheckKoef0;
        private CheckBox CheckKoef1;
        private CheckBox CheckKoef2;
        private CheckBox CheckKoef3;
        private Label label1;
        private GroupBox groupBox1;
        private Button AcceptButton;
        private Button ReturnButton;
    }
}