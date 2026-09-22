
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace BAR_Monobattles
{
    public class MainViewModel : JsonConverter<Dictionary<String, List<Unit>>>, INotifyPropertyChanged
    {
        public MainViewModel()
        {
            RollUnitBtnEnabled = true;
            T1_RerollUnitBtnEnabled = false;
            T2_RerollUnitBtnEnabled = false;
            FactionCB_enabled = true;
            UnitTypeCB_enabled = true;
            AllowContructors = false;
            AllowZeroAttackUnits = false;
            ResetVis = Visibility.Hidden;
        }

        #region Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties        

        private string _chosenFaction = "";
        public string ChosenFaction
        {
            get
            {
                return _chosenFaction;
            }
            set
            {
                _chosenFaction = value;
                NotifyPropertyChanged(nameof(ChosenFaction));
            }
        }

        private string _chosenUType = "";
        public string ChosenUType
        {
            get
            {
                return _chosenUType;
            }
            set
            {
                _chosenUType = value;
                NotifyPropertyChanged(nameof(ChosenUType));
            }
        }

        private bool _factionCB_enabled;
        public bool FactionCB_enabled
        {
            get
            {
                return _factionCB_enabled;
            }
            set
            {
                _factionCB_enabled = value;
                NotifyPropertyChanged(nameof(FactionCB_enabled));
            }
        }

        private bool _unitTypeCB_enabled;
        public bool UnitTypeCB_enabled
        {
            get
            {
                return _unitTypeCB_enabled;
            }
            set
            {
                _unitTypeCB_enabled = value;
                NotifyPropertyChanged(nameof(UnitTypeCB_enabled));
            }
        }

        private Visibility _resetVis;
        public Visibility ResetVis
        {
            get
            {
                return _resetVis;
            }
            set
            {
                _resetVis = value;
                NotifyPropertyChanged(nameof(ResetVis));
            }
        }

        private Unit _t1_unit;
        public Unit T1_Unit
        {
            get
            {
                if (_t1_unit == null)
                {
                    _t1_unit = new Unit();
                }
                return _t1_unit;
            }
            set
            {
                _t1_unit = value;
                NotifyPropertyChanged(nameof(T1_Unit));
            }
        }

        private Unit _t2_unit;
        public Unit T2_Unit
        {
            get
            {
                if (_t2_unit == null)
                {
                    _t2_unit = new Unit();
                }
                return _t2_unit;
            }
            set
            {
                _t2_unit = value;
                NotifyPropertyChanged(nameof(T2_Unit));
            }
        }


        private bool _allowContructors;
        public bool AllowContructors
        {
            get
            {
                return _allowContructors;
            }
            set
            {
                _allowContructors = value;
                NotifyPropertyChanged(nameof(AllowContructors));
            }
        }

        private bool _allowZeroAttackUnits;
        public bool AllowZeroAttackUnits
        {
            get
            {
                return _allowZeroAttackUnits;
            }
            set
            {
                _allowZeroAttackUnits = value;
                NotifyPropertyChanged(nameof(AllowZeroAttackUnits));
            }
        }

        private bool _rollUnitBtnEnabled;
        public bool RollUnitBtnEnabled
        {
            get
            {
                return _rollUnitBtnEnabled;
            }
            set
            {
                _rollUnitBtnEnabled = value;
                NotifyPropertyChanged(nameof(RollUnitBtnEnabled));
            }
        }

        private bool _t1_RerollUnitBtnEnabled;
        public bool T1_RerollUnitBtnEnabled
        {
            get
            {
                return _t1_RerollUnitBtnEnabled;
            }
            set
            {
                _t1_RerollUnitBtnEnabled = value;
                NotifyPropertyChanged(nameof(T1_RerollUnitBtnEnabled));
            }
        }

        private bool _t2_RerollUnitBtnEnabled;
        public bool T2_RerollUnitBtnEnabled
        {
            get
            {
                return _t2_RerollUnitBtnEnabled;
            }
            set
            {
                _t2_RerollUnitBtnEnabled = value;
                NotifyPropertyChanged(nameof(T2_RerollUnitBtnEnabled));
            }
        }

        #endregion

        #region RollUnit
        public Dictionary<String, List<Unit>> unitList = new Dictionary<String, List<Unit>>();
        public bool T1_rerolled = false;
        public bool T2_rerolled = false;
        private bool constrRemoved = false;
        private bool zeroAtkRemoved = false;
        private string firstUtype = "";
       

        public void RollUnit(string Tier, bool isReroll = false)
        {
            Random random = new();

            if (ChosenFaction == "Random")
            {
                ChosenFaction = random.GetItems<string>(["Cortex", "Legion", "Armada"], 1)[0];
            }

            if (ChosenUType == "Random")
            {
                ChosenUType = random.GetItems<string>(["Bots", "Vehicles", "Aircraft", "Sea", "Hover"], 1)[0];

            }

            if (ChosenUType != firstUtype && unitList.Count != 0)
            {
                unitList = GetUnitList();
                constrRemoved = false;
                zeroAtkRemoved = false;
            }

            if (!isReroll)
            {
                if (unitList == null || unitList.Count == 0)
                {
                    unitList = GetUnitList();
                    constrRemoved = false;
                    zeroAtkRemoved = false;
                }
            }

            if (unitList != null && unitList.Count > 0)
            {
                firstUtype = ChosenUType;
                if (!AllowContructors && !constrRemoved)
                {
                    constrRemoved = true;

                    foreach (var tier in unitList.Values)
                    {
                        tier.RemoveAll(u =>
                            u.UnitDescription.Contains("Constructor", StringComparison.OrdinalIgnoreCase));
                    }
                }

                if (!AllowZeroAttackUnits && !zeroAtkRemoved)
                {
                    zeroAtkRemoved = true;
                    foreach (var tier in unitList.Values)
                    {
                        tier.RemoveAll(u =>
                            u.CanAttack == 0);
                    }
                }

                if (isReroll)
                {
                    if (unitList.TryGetValue(Tier, out var units) && units.Count > 0)
                    {
                        Unit selectedUnit = units[random.Next(units.Count)];

                        switch (Tier)
                        {
                            case "Tier_1":
                                T1_Unit = selectedUnit;
                                break;

                            case "Tier_2":
                                T2_Unit = selectedUnit;
                                break;
                        }
                    }
                }
                else
                {
                    if (unitList.TryGetValue("Tier_1", out var tier1Units) && tier1Units.Count > 0)
                    {
                        T1_Unit = tier1Units[random.Next(tier1Units.Count)];
                    }

                    if (unitList.TryGetValue("Tier_2", out var tier2Units) && tier2Units.Count > 0)
                    {
                        T2_Unit = tier2Units[random.Next(tier2Units.Count)];
                    }
                }

            }

            if(T1_rerolled && T2_rerolled)
            {
                ResetVis = Visibility.Visible;
            }
        }
        #endregion

        public void ResetAll()
        {
            T1_Unit = new Unit(); 
            T2_Unit = new Unit();
            RollUnitBtnEnabled = true;
            T1_RerollUnitBtnEnabled = true;
            T2_RerollUnitBtnEnabled = true;
            UnitTypeCB_enabled = true;
            FactionCB_enabled = true;
            ResetVis = Visibility.Hidden;
            zeroAtkRemoved = false;
            constrRemoved = false;
        }

        #region Get Units from list
        public Dictionary<String, List<Unit>> GetUnitList()
        {
            Utf8JsonReader utf8JsonReader = new Utf8JsonReader();
            Type type = typeof(Unit);
            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
            var UnitList = Read(ref utf8JsonReader, type, jsonSerializerOptions);

            if (UnitList == null)
            {
                return new Dictionary<String, List<Unit>>();
            }
            return UnitList;
        }

        public override Dictionary<string, List<Unit>>? Read( ref Utf8JsonReader reader,  Type typeToConvert,  JsonSerializerOptions options)
        {
            string resourceName = $"{typeof(Unit).Assembly.GetName().Name}.UnitLists.{ChosenFaction}Units.json";

            using Stream? stream = typeof(Unit).Assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                MessageBox.Show($"Can't find embedded Unit-List File!\n\n{resourceName}", "Error", MessageBoxButton.OK,  MessageBoxImage.Error);

                return null;
            }

            using JsonDocument document = JsonDocument.Parse(stream);

            var result = new Dictionary<string, List<Unit>>();

            // Hover has no T2, so use Vehicles T2
            if (ChosenUType == "Hover")
            {
                // Hover T1
                if (document.RootElement.TryGetProperty("Hover", out JsonElement hoverUnits))
                {
                    foreach (JsonElement tierObject in hoverUnits.EnumerateArray())
                    {
                        if (tierObject.TryGetProperty(
                                "Tier_1",
                                out JsonElement tier1))
                        {
                            var units = JsonSerializer.Deserialize<List<Unit>>(
                                tier1.GetRawText(),
                                options);

                            if (units != null)
                                result["Tier_1"] = units;
                        }
                    }
                }

                // Vehicles T2
                if (document.RootElement.TryGetProperty("Vehicles", out JsonElement vehicles))
                {
                    foreach (JsonElement tierObject in vehicles.EnumerateArray())
                    {
                        if (tierObject.TryGetProperty("Tier_2", out JsonElement tier2))
                        {
                            var units = JsonSerializer.Deserialize<List<Unit>>(tier2.GetRawText(), options);

                            if (units != null)
                                result["Tier_2"] = units;
                        }
                    }
                }
            }
            else // Roll for Bots, Vehicles, Aircraft, Sea
            {                
                if (document.RootElement.TryGetProperty(ChosenUType, out JsonElement category))
                {
                    foreach (JsonElement tierObject in category.EnumerateArray())
                    {
                        foreach (JsonProperty tier in tierObject.EnumerateObject())
                        {
                            var units = JsonSerializer.Deserialize<List<Unit>>(tier.Value.GetRawText(), options);

                            if (units != null)
                            {
                                foreach (Unit unit in units)
                                {
                                    unit.UnitTier = tier.Name;

                                    if (string.IsNullOrWhiteSpace(unit.UnitImage))
                                    {
                                        unit.UnitImage = null;
                                    }
                                }
                                result[tier.Name] = units;
                            }
                        }
                    }
                }
            }
            return result;
        }
        #endregion

        public override void Write(Utf8JsonWriter writer, Dictionary<string, List<Unit>> value, JsonSerializerOptions options)
        {
            //Only for Interface Implementation here. Not needed for Logic
            throw new NotImplementedException();
        }
    }
}
