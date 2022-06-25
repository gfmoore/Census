namespace Census.ViewModels;

public partial class CensusDetailViewModel : ObservableObject
{

  [ObservableProperty]
  private Friend friend;  //passed through this constructor from the CollectionView.SelectedItem

  //[ObservableProperty]
  //private string fName;

  //[ObservableProperty]
  //private string lName;

  //[ObservableProperty]
  //private string groupId;

  //[ObservableProperty]
  //private string mobile;

  //[ObservableProperty]
  //private string landline;
 
  //[ObservableProperty]
  //private string email;
 
  //[ObservableProperty]
  //private string address;
 

  public CensusDetailViewModel(Friend friend)
  {
    //I've got access to the selected item now, it's long winded approach!!
    //f = fr;
    Friend = friend;
    //fName     = friend.FName;
    //lName     = friend.LName;
    //groupId   = friend.GroupId;
    //mobile    = friend.Mobile;
    //landline  = friend.Landline;
    //email     = friend.Email;
    //address   = friend.Address;
  }

  public ICommand UpdateReturnMainPageCommand => new Command<Object>(async (Object e) =>
  {
    //f.FName = FName;
    //f.LName = LName;
    //f.GroupId = GroupId;
    //f.Mobile = Mobile;
    //f.Landline = Landline;
    //f.Email = Email;
    //f.Address = Address;
    //await App.Database.UpdateFriendAsync(f);

    INavigation navigation = App.Current.MainPage.Navigation;
    await navigation.PopModalAsync();
  });

  public ICommand AddReturnMainPageCommand => new Command<Object>(async (Object e) =>
  {
    //f.FName = FName;
    //f.LName = LName;
    //f.GroupId = GroupId;
    //f.Mobile = Mobile;
    //f.Landline = Landline;
    //f.Email = Email;
    //f.Address = Address;
    //f.Id = 0;
    //await App.Database.SaveFriendAsync(f);

    INavigation navigation = App.Current.MainPage.Navigation;
    await navigation.PopModalAsync();
  });

  public ICommand DeleteReturnMainPageCommand => new Command<Object>(async (Object e) =>
  {
    //await App.Database.DeleteFriendAsync(f);

    INavigation navigation = App.Current.MainPage.Navigation;
    await navigation.PopModalAsync();
  });

  public ICommand CancelReturnMainPageCommand => new Command<Object>(async (Object e) =>
  {
    INavigation navigation = App.Current.MainPage.Navigation;
    await navigation.PopModalAsync();
  });

}
