namespace Census.ViewModels;

[QueryProperty(nameof(Friend), "Friend")]

public partial class CensusDetailViewModel : ObservableObject
{

  [ObservableProperty]
  private Friend friend;

  [ObservableProperty]
  private bool entryEnabled = true;

  public CensusDetailViewModel()
  {

  }

  [RelayCommand]
  public async void UpdateReturnMainPage()
  {
    //I'll need to encrypt back to the database!!!
    await App.Database.UpdateFriendAsync(Friend);
    Debug.WriteLine($"Friend {Friend.FName} {Friend.LName} updated!");

    //send a message to mainpage to resort data
    MessagingCenter.Send(new MessagingMarker(), "FriendUpdated");

    await Shell.Current.GoToAsync("..");
  }

  [RelayCommand]
  public async void AddReturnMainPage()
  {
    Friend.Id = 0;  //will this be enough to add a new friend?
    await App.Database.SaveFriendAsync(Friend);
    Debug.WriteLine($"New friend {Friend.FName} {Friend.LName} added!");

    //send a message to mainpage to resort data
    MessagingCenter.Send(new MessagingMarker(), "FriendUpdated");

    await Shell.Current.GoToAsync("..");
  }

  [RelayCommand]
  public async void DeleteReturnMainPage()
  {
    bool response = await Application.Current.MainPage.DisplayAlert("Delete?", "Do you want to delete this Friend?", "Yes", "No/Cancel");
    if (!response) return;

    await App.Database.DeleteFriendAsync(Friend);
    Debug.WriteLine($"Friend {Friend.FName} {Friend.LName} deleted!");
    Friend = null;

    //send a message to mainpage to resort data
    MessagingCenter.Send(new MessagingMarker(), "FriendUpdated");

    await Shell.Current.GoToAsync("..");
  }

  [RelayCommand]
  public async static void CancelReturnMainPage()
  {
    //dont need a message

    await Shell.Current.GoToAsync("..");
  }

  public void DismissKeyboard()
  {
    EntryEnabled = false;
    EntryEnabled = true;
  }
}
