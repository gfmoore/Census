namespace Census;

public partial class MainPage : ContentPage
{
  public MainPage()
  {
    InitializeComponent();
    BindingContext = new CensusViewModel();
  }
  //protected override void OnAppearing()
  //{
  //  base.OnAppearing();
  //}

}