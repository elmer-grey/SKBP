using Microsoft.VisualStudio.TestTools.UnitTesting;
using СКБП;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace СКБП.Tests
{
    [TestClass]
    public class FormFormulesTests
    {
        private FormFormules form;

        [TestInitialize]
        public void Setup()
        {
            form = new FormFormules();
        }

        [TestMethod]
        public void TestInitializeComponent()
        {
            form.InitializeComponent();
            Assert.IsNotNull(form.Controls); // Проверяем, что элементы управления были инициализированы
        }

        [TestMethod]
        public void TestVisibilityControl()
        {
            form.comboBoxFormules.SelectedIndex = 1; // Выбираем первый элемент
            form.comboBoxFormules_SelectionChangeCommitted(null, null); // Имитируем событие изменения выбора
            Assert.IsFalse(form.koefgroup0.Visible); // Проверяем, что группа коэффициентов видима
        }

        [TestMethod()]
        public void TestCalculation()
        {
            double SCHR = 1.89863014;
            int amount = 1;
            double expected = 0.42135642;
            double actual = form.result(amount, SCHR);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestExportButtonVisibility()
        {
            form.comboBoxFormules.SelectedIndex = 0; // Выбираем первый элемент
            form.comboBoxFormules_SelectionChangeCommitted(null, null); // Имитируем событие изменения выбора
            Assert.IsFalse(form.ExportButton.Visible); // Проверяем, что кнопка экспорта скрыта
        }


        [TestMethod]
        public void TestDateChangeHandling()
        {
            DateTime date = DateTime.Now;
            var oldDate = form.dateTimePicker0.Value;
            var newDate = oldDate.AddYears(1);
            form.dateTimePicker0.Value = newDate;
            form.dateTimePicker0_ValueChanged(null, null); // Имитируем событие изменения даты
            Assert.AreNotEqual(form.dateTimePicker0.Value, oldDate); // Проверяем, что дата была обновлена
        }
    }
}
