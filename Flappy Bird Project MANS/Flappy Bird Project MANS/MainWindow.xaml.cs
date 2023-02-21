using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;

namespace Flappy_Bird_Project_MANS
{
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();

        double score; // keep track of the score
        int gravity = 8; // set the gravity constant
        bool gameOver; // flag to indicate whether the game is over
        Rect flappyBirdHitBox; // hit box of the flappy bird
        public MainWindow()
        {
            InitializeComponent();
            gameTimer.Tick += MainEventTimer; // attach the timer tick event
            gameTimer.Interval = TimeSpan.FromMilliseconds(20); // set the interval to 20 milliseconds
            StartGame();
        }

        private void MainEventTimer(object sender, EventArgs e)
        {
            // Updating the score display on the canvas
            txtScore.Content = "Score: " + score;

            // Creating a rectangle for hit detection for the flappy bird
            flappyBirdHitBox = new Rect(Canvas.GetLeft(flappyBird), Canvas.GetTop(flappyBird), flappyBird.Width - 5, flappyBird.Height);

            // update the position of the flappy bird
            Canvas.SetTop(flappyBird, Canvas.GetTop(flappyBird) + gravity);

            // if the bird goes out of bounds, end the game
            if (Canvas.GetTop(flappyBird) < -10 || Canvas.GetTop(flappyBird) > 500)
            {
                EndGame();
            }

            // Loop through all children in the canvas to check for intersection with pipes
            foreach (var x in MyCanvas.Children.OfType<Image>())
            {
                // Check if the child is a pipe
                if ((string)x.Tag == "obs1" || (string)x.Tag == "obs2" || (string)x.Tag == "obs3")
                {
                    // Move the pipe to the left
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 5);

                    // Increase score if the pipe goes off the screen
                    if (Canvas.GetLeft(x) < - 100)
                    {
                        Canvas.SetLeft(x, 800);
                        score += .5;
                    }

                    // Check for collision with the pipe
                    Rect pipeHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (flappyBirdHitBox.IntersectsWith(pipeHitBox))
                    {
                        EndGame();
                    }
                }
                // Check if the child is a cloud
                if ((string)x.Tag == "cloud")
                {
                    // Move the cloud to the left
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 2);
                    if (Canvas.GetLeft(x) < -250)
                    {
                        Canvas.SetLeft(x, 550);
                    }
                }
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            //Check if the space bar is pressed
            if (e.Key == Key.Space)
            {
                //Rotate the flappy bird image downwards by 20 degrees and change gravity to -6
                flappyBird.RenderTransform = new RotateTransform(-20, flappyBird.Width /2, flappyBird.Height /2);
                gravity = -6;
            }
            //Check if R key is pressed and game is over
            if (e.Key == Key.R && gameOver == true)
            {
                //Start the game
                StartGame();
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            //Rotate the flappy bird image upwards by 5 degrees and change gravity to 8
            flappyBird.RenderTransform = new RotateTransform(5, flappyBird.Width / 2, flappyBird.Height / 2);
            gravity = 8;
        }
        private void StartGame()
        {
            //Set focus to canvas
            MyCanvas.Focus();
            int temp = 300;
            score = 0;
            gameOver = false;

            //Set the initial position of flappy bird
            Canvas.SetTop(flappyBird, 30);

            //Set the initial position of obstacles and clouds
            foreach (var x in MyCanvas.Children.OfType<Image>())
            {
                if ((string)x.Tag == "obs1")
                {
                    Canvas.SetLeft(x, 500);
                }
                if ((string)x.Tag == "obs2")
                {
                    Canvas.SetLeft(x, 800);
                }
                if ((string)x.Tag == "obs3")
                {
                    Canvas.SetLeft(x, 1100);
                }

                if ((string)x.Tag == "cloud")
                {
                    Canvas.SetLeft(x, 300 + temp);
                    temp = 800;
                }
            }

            //Start the game timer
            gameTimer.Start(); 

        }

        private void EndGame()
        {
            //Stop the game timer
            gameTimer.Stop();
            gameOver = true;
            //Display Game Over message and instructions to restart the game
            txtScore.Content += " Game Over, Press R to try again";
        }
    }
}
