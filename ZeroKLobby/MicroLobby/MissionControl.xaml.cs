﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Navigation;
using ZeroKLobby.ServiceReference;
using UserControl = System.Windows.Controls.UserControl;

namespace ZeroKLobby.MicroLobby
{
	/// <summary>
	/// Interaction logic for MissionControl.xaml
	/// </summary>
	public partial class MissionControl: UserControl, INavigatable
	{
		MissionServiceClient client;

		public MissionControl()
		{
			InitializeComponent();
		}

		void client_GetMissionByIDCompleted(object sender, ServiceReference.GetMissionByIDCompletedEventArgs e)
		{
			
			
		}

		void PerformAction(string actionString)
		{
			var parts = actionString.Split(':');
			if (parts.Length == 2)
			{
				if (parts[0] == "start_mission")
				{
					int missionID;
					if (int.TryParse(parts[1], out missionID)) StartMission(missionID);
				}
			}
		}

		void StartMission(int missionID)
		{
			// client.GetMissionByIDAsync(missionID);
			MessageBox.Show("Not implemented");
		}

		public string PathHead { get { return "http://zero-k.info/Missions.mvc"; } }

		public bool TryNavigate(params string[] path)
		{
			var pathString = String.Join("/", path);
			if (!pathString.StartsWith(PathHead)) return false;
			if (pathString != webBrowser.Source.OriginalString) webBrowser.Navigate(pathString);
			return true;
		}

		void WebBrowser_Navigated(object sender, NavigationEventArgs e)
		{
			if (Process.GetCurrentProcess().ProcessName == "devenv") return;
			NavigationControl.Instance.Path = e.Uri.OriginalString;
		}

		void WebBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
		{
			if (Process.GetCurrentProcess().ProcessName == "devenv") return;
			var parts = e.Uri.OriginalString.Split('@');
			if (parts.Length < 2) return;
			for (var i = 1; i < parts.Length; i++)
			{
				var action = parts[i];
				PerformAction(action);
			}
			e.Cancel = true;
			var url = parts[0].Replace("zerok://", String.Empty);
			webBrowser.Navigate(url);
		}

		private void webBrowser_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			if (Process.GetCurrentProcess().ProcessName == "devenv") return;
			client = new MissionServiceClient();
			client.GetMissionByIDCompleted += client_GetMissionByIDCompleted;
			webBrowser.Source = new Uri("http://zero-k.info/Missions.mvc");
		}
	}
}