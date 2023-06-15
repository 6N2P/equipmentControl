using EquipmentControl.Model;
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
using System.Windows.Controls;

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
            DateFrom = DateTime.Now;
            DateTo = DateFrom.AddMonths(2);
        }


        ExcelHandler excelHandler;
        ObservableCollection<Equipment> equipments;
        ObservableCollection<Equipment> equipmentCheckList;
        ObservableCollection<Equipment> equipmentsSerchList;
        string path;
        DateTime dateNow;
        DateTime date;
        DateTime dateFrom;
        DateTime dateTo;
        int countAllequipments;
        int countEquipmentCheckList;
        int countEquipmentSerchList;
        int countMons;
        string serchCompanyTB;
        string serchAdresTB;
        string serchEquipmentTB;
        string serchNamberEquipmentTB;


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
        public string SerchCompanyTB
        {
            get => serchCompanyTB;
            set
            {
                serchCompanyTB = value;
                OnPropertyChanged("SerchCompanyTB");
            }
        }
        public string SerchAdresTB
        {
            get => serchAdresTB;
            set
            {
                serchAdresTB = value;
                OnPropertyChanged("SerchAdresTB");
            }
        }
        public string SerchhEquipmentTB
        {
            get => serchEquipmentTB;
            set
            {
                serchEquipmentTB = value;
                OnPropertyChanged("SerchhEquipmentTB");
            }
        }
        public string SerchNamberEquipmentTB
        {
            get => serchNamberEquipmentTB;
            set
            {
                serchNamberEquipmentTB = value;
                OnPropertyChanged("SerchNamberEquipmentTB");
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
        public DateTime DateFrom
        {
            get => dateFrom;
            set
            {
                dateFrom = value;
                OnPropertyChanged("DateFrom");
            }
        }
        public DateTime DateTo
        {
            get => dateTo;
            set
            {
                dateTo = value;
                OnPropertyChanged("DateTo");
            }
        }

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
        DelegateCommand _serchButton;
        public DelegateCommand SerchButton
        {
            get
            {
                return _serchButton ??
                    (_serchButton = new DelegateCommand(obj =>
                    {
                        SerchForData();
                    }));
            }
        }

        DelegateCommand _cleanSerchFildsCommand;
        public DelegateCommand CleanSerchFildsCommand
        {
            get
            {
                return _cleanSerchFildsCommand ?? (
                    _cleanSerchFildsCommand = new DelegateCommand(obj =>
                    {
                        ClearnField();
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
                EquipmentsSerchList =  UpdateEquipmentCheckList(equipments);
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

        void SerchForData()
        {
            if (CorectRengDate())
            {
                if (equipments != null)
                {
                    ObservableCollection<Equipment> serchEquipment = new ObservableCollection<Equipment>();
                    ObservableCollection<Equipment> serchEquipmentForDate = new ObservableCollection<Equipment>();
                    ObservableCollection<Equipment> serchEquipmentForCompany = new ObservableCollection<Equipment>();
                    ObservableCollection<Equipment> serchEquipmentForAdres = new ObservableCollection<Equipment>();
                    ObservableCollection<Equipment> serchEquipmentForName = new ObservableCollection<Equipment>();
                    ObservableCollection<Equipment> serchEquipmentForNumber = new ObservableCollection<Equipment>();


                    foreach (var equipment in AllEqupments)
                    {
                        if(equipment.DateOfNextVerification >= DateFrom && equipment.DateOfNextVerification <= DateTo)
                        {
                            serchEquipmentForDate.Add(equipment);
                        }
                    }

                    serchEquipment = serchEquipmentForDate;
                    
                    if (!string.IsNullOrEmpty(SerchCompanyTB))
                    {
                        foreach (var equipment in serchEquipment)
                        {
                            if( equipment.NameCompany.ToLower().Contains(SerchCompanyTB.ToLower()))
                            {
                                serchEquipmentForCompany.Add(equipment);
                            }
                        }
                        serchEquipment = serchEquipmentForCompany;
                    }

                    if (!string.IsNullOrEmpty(SerchAdresTB))
                    {
                        foreach (var equipment in serchEquipment)
                        {
                            if (equipment.Adres.ToLower().Contains(SerchAdresTB.ToLower()))
                            {
                                serchEquipmentForAdres.Add(equipment);
                            }
                        }
                        serchEquipment = serchEquipmentForAdres;
                    }

                    if (!string.IsNullOrEmpty(SerchhEquipmentTB))
                    {
                        foreach (var equipment in serchEquipment)
                        {
                            if (equipment.Name.ToLower().Contains(SerchhEquipmentTB.ToLower()))
                            {
                                serchEquipmentForName.Add(equipment);
                            }
                        }
                        serchEquipment = serchEquipmentForName;
                    }

                    if (!string.IsNullOrEmpty(SerchNamberEquipmentTB))
                    {
                        foreach(var equipment in serchEquipment)
                        {
                            if(equipment.Number.Contains(SerchNamberEquipmentTB))
                            {
                                serchEquipmentForNumber.Add(equipment);
                            }
                        }
                        serchEquipment = serchEquipmentForNumber;
                    }

                    EquipmentsSerchList = serchEquipment;
                }
            }
            else
            {
                MessageBox.Show("Дата 'от' больше чем 'до'", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        bool CorectRengDate()
        {
            if (DateFrom < DateTo) return true;
            return false;
        }

        void ClearnField()
        {
            SerchCompanyTB = string.Empty;
            SerchAdresTB = string.Empty;
            SerchhEquipmentTB = string.Empty;
            SerchNamberEquipmentTB = string.Empty;
        }
    }

    
}
