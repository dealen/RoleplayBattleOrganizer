using RoleplayBattleOrganizer.Models;
using RoleplayBattleOrganizer.Models.Helpers;
using RoleplayBattleOrganizer.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System;
using System.Windows;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace RoleplayBattleOrganizer.ViewsModels
{
    public class VMMain : ViewModel, IDataErrorInfo
    {
        #region Private Fields

        private List<Fighter> _lstFighters;
        private List<Fighter> _lstSelectedFighters;
        private List<Fighter> _lstSavedFighters;
        private List<FighterGamerType> _lstFighterGamerType;
        private List<FighterSystemType> _lstFighterSystemType;

        private ICollectionView _oFightersCollection;
        private ICollectionView _oSavedFightersCollection;
        private FighterSystemType _oSelectedFighterSystemType;
        private FighterGamerType _oSelectedFighterGamerType;
        private Effects _oFightEffects;
        private Fighter _oSelectedFighter;
        private Fighter _oSelectedSavedFighter;

        private ViewModelCommand _cmdCommandAddDamage;
        private ViewModelCommand _cmdCommandAddHealing;
        private ViewModelCommand _cmdCommandClose;
        private ViewModelCommand _cmdCommandAbout;
        private ViewModelCommand _cmdCommandAddGamer;
        private ViewModelCommand _cmdCommandResetTable;
        private ViewModelCommand _cmdCommandDeleteSelected;

        private int _intDamage;
        private int _intHealing;
        private int _intInitiativeRoll;
        private string _strNameNew;
        private int _intHpNew;
        private string _strProfessionNew;
        private string _strGamerNew;
        private string _strLog;
        private ViewModelCommand _cmdCommandAddSavedFighterToFightersList;
        private ViewModelCommand _cmdCommandAddToSavedFighterList;
        private ViewModelCommand _cmdCommandDeleteFromSavedFightersList;
        private ViewModelCommand _cmdCommandOnAppUnloaded;
        private ViewModelCommand _cmdCommandOnAppLoaded;
        private bool _bWhChecked;
        private bool _bIsDnDChecked;
        private Fighter _oCurrentEditFighter;

        #endregion Private Fields



        #region IDataErrorInfo

        //
        // To be implemented
        //

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion



        #region Public Constructor

        public VMMain()
        {
            IsWHChecked = true;
            Log = "Log";
            if (FighterGamerType != null && FighterGamerType.Count > 0)
                SelectedFighterGamerType = FighterGamerType[0];
            if (FighterSystemType != null && FighterSystemType.Count > 0)
                SelectedFighterSystemType = FighterSystemType[0];
        }

        #endregion Public Constructor



        #region Public Properties

        public bool IsWHChecked
        {
            get
            {
                return _bWhChecked;
            }
            set
            {
                _bWhChecked = value;
                OnPropertyChanged(nameof(IsWHChecked), nameof(IsDnDChecked),
                    nameof(SavedFighters), nameof(SavedFightersCollection));
            }
        }

        public bool IsDnDChecked
        {
            get { return _bIsDnDChecked; }
            set
            {
                _bIsDnDChecked = value;
                OnPropertyChanged(nameof(IsWHChecked), nameof(IsDnDChecked),
                    nameof(SavedFighters), nameof(SavedFightersCollection));
            }
        }

        public Effects FightEffects
        {
            get
            {
                _oFightEffects = null;
                _oFightEffects = new Effects(SelectedFighter);
                return _oFightEffects;
            }
        }


        public List<Fighter> SavedFighters
        {
            get
            {
                if (_lstSavedFighters == null)
                    _lstSavedFighters = new List<Fighter>();

                return _lstSavedFighters.OrderByDescending(x => x.Initiative).ToList(); ;
            }
            set
            {
                _lstSavedFighters = value;
                _lstSavedFighters = _lstSavedFighters.OrderByDescending(x => x.Initiative).ToList();
                OnPropertyChanged(nameof(SavedFighters), nameof(SavedFightersCollection));
            }
        }


        public List<Fighter> Fighters
        {
            get
            {
                if (_lstFighters == null)
                    _lstFighters = new List<Fighter>();
                //    {
                //        new Fighter(12, 22, "Dealen", "Kuba", "Bard", FighterSystem.WHFB, FighterType.Player),
                //        new Fighter(19, 15, "Thug", "", "Banita", FighterSystem.WHFB, FighterType.NPC),
                //        new Fighter(19, 24, "Thug1", "", "Banita", FighterSystem.WHFB, FighterType.NPC),
                //        new Fighter(19, 25, "Thug2", "", "Banita", FighterSystem.WHFB, FighterType.NPC),
                //        new Fighter(19, 26, "Thug3", "", "Banita", FighterSystem.WHFB, FighterType.NPC)
                //    };
                //_lstFighters = _lstFighters.OrderByDescending(x => x.Initiative).ToList();

                return _lstFighters.OrderByDescending(x => x.InitiativeRoll).ThenBy(x => x.Initiative).ToList(); ;
            }
            set
            {
                _lstFighters = value;
                _lstFighters = _lstFighters.OrderByDescending(x => x.Initiative).ToList();
                OnPropertyChanged(nameof(Fighters), nameof(FightersCollection));
            }
        }

        public ICollectionView SavedFightersCollection
        {
            get
            {
                if (IsWHChecked)
                {
                    var _tmpData = SavedFighters?.Where(x => x.FighterSystem == FighterSystem.WHFB);
                    var ocFighters = new ObservableCollection<Fighter>(_tmpData);
                    _oSavedFightersCollection = new CollectionView(ocFighters);
                }
                else if (IsDnDChecked)
                {
                    var _tmpData = SavedFighters?.Where(x => x.FighterSystem == FighterSystem.D20);
                    var ocFighters = new ObservableCollection<Fighter>(_tmpData);
                    _oSavedFightersCollection = new CollectionView(ocFighters);
                }
                return _oSavedFightersCollection;
            }
        }

        public ICollectionView FightersCollection
        {
            get
            {
                var ocFighters = new ObservableCollection<Fighter>(Fighters);
                _oFightersCollection = new CollectionView(ocFighters);
                return _oFightersCollection;
            }
        }

        public Fighter SelectedSavedFighter
        {
            get { return _oSelectedSavedFighter; }
            set
            {
                _oSelectedSavedFighter = value;
                OnSelectedFighter();
                OnPropertyChanged(
                    nameof(SelectedSavedFighter));
            }
        }

        public Fighter CurrentEditFighter
        {
            get { return _oCurrentEditFighter; }
            set
            {
                _oCurrentEditFighter = value;
                OnPropertyChanged(nameof(CurrentEditFighter));
            }
        }

        public Fighter SelectedFighter
        {
            get { return _oSelectedFighter; }
            set
            {
                _oSelectedFighter = value;
                OnPropertyChanged(
                    nameof(SelectedFighter),
                    nameof(CurrentFighterLabel),
                    nameof(IsDamageEnabled),
                    nameof(IsHealingEnabled));
            }
        }

        public List<Fighter> SelectedFighters
        {
            get { return _lstSelectedFighters; }
            set
            {
                _lstSelectedFighters = value;
                OnPropertyChanged(nameof(SelectedFighters));
            }
        }

        public string InitiativeText
        {
            get { return "Inicjatywa"; }
        }

        public string NameText
        {
            get { return "Imię"; }
        }

        public string ProfessionText
        {
            get { return "Klasa"; }
        }

        public string HealthPointsText
        {
            get { return "HP"; }
        }

        public string MaxText
        {
            get { return "Max HP"; }
        }

        public int Damage
        {
            get { return _intDamage; }
            set
            {
                _intDamage = value;
                OnPropertyChanged(nameof(Damage), nameof(IsDamageEnabled));
            }
        }

        public int Healing
        {
            get { return _intHealing; }
            set
            {
                _intHealing = value;
                OnPropertyChanged(nameof(Healing), nameof(IsHealingEnabled));
            }
        }

        public string CurrentFighterLabel
        {
            get
            {
                if (SelectedFighter == null)
                    return "";
                return $"{SelectedFighter.Name} - {SelectedFighter.Gamer}";
            }
        }

        public bool IsDamageEnabled
        {
            get
            {
                return SelectedFighter != null;
            }
        }

        public bool IsHealingEnabled
        {
            get { return IsDamageEnabled; }
        }

        public int InitiativeRoll
        {
            get { return _intInitiativeRoll; }
            set
            {
                _intInitiativeRoll = value;
                OnPropertyChanged(nameof(InitiativeRoll), nameof(IsAddNewGamerEnabled));
            }
        }

        public string NameNew
        {
            get { return _strNameNew; }
            set
            {
                _strNameNew = value;
                OnPropertyChanged(nameof(NameNew), nameof(IsAddNewGamerEnabled));
            }
        }

        public int HpNew
        {
            get { return _intHpNew; }
            set
            {
                _intHpNew = value;
                OnPropertyChanged(nameof(HpNew), nameof(IsAddNewGamerEnabled));
            }
        }

        public string GamerNew
        {
            get { return _strGamerNew; }
            set
            {
                _strGamerNew = value;
                OnPropertyChanged(nameof(GamerNew), nameof(IsAddNewGamerEnabled));
            }
        }

        public string ProfessionNew
        {
            get { return _strProfessionNew; }
            set
            {
                _strProfessionNew = value;
                OnPropertyChanged(nameof(ProfessionNew), nameof(IsAddNewGamerEnabled));
            }
        }

        public List<FighterGamerType> FighterGamerType
        {
            get
            {
                if (_lstFighterGamerType == null)
                {
                    _lstFighterGamerType = new List<FighterGamerType>()
                    {
                        new FighterGamerType() { Id = 2, Name = "Gracz", Type = FighterType.Player },
                        new FighterGamerType() { Id = 1, Name = "NPC", Type = FighterType.NPC }
                    };
                }
                return _lstFighterGamerType;
            }
        }

        public FighterGamerType SelectedFighterGamerType
        {
            get { return _oSelectedFighterGamerType; }
            set
            {
                _oSelectedFighterGamerType = value;
                OnPropertyChanged(nameof(SelectedFighterGamerType), nameof(IsAddNewGamerEnabled));
            }
        }

        public List<FighterSystemType> FighterSystemType
        {
            get
            {
                if (_lstFighterSystemType == null)
                {
                    _lstFighterSystemType = new List<FighterSystemType>()
                    {
                        new FighterSystemType() { Id = 1, Name = "D&D", System = FighterSystem.D20 },
                        new FighterSystemType() { Id = 1, Name = "WHFB", System = FighterSystem.WHFB }
                    };
                }
                return _lstFighterSystemType;
            }
        }

        public FighterSystemType SelectedFighterSystemType
        {
            get { return _oSelectedFighterSystemType; }
            set
            {
                _oSelectedFighterSystemType = value;
                OnPropertyChanged(nameof(SelectedFighterSystemType), nameof(IsAddNewGamerEnabled));
            }
        }

        public bool IsAddNewGamerEnabled
        {
            get
            {
                if (HpNew <= 0
                || InitiativeRoll <= 0
                || (NameNew != null && NameNew.Equals(string.Empty))
                || SelectedFighterGamerType == null
                || SelectedFighterSystemType == null
                || (GamerNew != null && GamerNew.Equals(string.Empty))
                || (ProfessionNew == null || ProfessionNew == string.Empty))
                    return false;
                return true;
            }
        }

        public string Log
        {
            get { return _strLog; }
            set
            {
                _strLog = value;
                OnPropertyChanged(nameof(Log));
            }
        }

        public string GetTime
        {
            get { return DateTime.Now.ToLongTimeString(); }
        }

        public bool Save
        {
            get
            {
                return SaveData(null);
            }
        }

        public bool Load
        {
            get
            {
                return LoadData(null);
            }
        }

        public bool IsNewPlayer
        {
            get
            {
                if (SelectedSavedFighter == null)
                    return true;
                if (NameNew != SelectedSavedFighter.Name)
                    return true;
                if (GamerNew != SelectedSavedFighter.Gamer)
                    return true;
                return false;
            }
        }
        
        #endregion Public Properties



        #region Public Commands

        public ViewModelCommand CommandAddDamage
        {
            get { return _cmdCommandAddDamage ?? (_cmdCommandAddDamage = new ViewModelCommand(AddDamage)); }
        }

        public ViewModelCommand CommandAddHealing
        {
            get { return _cmdCommandAddHealing ?? (_cmdCommandAddHealing = new ViewModelCommand(AddHealing)); }
        }

        public ViewModelCommand CommandAddGamer
        {
            get { return _cmdCommandAddGamer ?? (_cmdCommandAddGamer = new ViewModelCommand(AddGamer)); }
        }

        public ViewModelCommand CommandClose
        {
            get { return _cmdCommandClose ?? (_cmdCommandClose = new ViewModelCommand(CloseApp)); }
        }

        public ViewModelCommand CommandAbout
        {
            get { return _cmdCommandAbout ?? (_cmdCommandAbout = new ViewModelCommand(About)); }
        }

        public ViewModelCommand CommandResetTable
        {
            get { return _cmdCommandResetTable ?? (_cmdCommandResetTable = new ViewModelCommand(ResetTable)); }
        }

        public ViewModelCommand CommandDeleteSelected
        {
            get { return _cmdCommandDeleteSelected ?? (_cmdCommandDeleteSelected = new ViewModelCommand(DeleteSelected)); }
        }

        public ViewModelCommand CommandAddSavedFighterToFightersList
        {
            get
            {
                return _cmdCommandAddSavedFighterToFightersList ?? (_cmdCommandAddSavedFighterToFightersList = new ViewModelCommand(AddSavedFighterToFightersList));
            }
        }

        public ViewModelCommand CommandAddToSavedFighterList
        {
            get
            {
                return _cmdCommandAddToSavedFighterList ?? (_cmdCommandAddToSavedFighterList = new ViewModelCommand(AddFighterToSavedFighterList));
            }
        }

        public ViewModelCommand CommandDeleteFromSavedFightersList
        {
            get
            {
                return _cmdCommandDeleteFromSavedFightersList ?? (_cmdCommandDeleteFromSavedFightersList = new ViewModelCommand(DeleteFromSavedFightersList));
            }
        }

        #endregion Public Commands



        #region Private Methods

        private bool SaveData(object o)
        {
            // AUtomapper
            var _path = AppDomain.CurrentDomain.BaseDirectory + "rpg.json";

            var fightersToSave = new List<FighterBase>();
            foreach (var fighter in SavedFighters)
            {
                fightersToSave.Add(new FighterBase()
                {
                    Id = fighter.Id,
                    FighterType = (int)fighter.FighterType,
                    FighterSystem = (int)fighter.FighterSystem,
                    Gamer = fighter.Gamer,
                    HealthPoints = fighter.HealthPoints,
                    MaxHealthPoints = fighter.MaxHealthPoints,
                    Initiative = fighter.Initiative,
                    Name = fighter.Name,
                    Profession = fighter.Profession
                });
            }

            var _fightersString = JsonConvert.SerializeObject(fightersToSave);

            File.WriteAllText(_path, _fightersString);
            
            return false;
        }

        private bool LoadData(object o)
        {
            // AUtomapper
            var _path = AppDomain.CurrentDomain.BaseDirectory + "rpg.json";
            if (File.Exists(_path))
            {
                var list = JsonConvert.DeserializeObject<List<FighterBase>>(File.ReadAllText(_path));
                var listOfFighters = new List<Fighter>();

                foreach (var fighter in list)
                {
                    listOfFighters.Add(new Fighter(fighter.Id, fighter.MaxHealthPoints, fighter.Initiative,
                        fighter.Name, fighter.Gamer, fighter.Profession, (FighterSystem)fighter.FighterSystem,
                        (FighterType)fighter.FighterType));
                }

                SavedFighters = listOfFighters;
            }

            return false;
        }

        private void AddFighterToSavedFighterList(object o)
        {
            if (SelectedFighter != null)
            {
                if (!_lstSavedFighters.Where(x => x.Name == SelectedFighter.Name).Any())
                    _lstSavedFighters.Add(SelectedFighter);
            }
            OnPropertyChanged(nameof(SavedFighters), nameof(SavedFightersCollection));
        }

        private void AddSavedFighterToFightersList(object o)
        {
            if (SelectedSavedFighter != null)
            {
                SelectedSavedFighter.SetInitiativeRoll();
                if (SelectedSavedFighter.FighterType == FighterType.NPC)
                    _lstFighters.Add(new Fighter(SelectedSavedFighter));
                else if (!_lstFighters.Where(x => x.Name == SelectedSavedFighter.Name).Any())
                    _lstFighters.Add(new Fighter(SelectedSavedFighter));

            }
            OnPropertyChanged(nameof(Fighters), nameof(FightersCollection));
        }

        private void DeleteFromSavedFightersList(object o)
        {
            if (SelectedSavedFighter != null)
                _lstSavedFighters.Remove(SelectedSavedFighter);
            OnPropertyChanged(nameof(SavedFighters), nameof(SavedFightersCollection));
        }

        private void OnSelectedFighter()
        {
            if (SelectedSavedFighter != null)
            {
                CurrentEditFighter = new Fighter(SelectedSavedFighter);

                InitiativeRoll = CurrentEditFighter.Initiative;
                NameNew = CurrentEditFighter.Name;
                GamerNew = CurrentEditFighter.Gamer;
                HpNew = CurrentEditFighter.MaxHealthPoints;
                ProfessionNew = CurrentEditFighter.Profession;
                SelectedFighterGamerType =
                    FighterGamerType.FirstOrDefault(x => x.Type == CurrentEditFighter.FighterType);
                SelectedFighterSystemType =
                    FighterSystemType.FirstOrDefault(x => x.System == CurrentEditFighter.FighterSystem);
            }
        }

        private void AddDamage(object o)
        {
            int damage = 0;
            int.TryParse(o.ToString(), out damage);
            FightEffects.AddDamage(damage);
            var damagedFighter = FightEffects.Fighter;
            _lstFighters.Remove(SelectedFighter);
            SelectedFighter = null;
            _lstFighters.Add(damagedFighter);
            Log = $"{GetTime} Zadano obrazenia [{damage}] graczowi {damagedFighter.Name}({damagedFighter.Gamer}).\n" + Log;
            OnPropertyChanged(nameof(Fighters), nameof(FightersCollection));
            SelectedFighter = damagedFighter;
        }

        private void AddHealing(object o)
        {
            int healing = 0;
            int.TryParse(o.ToString(), out healing);
            FightEffects.AddHealing(healing);
            var healedFighter = FightEffects.Fighter;
            _lstFighters.Remove(SelectedFighter);
            SelectedFighter = null;
            _lstFighters.Add(healedFighter);
            Log = $"{GetTime} Uleczono [{healing}] gracza {healedFighter.Name}({healedFighter.Gamer}).\n" + Log;
            OnPropertyChanged(nameof(Fighters), nameof(FightersCollection));
            SelectedFighter = healedFighter;
        }

        private void AddGamer(object o)
        {
            if (!IsAddNewGamerEnabled)
            {
                MessageBox.Show("Wypełnij wszystkie pola opisujące nową postać.");
                return;
            }
            if (SavedFighters != null)
            {
                if (IsNewPlayer)
                    NewGamer();
                else
                    EditGamer();
            }
        }

        private void CloseApp(object o)
        {
            Application.Current.MainWindow.Close();
        }

        private void About(object o)
        {
            string about =
                "Opracowane przez Jakub Marcickiewicz" +
                "\nmarcickiewiczjakub@gmail.com";
            MessageBox.Show(about, "O programie", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
        }

        private void ResetTable(object o)
        {
            _lstFighters.Clear();
            Log = $"\n{GetTime} Lista postaci wyczyszczona.\n\n" + Log;
            OnPropertyChanged(nameof(Fighters), nameof(FightersCollection));
        }

        private void DeleteSelected(object o)
        {
            var item = o as Fighter;
            //var fighterToRemove = _lstFighters.FindIndex(x => x.Name == item.Name && x.Gamer == item.Gamer);
            //_lstFighters.RemoveAt(fighterToRemove);
            _lstFighters.Remove(SelectedFighter);
            Log = $"{GetTime} Usunięto gracza {item.Name}({item.Gamer}).\n" + Log;
            OnPropertyChanged(nameof(Fighters), nameof(FightersCollection));
        }

        private void NewGamer()
        {
            var fighter = new Fighter(
                        Guid.NewGuid(),
                        HpNew,
                        InitiativeRoll,
                        NameNew,
                        GamerNew,
                        ProfessionNew,
                        SelectedFighterSystemType.System,
                        SelectedFighterGamerType.Type);
            _lstSavedFighters.Add(fighter);
            Log = $"{GetTime} Dodano gracza {fighter.Name}({fighter.Gamer}).\n" + Log;
            OnPropertyChanged(nameof(SavedFighters), nameof(SavedFightersCollection));
        }

        private void EditGamer()
        {
            var fighter = SavedFighters.FirstOrDefault(x => x.Id == CurrentEditFighter.Id);
            var editedFighter = new Fighter(
                                    CurrentEditFighter.Id,
                                    HpNew,
                                    InitiativeRoll,
                                    NameNew,
                                    GamerNew,
                                    ProfessionNew,
                                    SelectedFighterSystemType.System,
                                    SelectedFighterGamerType.Type);

            _lstSavedFighters.Remove(fighter);
            _lstSavedFighters.Add(editedFighter);
            LogChanges(editedFighter, fighter);
            OnPropertyChanged(nameof(SavedFighters), nameof(SavedFightersCollection));
            CurrentEditFighter = null;
        }

        private void LogChanges(Fighter editedFighter, Fighter baseFighter)
        {
            string logMessage = "";

            logMessage += $"{GetTime} Edytowano gracza {editedFighter.Name}({editedFighter.Gamer}). ";
            logMessage += $"\n Zmiany: \n";
            if (editedFighter.Initiative != baseFighter.Initiative)
                logMessage += $"Inicjatywa - {editedFighter.Initiative}(było {baseFighter.Initiative}). \n";
            if (editedFighter.MaxHealthPoints != baseFighter.MaxHealthPoints)
                logMessage += $"Punkty życia - {editedFighter.MaxHealthPoints}(było {baseFighter.MaxHealthPoints}). \n";
            if (editedFighter.Gamer != baseFighter.Gamer)
                logMessage += $"Gracz - {editedFighter.Gamer}(było {baseFighter.Gamer}). \n";
            if (editedFighter.FighterType != baseFighter.FighterType)
                logMessage += $"Typ postaci - {editedFighter.FighterType}(było {baseFighter.FighterType}). \n";
            if (editedFighter.FighterSystem != baseFighter.FighterSystem)
                logMessage += $"System - {editedFighter.FighterSystem}(było {baseFighter.FighterSystem}). \n";
            if (editedFighter.Profession != baseFighter.Profession)
                logMessage += $"Profesja/klasa - {editedFighter.Profession}(było {baseFighter.Profession}). \n";

            Log = $"{logMessage}\n" + Log;
        }

        #endregion Private Methods
    }
}
