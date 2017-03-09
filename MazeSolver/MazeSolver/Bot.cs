using MazeSolver.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    class Bot
    {

        private GameHelper gameHelper { get; set; }

        public Bot(GameHelper helper)
        {
            gameHelper = helper;
        }

        public Direction chooseDirection()
        {
            return gameHelper.clockRoundCheck(whereWasLastPosition());
        }

       

        private Direction whereWasLastPosition()
        {
            if(gameHelper.getLastPosition().X == gameHelper.getPlayerPosition().X &&
                gameHelper.getLastPosition().Y < gameHelper.getPlayerPosition().Y)
            {
                return Direction.Up;
            }
            else if(gameHelper.getLastPosition().X > gameHelper.getPlayerPosition().X &&
                gameHelper.getLastPosition().Y == gameHelper.getPlayerPosition().Y)
            {
                return Direction.Right;
            }
            else if (gameHelper.getLastPosition().X == gameHelper.getPlayerPosition().X &&
              gameHelper.getLastPosition().Y > gameHelper.getPlayerPosition().Y)
            {
                return Direction.Down;
            }
            
            return Direction.Left;
        }

        


        
        

    }
}
