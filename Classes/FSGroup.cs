namespace Census.Classes;

public class FSGroup : ObservableObject
{
  [PrimaryKey, AutoIncrement]
  public int Id { get; set; }

  private string groupId;
  public string GroupId
  {
    get => groupId;
    set => SetProperty(ref groupId, value);
  }

  private string groupLeader;
  public string GroupLeader
  {
    get => groupLeader;
    set => SetProperty(ref groupLeader, value);
  }
}