using System;
using SwinGameSDK;
using Core = SwinGameSDK.SwinGame;

namespace MyGame
{
	public class BackgroundChanger
	{
		public BackgroundChanger ()
		{
			
		}


		public static void Main(){
			Core.OpenGraphicsWindow ("bg", 768, 600);

			do
			{
				Core.ClearScreen(Color.Red);
				Core.ProcessEvents();
				Core.RefreshScreen();
				Core.GUISetBackgroundColor(Color.Black);
				Core.GUISetBackgroundColor(Color.Green);
				Core.GUISetBackgroundColorInactive(Color.Red);
				Core.RefreshScreen();
				Core.ProcessEvents();
				Core.RefreshScreen();
			} while(!Core.WindowCloseRequested ());
		}
	}
		
}

