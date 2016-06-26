using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Snake
{
  public class SnakeObj : FrameworkElement
  {
    #region Attributes
    Canvas paintCanvas;
    Brush snakeBrush;
    List<Point> snakePoints;
    //private VisualCollection children;
    #endregion

    public SnakeObj(Canvas canvas) 
    {
      this.paintCanvas = canvas;
      this.SnakeLength = 0;
      this.snakeBrush = Brushes.Red;
      this.snakePoints = new List<Point>();
      this.SnakeLength = 10;
      //children = new VisualCollection(this);
      //children.Add(CreateDrawingVisualElipses());
    }

    public int SnakeLength { get; set; }
    public static double HeadSize { get { return 10; } }
    public List<Point> SnakePoints 
    {
      get { return snakePoints; }
      set { snakePoints = value; }
    }

    //// Provide a required override for the VisualChildrenCount property. 
    //protected override int VisualChildrenCount
    //{
    //  get { return children.Count; }
    //}

    //// Provide a required override for the GetVisualChild method. 
    //protected override Visual GetVisualChild(int index)
    //{
    //  if (index < 0 || index >= children.Count)
    //  {
    //    throw new ArgumentOutOfRangeException();
    //  }

    //  return children[index];
    //}

    public void paintSnake(Point currentposition)
    {
      Ellipse newEllipse = new Ellipse();
      newEllipse.Fill = snakeBrush;
      newEllipse.Width = HeadSize;
      newEllipse.Height = HeadSize;
      Canvas.SetLeft(newEllipse, currentposition.X);
      Canvas.SetTop(newEllipse, currentposition.Y);
      int count = paintCanvas.Children.Count;
      paintCanvas.Children.Add(newEllipse);
      snakePoints.Add(currentposition);
      try
      {
        if (count > this.SnakeLength)
        {
          paintCanvas.Children.RemoveAt(count - SnakeLength);
          snakePoints.RemoveAt(count - SnakeLength - 1);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message.ToString());
      }
    }
  }
}
