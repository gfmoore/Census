using SQLite;

namespace Census.Classes;

public class Friend
{
  [PrimaryKey, AutoIncrement]
  public int Id { get; set; }
  public string LName { get; set; }
  public string FName { get; set; }
  public int GroupId { get; set; }
  public string Mobile { get; set; }
  public string Landline { get; set; }
  public string Email { get; set; }
  public string Address { get; set; }
  public bool IsVisible { get; set; }
}
