using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace EkandidatoApp;

public class VotingViewModel : INotifyPropertyChanged
{
    // Bind the page directly to the shared repository lists
    public ObservableCollection<CandidateVm> President => CandidateRepository.President;
    public ObservableCollection<CandidateVm> VicePresident => CandidateRepository.VicePresident;
    public ObservableCollection<CandidateVm> Secretary => CandidateRepository.Secretary;
    public ObservableCollection<CandidateVm> Treasurer => CandidateRepository.Treasurer;
    public ObservableCollection<CandidateVm> PRO => CandidateRepository.PRO;

    private CandidateVm? _selectedPresident;
    public CandidateVm? SelectedPresident
    {
        get => _selectedPresident;
        set { _selectedPresident = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsReady)); }
    }

    private CandidateVm? _selectedVicePresident;
    public CandidateVm? SelectedVicePresident
    {
        get => _selectedVicePresident;
        set { _selectedVicePresident = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsReady)); }
    }

    private CandidateVm? _selectedSecretary;
    public CandidateVm? SelectedSecretary
    {
        get => _selectedSecretary;
        set { _selectedSecretary = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsReady)); }
    }

    private CandidateVm? _selectedTreasurer;
    public CandidateVm? SelectedTreasurer
    {
        get => _selectedTreasurer;
        set { _selectedTreasurer = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsReady)); }
    }

    private CandidateVm? _selectedPRO;
    public CandidateVm? SelectedPRO
    {
        get => _selectedPRO;
        set { _selectedPRO = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsReady)); }
    }

    public bool IsReady =>
        SelectedPresident != null &&
        SelectedVicePresident != null &&
        SelectedSecretary != null &&
        SelectedTreasurer != null &&
        SelectedPRO != null;

    public ICommand SelectCandidateCommand { get; }
    public ICommand SubmitVoteCommand { get; }

    public VotingViewModel()
    {
        // Seed only if there is nothing yet (so preview works before you add real data)
        if (!President.Any() && !VicePresident.Any() && !Secretary.Any() && !Treasurer.Any() && !PRO.Any())
        {
            President.Add(new CandidateVm { Name = "Alice Cruz", Party = "Party A", Slogan = "Forward together" });
            President.Add(new CandidateVm { Name = "Ben Reyes", Party = "Party B", Slogan = "Serve and lead" });
            VicePresident.Add(new CandidateVm { Name = "Cara D.", Party = "Party A", Slogan = "Unity first" });
            VicePresident.Add(new CandidateVm { Name = "Dino L.", Party = "Party B", Slogan = "Action now" });
            Secretary.Add(new CandidateVm { Name = "Eunice M.", Party = "Party A", Slogan = "Organize & optimize" });
            Treasurer.Add(new CandidateVm { Name = "Ferdie N.", Party = "Party A", Slogan = "Accountable funds" });
            PRO.Add(new CandidateVm { Name = "Gino O.", Party = "Party A", Slogan = "Clear comms" });
        }

        SelectCandidateCommand = new Command<CandidateVm>(OnSelectCandidate);

        SubmitVoteCommand = new Command(async () =>
        {
            if (!IsReady)
            {
                await Application.Current?.MainPage?.DisplayAlert("Incomplete", "Please pick one for each required position.", "OK");
                return;
            }
            await Application.Current?.MainPage?.DisplayAlert("Submitted", "Your vote has been submitted.", "OK");
        });
    }

    private void OnSelectCandidate(CandidateVm? candidate)
    {
        if (candidate is null) return;

        void Pick(ObservableCollection<CandidateVm> list, ref CandidateVm? selectedProp, CandidateVm choice)
        {
            foreach (var c in list) c.IsSelected = false;
            choice.IsSelected = true;
            selectedProp = choice;
            OnPropertyChanged(nameof(IsReady));
        }

        if (President.Contains(candidate)) Pick(President, ref _selectedPresident, candidate);
        else if (VicePresident.Contains(candidate)) Pick(VicePresident, ref _selectedVicePresident, candidate);
        else if (Secretary.Contains(candidate)) Pick(Secretary, ref _selectedSecretary, candidate);
        else if (Treasurer.Contains(candidate)) Pick(Treasurer, ref _selectedTreasurer, candidate);
        else if (PRO.Contains(candidate)) Pick(PRO, ref _selectedPRO, candidate);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}