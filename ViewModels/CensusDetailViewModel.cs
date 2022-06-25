namespace Census.ViewModels;

public partial class CensusDetailViewModel : ObservableObject
{
  [ObservableProperty]
  private Friend friend;

  public ICommand ReturnMainPageCommand => new Command<Object>(async (Object e) =>
  {

    INavigation navigation = App.Current.MainPage.Navigation;
    await navigation.PopAsync();
  });
}
