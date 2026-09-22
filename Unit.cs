namespace BAR_Monobattles
{
    public class Unit
    {
        private string _unitname = "";
        public string UnitName
        {
            get
            {
                if (_unitname == null)
                {
                    _unitname = string.Empty;
                }
                return _unitname;
            }
            set
            {
                _unitname = value;
            }
        }

        private string _unitdescription = "";
        public string UnitDescription
        {
            get
            {
                if (_unitdescription == null)
                {
                    _unitdescription = string.Empty;
                }
                return _unitdescription;
            }
            set
            {
                _unitdescription = value;
            }
        }

        private string _unittier = "";
        public string UnitTier
        {
            get
            {
                if (_unittier == null)
                {
                    _unittier = string.Empty;
                }
                return _unittier;
            }
            set
            {
                _unittier = value;
            }
        }

        private string _building = "";
        public string Building
        {
            get
            {
                if (_building == null)
                {
                    _building = string.Empty;
                }
                return _building;
            }
            set
            {
                _building = value;
            }
        }

        private string? _unitimage;

        public string? UnitImage
        {
            get => _unitimage;
            set => _unitimage = value;
        }

        public int CanAttack { get; set; } = 0;

    }
}
