﻿namespace Census;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
    Application.Current.UserAppTheme = AppTheme.Dark;
  }
}