namespace Census.ViewModels;

public partial class CensusDetailViewModel : ObservableObject
{

  [ObservableProperty]
  private Friend friend;  //passed through this constructor from the CollectionView.SelectedItem

  public CensusDetailViewModel(Friend friend)
  {
    Friend = friend;  //is this constructor and assignment necessary to link to the ObservableProperty?
  }

  public ICommand UpdateReturnMainPageCommand => new Command<Object>(async (Object e) =>
  {
    await App.Database.UpdateFriendAsync(Friend);

    INavigation navigation = App.Current.MainPage.Navigation;
    await navigation.PopModalAsync();
  });

  public ICommand AddReturnMainPageCommand => new Command<Object>(async (Object e) =>
  {
    Friend.Id = 0;  //will this be enough to add a new friend?
    await App.Database.SaveFriendAsync(Friend);

    INavigation navigation = App.Current.MainPage.Navigation;
    await navigation.PopModalAsync();
  });

  public ICommand DeleteReturnMainPageCommand => new Command<Object>(async (Object e) =>
  {
    await App.Database.DeleteFriendAsync(Friend);

    INavigation navigation = App.Current.MainPage.Navigation;
    await navigation.PopModalAsync();
  });

  public ICommand CancelReturnMainPageCommand => new Command<Object>(async (Object e) =>
  {
    INavigation navigation = App.Current.MainPage.Navigation;
    await navigation.PopModalAsync();
  });

}
