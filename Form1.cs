namespace SnakeGame;

public partial class Form1 : Form
{
    private List<Point> Snake = new List<Point>();
    private Point Food = Point.Empty;
    private int Score = 0;
    private int CellSize = 20;
    private Direction CurrentDirection = Direction.Right;
    private System.Windows.Forms.Timer GameTimer = new System.Windows.Forms.Timer();

    public Form1()
    {
        InitializeComponent();
        this.Text = "Snake Game";
        this.Width = 800;
        this.Height = 600;
        this.DoubleBuffered = true;
        GameTimer.Interval = 100;
        GameTimer.Tick += GameTimer_Tick;
        this.KeyDown += MainForm_KeyDown;

        StartGame();
    }

    public void StartGame()
    {
        Snake.Clear();
        Snake.Add(new Point(5, 5)); 
        Score = 0;
        CurrentDirection = Direction.Right;
        Food = GenerateFood();
        GameTimer.Start();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;

        foreach (Point body in Snake)
        {
            g.FillRectangle(Brushes.Green, body.X * CellSize, body.Y * CellSize, CellSize, CellSize);
        }
        g.FillRectangle(Brushes.Red, Food.X * CellSize, Food.Y * CellSize, CellSize, CellSize);
        g.DrawString($"Score: {Score}", new Font("Arial", 16), Brushes.Black, new PointF(10, 10));

    }

    private void GameTimer_Tick(object sender, EventArgs e)
    {
        MoveSnake();
        CheckCollision();
        this.Invalidate(); // Atualiza a marcação da cobra
    }

    private void MoveSnake()
    {
        Point head = Snake[0];
        Point newHead = head;

        switch (CurrentDirection)
        {
            case Direction.Up: newHead.Y -= 1; break;
            case Direction.Down:  newHead.Y += 1; break;
            case Direction.Left:  newHead.X -= 1; break;
            case Direction.Right:  newHead.X += 1; break;
        }

        Snake.Insert(0, newHead);
        if(newHead != Food) Snake.RemoveAt(Snake.Count - 1);
        else
        {
            Score++;
            Food = GenerateFood();
        }
        
    }

    public void MainForm_KeyDown(Object sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Up: if (CurrentDirection != Direction.Down) CurrentDirection = Direction.Up; break;
            case Keys.Down: if (CurrentDirection != Direction.Up) CurrentDirection = Direction.Down; break;
            case Keys.Left: if (CurrentDirection != Direction.Right) CurrentDirection = Direction.Left; break;
            case Keys.Right: if (CurrentDirection != Direction.Left) CurrentDirection = Direction.Right; break;
        }
    }

    public Point GenerateFood()
    {
        Random rnd = new Random();
        Point food;
        do
        {
            food = new Point(rnd.Next(this.ClientSize.Width / CellSize), rnd.Next(this.ClientSize.Height / CellSize));
        }
        while(Snake.Contains(food));
        return food;
    }

    public void CheckCollision()
    {
        Point head = Snake[0];
        
        for (int i = 1; i < Snake.Count; i++)
            {
                if (Snake[i] == head)
                {
                    EndGame();
                }
            }
    }

    public void EndGame()
    {
        GameTimer.Stop();
        MessageBox.Show($"Game Over! Score: {Score}");
    }

}
