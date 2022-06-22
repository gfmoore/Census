namespace Census;

public partial class MainPage : ContentPage
{
  public MainPage()
  {
    InitializeComponent();
    this.BindingContext = new CensusViewModel();
  }
}