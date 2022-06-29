namespace Census;

public partial class DetailPage : ContentPage
{
//public DetailPage(Friend f)
public DetailPage()
{
    InitializeComponent();

    this.BindingContext = new CensusDetailViewModel();
  }
}