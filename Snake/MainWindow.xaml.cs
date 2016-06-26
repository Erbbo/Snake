using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Snake
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    #region Main Window Attributes
    Point startingPosition;
    Point currentPosition;
    int previousDirection;
    int currentDirection;
    Random rnd;
    SnakeObj snake;
    Point food;
    double canvasWidth;
    double canvasHeight;
    int index;
    #endregion

    public MainWindow()
    {
      InitializeComponent();
      Initialize();
    }

    private void Initialize()
    {
      this.snake = new SnakeObj(this.canvas);
      this.startingPosition = new Point(10, 10);
      this.rnd = new Random();
      food = new Point();
      index = 0;
      this.CreateTimer();
    }

    public void CreateTimer()
    {
      DispatcherTimer timer = new DispatcherTimer();
      timer.Tick += timer_Tick;
      timer.Interval = TimeSpan.FromMilliseconds(1);
      timer.Start();

      this.KeyDown += canvas_KeyDown;
      snake.paintSnake(startingPosition);
      currentPosition = startingPosition;

      food.X = rnd.Next(20, 180);
      food.Y = rnd.Next(20, 180);

      createFood();
      //this.FindPath();
    }

    //private void FindPath()
    //{
    //  switch (Key.Down)
    //  {
    //    case (int)MOVINGDIRECTION.UPWARDS:
    //      if (previousDirection != (int)MOVINGDIRECTION.UPWARDS)
    //        currentDirection = (int)MOVINGDIRECTION.DOWNWARDS;
    //      break;
    //    case (int)MOVINGDIRECTION.DOWNWARDS:
    //      if (previousDirection != (int)MOVINGDIRECTION.DOWNWARDS)
    //        currentDirection = (int)MOVINGDIRECTION.UPWARDS;
    //      break;
    //    case (int)MOVINGDIRECTION.TORIGHT:
    //      if (previousDirection != (int)MOVINGDIRECTION.TORIGHT)
    //        currentDirection = (int)MOVINGDIRECTION.TOLEFT;
    //      break;
    //    case (int)MOVINGDIRECTION.TOLEFT:
    //      if (previousDirection != (int)MOVINGDIRECTION.TOLEFT)
    //        currentDirection = (int)MOVINGDIRECTION.TORIGHT;
    //      break;
    //  }
    //  previousDirection = currentDirection;
    //}

    //private int GetDirection()
    //{
    //  int direction = 1;
    //  if (CheckRightBoundary())
    //  {
    //    this.currentPosition.Y += 1;
    //  }
    //  if (CheckLeftBoundary())
    //  {
    //    this.currentPosition.Y += 1;
    //  }

    //  if (currentPosition.X - food.X > 0 && food.X - currentPosition.X > 0 && previousDirection != (int)MOVINGDIRECTION.TOLEFT)
    //    return (int)MOVINGDIRECTION.TORIGHT;
    //  if (currentPosition.X - food.X < 0 && food.X - currentPosition.X < 0  && previousDirection != (int)MOVINGDIRECTION.TORIGHT)
    //    return (int)MOVINGDIRECTION.TOLEFT;
    //  if (food.X - currentPosition.X > 0 && previousDirection != (int)MOVINGDIRECTION.TORIGHT)
    //    return (int)MOVINGDIRECTION.TOLEFT;
    //  if (food.X - currentPosition.X < 0 && food.X - currentPosition.X < 0 && previousDirection != (int)MOVINGDIRECTION.TOLEFT)
    //    return (int)MOVINGDIRECTION.TORIGHT;
     
    //  if (currentPosition.Y - food.Y > 0 && food.Y - currentPosition.Y < 0 && previousDirection != (int)MOVINGDIRECTION.UPWARDS)
    //    return (int)MOVINGDIRECTION.DOWNWARDS;
    //  if (currentPosition.Y - food.Y < 0 && previousDirection != (int)MOVINGDIRECTION.DOWNWARDS)
    //    return (int)MOVINGDIRECTION.UPWARDS;
    //  if (food.Y - currentPosition.Y < 0 && previousDirection != (int)MOVINGDIRECTION.DOWNWARDS)
    //    return (int)MOVINGDIRECTION.UPWARDS;
    //  if (food.Y - currentPosition.Y > 0 && currentPosition.Y - food.Y < 0 && previousDirection != (int)MOVINGDIRECTION.UPWARDS)
    //    return (int)MOVINGDIRECTION.DOWNWARDS;
     
    //  return direction;
    //}

    public void createFood()
    {
      //DrawingVisual drawingVisual = new DrawingVisual();
      //DrawingContext drawingContext = drawingVisual.RenderOpen();
      Ellipse newEllipse = new Ellipse();
      newEllipse.Fill = Brushes.Pink;
      newEllipse.Width = SnakeObj.HeadSize;
      newEllipse.Height = SnakeObj.HeadSize;
      Canvas.SetTop(newEllipse, food.Y);
      Canvas.SetLeft(newEllipse, food.X);
      //drawingContext.DrawEllipse(Brushes.Pink, new Pen(Brushes.Pink, 10), new Point(20, 50), 500.00, 500.00);
      canvas.Children.Insert(0, newEllipse);
      //drawingContext.Close();
      //return drawingVisual;
    }

    private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      canvasWidth = this.canvas.ActualWidth - 10;
      canvasHeight = this.canvas.ActualHeight - 10;
    }

    //void MainWindow_KeyDown(object sender, KeyEventArgs e)
    //{
    //}

    void timer_Tick(object sender, EventArgs e)
    {
      //this.FindPath();
      switch (currentDirection)
      {
        case (int)MOVINGDIRECTION.DOWNWARDS:
          currentPosition.Y += 1;
          this.snake.paintSnake(currentPosition);
          break;
        case (int)MOVINGDIRECTION.UPWARDS:
          currentPosition.Y -= 1;
          this.snake.paintSnake(currentPosition);
          break;
        case (int)MOVINGDIRECTION.TOLEFT:
          currentPosition.X -= 1;
          this.snake.paintSnake(currentPosition);
          break;
        case (int)MOVINGDIRECTION.TORIGHT:
          currentPosition.X += 1;
          this.snake.paintSnake(currentPosition);
          break;
      }

      CheckBodyHit();
      CheckCollision();
      CheckBoundaryHit();
    }

    private void CheckBoundaryHit()
    {
      if (currentPosition.X < 2 || 
          currentPosition.X > this.canvas.ActualWidth - 10 ||
          currentPosition.Y < 2 ||
          currentPosition.Y > this.canvas.ActualHeight - 10)
      {
        GameOver();
      }
    }

    private void CheckBodyHit()
    {
      int snakeCount = this.snake.SnakePoints.Count;
      for (int i = 0; i < (snakeCount - SnakeObj.HeadSize * 4); i++)
      {
        Point point = new Point(this.snake.SnakePoints[i].X, this.snake.SnakePoints[i].Y);
        if ((Math.Abs(point.X - currentPosition.X) < (SnakeObj.HeadSize)) &&
             (Math.Abs(point.Y - currentPosition.Y) < (SnakeObj.HeadSize)))
        {
          GameOver();
          break;
        }
      }
    }

    private void CheckCollision()
    {
      if ((Math.Abs(food.X - currentPosition.X) < SnakeObj.HeadSize) &&
          (Math.Abs(food.Y - currentPosition.Y) < SnakeObj.HeadSize))
      {
        this.snake.SnakeLength += 10;
        food.X = rnd.Next(0, (int)canvasWidth);
        food.Y = rnd.Next(0, (int)canvasHeight);
        this.canvas.Children.RemoveAt(0);
        this.createFood();
      }
    }

    //private bool CheckBoundaries()
    //{
    //  return (currentPosition.X <= 0) || (currentPosition.X >= canvasWidth) || (currentPosition.Y <= 0) || (currentPosition.Y >= canvasHeight);
    //}

    private void GameOver()
    {
      MessageBoxResult result = MessageBox.Show("Would you like to try again?", "Snake", MessageBoxButton.YesNo);
      switch (result)
      {
        case MessageBoxResult.Yes:
          Process.Start(Application.ResourceAssembly.Location);
          Application.Current.Shutdown();
          break;
        case MessageBoxResult.No:
          Application.Current.Shutdown();
          break;
      }
    }

    enum MOVINGDIRECTION
    {
      UPWARDS = 1,
      DOWNWARDS = 2,
      TORIGHT = 3,
      TOLEFT = 4
    }

    private void canvas_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Down:
          if (previousDirection != (int)MOVINGDIRECTION.UPWARDS)
            currentDirection = (int)MOVINGDIRECTION.DOWNWARDS;
          break;
        case Key.Up:
          if (previousDirection != (int)MOVINGDIRECTION.DOWNWARDS)
            currentDirection = (int)MOVINGDIRECTION.UPWARDS;
          break;
        case Key.Left:
          if (previousDirection != (int)MOVINGDIRECTION.TORIGHT)
            currentDirection = (int)MOVINGDIRECTION.TOLEFT;
          break;
        case Key.Right:
          if (previousDirection != (int)MOVINGDIRECTION.TOLEFT)
            currentDirection = (int)MOVINGDIRECTION.TORIGHT;
          break;
      }
      previousDirection = currentDirection;
    }
  }
}
