using System;
using System.Collections.Generic;
using ClosedXML.Excel;
using Microsoft.Win32;
using WpfHR.Models;

namespace WpfHR.Services
{
    public class ProgramSaveService
    {
        public string GetSaveFilePath(Employee employee)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Сохранить программу адаптации",
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = $"{employee.Department}_{employee.Position}_{employee.FullName}_{DateTime.Now:yyyyMMdd}.xlsx",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        public void SaveProgramToFile(string filePath, Employee employee, List<Module> modules, List<string> mentors)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Программа адаптации");

            worksheet.Cell(1, 1).Value = "ФИО Сотрудника";
            worksheet.Cell(1, 2).Value = "Отдел";
            worksheet.Cell(1, 3).Value = "Должность";
            worksheet.Cell(1, 4).Value = "Дата начала";

            worksheet.Cell(2, 1).Value = employee.FullName;
            worksheet.Cell(2, 2).Value = employee.Department;
            worksheet.Cell(2, 3).Value = employee.Position;
            worksheet.Cell(2, 4).Value = DateTime.Now.ToString("dd.MM.yyyy");

            worksheet.Cell(4, 1).Value = "Выбранные модули";
            for (int i = 0; i < modules.Count; i++)
            {
                worksheet.Cell(5 + i, 1).Value = modules[i].Name;
            }

            worksheet.Cell(5 + modules.Count, 1).Value = "Наставники";
            for (int i = 0; i < mentors.Count; i++)
            {
                worksheet.Cell(6 + modules.Count + i, 1).Value = mentors[i];
            }

            workbook.SaveAs(filePath);
        }
    }
}
