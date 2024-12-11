namespace СКБП
{
    partial class FormFormules
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
        public void InitializeComponent()
        {
            comboBoxFormules = new ComboBox();
            koefgroup2 = new GroupBox();
            label14 = new Label();
            groupBox2 = new GroupBox();
            YearForwardText2 = new Label();
            YearAgoText2 = new Label();
            YearForward2 = new RadioButton();
            YearAgo2 = new RadioButton();
            label13 = new Label();
            LevelComboBox2 = new ComboBox();
            label15 = new Label();
            dateTimePicker2 = new DateTimePicker();
            label16 = new Label();
            resultKoef2 = new TextBox();
            label6 = new Label();
            amountKoef2 = new TextBox();
            label7 = new Label();
            SChRText2 = new TextBox();
            koefgroup3 = new GroupBox();
            label12 = new Label();
            LevelComboBox3 = new ComboBox();
            label11 = new Label();
            label8 = new Label();
            SChRText3 = new TextBox();
            dateTimePicker3 = new DateTimePicker();
            resultKoef3 = new TextBox();
            label5 = new Label();
            label9 = new Label();
            amountKoef3 = new TextBox();
            label10 = new Label();
            groupBox1 = new GroupBox();
            YearForwardText3 = new Label();
            YearAgoText3 = new Label();
            YearForward3 = new RadioButton();
            YearAgo3 = new RadioButton();
            koefgroup0 = new GroupBox();
            label25 = new Label();
            LevelComboBox0 = new ComboBox();
            label26 = new Label();
            label27 = new Label();
            SChRText0 = new TextBox();
            dateTimePicker0 = new DateTimePicker();
            label28 = new Label();
            groupBox4 = new GroupBox();
            YearForwardText0 = new Label();
            YearAgoText0 = new Label();
            YearForward0 = new RadioButton();
            YearAgo0 = new RadioButton();
            resultKoef0 = new TextBox();
            label1 = new Label();
            amountKoef0 = new TextBox();
            label2 = new Label();
            koefgroup1 = new GroupBox();
            label19 = new Label();
            LevelComboBox1 = new ComboBox();
            label20 = new Label();
            label21 = new Label();
            SChRText1 = new TextBox();
            dateTimePicker1 = new DateTimePicker();
            label22 = new Label();
            groupBox3 = new GroupBox();
            YearForwardText1 = new Label();
            YearAgoText1 = new Label();
            YearForward1 = new RadioButton();
            YearAgo1 = new RadioButton();
            resultKoef1 = new TextBox();
            label3 = new Label();
            amountKoef1 = new TextBox();
            label4 = new Label();
            ExportButton = new Button();
            nameform = new Label();
            koefgroup2.SuspendLayout();
            groupBox2.SuspendLayout();
            koefgroup3.SuspendLayout();
            groupBox1.SuspendLayout();
            koefgroup0.SuspendLayout();
            groupBox4.SuspendLayout();
            koefgroup1.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // comboBoxFormules
            // 
            comboBoxFormules.Font = new Font("Times New Roman", 14.25F);
            comboBoxFormules.ImeMode = ImeMode.On;
            comboBoxFormules.Items.AddRange(new object[] { "-", "Коэффициент оборота по приему", "Коэффициент оборота по выбытию", "Коэффициент текучести кадров", "Коэффициент постоянства состава персонала предприятия" });
            comboBoxFormules.Location = new Point(24, 30);
            comboBoxFormules.Name = "comboBoxFormules";
            comboBoxFormules.Size = new Size(542, 29);
            comboBoxFormules.TabIndex = 1;
            comboBoxFormules.TabStop = false;
            comboBoxFormules.Text = "Выберите формулу:";
            comboBoxFormules.SelectionChangeCommitted += comboBoxFormules_SelectionChangeCommitted;
            comboBoxFormules.KeyDown += comboBoxFormule_KeyDown;
            // 
            // koefgroup2
            // 
            koefgroup2.Controls.Add(label14);
            koefgroup2.Controls.Add(groupBox2);
            koefgroup2.Controls.Add(label13);
            koefgroup2.Controls.Add(LevelComboBox2);
            koefgroup2.Controls.Add(label15);
            koefgroup2.Controls.Add(dateTimePicker2);
            koefgroup2.Controls.Add(label16);
            koefgroup2.Controls.Add(resultKoef2);
            koefgroup2.Controls.Add(label6);
            koefgroup2.Controls.Add(amountKoef2);
            koefgroup2.Controls.Add(label7);
            koefgroup2.Controls.Add(SChRText2);
            koefgroup2.Location = new Point(29, 350);
            koefgroup2.Name = "koefgroup2";
            koefgroup2.Size = new Size(579, 290);
            koefgroup2.TabIndex = 9;
            koefgroup2.TabStop = false;
            koefgroup2.Visible = false;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label14.Location = new Point(295, 72);
            label14.Name = "label14";
            label14.Size = new Size(211, 21);
            label14.TabIndex = 18;
            label14.Text = "Конец периода подсчёта:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(YearForwardText2);
            groupBox2.Controls.Add(YearAgoText2);
            groupBox2.Controls.Add(YearForward2);
            groupBox2.Controls.Add(YearAgo2);
            groupBox2.Location = new Point(289, 83);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(284, 70);
            groupBox2.TabIndex = 19;
            groupBox2.TabStop = false;
            // 
            // YearForwardText2
            // 
            YearForwardText2.AutoSize = true;
            YearForwardText2.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            YearForwardText2.Location = new Point(128, 45);
            YearForwardText2.Name = "YearForwardText2";
            YearForwardText2.Size = new Size(53, 19);
            YearForwardText2.TabIndex = 3;
            YearForwardText2.Text = "label13";
            YearForwardText2.Visible = false;
            // 
            // YearAgoText2
            // 
            YearAgoText2.AutoSize = true;
            YearAgoText2.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            YearAgoText2.Location = new Point(117, 24);
            YearAgoText2.Name = "YearAgoText2";
            YearAgoText2.Size = new Size(53, 19);
            YearAgoText2.TabIndex = 2;
            YearAgoText2.Text = "label12";
            YearAgoText2.Visible = false;
            // 
            // YearForward2
            // 
            YearForward2.AutoSize = true;
            YearForward2.Font = new Font("Times New Roman", 14.25F);
            YearForward2.Location = new Point(6, 42);
            YearForward2.Name = "YearForward2";
            YearForward2.Size = new Size(116, 25);
            YearForward2.TabIndex = 1;
            YearForward2.Text = "Год вперёд";
            YearForward2.UseVisualStyleBackColor = true;
            YearForward2.CheckedChanged += YearForward2_CheckedChanged_1;
            // 
            // YearAgo2
            // 
            YearAgo2.AutoSize = true;
            YearAgo2.Font = new Font("Times New Roman", 14.25F);
            YearAgo2.Location = new Point(6, 21);
            YearAgo2.Name = "YearAgo2";
            YearAgo2.Size = new Size(105, 25);
            YearAgo2.TabIndex = 0;
            YearAgo2.Text = "Год назад";
            YearAgo2.UseVisualStyleBackColor = true;
            YearAgo2.CheckedChanged += YearAgo2_CheckedChanged_1;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label13.Location = new Point(6, 163);
            label13.Name = "label13";
            label13.Size = new Size(242, 21);
            label13.TabIndex = 21;
            label13.Text = "Рассматриваемая должность";
            // 
            // LevelComboBox2
            // 
            LevelComboBox2.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            LevelComboBox2.FormattingEnabled = true;
            LevelComboBox2.Items.AddRange(new object[] { "Начальник отдела", "Менеджер", "Бухгалтер" });
            LevelComboBox2.Location = new Point(295, 160);
            LevelComboBox2.Name = "LevelComboBox2";
            LevelComboBox2.Size = new Size(278, 29);
            LevelComboBox2.TabIndex = 20;
            LevelComboBox2.Text = "-";
            LevelComboBox2.SelectedIndexChanged += LevelComboBox2_SelectedIndexChanged;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label15.Location = new Point(6, 70);
            label15.Name = "label15";
            label15.Size = new Size(218, 21);
            label15.TabIndex = 17;
            label15.Text = "Начало периода подсчёта:";
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.CustomFormat = "";
            dateTimePicker2.Font = new Font("Times New Roman", 14.25F);
            dateTimePicker2.Location = new Point(6, 107);
            dateTimePicker2.MaxDate = new DateTime(2200, 12, 31, 0, 0, 0, 0);
            dateTimePicker2.MinDate = new DateTime(1900, 1, 1, 0, 0, 0, 0);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(208, 29);
            dateTimePicker2.TabIndex = 16;
            dateTimePicker2.Value = new DateTime(2024, 6, 5, 0, 0, 0, 0);
            dateTimePicker2.ValueChanged += dateTimePicker2_ValueChanged;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label16.Location = new Point(6, 30);
            label16.Name = "label16";
            label16.Size = new Size(46, 21);
            label16.TabIndex = 15;
            label16.Text = "СЧР";
            // 
            // resultKoef2
            // 
            resultKoef2.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            resultKoef2.Location = new Point(295, 255);
            resultKoef2.Name = "resultKoef2";
            resultKoef2.ReadOnly = true;
            resultKoef2.Size = new Size(278, 29);
            resultKoef2.TabIndex = 7;
            resultKoef2.Text = "0";
            resultKoef2.TextChanged += resultKoef2_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label6.Location = new Point(6, 258);
            label6.Name = "label6";
            label6.Size = new Size(86, 21);
            label6.TabIndex = 6;
            label6.Text = "Результат";
            // 
            // amountKoef2
            // 
            amountKoef2.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            amountKoef2.Location = new Point(295, 209);
            amountKoef2.Name = "amountKoef2";
            amountKoef2.ReadOnly = true;
            amountKoef2.Size = new Size(278, 29);
            amountKoef2.TabIndex = 5;
            amountKoef2.TextChanged += amountKoef2_TextChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label7.Location = new Point(6, 195);
            label7.Name = "label7";
            label7.Size = new Size(283, 63);
            label7.TabIndex = 4;
            label7.Text = "Кол−во уволившихся \r\nпо собственному желанию и за\r\nнарушение трудовой дисциплины\r\n";
            // 
            // SChRText2
            // 
            SChRText2.BackColor = SystemColors.Info;
            SChRText2.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            SChRText2.HideSelection = false;
            SChRText2.Location = new Point(295, 20);
            SChRText2.Name = "SChRText2";
            SChRText2.ReadOnly = true;
            SChRText2.ShortcutsEnabled = false;
            SChRText2.Size = new Size(278, 29);
            SChRText2.TabIndex = 14;
            SChRText2.TabStop = false;
            // 
            // koefgroup3
            // 
            koefgroup3.Controls.Add(label12);
            koefgroup3.Controls.Add(LevelComboBox3);
            koefgroup3.Controls.Add(label11);
            koefgroup3.Controls.Add(label8);
            koefgroup3.Controls.Add(SChRText3);
            koefgroup3.Controls.Add(dateTimePicker3);
            koefgroup3.Controls.Add(resultKoef3);
            koefgroup3.Controls.Add(label5);
            koefgroup3.Controls.Add(label9);
            koefgroup3.Controls.Add(amountKoef3);
            koefgroup3.Controls.Add(label10);
            koefgroup3.Controls.Add(groupBox1);
            koefgroup3.Location = new Point(29, 54);
            koefgroup3.Name = "koefgroup3";
            koefgroup3.Size = new Size(579, 290);
            koefgroup3.TabIndex = 10;
            koefgroup3.TabStop = false;
            koefgroup3.Visible = false;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label12.Location = new Point(6, 163);
            label12.Name = "label12";
            label12.Size = new Size(242, 21);
            label12.TabIndex = 13;
            label12.Text = "Рассматриваемая должность";
            // 
            // LevelComboBox3
            // 
            LevelComboBox3.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            LevelComboBox3.FormattingEnabled = true;
            LevelComboBox3.Items.AddRange(new object[] { "Начальник отдела", "Менеджер", "Бухгалтер" });
            LevelComboBox3.Location = new Point(295, 160);
            LevelComboBox3.Name = "LevelComboBox3";
            LevelComboBox3.Size = new Size(278, 29);
            LevelComboBox3.TabIndex = 12;
            LevelComboBox3.Text = "-";
            LevelComboBox3.SelectedIndexChanged += LevelComboBox3_SelectedIndexChanged;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label11.Location = new Point(295, 72);
            label11.Name = "label11";
            label11.Size = new Size(211, 21);
            label11.TabIndex = 10;
            label11.Text = "Конец периода подсчёта:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label8.Location = new Point(6, 70);
            label8.Name = "label8";
            label8.Size = new Size(218, 21);
            label8.TabIndex = 9;
            label8.Text = "Начало периода подсчёта:";
            // 
            // SChRText3
            // 
            SChRText3.BackColor = SystemColors.Info;
            SChRText3.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            SChRText3.HideSelection = false;
            SChRText3.Location = new Point(295, 20);
            SChRText3.Name = "SChRText3";
            SChRText3.ReadOnly = true;
            SChRText3.ShortcutsEnabled = false;
            SChRText3.Size = new Size(278, 29);
            SChRText3.TabIndex = 1;
            SChRText3.TabStop = false;
            // 
            // dateTimePicker3
            // 
            dateTimePicker3.CustomFormat = "";
            dateTimePicker3.Font = new Font("Times New Roman", 14.25F);
            dateTimePicker3.Location = new Point(6, 107);
            dateTimePicker3.MaxDate = new DateTime(2200, 12, 31, 0, 0, 0, 0);
            dateTimePicker3.MinDate = new DateTime(1900, 1, 1, 0, 0, 0, 0);
            dateTimePicker3.Name = "dateTimePicker3";
            dateTimePicker3.Size = new Size(208, 29);
            dateTimePicker3.TabIndex = 8;
            dateTimePicker3.Value = new DateTime(2024, 6, 5, 0, 0, 0, 0);
            dateTimePicker3.ValueChanged += dateTimePicker3_ValueChanged;
            // 
            // resultKoef3
            // 
            resultKoef3.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            resultKoef3.HideSelection = false;
            resultKoef3.Location = new Point(295, 255);
            resultKoef3.Name = "resultKoef3";
            resultKoef3.ReadOnly = true;
            resultKoef3.ShortcutsEnabled = false;
            resultKoef3.Size = new Size(278, 29);
            resultKoef3.TabIndex = 7;
            resultKoef3.TabStop = false;
            resultKoef3.Text = "0";
            resultKoef3.TextChanged += resultKoef3_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label5.Location = new Point(6, 30);
            label5.Name = "label5";
            label5.Size = new Size(46, 21);
            label5.TabIndex = 2;
            label5.Text = "СЧР";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label9.Location = new Point(6, 258);
            label9.Name = "label9";
            label9.Size = new Size(86, 21);
            label9.TabIndex = 6;
            label9.Text = "Результат";
            // 
            // amountKoef3
            // 
            amountKoef3.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            amountKoef3.Location = new Point(295, 209);
            amountKoef3.Name = "amountKoef3";
            amountKoef3.ReadOnly = true;
            amountKoef3.Size = new Size(278, 29);
            amountKoef3.TabIndex = 5;
            amountKoef3.TextChanged += amountKoef3_TextChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label10.Location = new Point(6, 202);
            label10.Name = "label10";
            label10.Size = new Size(208, 42);
            label10.TabIndex = 4;
            label10.Text = "Кол-во работников\r\nпроработавших весь год\r\n";
            label10.Click += label10_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(YearForwardText3);
            groupBox1.Controls.Add(YearAgoText3);
            groupBox1.Controls.Add(YearForward3);
            groupBox1.Controls.Add(YearAgo3);
            groupBox1.Location = new Point(289, 83);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(284, 70);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            // 
            // YearForwardText3
            // 
            YearForwardText3.AutoSize = true;
            YearForwardText3.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            YearForwardText3.Location = new Point(128, 37);
            YearForwardText3.Name = "YearForwardText3";
            YearForwardText3.Size = new Size(53, 19);
            YearForwardText3.TabIndex = 3;
            YearForwardText3.Text = "label13";
            YearForwardText3.Visible = false;
            // 
            // YearAgoText3
            // 
            YearAgoText3.AutoSize = true;
            YearAgoText3.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            YearAgoText3.Location = new Point(117, 16);
            YearAgoText3.Name = "YearAgoText3";
            YearAgoText3.Size = new Size(53, 19);
            YearAgoText3.TabIndex = 2;
            YearAgoText3.Text = "label12";
            YearAgoText3.Visible = false;
            // 
            // YearForward3
            // 
            YearForward3.AutoSize = true;
            YearForward3.Font = new Font("Times New Roman", 14.25F);
            YearForward3.Location = new Point(6, 34);
            YearForward3.Name = "YearForward3";
            YearForward3.Size = new Size(116, 25);
            YearForward3.TabIndex = 1;
            YearForward3.Text = "Год вперёд";
            YearForward3.UseVisualStyleBackColor = true;
            YearForward3.CheckedChanged += YearForward3_CheckedChanged_1;
            // 
            // YearAgo3
            // 
            YearAgo3.AutoSize = true;
            YearAgo3.Font = new Font("Times New Roman", 14.25F);
            YearAgo3.Location = new Point(6, 13);
            YearAgo3.Name = "YearAgo3";
            YearAgo3.Size = new Size(105, 25);
            YearAgo3.TabIndex = 0;
            YearAgo3.Text = "Год назад";
            YearAgo3.UseVisualStyleBackColor = true;
            YearAgo3.CheckedChanged += YearAgo3_CheckedChanged_1;
            // 
            // koefgroup0
            // 
            koefgroup0.Controls.Add(label25);
            koefgroup0.Controls.Add(LevelComboBox0);
            koefgroup0.Controls.Add(label26);
            koefgroup0.Controls.Add(label27);
            koefgroup0.Controls.Add(SChRText0);
            koefgroup0.Controls.Add(dateTimePicker0);
            koefgroup0.Controls.Add(label28);
            koefgroup0.Controls.Add(groupBox4);
            koefgroup0.Controls.Add(resultKoef0);
            koefgroup0.Controls.Add(label1);
            koefgroup0.Controls.Add(amountKoef0);
            koefgroup0.Controls.Add(label2);
            koefgroup0.Location = new Point(677, 43);
            koefgroup0.Name = "koefgroup0";
            koefgroup0.Size = new Size(579, 290);
            koefgroup0.TabIndex = 11;
            koefgroup0.TabStop = false;
            koefgroup0.Visible = false;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label25.Location = new Point(6, 163);
            label25.Name = "label25";
            label25.Size = new Size(242, 21);
            label25.TabIndex = 21;
            label25.Text = "Рассматриваемая должность";
            // 
            // LevelComboBox0
            // 
            LevelComboBox0.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            LevelComboBox0.FormattingEnabled = true;
            LevelComboBox0.Items.AddRange(new object[] { "Начальник отдела", "Менеджер", "Бухгалтер" });
            LevelComboBox0.Location = new Point(295, 160);
            LevelComboBox0.Name = "LevelComboBox0";
            LevelComboBox0.Size = new Size(278, 29);
            LevelComboBox0.TabIndex = 20;
            LevelComboBox0.Text = "-";
            LevelComboBox0.SelectedIndexChanged += LevelComboBox0_SelectedIndexChanged;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label26.Location = new Point(295, 72);
            label26.Name = "label26";
            label26.Size = new Size(211, 21);
            label26.TabIndex = 18;
            label26.Text = "Конец периода подсчёта:";
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label27.Location = new Point(6, 70);
            label27.Name = "label27";
            label27.Size = new Size(218, 21);
            label27.TabIndex = 17;
            label27.Text = "Начало периода подсчёта:";
            // 
            // SChRText0
            // 
            SChRText0.BackColor = SystemColors.Info;
            SChRText0.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            SChRText0.HideSelection = false;
            SChRText0.Location = new Point(295, 20);
            SChRText0.Name = "SChRText0";
            SChRText0.ReadOnly = true;
            SChRText0.ShortcutsEnabled = false;
            SChRText0.Size = new Size(278, 29);
            SChRText0.TabIndex = 14;
            SChRText0.TabStop = false;
            // 
            // dateTimePicker0
            // 
            dateTimePicker0.CustomFormat = "";
            dateTimePicker0.Font = new Font("Times New Roman", 14.25F);
            dateTimePicker0.Location = new Point(6, 107);
            dateTimePicker0.MaxDate = new DateTime(2200, 12, 31, 0, 0, 0, 0);
            dateTimePicker0.MinDate = new DateTime(1900, 1, 1, 0, 0, 0, 0);
            dateTimePicker0.Name = "dateTimePicker0";
            dateTimePicker0.Size = new Size(208, 29);
            dateTimePicker0.TabIndex = 16;
            dateTimePicker0.Value = new DateTime(2024, 6, 5, 0, 0, 0, 0);
            dateTimePicker0.ValueChanged += dateTimePicker0_ValueChanged;
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label28.Location = new Point(6, 30);
            label28.Name = "label28";
            label28.Size = new Size(46, 21);
            label28.TabIndex = 15;
            label28.Text = "СЧР";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(YearForwardText0);
            groupBox4.Controls.Add(YearAgoText0);
            groupBox4.Controls.Add(YearForward0);
            groupBox4.Controls.Add(YearAgo0);
            groupBox4.Location = new Point(289, 83);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(284, 70);
            groupBox4.TabIndex = 19;
            groupBox4.TabStop = false;
            // 
            // YearForwardText0
            // 
            YearForwardText0.AutoSize = true;
            YearForwardText0.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            YearForwardText0.Location = new Point(128, 37);
            YearForwardText0.Name = "YearForwardText0";
            YearForwardText0.Size = new Size(53, 19);
            YearForwardText0.TabIndex = 3;
            YearForwardText0.Text = "label13";
            YearForwardText0.Visible = false;
            // 
            // YearAgoText0
            // 
            YearAgoText0.AutoSize = true;
            YearAgoText0.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            YearAgoText0.Location = new Point(117, 16);
            YearAgoText0.Name = "YearAgoText0";
            YearAgoText0.Size = new Size(53, 19);
            YearAgoText0.TabIndex = 2;
            YearAgoText0.Text = "label12";
            YearAgoText0.Visible = false;
            // 
            // YearForward0
            // 
            YearForward0.AutoSize = true;
            YearForward0.Font = new Font("Times New Roman", 14.25F);
            YearForward0.Location = new Point(6, 34);
            YearForward0.Name = "YearForward0";
            YearForward0.Size = new Size(116, 25);
            YearForward0.TabIndex = 1;
            YearForward0.Text = "Год вперёд";
            YearForward0.UseVisualStyleBackColor = true;
            YearForward0.CheckedChanged += YearForward0_CheckedChanged_1;
            // 
            // YearAgo0
            // 
            YearAgo0.AutoSize = true;
            YearAgo0.Font = new Font("Times New Roman", 14.25F);
            YearAgo0.Location = new Point(6, 13);
            YearAgo0.Name = "YearAgo0";
            YearAgo0.Size = new Size(105, 25);
            YearAgo0.TabIndex = 0;
            YearAgo0.Text = "Год назад";
            YearAgo0.UseVisualStyleBackColor = true;
            YearAgo0.CheckedChanged += YearAgo0_CheckedChanged_1;
            // 
            // resultKoef0
            // 
            resultKoef0.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            resultKoef0.Location = new Point(295, 255);
            resultKoef0.Name = "resultKoef0";
            resultKoef0.ReadOnly = true;
            resultKoef0.Size = new Size(278, 29);
            resultKoef0.TabIndex = 7;
            resultKoef0.Text = "0";
            resultKoef0.TextChanged += resultKoef0_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(6, 258);
            label1.Name = "label1";
            label1.Size = new Size(86, 21);
            label1.TabIndex = 6;
            label1.Text = "Результат";
            // 
            // amountKoef0
            // 
            amountKoef0.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            amountKoef0.Location = new Point(295, 209);
            amountKoef0.Name = "amountKoef0";
            amountKoef0.ReadOnly = true;
            amountKoef0.Size = new Size(278, 29);
            amountKoef0.TabIndex = 5;
            amountKoef0.TextChanged += amountKoef0_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(6, 202);
            label2.Name = "label2";
            label2.Size = new Size(175, 42);
            label2.TabIndex = 4;
            label2.Text = "Кол−во принятого\r\nна работу персонала\r\n";
            // 
            // koefgroup1
            // 
            koefgroup1.Controls.Add(label19);
            koefgroup1.Controls.Add(LevelComboBox1);
            koefgroup1.Controls.Add(label20);
            koefgroup1.Controls.Add(label21);
            koefgroup1.Controls.Add(SChRText1);
            koefgroup1.Controls.Add(dateTimePicker1);
            koefgroup1.Controls.Add(label22);
            koefgroup1.Controls.Add(groupBox3);
            koefgroup1.Controls.Add(resultKoef1);
            koefgroup1.Controls.Add(label3);
            koefgroup1.Controls.Add(amountKoef1);
            koefgroup1.Controls.Add(label4);
            koefgroup1.Location = new Point(683, 370);
            koefgroup1.Name = "koefgroup1";
            koefgroup1.Size = new Size(579, 290);
            koefgroup1.TabIndex = 12;
            koefgroup1.TabStop = false;
            koefgroup1.Visible = false;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label19.Location = new Point(6, 163);
            label19.Name = "label19";
            label19.Size = new Size(242, 21);
            label19.TabIndex = 21;
            label19.Text = "Рассматриваемая должность";
            // 
            // LevelComboBox1
            // 
            LevelComboBox1.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            LevelComboBox1.FormattingEnabled = true;
            LevelComboBox1.Items.AddRange(new object[] { "Начальник отдела", "Менеджер", "Бухгалтер" });
            LevelComboBox1.Location = new Point(295, 160);
            LevelComboBox1.Name = "LevelComboBox1";
            LevelComboBox1.Size = new Size(278, 29);
            LevelComboBox1.TabIndex = 20;
            LevelComboBox1.Text = "-";
            LevelComboBox1.SelectedIndexChanged += LevelComboBox1_SelectedIndexChanged;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label20.Location = new Point(295, 72);
            label20.Name = "label20";
            label20.Size = new Size(211, 21);
            label20.TabIndex = 18;
            label20.Text = "Конец периода подсчёта:";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label21.Location = new Point(6, 70);
            label21.Name = "label21";
            label21.Size = new Size(218, 21);
            label21.TabIndex = 17;
            label21.Text = "Начало периода подсчёта:";
            // 
            // SChRText1
            // 
            SChRText1.BackColor = SystemColors.Info;
            SChRText1.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            SChRText1.HideSelection = false;
            SChRText1.Location = new Point(295, 20);
            SChRText1.Name = "SChRText1";
            SChRText1.ReadOnly = true;
            SChRText1.ShortcutsEnabled = false;
            SChRText1.Size = new Size(278, 29);
            SChRText1.TabIndex = 14;
            SChRText1.TabStop = false;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.CustomFormat = "";
            dateTimePicker1.Font = new Font("Times New Roman", 14.25F);
            dateTimePicker1.Location = new Point(6, 107);
            dateTimePicker1.MaxDate = new DateTime(2200, 12, 31, 0, 0, 0, 0);
            dateTimePicker1.MinDate = new DateTime(1900, 1, 1, 0, 0, 0, 0);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(208, 29);
            dateTimePicker1.TabIndex = 16;
            dateTimePicker1.Value = new DateTime(2024, 6, 5, 0, 0, 0, 0);
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label22.Location = new Point(6, 30);
            label22.Name = "label22";
            label22.Size = new Size(46, 21);
            label22.TabIndex = 15;
            label22.Text = "СЧР";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(YearForwardText1);
            groupBox3.Controls.Add(YearAgoText1);
            groupBox3.Controls.Add(YearForward1);
            groupBox3.Controls.Add(YearAgo1);
            groupBox3.Location = new Point(289, 83);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(284, 70);
            groupBox3.TabIndex = 19;
            groupBox3.TabStop = false;
            // 
            // YearForwardText1
            // 
            YearForwardText1.AutoSize = true;
            YearForwardText1.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            YearForwardText1.Location = new Point(128, 37);
            YearForwardText1.Name = "YearForwardText1";
            YearForwardText1.Size = new Size(53, 19);
            YearForwardText1.TabIndex = 3;
            YearForwardText1.Text = "label13";
            YearForwardText1.Visible = false;
            // 
            // YearAgoText1
            // 
            YearAgoText1.AutoSize = true;
            YearAgoText1.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            YearAgoText1.Location = new Point(117, 16);
            YearAgoText1.Name = "YearAgoText1";
            YearAgoText1.Size = new Size(53, 19);
            YearAgoText1.TabIndex = 2;
            YearAgoText1.Text = "label12";
            YearAgoText1.Visible = false;
            // 
            // YearForward1
            // 
            YearForward1.AutoSize = true;
            YearForward1.Font = new Font("Times New Roman", 14.25F);
            YearForward1.Location = new Point(6, 34);
            YearForward1.Name = "YearForward1";
            YearForward1.Size = new Size(116, 25);
            YearForward1.TabIndex = 1;
            YearForward1.Text = "Год вперёд";
            YearForward1.UseVisualStyleBackColor = true;
            YearForward1.CheckedChanged += YearForward1_CheckedChanged_1;
            // 
            // YearAgo1
            // 
            YearAgo1.AutoSize = true;
            YearAgo1.Font = new Font("Times New Roman", 14.25F);
            YearAgo1.Location = new Point(6, 13);
            YearAgo1.Name = "YearAgo1";
            YearAgo1.Size = new Size(105, 25);
            YearAgo1.TabIndex = 0;
            YearAgo1.Text = "Год назад";
            YearAgo1.UseVisualStyleBackColor = true;
            YearAgo1.CheckedChanged += YearAgo1_CheckedChanged_1;
            // 
            // resultKoef1
            // 
            resultKoef1.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            resultKoef1.Location = new Point(295, 255);
            resultKoef1.Name = "resultKoef1";
            resultKoef1.ReadOnly = true;
            resultKoef1.Size = new Size(278, 29);
            resultKoef1.TabIndex = 7;
            resultKoef1.Text = "0";
            resultKoef1.TextChanged += resultKoef1_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label3.Location = new Point(6, 258);
            label3.Name = "label3";
            label3.Size = new Size(86, 21);
            label3.TabIndex = 6;
            label3.Text = "Результат";
            // 
            // amountKoef1
            // 
            amountKoef1.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            amountKoef1.Location = new Point(295, 209);
            amountKoef1.Name = "amountKoef1";
            amountKoef1.ReadOnly = true;
            amountKoef1.Size = new Size(278, 29);
            amountKoef1.TabIndex = 5;
            amountKoef1.TextChanged += amountKoef1_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label4.Location = new Point(6, 213);
            label4.Name = "label4";
            label4.Size = new Size(270, 21);
            label4.TabIndex = 4;
            label4.Text = "Кол−во выбывших сотрудников\r\n";
            // 
            // ExportButton
            // 
            ExportButton.AutoSize = true;
            ExportButton.BackColor = Color.Transparent;
            ExportButton.DialogResult = DialogResult.OK;
            ExportButton.FlatAppearance.BorderSize = 0;
            ExportButton.FlatStyle = FlatStyle.Popup;
            ExportButton.ForeColor = SystemColors.ButtonFace;
            ExportButton.Image = Properties.Resources.export1;
            ExportButton.Location = new Point(572, 23);
            ExportButton.Name = "ExportButton";
            ExportButton.Size = new Size(36, 36);
            ExportButton.TabIndex = 13;
            ExportButton.TextImageRelation = TextImageRelation.TextAboveImage;
            ExportButton.UseVisualStyleBackColor = false;
            ExportButton.Click += ExportButton_Click;
            // 
            // nameform
            // 
            nameform.AutoSize = true;
            nameform.Location = new Point(26, 6);
            nameform.Name = "nameform";
            nameform.Size = new Size(173, 15);
            nameform.TabIndex = 14;
            nameform.Text = "Расчёты выполняет: Unknown";
            // 
            // FormFormules
            // 
            AutoScaleMode = AutoScaleMode.None;
            AutoSize = true;
            ClientSize = new Size(1132, 749);
            Controls.Add(nameform);
            Controls.Add(ExportButton);
            Controls.Add(comboBoxFormules);
            Controls.Add(koefgroup3);
            Controls.Add(koefgroup2);
            Controls.Add(koefgroup1);
            Controls.Add(koefgroup0);
            Name = "FormFormules";
            StartPosition = FormStartPosition.CenterParent;
            Text = "HR-калькулятор";
            FormClosed += FormFormules_FormClosed;
            Load += FormFormules_Load;
            koefgroup2.ResumeLayout(false);
            koefgroup2.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            koefgroup3.ResumeLayout(false);
            koefgroup3.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            koefgroup0.ResumeLayout(false);
            koefgroup0.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            koefgroup1.ResumeLayout(false);
            koefgroup1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private GroupBox koefgroup2;
        private TextBox resultKoef2;
        private Label label6;
        private TextBox amountKoef2;
        private Label label7;
        private TextBox textBox6;
        private TextBox resultKoef3;
        private Label label9;
        private TextBox amountKoef3;
        private Label label10;
        private TextBox resultKoef0;
        private Label label1;
        private TextBox amountKoef0;
        private Label label2;
        private GroupBox koefgroup1;
        private TextBox resultKoef1;
        private Label label3;
        private TextBox amountKoef1;
        private Label label4;
        private TextBox SChRText3;
        private Label label5;
        private Label nameform;
        private Label label11;
        private Label label8;
        private DateTimePicker dateTimePicker3;
        private GroupBox groupBox1;
        private RadioButton YearForward3;
        private RadioButton YearAgo3;
        private Label YearForwardText3;
        private Label YearAgoText3;
        private Label label12;
        private ComboBox LevelComboBox3;
        private GroupBox groupBox2;
        private Label label14;
        private Label YearForwardText2;
        private Label YearAgoText2;
        private RadioButton YearForward2;
        private RadioButton YearAgo2;
        private Label label13;
        private ComboBox LevelComboBox2;
        private Label label15;
        private DateTimePicker dateTimePicker2;
        private Label label16;
        private TextBox SChRText2;
        private Label label19;
        private ComboBox LevelComboBox1;
        private Label label20;
        private Label label21;
        private TextBox SChRText1;
        private DateTimePicker dateTimePicker1;
        private Label label22;
        private GroupBox groupBox3;
        private Label YearForwardText1;
        private Label YearAgoText1;
        private RadioButton YearForward1;
        private RadioButton YearAgo1;
        private Label label25;
        private ComboBox LevelComboBox0;
        private Label label26;
        private Label label27;
        private TextBox SChRText0;
        private Label label28;
        private GroupBox groupBox4;
        private Label YearAgoText0;
        private RadioButton YearForward0;
        private RadioButton YearAgo0;
        public ComboBox comboBoxFormules;
        public GroupBox koefgroup3;
        public Button ExportButton;
        public GroupBox koefgroup0;
        public DateTimePicker dateTimePicker0;
        public Label YearForwardText0;
    }
}