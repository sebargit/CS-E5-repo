using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using System.IO;

namespace E5_1
{
    public partial class Form1 : Form
    {

        private string startDir1 = AppDomain.CurrentDomain.BaseDirectory;
        private string startDir = "C:\\Users\\Sergey\\Desktop\\3";

        
        private string nomenclatureFile = "СПРАВОЧНИК  НОМЕНКЛАТУРЫ.xls";



        public Form1()
        {
            InitializeComponent();
            this.textBox1.Text = startDir1;
            this.textBox2.Text = nomenclatureFile;
        }

        private void CreateTmpFile(string FromFileName, string ToFilename)
        {
            var app = new Excel.Application();
            CreateTmpFile(FromFileName, ToFilename, app);
            app.Quit();
        }

        private void CreateTmpFile(string FromFileName, string ToFilename, Excel.Application app)
        {

            if (File.Exists(FromFileName))
            { 
                var DisplayAlerts = app.DisplayAlerts;
                app.DisplayAlerts = false;
                var wb = app.Workbooks.Open(Filename: FromFileName, ReadOnly:true);
                wb.SaveAs(Filename: ToFilename, FileFormat: Excel.XlFileFormat.xlOpenXMLWorkbook, ConflictResolution: Excel.XlSaveConflictResolution.xlLocalSessionChanges);
                wb.Close(false);
                app.DisplayAlerts = DisplayAlerts;
            }
        }



        private string CreateTmpDir(string startDir)
        {
            if (Directory.Exists(startDir))
            {
                int i = 1;
                while (Directory.Exists(startDir + i)) i++;
                Directory.CreateDirectory(startDir + i);
                return startDir + i + "\\";
            }
            else return "";
        }

        //
        // Создание временных копий файлов прайсов
        private void CopyTmpFiles(string tmpDir)
        {
            FileInfo fi = new FileInfo(startDir + "#Описание.xlsx");
            ExcelPackage ep = new ExcelPackage(fi);
            ExcelWorkbook wb = ep.Workbook;

            string xlsxFileName;
            var app = new Excel.Application();


            string A = "A";
            var FileList = wb.Worksheets["Лист1"].Cells[A + "3:" + A + "100"].ToArray();
            int i = 0;

            while (i < FileList.Count())
            {

                if (FileList[i].Value == null)
                {
                    i++; continue;
                }
                Console.WriteLine(i + " - " + FileList[i].Value.ToString().Trim() + ".xlsx");

                xlsxFileName = FileList[i].Value.ToString().Trim() + ".xlsx";

                var chkFiles = Directory.GetFiles(startDir);

                foreach (string filename in chkFiles)
                {

                    if (filename.ToLower() == (startDir + xlsxFileName).ToLower() ||
                        filename.ToLower() == (startDir + xlsxFileName.Replace("ё", "е")).ToLower())
                    {
                        File.Copy(filename, tmpDir + xlsxFileName);
                    }

                    else if (filename.ToLower() == (startDir + xlsxFileName.Substring(0, xlsxFileName.Length - 1)).ToLower() ||
                            filename.ToLower() == (startDir + xlsxFileName.Substring(0, xlsxFileName.Length - 1).Replace("ё", "е")).ToLower())
                    {
                        CreateTmpFile(filename, tmpDir + xlsxFileName, app);
                    }

                }

                i++;
            }
            Console.WriteLine(i + " - " + FileList.Count());
            app.Quit();
        }

        //private void Button1_Click(object sender, EventArgs e)
        //{
        //    startDir = this.textBox1.Text;
        //    if (startDir.Substring(startDir.Length - 1) != "\\")
        //    {
        //        startDir = startDir+"\\";
        //    }
        //    string tmpDir = CreateTmpDir(startDir);

        //    if (tmpDir != "")
        //    {

                
        //        //CopyTmpFiles(tmpDir);
        //        CreateTmpFile(startDir + "СПРАВОЧНИК НОМЕНКЛАТУРЫ.xls", tmpDir + "СПРАВОЧНИК НОМЕНКЛАТУРЫ.xlsx");
        //    }


        //    this.Close();
        //}

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            startDir = this.textBox1.Text;
            nomenclatureFile= this.textBox2.Text;
            if (startDir.Substring(startDir.Length - 1) != "\\")
            {
                startDir = startDir + "\\";
            }
            string tmpDir = CreateTmpDir(startDir);

            if (tmpDir != "")
            {


                CopyTmpFiles(tmpDir);
                CreateTmpFile(startDir + nomenclatureFile, tmpDir + nomenclatureFile + "x");
                MessageBox.Show("Файлы лежат здесь:\n" + tmpDir);
            }
            else
            {
                MessageBox.Show("Файлы не скопированы" );
            }
            

            this.Close();
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}