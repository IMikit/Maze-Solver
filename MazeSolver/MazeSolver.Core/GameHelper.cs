using System;
using MazeSolver.Core.Game;
using System.Collections.Generic;
using System.Drawing;

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
            lastPosition = new Position();
            lastPosition.X = 1;
            lastPosition.Y = 1;
            initMap();
        }

        public Size getDimensions()
        {
            Size dimensions = new Size();
            dimensions.Width = game.Maze.Width;
            dimensions.Height = game.Maze.Height;
            return dimensions;
        }

        public string[][] getVisiblesCells() //zsqd
        {
            //Console.WriteLine("Before getVisibleCells");
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

        public Position getPlayerPosition()
        {
            return playerGame.Player.CurrentPosition;
        }

        public Position getLastPosition()
        {
            return lastPosition;
        }

        /**
         * Return the number of walkable way adjacent to the current player's position
         * */
        public int getNumberOfWay()
        {
            string[][] visibleCells = getVisiblesCells();
            int nbOfWay = 0;
            foreach (string[] x in visibleCells)
            {
                foreach (string y in x)
                {
                    if (" " == y)
                    {
                        ++nbOfWay;
                    }
                }
            }
            return nbOfWay;
        }

        public bool move(Direction direction)
        {
            int tmpX = playerGame.Player.CurrentPosition.X;
            int tmpY = playerGame.Player.CurrentPosition.Y;

            if (verifiyDestination(direction))
            {
                lastPosition = playerGame.Player.CurrentPosition;
                playerGame.Player = gameClient.MovePlayer(game.Key, playerGame.Player.Key, direction);

                if (playerGame.Player.CurrentPosition.X == tmpX && playerGame.Player.CurrentPosition.Y == tmpY)
                {
                    return false;
                }
                return true;

            }

            Console.WriteLine("Impossible de se déplacer dans cette direction");
            return false;
        }

        public Direction clockRoundCheck(Direction directionOfLast)
        {
            if (directionOfLast == Direction.Left)
            {
                return checkFromLeft(getWalkableCell());
            }
            else if (directionOfLast == Direction.Up)
            {
                return checkFromUp(getWalkableCell());
            }
            else if (directionOfLast == Direction.Right)
            {
                return checkFromRight(getWalkableCell());
            }
            Console.WriteLine("Come from bottom");
            return checkFromDown(getWalkableCell());
        }

        public string getEndTag()
        {
            return playerGame.Player.SecretMessage;
        }

        public void closeGame()
        {
            gameClient.Close();
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

        /* Following methods do a roundclock check to return the next path with left hand on wall */
        /* Bloxk RoundClock check */
        private Direction checkFromLeft(List<Direction> walkableDirections)
        {
            if (walkableDirections.Contains(Direction.Up))
            {
                return Direction.Up;
            }
            else if (walkableDirections.Contains(Direction.Right))
            {
                return Direction.Right;
            }
            else if (walkableDirections.Contains(Direction.Down))
            {
                return Direction.Down;
            }
            return Direction.Left;
        }
        private Direction checkFromUp(List<Direction> walkableDirections)
        {
            if (walkableDirections.Contains(Direction.Right))
            {
                return Direction.Right;
            }
            else if (walkableDirections.Contains(Direction.Down))
            {
                return Direction.Down;
            }
            else if (walkableDirections.Contains(Direction.Left))
            {
                return Direction.Left;
            }
            return Direction.Up;
        }
        private Direction checkFromRight(List<Direction> walkableDirections)
        {
            if (walkableDirections.Contains(Direction.Down))
            {
                return Direction.Down;
            }
            else if (walkableDirections.Contains(Direction.Left))
            {
                return Direction.Left;
            }
            else if (walkableDirections.Contains(Direction.Up))
            {
                return Direction.Up;
            }
            return Direction.Right;
        }
        private Direction checkFromDown(List<Direction> walkableDirections)
        {
            if (walkableDirections.Contains(Direction.Left))
            {
                return Direction.Left;
            }
            else if (walkableDirections.Contains(Direction.Up))
            {
                return Direction.Up;
            }
            else if (walkableDirections.Contains(Direction.Right))
            {
                return Direction.Right;
            }
            return Direction.Down;
        }
        /* Endblock RoundClock Check */

        private bool verifiyDestination(Direction direction)
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

        private Direction convertPositionToDirection(Position positionToCompare)
        {

            if (positionToCompare.X == playerGame.Player.CurrentPosition.X &&
                positionToCompare.Y < playerGame.Player.CurrentPosition.Y)
            {
                return Direction.Up;
            }
            else if (positionToCompare.X > playerGame.Player.CurrentPosition.X &&
                positionToCompare.Y == playerGame.Player.CurrentPosition.Y)
            {
                return Direction.Right;
            }
            else if (positionToCompare.X == playerGame.Player.CurrentPosition.X &&
              positionToCompare.Y > playerGame.Player.CurrentPosition.Y)
            {
                return Direction.Down;
            }
            else if (positionToCompare.X < playerGame.Player.CurrentPosition.X &&
                      positionToCompare.Y == playerGame.Player.CurrentPosition.Y)
            {
                return Direction.Left;
            }
            return Direction.Down;
        }

        private List<Direction> getWalkableCell()
        {
            List<Direction> walkableCells = new List<Direction>();
            foreach (Cell cell in playerGame.Player.VisibleCells)
            {
                if ((playerGame.Player.CurrentPosition.X == cell.Position.X && playerGame.Player.CurrentPosition.Y == cell.Position.Y - 1 ||
                    playerGame.Player.CurrentPosition.X == cell.Position.X && playerGame.Player.CurrentPosition.Y == cell.Position.Y + 1 ||
                    playerGame.Player.CurrentPosition.X - 1 == cell.Position.X && playerGame.Player.CurrentPosition.Y == cell.Position.Y ||
                    playerGame.Player.CurrentPosition.X + 1 == cell.Position.X && playerGame.Player.CurrentPosition.Y == cell.Position.Y) &&
                    (cell.CellType == CellType.Empty || CellType.Start == cell.CellType || CellType.End == cell.CellType))
                {
                    if (cell.Position.X == lastPosition.X && cell.Position.Y == lastPosition.Y)
                    {
                        continue;
                    }
                    walkableCells.Add(convertPositionToDirection(cell.Position));
                }
            }
            return walkableCells;
        }

    }
}
