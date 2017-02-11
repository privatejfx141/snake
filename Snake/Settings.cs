using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    class Settings
    {
        // dimensions of the circles.
        public static int width { get; set; }
        public static int height { get; set; }
        // how fast the player is moving.
        public static int speed { get; set; }
        // total score of the current game.
        public static int score { get; set; }
        // how much points is to be added for food eaten.
        public static int points { get; set; }
        // determines if game will end.
        public static bool gameOver { get; set; }
        // directions for snake movement.
        public static Direction direction { get; set; }

        public Settings()
        {
            width = 16;
            height = 16;
            speed = 16;
            score = 0;
            gameOver = false;
            direction = Direction.Down;
        }
    }
}
