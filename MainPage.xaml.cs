namespace Census;

public partial class MainPage : ContentPage
{
  public MainPage(CensusViewModel viewModel)
  {
    InitializeComponent();
    BindingContext = viewModel;
  }
}