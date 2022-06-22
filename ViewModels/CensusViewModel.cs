using Census.Classes;

namespace Census.ViewModels;

public partial class CensusViewModel : ObservableObject
{
  [ObservableProperty]
  public bool importVisible = true;

  [ObservableProperty]
  public bool destroyVisible = true;

  [ObservableProperty]
  public ObservableCollection<Friend> friendsOC = new();

  [ObservableProperty]
  List<string> fSGroups = new() { "All", "1", "2", "3", "4", "5", "6", "" };

  [ObservableProperty]
  private int productGroupsIndex = -1;

  public CensusViewModel()
  {
    Console.WriteLine("At CensusViewModel constructor");
    LoadData();
  }

  public async void LoadData()
  {
    //FriendsOC.Clear();
    List<Friend> l = await App.Database.GetFriendsAsync();
    FriendsOC = new ObservableCollection<Friend>(l);
  }

  public ICommand AddFriendCommand => new Command(() =>
  {
    Console.WriteLine("Add Friend");
    Friend g = new()
    {
      FName = "A",
      LName = "T"
    };
    FriendsOC.Add(g);
  });

  public Friend SelectedItem { get; set; }

  //respond to item select
  public ICommand SelectionChangedCommand => new Command<Object>((Object e) =>
  {
    Console.WriteLine($"selection made {SelectedItem.FName} {SelectedItem.LName}");
    SelectedItem = null;
  });

  //Reveal Actions
  public ICommand RevealCommand => new Command(() =>
  {
    Console.WriteLine("Reveal");
    ImportVisible = !ImportVisible;
    DestroyVisible = !DestroyVisible;
  });

  //Destroy
  int pushCount = 0;
  public ICommand DestroyCommand => new Command(async () =>
  {
    pushCount++;
    if (pushCount == 3)
    {
      Console.WriteLine($"DESTROY...");
      await App.Database.DeleteAllFriendsAsync();
      await App.Database.DeleteAllGroupsAsync();
      Console.WriteLine($"DESTROYED!!");

      pushCount = 0;
    }

  });

  //sort and filter list
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

  [ObservableProperty]
  int indexChanged = 0;

  partial void OnProductGroupsIndexChanged(int value)
  {
    filterData(value);  //to use async await
  }

  private async void filterData(int value)
  {
    //get original data from sqlite
    List<Friend> l = await App.Database.GetFriendsAsync();
    FriendsOC = new ObservableCollection<Friend>(l);
    if (value != 0)  //0 = All
    {
      FriendsOC = new ObservableCollection<Friend>(FriendsOC.Where(x => x.GroupId == value.ToString()));
    }
  }


  //Import Friends and Groups from json files
  public static ICommand ImportFriendsCommand => new Command(async () =>
  {
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

    await PickFriends(options);
    Console.WriteLine("Friends imported.");


    Console.WriteLine($"Import groups");
    options.PickerTitle = "Please select groups.json file";

    await PickGroups(options);
    Console.WriteLine("Groups imported");


    //refresh
    //FriendsList.ItemsSource = await App.Database.GetFriendsAsync();
  });



  public static async Task<FileResult> PickFriends(PickOptions options)
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

  public static async Task<FileResult> PickGroups(PickOptions options)
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

}


