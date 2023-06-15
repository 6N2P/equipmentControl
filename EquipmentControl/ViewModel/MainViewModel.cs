﻿using EquipmentControl.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Markup;

namespace EquipmentControl.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region PropertyChange
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion PropertyChange

        public MainViewModel()
        {
            DateNow = DateTime.Now;
            CountMons = 2;
            Date = DateTime.Now;
        }


        ExcelHandler excelHandler;
        ObservableCollection<Equipment> equipments;
        ObservableCollection<Equipment> equipmentCheckList;
        ObservableCollection<Equipment> equipmentsSerchList;
        string path;
        DateTime dateNow;
        DateTime date;
        int countAllequipments;
        int countEquipmentCheckList;
        int countEquipmentSerchList;
        int countMons;


        #region Property
        public int CountMons
        {
            get => countMons;
            set
            {
                countMons = value;
                OnPropertyChanged("CountMons");
            }
        }
        public int CountEquipmentSerchList
        {
            get => countEquipmentSerchList;
            set
            {
                countEquipmentSerchList = value;
                OnPropertyChanged("CountEquipmentSerchList");
            }
        }
        public int CountAllequipments
        {
            get => countAllequipments;
            set
            {
                countAllequipments = value;
                OnPropertyChanged("CountAllequipments");
            }
        }
        public int CountEquipmentCheckList
        {
            get => countEquipmentCheckList;
            set
            {
                countEquipmentCheckList = value;
                OnPropertyChanged("CountEquipmentCheckList");
            }
        }
        public DateTime DateNow
        {
            get => dateNow;
            set
            {
                dateNow = value;
                OnPropertyChanged("DateNaw");
            }
        }
        public string Path
        {
            get => path;
            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }
        public ObservableCollection<Equipment> EquipmentCheckList
        {
            get => equipmentCheckList;
            set
            {
                equipmentCheckList = value;
                CountEquipmentCheckList = EquipmentCheckList.Count;
                OnPropertyChanged("EquipmentCheckList");
            }
        }
        public ObservableCollection<Equipment> AllEqupments
        {
            get => equipments;
            set
            {
                equipments = value;
                CountAllequipments = AllEqupments.Count;
                OnPropertyChanged("AllEqupments");
            }
        }
        public ObservableCollection<Equipment> EquipmentsSerchList
        {
            get => equipmentsSerchList;
            set
            {
                equipmentsSerchList = value;
                CountEquipmentSerchList = EquipmentsSerchList.Count;
                OnPropertyChanged("EquipmentsSerchList");
            }
        }

        public DateTime Date
        {
            get => date;
            set
            {
                date = value;

                OnPropertyChanged("Date");

            }
        }
        public HashSet<DateTime> Dates { get; } = new HashSet<DateTime>();
        #endregion Property

        #region Commands
        DelegateCommand _openFileCommand;
        public DelegateCommand OpenFileComand
        {
            get
            {
                return _openFileCommand ??
                    (_openFileCommand = new DelegateCommand(obj =>
                    {
                        OpenFile();
                    }));
            }
        }
        DelegateCommand _updateEquipmentCheckListCommand;
        public DelegateCommand UpdateEquipmentCheckListCommand
        {
            get
            {
                return _updateEquipmentCheckListCommand ??
                    (_updateEquipmentCheckListCommand = new DelegateCommand(obj =>
                    {
                        EquipmentCheckList = UpdateEquipmentCheckList(equipments);
                    }));
            }
        }
        DelegateCommand _clickDate;
        public DelegateCommand ClickDate
        {
            get
            {
                return _clickDate ??
                    (_clickDate = new DelegateCommand(obj =>
                    {
                        GetEquipmentFoDate();
                    }));
            }
        }
        #endregion Commands

        void OpenFile()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel Files|*.xlsx|All files (*.*)|*.*";
                openFileDialog.ShowDialog();
                
               
                path = openFileDialog.FileName.ToString();

                excelHandler = new ExcelHandler();
                AllEqupments = excelHandler.GetEquipmentFail(path);

                EquipmentCheckList = UpdateEquipmentCheckList(equipments);

                GetDateNextVerification();
               
            }
            catch (Exception ex)
            {
             //   MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }

        ObservableCollection<Equipment> UpdateEquipmentCheckList(ObservableCollection<Equipment>  allequipment)
        {
            dateNow = DateTime.Now;

            string monsNow = $"01.{dateNow.Month}.{dateNow.Year}";
            DateTime dateStartRenge = DateTime.Parse(monsNow);

            //TimeSpan timeSpan = TimeSpan.FromDays(30);
            DateTime dateEndRenge = dateStartRenge.AddMonths(CountMons);

            ObservableCollection<Equipment> updateEquipments = new ObservableCollection<Equipment>();
            foreach(Equipment equip in allequipment)
            {
                if (equip.DateOfNextVerification >= dateStartRenge && equip.DateOfNextVerification <= dateEndRenge)
                {
                    updateEquipments.Add(equip);
                }
            }

            return updateEquipments;
        }
        void GetDateNextVerification()
        {
            if (Dates.Count > 0)  Dates.Clear();

            List<DateTime> datesList = new List<DateTime>();
            foreach(var eq in AllEqupments )
            {
                DateTime date = new DateTime();
                date =(DateTime) eq.DateOfNextVerification;

                datesList.Add(date);
            }
            datesList.Sort();
            var listClean=datesList.Distinct().ToHashSet();
            foreach(var listDate in listClean)
            {
                this.Dates.Add(listDate);
            }
            
        }

        void GetEquipmentFoDate()
        {
            if (equipments != null)
            {
                ObservableCollection<Equipment> equipmentFoDate = new ObservableCollection<Equipment>();

                foreach (var eq in AllEqupments)
                {
                    if (eq.DateOfNextVerification == Date) equipmentFoDate.Add(eq);
                }
                EquipmentsSerchList = equipmentFoDate;
            }
        }
    }

    
}
