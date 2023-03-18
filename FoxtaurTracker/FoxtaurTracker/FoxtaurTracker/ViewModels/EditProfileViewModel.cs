using System.ComponentModel;
using System.Windows.Input;
using ColorPicker.Maui;
using FoxtaurTracker.Models;
using LibWebClient.Models;
using LibWebClient.Models.DTOs;
using LibWebClient.Models.Enums;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;

namespace FoxtaurTracker.ViewModels;

public class EditProfileViewModel : IQueryAttributable, INotifyPropertyChanged
{
    private readonly IWebClient _webClient;

    private User _userModel;
    private Profile _profile;
    private IReadOnlyCollection<Team> _teams;

    private DateTime _dateOfBirthMaxDate { get; set; }

    private List<BodySexItem> _bodySex = new List<BodySexItem>();
    private List<CategoryItem> _categoryItems = new List<CategoryItem>();
    private List<TeamItem> _teamItems = new List<TeamItem>();

    #region Commands

    /// <summary>
    /// Log in
    /// </summary>
    public ICommand UpdateProfileCommand { get; private set; }

    #endregion
    
    #region Profile fields

    private string _firstName { get; set; }
    private string _middleName { get; set; }
    private string _lastName { get; set; }
    private string _phone { get; set; }
    private DateTime _dateOfBirth { get; set; }
    private Color _hunterColor { get; set; }
    private int _bodySexIndex { get; set; }
    private int _categoryIndex { get; set; }
    private int _teamIndex { get; set; }

    /// <summary>
    /// First name
    /// </summary>
    public string FirstName
    {
        get
        {
            return _firstName;
        }
        set
        {
            _firstName = value;
            RaisePropertyChanged(nameof(FirstName));
        }
    }
    
    /// <summary>
    /// Middle name
    /// </summary>
    public string MiddleName
    {
        get
        {
            return _middleName;
        }
        set
        {
            _middleName = value;
            RaisePropertyChanged(nameof(MiddleName));
        }
    }
    
    /// <summary>
    /// Last name
    /// </summary>
    public string LastName
    {
        get
        {
            return _lastName;
        }
        set
        {
            _lastName = value;
            RaisePropertyChanged(nameof(LastName));
        }
    }
    
    /// <summary>
    /// Phone
    /// </summary>
    public string Phone
    {
        get
        {
            return _phone;
        }
        set
        {
            _phone = value;
            RaisePropertyChanged(nameof(Phone));
        }
    }
    
    /// <summary>
    /// Date of birth
    /// </summary>
    public DateTime DateOfBirth
    {
        get
        {
            return _dateOfBirth;
        }
        set
        {
            _dateOfBirth = value;
            RaisePropertyChanged(nameof(DateOfBirth));
        }
    }
    
    /// <summary>
    /// Hunter color
    /// </summary>
    public Color HunterColor
    {
        get
        {
            return _hunterColor;
        }
        set
        {
            _hunterColor = value;
            RaisePropertyChanged(nameof(HunterColor));
        }
    }
    
    /// <summary>
    /// Body sex index
    /// </summary>
    public int BodySexIndex
    {
        get
        {
            return _bodySexIndex;
        }
        set
        {
            _bodySexIndex = value;
            RaisePropertyChanged(nameof(BodySexIndex));
        }
    }
    
    /// <summary>
    /// Category index
    /// </summary>
    public int CategoryIndex
    {
        get
        {
            return _categoryIndex;
        }
        set
        {
            _categoryIndex = value;
            RaisePropertyChanged(nameof(CategoryIndex));
        }
    }
    
    /// <summary>
    /// Team index
    /// </summary>
    public int TeamIndex
    {
        get
        {
            return _teamIndex;
        }
        set
        {
            _teamIndex = value;
            RaisePropertyChanged(nameof(TeamIndex));
        }
    }

    #endregion
    
    /// <summary>
    /// Max date of birth
    /// </summary>
    public DateTime DateOfBirthMaxDate
    {
        get
        {
            return _dateOfBirthMaxDate;
        }
        set
        {
            _dateOfBirthMaxDate = value;
            RaisePropertyChanged(nameof(DateOfBirthMaxDate));
        }
    }
    
    /// <summary>
    /// Body sexes
    /// </summary>
    public List<BodySexItem> BodySexItems
    {
        get
        {
            return _bodySex;
        }
        set
        {
            _bodySex = value;
            RaisePropertyChanged(nameof(BodySexItems));
        }
    }
    
    /// <summary>
    /// Categories
    /// </summary>
    public List<CategoryItem> CategoryItems
    {
        get
        {
            return _categoryItems;
        }
        set
        {
            _categoryItems = value;
            RaisePropertyChanged(nameof(CategoryItems));
        }
    }
    
    /// <summary>
    /// Teams
    /// </summary>
    public List<TeamItem> TeamItems
    {
        get
        {
            return _teamItems;
        }
        set
        {
            _teamItems = value;
            RaisePropertyChanged(nameof(TeamItems));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public EditProfileViewModel()
    {
        _webClient = App.ServicesProvider.GetService<IWebClient>();
        
        DateOfBirthMaxDate = DateTime.UtcNow;
        
        #region Body sexes
        
        BodySexItems.Add(new BodySexItem(BodySex.Male, "Male", (int)BodySex.Male));
        BodySexItems.Add(new BodySexItem(BodySex.FtM, "FtM", (int)BodySex.FtM));
        BodySexItems.Add(new BodySexItem(BodySex.MtF, "MtF", (int)BodySex.MtF));
        BodySexItems.Add(new BodySexItem(BodySex.Female, "Female", (int)BodySex.Female));
        BodySexItems.Add(new BodySexItem(BodySex.Intersex, "Intersex", (int)BodySex.Intersex));
        BodySexItems.Add(new BodySexItem(BodySex.NotSpecified, "Not specified", (int)BodySex.NotSpecified));
        
        #endregion

        #region Categories

        CategoryItems.Add(new CategoryItem(Category.NoCategory, "No category", (int)Category.NoCategory));
        CategoryItems.Add(new CategoryItem(Category.Junior3, "3rd Junior", (int)Category.Junior3));
        CategoryItems.Add(new CategoryItem(Category.Junior2, "2nd Junior", (int)Category.Junior2));
        CategoryItems.Add(new CategoryItem(Category.Junior1, "1st Junior", (int)Category.Junior1));
        CategoryItems.Add(new CategoryItem(Category.Third, "3rd", (int)Category.Third));
        CategoryItems.Add(new CategoryItem(Category.Second, "2nd", (int)Category.Second));
        CategoryItems.Add(new CategoryItem(Category.First, "1st", (int)Category.First));
        CategoryItems.Add(new CategoryItem(Category.CandidateToMaster, "Candidate", (int)Category.CandidateToMaster));
        CategoryItems.Add(new CategoryItem(Category.Master, "Master", (int)Category.Master));
        CategoryItems.Add(new CategoryItem(Category.InternationalMaster, "International Master", (int)Category.InternationalMaster));
        CategoryItems.Add(new CategoryItem(Category.HonoredMaster, "Honored Master", (int)Category.HonoredMaster));
        CategoryItems.Add(new CategoryItem(Category.NotSpecified, "Not specified", (int)Category.NotSpecified));

        #endregion

        #region Commands binding

        UpdateProfileCommand = new Command(async () => await UpdateProfileAsync());

        #endregion
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _userModel = (User)query["UserModel"];
        _profile = (Profile)query["Profile"];
    }

    public void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    private async Task UpdateProfileAsync()
    {
        HunterColor.ToRgba(out var hunterColorR, out var hunterColorG, out var hunterColorB, out var hunterColorA);
        
        var request = new ProfileUpdateRequest
        (
            FirstName,
            MiddleName,
            LastName,
            BodySexItems.Single(s => s.Index == BodySexIndex).Id,
            DateOfBirth.ToUniversalTime(),
            Phone,
            TeamIndex != 0 ? TeamItems.Single(t => t.Index == TeamIndex).Team.Id : null,
            CategoryItems.Single(c => c.Index == CategoryIndex).Id,
            new ColorDto() { R = hunterColorR, G = hunterColorG, B = hunterColorB, A = hunterColorA }
        );

        _profile = await _webClient.UpdateProfileAsync(request);
        
        var navigationParameter = new Dictionary<string, object>
        {
            { "IsFromRegistrationPage", false },
            { "UserModel", _userModel }
        };

        await Shell.Current.GoToAsync("mainPage", navigationParameter);
    }

    public async Task OnPageLoadedAsync(Object source, EventArgs args)
    {
        _teams = _webClient.GetAllTeamsAsync().Result;
        
        await ShowProfileFieldsAsync();
    }
    
    private async Task ShowProfileFieldsAsync()
    {
        FirstName = _profile.FirstName;
        MiddleName = _profile.MiddleName;
        LastName = _profile.LastName;
        Phone = _profile.Phone;
        DateOfBirth = _profile.DateOfBirth.ToLocalTime();
        HunterColor = new Microsoft.Maui.Graphics.Color(_profile.Color.R, _profile.Color.G, _profile.Color.B, _profile.Color.A);
        BodySexIndex = _bodySex.Single(s => s.Id == _profile.Sex).Index;
        CategoryIndex = _categoryItems.Single(c => c.Id == _profile.Category).Index;

        #region Teams

        TeamItems = new List<TeamItem>();
        
        TeamItems.Add(new TeamItem(new Team(Guid.Empty, "No team", System.Drawing.Color.White), 0));

        var teamsAsList = _teams.ToList();
        for (int index = 0; index < _teams.Count; index++)
        {
            TeamItems.Add(new TeamItem(teamsAsList[index], index + 1));
        }

        TeamItems = new List<TeamItem>(TeamItems); // Dirty way to force picket to update

        TeamIndex = _profile.Team != null
            ? TeamItems.Single(t => t.Team.Id == _profile.Team.Id).Index
            : 0;

        #endregion
    }
}