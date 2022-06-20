using Census.Classes;
using SQLite;

namespace Census.Database;

public class Database
{
  private readonly SQLiteAsyncConnection _database;

  public Database(string dbPath)
  {
    _database = new SQLiteAsyncConnection(dbPath);
    _database.CreateTableAsync<Friend>();
    _database.CreateTableAsync<FSGroup>();
  }

  public Task<List<Friend>> GetFriendsAsync()
  {
    return _database.Table<Friend>().ToListAsync();
  }

  public Task<int> SaveFriendAsync(Friend friend)
  {
    return _database.InsertAsync(friend);
  }

  public Task<int> DeleteFriendAsync(Friend friend)
  {
    return _database.DeleteAsync(friend);
  }

  public Task<int> UpdateFriendAsync(Friend friend)
  {
    return _database.UpdateAsync(friend);
  }

  public Task<int> DeleteAllFriendsAsync() => _database.DeleteAllAsync<Friend>();



  //FS Groups
  public Task<List<FSGroup>> GetFSGroupsAsync()
  {
    return _database.Table<FSGroup>().ToListAsync();
  }

  public Task<int> SaveFSGroupAsync(FSGroup fsgroup)
  {
    return _database.InsertAsync(fsgroup);
  }

  public Task<int> DeleteFSGroupAsync(FSGroup fsgroup)
  {
    return _database.DeleteAsync(fsgroup);
  }

  public Task<int> UpdateGroupAsync(FSGroup fsgroup)
  {
    return _database.UpdateAsync(fsgroup);
  }

  public Task<int> DeleteAllGroupsAsync() => _database.DeleteAllAsync<FSGroup>();

}
