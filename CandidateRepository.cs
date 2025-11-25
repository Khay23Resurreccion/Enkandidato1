using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EkandidatoApp
{
    public static class CandidateRepository
    {
        public static string? PartyListName { get; set; }
        public static string? PartyCoverPhotoPath { get; set; }

        public static ObservableCollection<CandidateVm> President { get; } = new();
        public static ObservableCollection<CandidateVm> VicePresident { get; } = new();
        public static ObservableCollection<CandidateVm> Secretary { get; } = new();
        public static ObservableCollection<CandidateVm> Treasurer { get; } = new();
        public static ObservableCollection<CandidateVm> PRO { get; } = new();
        public static ObservableCollection<CandidateVm> SeniorHighRep { get; } = new();
        public static ObservableCollection<CandidateVm> FirstYearRep { get; } = new();
        public static ObservableCollection<CandidateVm> SecondYearRep { get; } = new();
        public static ObservableCollection<CandidateVm> ThirdYearRep { get; } = new();
        public static ObservableCollection<CandidateVm> FourthYearRep { get; } = new();

        public static void ClearAll()
        {
            President.Clear(); VicePresident.Clear(); Secretary.Clear(); Treasurer.Clear(); PRO.Clear();
            SeniorHighRep.Clear(); FirstYearRep.Clear(); SecondYearRep.Clear(); ThirdYearRep.Clear(); FourthYearRep.Clear();
        }
    }

    public class CandidateVm : INotifyPropertyChanged
    {
        private string _name = "";
        private string _yearSection = "";
        private string _party = "";
        private string _slogan = "";
        private string? _photoPath;
        private bool _isSelected;

        public string Name { get => _name; set { _name = value; PropertyChanged?.Invoke(this, new(nameof(Name))); } }
        public string YearSection { get => _yearSection; set { _yearSection = value; PropertyChanged?.Invoke(this, new(nameof(YearSection))); } }
        public string Party { get => _party; set { _party = value; PropertyChanged?.Invoke(this, new(nameof(Party))); } }
        public string Slogan { get => _slogan; set { _slogan = value; PropertyChanged?.Invoke(this, new(nameof(Slogan))); } }
        public string? PhotoPath { get => _photoPath; set { _photoPath = value; PropertyChanged?.Invoke(this, new(nameof(PhotoPath))); } }

        public bool IsSelected { get => _isSelected; set { _isSelected = value; PropertyChanged?.Invoke(this, new(nameof(IsSelected))); } }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}