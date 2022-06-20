using SQLite;

namespace Census.Classes;

public class FSGroup
{
  [PrimaryKey, AutoIncrement]
  public int Id { get; set; }

  public string GroupId { get; set; }
  public string GroupLeader { get; set; }
}