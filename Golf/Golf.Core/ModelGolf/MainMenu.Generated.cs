/* Generated by MyraPad at 03/03/2021 16:58:19 */
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI.Properties;

#if MONOGAME || FNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#elif STRIDE
using Stride.Core.Mathematics;
#else
using System.Drawing;
using System.Numerics;
#endif

namespace UI
{
	partial class MainMenu: Panel
	{
		private void BuildUI()
		{
			var label1 = new Label();
			label1.Text = "GolfVR";
			//label1.TextColor = Color.Orange;
			label1.Top = 10;
			label1.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;

			_menuStartNewGame = new MenuItem();
			_menuStartNewGame.Text = "Start New Game";
			_menuStartNewGame.Id = "_menuStartNewGame";

			_menuOptions = new MenuItem();
			_menuOptions.Text = "Options";
			_menuOptions.Id = "_menuOptions";

			_menuQuit = new MenuItem();
			_menuQuit.Text = "Quit";
			_menuQuit.Id = "_menuQuit";

			var verticalMenu1 = new VerticalMenu();
			verticalMenu1.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;
			verticalMenu1.VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Center;
			verticalMenu1.Items.Add(_menuStartNewGame);
			verticalMenu1.Items.Add(_menuOptions);
			verticalMenu1.Items.Add(_menuQuit);

			
			Widgets.Add(label1);
			Widgets.Add(verticalMenu1);
		}

		
		public MenuItem _menuStartNewGame;
		public MenuItem _menuOptions;
		public MenuItem _menuQuit;
	}
}
