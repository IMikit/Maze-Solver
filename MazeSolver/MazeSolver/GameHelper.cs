using System;
using MazeSolver.ServiceReference1;

namespace MazeSolver
{
	public class GameHelper
	{

		private GameClient gameClient { get; set; }
		private PlayerGame playerGame { get; set; }
		private Game game { get; set; }

		public GameHelper()
		{
		}

		public void initNewGame(Difficulty difficulty, string playerName)
		{
			gameClient = new GameClient();
			playerGame = gameClient.CreateGame(difficulty, playerName);
			game = gameClient.LoadGame(playerGame.Key);

		}


		public int[] getDimensions()
		{
			int[] dimensions = new int[2];
			dimensions[0] = game.Maze.Width;
			dimensions[1] = game.Maze.Height;
			return dimensions;
		}

		public string[][] getVisiblesCells()
		{
			//List<List<string>> map = new List<List<string>>();
			//string[,] chess2 = new string[20,20];
			string[][] map = new string[game.Maze.Height][];
			for (int i = 0; i < game.Maze.Height; i++)
			{
				map[i] = new string[game.Maze.Width];
			}
			foreach (Cell cell in playerGame.Player.VisibleCells)
			{
				map[cell.Position.X][cell.Position.Y] = cell.CellType.ToString().Substring(0, 1);
			}
			return map;
		}

		public void closeGame()
		{
			gameClient.Close();
		}
	}
}
