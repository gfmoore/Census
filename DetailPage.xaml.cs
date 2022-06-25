namespace Census;

public partial class DetailPage : ContentPage
{
public DetailPage(Friend f)
{
    InitializeComponent();
    this.BindingContext = new CensusDetailViewModel(f);
  }
}