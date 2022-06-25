using Microsoft.Maui.ApplicationModel.Communication;

namespace Census.ViewModels;

public partial class CensusViewModel : ObservableObject
{
  public CensusViewModel()
  {
    LoadData();
    SetupTimer();
  }


  //selected item in the list of friends
  [ObservableProperty]
  private Friend selectedItem;

  [ObservableProperty]
  private bool passwordVisible = true;

  [ObservableProperty]
  private bool importVisible = true;

  [ObservableProperty]
  private bool destroyVisible = true;

  [ObservableProperty]
  private bool encryptVisible = true;

  [ObservableProperty]
  private bool decryptVisible = true;


  [ObservableProperty]
  private ObservableCollection<Friend> friendsOC = new();

  public List<Friend> lf = new(); //used for holding the unencrypted data for use in filtering

  //Picker for groups
  [ObservableProperty]
  List<string> fSGroups = new() { "All", "1", "2", "3", "4", "5", "6", "" };

  [ObservableProperty]
  private int fSGroupsIndex = -1;

  [ObservableProperty]
  private string fSGroupsSelectedItem = String.Empty;  //not used yet


  [ObservableProperty]
  private string passphrase;

  [ObservableProperty]
  private bool passphraseEntryEnabled = true; //connected the the passphrase entry control (view



  partial void OnFSGroupsIndexChanged(int value)
  {
    FilterData(value);  //to use async await  //using the index
    //FilterData(FSGroupsSelectedItem);       //using the string, doesn't work
  }


  public async void LoadData()
  {
    List<Friend> l = await App.Database.GetFriendsAsync();
    FriendsOC = new ObservableCollection<Friend>(l);
    //make a copy for filtering of unencrypted data
    lf = l;
  }

  //respond to item select in list of friends
  public ICommand SelectionChangedCommand => new Command<Object>(async (Object e) =>
  {
    Console.WriteLine($"Selection made {SelectedItem.FName} {SelectedItem.LName}");

    INavigation navigation = App.Current.MainPage.Navigation;
    await navigation.PushModalAsync(new DetailPage(SelectedItem));

  });






  //Reveal Actions - for decrypt encrypt import destroy buttons
  public ICommand RevealCommand => new Command(() =>
  {
    Console.WriteLine("Reveal");
    ToggleButtons();
  });
  
  public void ToggleButtons()
  {
    PasswordVisible = !PasswordVisible;
    ImportVisible = !ImportVisible;
    DestroyVisible = !DestroyVisible;
    EncryptVisible = !EncryptVisible;
    DecryptVisible = !DecryptVisible;
  }


  //sort and filter displayed data
  public ICommand SortLNameCommand => new Command(() =>
  {
    Console.WriteLine("Sort LName");
    FriendsOC = new ObservableCollection<Friend>(FriendsOC.OrderBy(x => x.LName).ThenBy(x => x.FName));
  });

  public ICommand SortFNameCommand => new Command(() =>
  {
    Console.WriteLine("Sort FName");
    FriendsOC = new ObservableCollection<Friend>(FriendsOC.OrderBy(x => x.FName));
  });

  private void FilterData(int value)  //if using the index from the pick
  //private void FilterData(string value) //using the string from the pick - doesnt work
  {
    //get original data from lf
    FriendsOC = new ObservableCollection<Friend>(lf);
    //if (value != 0)  //0 = All
    if (FSGroups[value] != "All")  
    {
      FriendsOC = new ObservableCollection<Friend>(FriendsOC.Where(x => x.GroupId == FSGroups[value]));
    }
  }


  //Import Friends and Groups from json files put in Downloads folder for device
  public ICommand ImportDataCommand => new Command( async () =>
  {
    Console.WriteLine("Import data");
    //create custom filetypes
    var customFileType = new FilePickerFileType(
      new Dictionary<DevicePlatform, IEnumerable<string>>
      {
          { DevicePlatform.Android, new[] { "application/json" } },
      });

    PickOptions options = new()
    {
      PickerTitle = "", //Only works for ios
      FileTypes = customFileType,
    };

    //get filepicker for friends.json
    Console.WriteLine($"Import friends");
    options.PickerTitle = "Please select friends.json file";

    await GrabFriends(options);
    Console.WriteLine("Friends imported.");


    Console.WriteLine($"Import groups");
    options.PickerTitle = "Please select groups.json file";

    await GrabGroups(options);
    Console.WriteLine("Groups imported");

    //refresh
    List<Friend> l = await App.Database.GetFriendsAsync();
    FriendsOC = new ObservableCollection<Friend>(l);
    
  });

  public static async Task<FileResult> GrabFriends(PickOptions options)
  {
    await App.Database.DeleteAllFriendsAsync();
    try
    {
      var result = await FilePicker.Default.PickAsync(options);
      if (result != null)
      {
        using var stream = await result.OpenReadAsync();
        var friendsjson = JsonSerializer.Deserialize<List<Friend>>(stream);

        //Go through list and add to db
        foreach (Friend f in friendsjson)
        {
          await App.Database.SaveFriendAsync(f);
        }

        Console.WriteLine("GM: JSON file data loaded");

      }

      return result;
    }
    catch (Exception ex)
    {
      Console.WriteLine("GM: Ooops in Json : " + ex.Message);
    }

    return null;
  }

  public static async Task<FileResult> GrabGroups(PickOptions options)
  {
    await App.Database.DeleteAllGroupsAsync();

    try
    {
      var result = await FilePicker.Default.PickAsync(options);
      if (result != null)
      {
        using var stream = await result.OpenReadAsync();
        var groupsjson = JsonSerializer.Deserialize<List<FSGroup>>(stream);

        //Go through list and add to db
        foreach (FSGroup g in groupsjson)
        {
          await App.Database.SaveFSGroupAsync(g);
        }

        Console.WriteLine("GM: Group JSON file data loaded");
      }

      return result;
    }
    catch (Exception ex)
    {
      Console.WriteLine("GM: Ooops in Json : " + ex.Message);
    }

    return null;
  }


  //Destroy database entries
  private static int pushCount = 0;
  private static readonly System.Timers.Timer timer = new(3000);
  public static void SetupTimer()  //called from constructir
  {
    timer.Elapsed += OnTimedEvent;
    timer.AutoReset = false;
  }

  public ICommand DestroyCommand => new Command(async () =>
  {
    pushCount++;
    if (pushCount == 1)
    {
      Console.WriteLine("Timer started");
      timer.Start();
    }
    if (pushCount == 3)
    {
      Console.WriteLine($"DESTROY...");
      await App.Database.DeleteAllFriendsAsync();
      await App.Database.DeleteAllGroupsAsync();
      Console.WriteLine($"DESTROYED!!");

      pushCount = 0;
    }
    //refresh
    List<Friend> l = await App.Database.GetFriendsAsync();
    FriendsOC = new ObservableCollection<Friend>(l);

    lf = null;
  });

  private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
  {
    // Code to be executed at the end of Timer 
    Console.WriteLine("Timer ending 3 second count");
    timer.Stop();
    timer.Close();
    pushCount = 0;
  }


  //Encrypt/decrypt data
  public ICommand EncryptCommand => new Command(() =>
  {
    Console.WriteLine("Start Encrypt displayed data ");
    EncryptData();
    Console.WriteLine("End Encrypt displayed data");
  });

  public ICommand DecryptCommand => new Command( () =>
  {
    Console.WriteLine("Start Decrypt displayed data ");
    DecryptData();
    Console.WriteLine("End Decrypt displayed data");

    //make a copy in the list lf for use in filtering
    lf = FriendsOC.ToList<Friend>();

    //if decrypted hide the text entry? and decrypt button
    PasswordVisible = false;
    DecryptVisible = false;
    ImportVisible = !ImportVisible;
    DestroyVisible = !DestroyVisible;
    EncryptVisible = !EncryptVisible;
  });

  public async void EncryptData()
  {
    //get an encryption key from the password
    byte[] encryptionKeyBytes = EncryptionHelper.CreateKey(Passphrase);
    //do the displayed data first
    foreach (Friend f in FriendsOC)
    {
      f.FName = EncryptionHelper.Encrypt(f.FName, encryptionKeyBytes);
      f.LName = EncryptionHelper.Encrypt(f.LName, encryptionKeyBytes);
      f.GroupId = EncryptionHelper.Encrypt(f.GroupId, encryptionKeyBytes);
      f.Mobile = EncryptionHelper.Encrypt(f.Mobile, encryptionKeyBytes);
      f.Landline = EncryptionHelper.Encrypt(f.Landline, encryptionKeyBytes);
      f.Email = EncryptionHelper.Encrypt(f.Email, encryptionKeyBytes);
      f.Address = EncryptionHelper.Encrypt(f.Address, encryptionKeyBytes);
    }
    //Writing encrypted data to sqlite db
    await App.Database.DeleteAllFriendsAsync();
    foreach (Friend f in FriendsOC)
    {
      await App.Database.SaveFriendAsync(f);
    }

    DismissKeyboard();
  }

  public void DecryptData()
  {
    //get an encryption key from the password
    byte[] encryptionKeyBytes = EncryptionHelper.CreateKey(Passphrase);

    foreach (Friend f in FriendsOC)
    {
      f.FName = EncryptionHelper.Decrypt(f.FName, encryptionKeyBytes);
      f.LName = EncryptionHelper.Decrypt(f.LName, encryptionKeyBytes);
      f.GroupId = EncryptionHelper.Decrypt(f.GroupId, encryptionKeyBytes);
      f.Mobile = EncryptionHelper.Decrypt(f.Mobile, encryptionKeyBytes);
      f.Landline = EncryptionHelper.Decrypt(f.Landline, encryptionKeyBytes);
      f.Email = EncryptionHelper.Decrypt(f.Email, encryptionKeyBytes);
      f.Address = EncryptionHelper.Decrypt(f.Address, encryptionKeyBytes);
    }
    DismissKeyboard();
  }

  public void DismissKeyboard()
  {
    Passphrase = "";
    PassphraseEntryEnabled = false;
    PassphraseEntryEnabled = true;
  }

}


