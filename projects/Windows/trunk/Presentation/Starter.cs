using System;
using System.Threading;

namespace Presentation
{
	public class Starter
	{

		public static bool CreatePreference()
		{
			var exist = Preference.Instance;
			var result = exist == null;
			if (result)
			{
				var window = new Preference();
				window.Show();
			}
			else
			{
				exist.Focus();
			}
			return result;
		}
	}
}
