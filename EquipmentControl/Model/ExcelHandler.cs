using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ExcelDataReader;


namespace EquipmentControl.Model
{
    public class ExcelHandler
    {
        /// <summary>
        /// Объект для работы с ексель файлом
        /// </summary>
        public ExcelHandler() { }
        /// <summary>
        /// Создоёт ObservableCollection<Employee> из ексель файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns></returns>
        public ObservableCollection<Equipment> GetEquipmentFail(string path)
        {
            DataTableCollection dataTableCollection = null;

            ObservableCollection<Equipment> equipments = new ObservableCollection<Equipment>();
            Equipment equipment;


            var stream = File.Open(path, FileMode.Open, FileAccess.Read);

            var reader = ExcelReaderFactory.CreateReader(stream);

             var result = reader.AsDataSet();

             var tables = result.Tables.Cast<DataTable>();
          


            foreach (var table in tables)
            {
                string nameOrg = table.TableName;

                

                string adres = string.Empty;
                
        
               

                int countRows = table.Rows.Count;

                for (int i = 1; i < countRows; i++)
                {
                  string  tempAdres = table.Rows[i][0].ToString();
                    if (adres == "" && tempAdres != "") adres = tempAdres;
                   if (adres != tempAdres ) adres = tempAdres;
                    if (adres == "" && tempAdres == "")
                    {
                        adres = " ";
                        continue;
                    }

                   string nameEquipment = table.Rows[i][1].ToString();


                   string numberEquipment = table.Rows[i][2].ToString();
                    string dateLast = table.Rows[i][3].ToString();
                    DateTime dateOfLastVerificationEquipmen = dateLast == ""? new DateTime(2000) : DateTime.Parse(dateLast);
                    string dateNext = table.Rows[i][4].ToString();
                   DateTime dateOfNextVerificationEquipmen =dateNext == ""?  new DateTime(2000) :DateTime.Parse(dateNext);

                    equipment = new Equipment(nameEquipment, numberEquipment,
                        dateOfLastVerificationEquipmen, dateOfNextVerificationEquipmen, adres, nameOrg);
                    equipments.Add(equipment);
                }



                //пример
                // daraGridView1.DataSource = table;
            }

          
            reader.Close();
            return equipments;
        }
    }
}
