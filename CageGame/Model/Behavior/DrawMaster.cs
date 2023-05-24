using System;
using System.Collections.Generic;
using System.Linq;

namespace CageGame
{
    public sealed class DrawMaster
    {
        private bool _isMouseDown;

        private Vector2 _drawLinePos1;
        private Vector2 _drawLinePos2;

        private double[] _activeCageBorderY;
        private double[] _activeCageBorderX;

        private List<Border> _drawingLines;
        private List<Border> _borders;

        public Border ActiveLine { get; private set; }

        public DrawMaster(List<Border> drawingBorders, List<Border> borders)
        {
            _activeCageBorderX = new double[2];
            _activeCageBorderY = new double[2];

            _isMouseDown = false;
            ActiveLine = new Border();
            CollisionMaster.GetInstance().DrawingLine = ActiveLine;

            _drawingLines = drawingBorders;
            _borders = borders;

            GameEvents.OnCageFail += () => DrawStop();
        }

        private Vector2 DrawVector(Vector2 SumVector, Vector2 Vector0)
        {
            if (Math.Abs(_drawLinePos1.X - SumVector.X) > Math.Abs(_drawLinePos1.Y - SumVector.Y))
                SumVector = new Vector2(SumVector.X, Vector0.Y);
            else
                SumVector = new Vector2(Vector0.X, SumVector.Y);

            if (_activeCageBorderX[0] != 0 && _activeCageBorderX[1] != 0)
                SumVector = new Vector2(Math.Clamp(SumVector.X, _activeCageBorderX[0], _activeCageBorderX[1]), SumVector.Y);

            if (_activeCageBorderY[0] != 0 && _activeCageBorderY[1] != 0)
                SumVector = new Vector2(SumVector.X, Math.Clamp(SumVector.Y, _activeCageBorderY[0], _activeCageBorderY[1]));

            return SumVector;
        }

        private void DrawStop()
        {
            _drawingLines.Clear();

            ActiveLine = new Border();
            _isMouseDown = false;

            _activeCageBorderX = new double[2];
            _activeCageBorderY = new double[2];
        }

        private void BorderCorrection()
        {
            if (ActiveLine.isVerticalBorder())
            {
                _activeCageBorderY[0] = Math.Min(ActiveLine.Point1.Y, ActiveLine.Point2.Y);
                _activeCageBorderY[1] = Math.Max(ActiveLine.Point1.Y, ActiveLine.Point2.Y);
            }
            else
            {
                _activeCageBorderX[0] = Math.Min(ActiveLine.Point1.X, ActiveLine.Point2.X);
                _activeCageBorderX[1] = Math.Max(ActiveLine.Point1.X, ActiveLine.Point2.X);
            }
        }

        private void CageDrawEnd()
        {
            Border firstLine = _drawingLines.First();
            Border lastLine = _drawingLines.Last();

            if (!(firstLine.Point2.X == lastLine.Point1.X && firstLine.Point2.Y == lastLine.Point1.Y))
            {
                DrawStop();
                return;
            }

            GameEvents.SendCageSucces(
                    new Vector2[] {
                                _drawingLines[0].Point1,
                                _drawingLines[1].Point1,
                                _drawingLines[2].Point1,
                                _drawingLines[3].Point1
                    }
                );

            foreach (Border line in _drawingLines)
            {
                int plus;
                var correctLine = line;

                if (line.isVerticalBorder())
                {
                    plus = (line.Point1.Y > line.Point2.Y) ? (1) : (-1);
                    correctLine = new Border(
                        new Vector2(line.Point1.X, line.Point1.Y + (Border.StrokeThickness / 2) * plus),
                        new Vector2(line.Point2.X, line.Point2.Y + (Border.StrokeThickness / 2) * -plus)
                        );
                }
                else
                {
                    plus = (line.Point1.X > line.Point2.X) ? (1) : (-1);
                    correctLine = new Border(
                        new Vector2(line.Point1.X + ((Border.StrokeThickness / 2) * plus), line.Point1.Y),
                        new Vector2(line.Point2.X + ((Border.StrokeThickness / 2) * -plus), line.Point2.Y)
                        );
                }


                _borders.Add(correctLine);
            }

            DrawStop();
        }

        public void OnDrawLineStart(Vector2 mousePosition)
        {
            _isMouseDown = true;

            if (ActiveLine.Init && _isMouseDown)
            {
                _drawLinePos1 = ActiveLine.Point1;

                _drawingLines.Add(ActiveLine);

                int drawLinesCount = _drawingLines.Count;

                if (drawLinesCount == 1 || drawLinesCount == 2)
                {
                    BorderCorrection();
                }

                if (drawLinesCount == 4)
                {
                    CageDrawEnd();
                }
            }
            else
            {
                _drawLinePos1 = mousePosition;
            }
        }

        public void OnDrawLineStop()
        {
            _isMouseDown = false;
            ActiveLine = new Border();

            _activeCageBorderX = new double[2];
            _activeCageBorderY = new double[2];

            _drawingLines.Clear();
        }

        public void OnDrawLine(Vector2 mousePosition)
        {
            Vector2 mousePos = mousePosition;
            _drawLinePos2 = DrawVector(mousePos, _drawLinePos1);

            if (ActiveLine.Init)
            {
                Vector2 linePoint1 = ActiveLine.Point1;
                Vector2 linePoint2 = ActiveLine.Point2;

                if (CollisionMaster.GetInstance().IntersectionLine(linePoint1, linePoint2))
                {
                    DrawStop();
                }
            }

            CollisionMaster.GetInstance().DrawingLine = ActiveLine;
        }

        public void Draw()
        {
            if (!_isMouseDown)
                return;

            ActiveLine = new Border(_drawLinePos2, _drawLinePos1);
        }
    }
}
