using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CageGame
{
    public sealed class GameDebug
    {
        private static GameDebug instance;

        const string LogFile = "Debug.txt";
        private StreamWriter _streamWriter;

        public Canvas DebugMap { get; set; }

        public static GameDebug GetInstance() 
        {
            if (instance == null)
                instance = new GameDebug();
            return instance;
        }

        private GameDebug()
        {
            FileStream fileStream = new FileStream(LogFile, FileMode.Create, FileAccess.Write);
            _streamWriter = new StreamWriter(fileStream);
        }

        ~GameDebug() => _streamWriter.Close();

        public void Log(string log) => _streamWriter.WriteLine(log);

        public void DrawLine(Vector2 point1, Vector2 point2)
        {
            Line DrawLine = new Line();
            DrawLine.StrokeThickness = 6;
            DrawLine.Stroke = Brushes.Green;
            DrawLine.X1 = point1.X;
            DrawLine.X2 = point2.X;
            DrawLine.Y1 = point1.Y;
            DrawLine.Y2 = point2.Y;

            DebugMap.Children.Add(DrawLine);
        }
    }
}
