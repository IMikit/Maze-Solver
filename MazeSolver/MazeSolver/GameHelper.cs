using System;
using MazeSolver.ServiceReference1;

namespace MazeSolver
{
    public class GameHelper
    {

        private GameClient gameClient { get; set; }
        private PlayerGame playerGame { get; set; }
        private Game game { get; set; }
        private string[][] map { get; set; }
        private bool isManualMode { get; set; }

        private Position lastPosition { get; set; }
        private Position[] walkableCells { get; set; }

        public GameHelper()
        {
        }

        public void initNewGame(Difficulty difficulty, string playerName)
        {
            gameClient = new GameClient();
            playerGame = gameClient.CreateGame(difficulty, playerName);
            game = gameClient.LoadGame(playerGame.Key);
            //mode // NOt use at the moment
            initMap();
        }


        public int[] getDimensions()
        {
            int[] dimensions = new int[2];
            dimensions[0] = game.Maze.Width;
            dimensions[1] = game.Maze.Height;
            return dimensions;
        }

        public string[][] getVisiblesCells() //zsqd
        {
            foreach (Cell cell in playerGame.Player.VisibleCells)
            {
                map[cell.Position.Y][cell.Position.X] = cell.CellType.ToString().Substring(0, 1);
                //Console.WriteLine(cell.Position.Y + " " + cell.Position.X);
                if (cell.CellType == CellType.Empty)
                {
                    map[cell.Position.Y][cell.Position.X] = " ";
                }
                if (playerGame.Player.CurrentPosition.X == cell.Position.X && playerGame.Player.CurrentPosition.Y == cell.Position.Y)
                {
                    map[cell.Position.Y][cell.Position.X] = "¤";
                    if (cell.CellType == CellType.End)
                    {
                        map[cell.Position.Y][cell.Position.X] = "?";
                    }
                }
            }
            return map;
        }

        public Position[] getWalkableCell()
        {

            foreach (Cell cell in playerGame.Player.VisibleCells)
            {
                if (playerGame.Player.CurrentPosition.X == cell.Position.X && playerGame.Player.CurrentPosition.Y == cell.Position.Y - 1 ||
                    playerGame.Player.CurrentPosition.X == cell.Position.X && playerGame.Player.CurrentPosition.Y == cell.Position.Y + 1 ||
                    playerGame.Player.CurrentPosition.X - 1 == cell.Position.X && playerGame.Player.CurrentPosition.Y == cell.Position.Y ||
                    playerGame.Player.CurrentPosition.X + 1 == cell.Position.X && playerGame.Player.CurrentPosition.Y == cell.Position.Y)
                {

                }
            }
            return null;
        }

        public bool move(Direction direction)
        {
            int tmpX = playerGame.Player.CurrentPosition.X;
            int tmpY = playerGame.Player.CurrentPosition.Y;

            if (verifiyDestination(direction))
            {
                playerGame.Player = gameClient.MovePlayer(game.Key, playerGame.Player.Key, direction);

                if (playerGame.Player.CurrentPosition.X == tmpX && playerGame.Player.CurrentPosition.Y == tmpY)
                {
                    return false;
                }
                return true;

            }
            else //verifiyDestination == false
            {
                Console.WriteLine("Impossible de se déplacer dans cette direction");
                return false;
            }
        }

        public void autoStraightMove(Direction direction)
        {
            bool isWallInFront = move(direction);

        }


        public bool verifiyDestination(Direction direction)
        {
            if (direction == Direction.Right && "W" !=
                getVisiblesCells()[playerGame.Player.CurrentPosition.Y][playerGame.Player.CurrentPosition.X + 1])
            {
                return true;
            }
            else if (direction == Direction.Left && "W" !=
               getVisiblesCells()[playerGame.Player.CurrentPosition.Y][playerGame.Player.CurrentPosition.X - 1])
            {
                return true;
            }
            else if (direction == Direction.Up && "W" !=
              getVisiblesCells()[playerGame.Player.CurrentPosition.Y - 1][playerGame.Player.CurrentPosition.X])
            {
                return true;
            }
            else if (direction == Direction.Down && "W" !=
              getVisiblesCells()[playerGame.Player.CurrentPosition.Y + 1][playerGame.Player.CurrentPosition.X])
            {
                return true;
            }
            return false;
        }

        public string getEndTag()
        {
            return playerGame.Player.SecretMessage;
        }

        private void initMap()
        {
            map = new string[playerGame.Maze.Height][];
            for (int i = 0; i < playerGame.Maze.Height; i++)
            {
                map[i] = new string[playerGame.Maze.Width];
            }
            for (int i = 0; i < playerGame.Maze.Height; i++)
            {
                for (int j = 0; j < playerGame.Maze.Width; j++)
                {
                    map[i][j] = " ";
                }
            }
        }

        public void closeGame()
        {
            gameClient.Close();
        }

    }
}
