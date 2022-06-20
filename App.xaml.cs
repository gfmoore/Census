﻿namespace Census;

public partial class App : Application
{
  //setup database
  private static Database.Database database;
  private static string filename = "database.db3";
  public static Database.Database Database
  {
    get
    {
      if (database == null)
      {
        database = new Database.Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), filename));
      }
      return database;
    }
  }

  public App()
  {
    InitializeComponent();

    MainPage = new AppShell();
  }
}
