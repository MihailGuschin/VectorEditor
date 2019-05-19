using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using VectorEditor.Helpers;

namespace VectorEditor.Model
{
    class SelectionTool : VectorShapeTool
    {
        public SelectionTool(ToolHandler workspace): base(ToolsEnum.Selection)
        {
            this.workspace = workspace;            
        }
        Shape selectedPoint;
        string selectedShape;
        bool PrimitiveMove;
        Point? startPos;
        Point[] lastLoc;
        private double CanvasLeft, CanvasTop;
        
        public void SelectPrimitive(object sender, MouseButtonEventArgs e)
        {
            selectedShape = "Polyline";
            workspace.selectedPrimitive = sender as Polyline;
            if (e.ClickCount > 1) // если произведено двойное нажатие, создается новый узел
            {
                var pos = e.GetPosition(workspace.Canvas);
                for (int i = 0; i < workspace.selectedPrimitive.Points.Count - 1; i++)
                {
                    var X1 = workspace.selectedPrimitive.Points[i].X;
                    var Y1 = workspace.selectedPrimitive.Points[i].Y;
                    var X2 = workspace.selectedPrimitive.Points[i + 1].X;
                    var Y2 = workspace.selectedPrimitive.Points[i + 1].Y;
                    if (((pos.X >= X1 && pos.X <= X2) || (pos.X > X2 && pos.X < X1)) && ((pos.Y >= Y1 && pos.Y <= Y2) || (pos.Y > Y2 && pos.Y < Y1)))
                    {
                        workspace.selectedPrimitive.Points.Insert(i + 1, pos);
                        var newNode = workspace.CreateNode(pos);
                        PolylineConfigurator.SetPolyline(newNode, workspace.selectedPrimitive);
                        PolylineConfigurator.GetNodes(workspace.selectedPrimitive).Insert(i + 1, newNode);
                        workspace.Canvas.Children.Add(newNode);
                        break;
                    }
                }
            }
            else // если кнопка в даннй момент продолжает быть нажатой
            {//подготавливаем объект к перетаскиванию
             // workspace.Canvas.MouseLeftButtonUp += OnMouseUpForSelection;
                PrimitiveMove = true;
                startPos = e.GetPosition(workspace.Canvas);
                var nodes = PolylineConfigurator.GetNodes(sender as Polyline);
                int i = 0;
                lastLoc = new Point[nodes.Count];
                foreach (var point in nodes)
                {
                    lastLoc[i++] = new Point(Canvas.GetLeft(point), Canvas.GetTop(point));
                }
            }
        }

        public void SelectedPrimitiveMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && workspace.selectedPrimitive != null && startPos.HasValue )
            {
                if (selectedShape == "Polyline")
                {
                    PolylineDrag(e);
                }
                else
                {
                    PolylineNodeDrag(e);
                }
            }
        }

        public void SelectedPrimitiveMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && PrimitiveMove==true)
            {
                PrimitiveMove = false;
                lastLoc = null;
                startPos = null;
            }
        }

        public void SelectNode(Shape point, MouseEventArgs e)
        {
            PrimitiveMove = true;
            selectedShape = "Node";
            selectedPoint = point;
            startPos = e.GetPosition(workspace.Canvas);
        }

        void PolylineNodeDrag(MouseEventArgs e)
        {
            Point pos = e.GetPosition(workspace.Canvas);
            Canvas.SetLeft(selectedPoint, pos.X);
            Canvas.SetTop(selectedPoint, pos.Y);

            var polyline = PolylineConfigurator.GetPolyline(selectedPoint as DependencyObject);
            var nodes = PolylineConfigurator.GetNodes(polyline);
            var index = nodes.FindIndex(p => p == selectedPoint);
            if (index >= 0)
            {
                pos.X += 4;
                pos.Y += 4;
                polyline.Points[index] = pos;
            }
            
        }

        void PolylineDrag(MouseEventArgs e)
        {
            Point currentPos = e.GetPosition(workspace.Canvas);
            var xOffset = currentPos.X - startPos.Value.X;
            var yOffset = currentPos.Y - startPos.Value.Y;

            int i = 0;
            var nodes = PolylineConfigurator.GetNodes(workspace.selectedPrimitive);
            foreach (var point in nodes)
            {
                var newX = (startPos.Value.X + xOffset);
                var newY = (startPos.Value.Y + yOffset);
                Point offset = new Point((startPos.Value.X - lastLoc[i].X), (startPos.Value.Y - lastLoc[i].Y));
                CanvasTop = newY - offset.Y;
                CanvasLeft = newX - offset.X;

                point.SetValue(Canvas.TopProperty, CanvasTop);
                point.SetValue(Canvas.LeftProperty, CanvasLeft);

                workspace.selectedPrimitive.Points[i] = new Point(CanvasLeft, CanvasTop);

                i++;
            }
        }
    }
}
