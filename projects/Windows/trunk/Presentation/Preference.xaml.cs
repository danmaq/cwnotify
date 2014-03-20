using System;
using System.Windows;

namespace Presentation
{
	/// <summary>
	/// Preference.xaml の相互作用ロジック
	/// </summary>
	public partial class Preference : Window
	{

		public static Preference Instance
		{
			get;
			private set;
		}

		static Preference()
		{
			Instance = null;
		}

		/// <summary>Constructor.</summary>
		public Preference()
		{
			if (Instance != null)
			{
				throw new InvalidOperationException();
			}
			else
			{
				Instance = this;
			}
			InitializeComponent();
			RegistEvent();
		}

		private void RegistEvent()
		{
			Closed += (sender, e) => Instance = null;
			MouseLeftButtonDown += (sender, e) => DragMove();
			ButtonClose.Click += (sender, e) => SystemCommands.CloseWindow(this);
			ButtonMinimize.Click += (sender, e) => SystemCommands.MinimizeWindow(this);
		}
	}
}
