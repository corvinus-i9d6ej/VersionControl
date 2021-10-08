using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel1 = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace Excel
{
    public partial class Form1 : Form
    {
        RealEstateEntities context = new RealEstateEntities();
        List<Flat> flats;
        Excel1.Application xlApp;
        Excel1.Workbook xlWb;
        Excel1.Worksheet xlSheet;
        public Form1()
        {
            InitializeComponent();
            LoadData();
            CreateExcel();
        }

        void LoadData()
        {
            flats = context.Flats.ToList();
        }

        void CreateExcel()
        {
            try
            {
                xlApp = new Excel1.Application();
                xlWb = xlApp.Workbooks.Add(Missing.Value);
                xlSheet = xlWb.ActiveSheet;

                CreateTable();

                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex)
            {
                string errorMsg = string.Format("Error: {0}\nLine: {1}", ex.Message, ex.Source);
                MessageBox.Show(errorMsg, "Error");

                xlWb.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlWb = null;
                xlApp = null;
            }
        }

        void CreateTable()
        {
            string[] headers = new string[]
            {
                "Kód",
                "Eladó",
                "Oldal",
                "Kerület",
                "Lift",
                "Szobák száma",
                "Alapterület (m2)",
                "Ár (mFt)",
                "Négyzetméter ár (Ft/m2)"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                xlSheet.Cells[1, i+1] = headers[i];
            }

            object[,] values = new object[flats.Count, headers.Length];

            int n = 0;
            foreach (Flat f in flats)
            {
                values[n, 0] = f.Code;
                values[n, 1] = f.Vendor;
                values[n, 2] = f.Side;
                values[n, 3] = f.District;
                if (f.Elevator)
                {
                    values[n, 4] = "Van";
                }
                else
                {
                    values[n, 4] = "Nincs";
                };
                values[n, 5] = f.NumberOfRooms;
                values[n, 6] = f.FloorArea;
                values[n, 7] = f.Price;
                values[n, 8] = "=" + GetCell(n + 2, 8) + "*1000000/" + GetCell(n + 2, 7);
                n++;
            }

            xlSheet.get_Range(
                GetCell(2,1), 
                GetCell(1+values.GetLength(0), values.GetLength(1))).Value2 = values;

            Excel1.Range headerRange = xlSheet.get_Range(GetCell(1, 1), GetCell(1, headers.Length));
            headerRange.Font.Bold = true;
            headerRange.VerticalAlignment = Excel1.XlVAlign.xlVAlignCenter;
            headerRange.HorizontalAlignment = Excel1.XlHAlign.xlHAlignCenter;
            headerRange.EntireColumn.AutoFit();
            headerRange.RowHeight = 40;
            headerRange.Interior.Color = Color.LightBlue;
            headerRange.BorderAround2(Excel1.XlLineStyle.xlContinuous, Excel1.XlBorderWeight.xlThick);
        }

        private string GetCell(int x, int y)
        {
            string ExcelCoordinate = "";
            int dividend = y;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                ExcelCoordinate = Convert.ToChar(65 + modulo).ToString() + ExcelCoordinate;
                dividend = (int)((dividend - modulo) / 26);
            }
            ExcelCoordinate += x.ToString();

            return ExcelCoordinate;
        }
    }
}
