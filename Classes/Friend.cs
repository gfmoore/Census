namespace Census.Classes;

public class Friend : ObservableObject
{
  [PrimaryKey, AutoIncrement]
  public int Id { get; set; }

  private string lName;
  public string LName
  {
    get => lName;
    set => SetProperty(ref lName, value);
  }

  private string fName;
  public string FName
  {
    get => fName;
    set => SetProperty(ref fName, value);
  }

  private string groupId;
  public string GroupId
  {
    get => groupId;
    set => SetProperty(ref groupId, value);
  }

  private string mobile;
  public string Mobile
  {
    get => mobile;
    set => SetProperty(ref mobile, value);
  }

  private string landline;
  public string Landline
  {
    get => landline;
    set => SetProperty(ref landline, value);
  }
  private string email;
  public string Email
  {
    get => email;
    set => SetProperty(ref email, value);
  }

  private string address;
  public string Address
  {
    get => address;
    set => SetProperty(ref address, value);
  }

  private bool isVisible;
  public bool IsVisible
  {
    get => isVisible;
    set => SetProperty(ref isVisible, value);
  }
}
