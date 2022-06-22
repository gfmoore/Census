namespace Census;

public partial class MainPage : ContentPage
{
  public MainPage()
  {
    InitializeComponent();
    this.BindingContext = new CensusViewModel();
  }
  //protected override void OnAppearing()
  //{
  //  base.OnAppearing();
  //}

  //public void OnSelectedIndexChanged(object sender, EventArgs e)
  //{
  //  // do stuff
  //}

}