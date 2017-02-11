using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class frmGame : Form
    {
        // snake is a list implementation.
        private List<Circle> snake = new List<Circle>();
        // food are circles.
        private Circle food = new Circle();

        public frmGame()
        {
            InitializeComponent();
        }

        private void startGame()
        {
            // disable the controls.
            lblGameOver.Visible = false;
            btnPlay.Enabled = false;
            newGameToolStripMenuItem.Enabled = false;
            // initialize with default settings.
            new Settings();
            // set game timer.
            tmrTimer.Interval = 1000 / Settings.speed;
            tmrTimer.Tick += updateScreen;
            tmrTimer.Start();
            // remove the whole snake.
            snake.Clear();
            // create the snake head node.
            Circle snakeHead = new Circle() { x = 16, y = 10 };
            // add the head to the body.
            snake.Add(snakeHead);
            lblScore.Text = Settings.score.ToString();
            generateFood();
        }

        /// <summary>
        /// Places random food objects in the game.
        /// </summary>
        private void generateFood()
        {
            // get the max x and y positions.
            int maxXPos = (picCanvas.Size.Width / Settings.width);
            int maxYPos = (picCanvas.Size.Height / Settings.height);

            // create a random food object.
            Random random = new Random();
            food = new Circle() { x = random.Next(0, maxXPos),
                y = random.Next(0, maxYPos) };
        }

        private void updateScreen(object sender, EventArgs e)
        {
            // check for game over.
            if (Settings.gameOver) {
                // check if Enter is pressed.
                if (Input.KeyPressed(Keys.Return)) { startGame(); }
            }
            else {

                Settings.duration += 1;

                if (Input.KeyPressed(Keys.D) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.A) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.W) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.S) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                movePlayer();
                lblTimer.Text = TimeSpan.FromSeconds(Settings.duration / Settings.speed).ToString();
                lblScore.Text = Settings.score.ToString();
            }

            picCanvas.Refresh();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void picCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (Settings.gameOver)
            {
                string msg = "Game Over!\nYour final score is: " + Settings.score + ".";
                msg += "\nPress 'Enter' to play again.";
                lblGameOver.Text = msg;
                lblGameOver.Visible = true;
            }
            else
            {
                Brush snakeColour;
                // draw the snake.
                for (int i=0; i<snake.Count; ++i)
                {
                    // draw and colour head black.
                    if (i == 0)
                        snakeColour = Brushes.Black;
                    // draw and colour rest of body green.
                    else
                        snakeColour = Brushes.Green;
                    // draw the whole snake.
                    canvas.FillEllipse(snakeColour,
                        new Rectangle(snake[i].x * Settings.width,
                        snake[i].y * Settings.height,
                        Settings.width, Settings.height));
                    // draw food.
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.x * Settings.width,
                        food.y * Settings.height, Settings.width,
                        Settings.height));

                }

            }

        }

        private void movePlayer()
        {
            for (int i=snake.Count-1; i>=0; i--) {
                // move the head.
                if (i == 0) {
                    switch (Settings.direction) {
                        case Direction.Right:
                            snake[i].x++;
                            break;
                        case Direction.Left:
                            snake[i].x--;
                            break;
                        case Direction.Up:
                            snake[i].y--;
                            break;
                        case Direction.Down:
                            snake[i].y++;
                            break;
                    }
                    // get max X and Y positions.
                    int maxXPos = (picCanvas.Size.Width / Settings.width);
                    int maxYPos = (picCanvas.Size.Height / Settings.height);
                    // detect collision with game boundaries.
                    if (snake[i].x < 0 || snake[i].y < 0 
                        || snake[i].x >= maxXPos || snake[i].y >= maxYPos) {
                        die();
                    }
                    // detect collision with body.
                    for (int j=1; j<snake.Count; ++j) {
                        if (snake[i].x == snake[j].x && snake[i].y == snake[j].y) {
                            die();
                        }
                    }
                    // detect collision with food.
                    if (snake[i].x == food.x && snake[i].y == food.y) {
                        eat();
                    }
                }
                // move the rest of the body.
                else {
                    // set current node to previous node's position.
                    snake[i].x = snake[i - 1].x;
                    snake[i].y = snake[i - 1].y;
                }

            }
        }

        private void eat()
        {
            // add circle to the body.
            Circle food = new Circle();
            food.x = snake[snake.Count - 1].x;
            food.y = snake[snake.Count - 1].y;
            snake.Add(food);
            // update score.
            Settings.score += Settings.points;
            // create a new food object.
            generateFood();
        }

        private void die()
        {
            Settings.gameOver = true;
            tmrTimer.Tick -= updateScreen;
            tmrTimer.Stop();
            btnPlay.Enabled = true;
        }

        private void frmGame_KeyDown(object sender, KeyEventArgs e)
        {
            // button is being pressed.
            Input.ChangeState(e.KeyCode, true);
        }

        private void frmGame_KeyUp(object sender, KeyEventArgs e)
        {
            // button is no longer being pressed.
            Input.ChangeState(e.KeyCode, false);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            Focus();
            startGame();
        }

        private void frmGame_Load(object sender, EventArgs e)
        {
            btnPlay.Text = "Play Game";
            tmrTimer.Enabled = false;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg = "This C# implementation of Snake was developed by Jeffrey Li, ";
            msg += "from December 29, 2016 to Feburary 11, 2017.";
            msg += "\nCoded in C# in Visual Studio 2015.";
            MessageBox.Show(msg, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnPlay_Click(sender, e);
        }
    }
}
