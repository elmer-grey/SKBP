namespace СКБП
{
    partial class FormAuthorization
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
            components = new System.ComponentModel.Container();
            SignButton = new Button();
            PassTextBox = new TextBox();
            NameTextBox = new TextBox();
            label2 = new Label();
            label1 = new Label();
            ChangeLabel = new LinkLabel();
            checkBox = new CheckBox();
            bindingSource1 = new BindingSource(components);
            button1 = new Button();
            textBox1 = new TextBox();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            SuspendLayout();
            // 
            // SignButton
            // 
            SignButton.Font = new Font("Times New Roman", 14.25F);
            SignButton.Location = new Point(203, 142);
            SignButton.Name = "SignButton";
            SignButton.Size = new Size(125, 37);
            SignButton.TabIndex = 3;
            SignButton.Text = "Вход";
            SignButton.UseVisualStyleBackColor = true;
            SignButton.Click += SignButton_Click;
            // 
            // PassTextBox
            // 
            PassTextBox.BorderStyle = BorderStyle.None;
            PassTextBox.Cursor = Cursors.IBeam;
            PassTextBox.Font = new Font("Times New Roman", 14.25F);
            PassTextBox.Location = new Point(293, 87);
            PassTextBox.Name = "PassTextBox";
            PassTextBox.PasswordChar = '*';
            PassTextBox.Size = new Size(161, 22);
            PassTextBox.TabIndex = 2;
            PassTextBox.WordWrap = false;
            PassTextBox.TextChanged += PassTextBox_TextChanged;
            PassTextBox.Enter += PassTextBox_Enter;
            // 
            // NameTextBox
            // 
            NameTextBox.CharacterCasing = CharacterCasing.Lower;
            NameTextBox.Cursor = Cursors.IBeam;
            NameTextBox.Font = new Font("Times New Roman", 14.25F);
            NameTextBox.Location = new Point(291, 36);
            NameTextBox.Name = "NameTextBox";
            NameTextBox.Size = new Size(192, 29);
            NameTextBox.TabIndex = 1;
            NameTextBox.TextChanged += NameTextBox_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Cursor = Cursors.IBeam;
            label2.Font = new Font("Times New Roman", 14.25F);
            label2.Location = new Point(27, 86);
            label2.Name = "label2";
            label2.Size = new Size(140, 21);
            label2.TabIndex = 7;
            label2.Text = "Введите пароль:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Cursor = Cursors.IBeam;
            label1.Font = new Font("Times New Roman", 14.25F);
            label1.Location = new Point(27, 39);
            label1.Name = "label1";
            label1.Size = new Size(227, 21);
            label1.TabIndex = 5;
            label1.Text = "Введите имя пользователя:";
            // 
            // ChangeLabel
            // 
            ChangeLabel.AutoSize = true;
            ChangeLabel.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            ChangeLabel.Location = new Point(144, 182);
            ChangeLabel.Name = "ChangeLabel";
            ChangeLabel.Size = new Size(240, 21);
            ChangeLabel.TabIndex = 4;
            ChangeLabel.TabStop = true;
            ChangeLabel.Text = "Сброс пароля пользователю";
            ChangeLabel.Click += ChangeLabel_Click;
            // 
            // checkBox
            // 
            checkBox.AutoSize = true;
            checkBox.Location = new Point(291, 118);
            checkBox.Name = "checkBox";
            checkBox.Size = new Size(129, 19);
            checkBox.TabIndex = 8;
            checkBox.Text = "Сохранить данные";
            checkBox.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.BackgroundImageLayout = ImageLayout.None;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = SystemColors.Control;
            button1.Image = Properties.Resources.Открыто;
            button1.Location = new Point(453, 86);
            button1.Name = "button1";
            button1.Size = new Size(28, 23);
            button1.TabIndex = 9;
            button1.TabStop = false;
            button1.TextImageRelation = TextImageRelation.TextAboveImage;
            button1.UseVisualStyleBackColor = true;
            button1.MouseDown += button1_MouseDown;
            button1.MouseUp += button1_MouseUp;
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Cursor = Cursors.IBeam;
            textBox1.Font = new Font("Times New Roman", 14.25F);
            textBox1.Location = new Point(291, 83);
            textBox1.Name = "textBox1";
            textBox1.PasswordChar = '*';
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(192, 29);
            textBox1.TabIndex = 10;
            textBox1.WordWrap = false;
            // 
            // FormAuthorization
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(509, 212);
            Controls.Add(button1);
            Controls.Add(checkBox);
            Controls.Add(ChangeLabel);
            Controls.Add(PassTextBox);
            Controls.Add(NameTextBox);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(SignButton);
            Controls.Add(textBox1);
            Name = "FormAuthorization";
            Text = "Форма авторизации";
            FormClosing += FormAuthorization_FormClosing;
            FormClosed += FormAuthorization_FormClosed;
            Load += FormAuthorization_Load;
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button SignButton;
        private TextBox PassTextBox;
        private TextBox NameTextBox;
        private Label label2;
        private Label label1;
        private LinkLabel ChangeLabel;
        private CheckBox checkBox;
        private BindingSource bindingSource1;
        private Button button1;
        private TextBox textBox1;
    }
}