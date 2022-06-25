namespace Census;

public partial class DetailPage : ContentPage
{
public DetailPage()
{
    InitializeComponent();
    this.BindingContext = new CensusDetailViewModel();
  }
}