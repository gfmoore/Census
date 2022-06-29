namespace Census;

public partial class DetailPage : ContentPage
{

public DetailPage(CensusDetailViewModel viewModel)
{
    InitializeComponent();

    //this.BindingContext = new CensusDetailViewModel();
    BindingContext = viewModel;
  }
}