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
            if(gameHelper.getPlayerPosition().X == gameHelper.getPlayerPosition().X &&
                gameHelper.getPlayerPosition().Y < gameHelper.getPlayerPosition().Y)
            {
                return Direction.Up;
            }
            else if(gameHelper.getPlayerPosition().X > gameHelper.getPlayerPosition().X &&
                gameHelper.getPlayerPosition().Y == gameHelper.getPlayerPosition().Y)
            {
                return Direction.Right;
            }
            else if (gameHelper.getPlayerPosition().X == gameHelper.getPlayerPosition().X &&
              gameHelper.getPlayerPosition().Y > gameHelper.getPlayerPosition().Y)
            {
                return Direction.Down;
            }
            
            return Direction.Left;
        }

        


        
        

    }
}
