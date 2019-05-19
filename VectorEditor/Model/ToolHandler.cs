using System;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Input;
using VectorEditor.Helpers;
using System.Windows.Media;
using VectorEditor.Model;

namespace VectorEditor.Model
{
    class ToolHandler
    {        
        public ToolHandler(Canvas canvas)
        {
            Canvas = canvas;
            canvas.MouseLeftButtonDown += CanvasLeftButtonDown;
            canvas.MouseLeftButtonUp += CanvasLeftButtonUp;
            canvas.MouseMove += CanvasMouseMove;
            polyline = new PolylineTool(this);
            selection = new SelectionTool(this);
            delete = new DeleteTool(this);
        }
        SolidColorBrush selectedColor;
        public SolidColorBrush SelectedColor
        {
            get => selectedColor;
            set
            {
                selectedColor = value;
                if (selectedPrimitive != null)
                    selectedPrimitive.Stroke = value;
            }
        }
        public Polyline selectedPrimitive;
        PolylineTool polyline;
        SelectionTool selection;
        DeleteTool delete;
        public Canvas Canvas;
        public ToolsEnum CurrentTool;
        
        /// <summary>
        /// Обработка движения мыши
        /// </summary>
        void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            switch (CurrentTool)
            {
                case ToolsEnum.Polyline:
                    break;
                case ToolsEnum.Selection:
                    selection.SelectedPrimitiveMove(sender, e);
                    break;
                case ToolsEnum.Delete: break;
            }
        }
        /// <summary>
        /// Клик на холсте
        /// </summary>
        void CanvasLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch(CurrentTool)
            {
                case ToolsEnum.Polyline:
                    polyline.AddPoint(sender);
                    break;
                case ToolsEnum.Selection: break;
                case ToolsEnum.Delete: break;                    
            }
        }
        /// <summary>
        /// Обрабатывает событие, когда кнопка отпускается
        /// </summary>        
        void CanvasLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (CurrentTool)
            {
                case ToolsEnum.Polyline:
                   // polyline.AddPoint(sender);
                    break;
                case ToolsEnum.Selection:
                    selection.SelectedPrimitiveMouseUp(sender, e);
                    break;
                case ToolsEnum.Delete: break;
            }
        }
        /// <summary>
        /// Клик левой кнопкой на ломаной линии
        /// </summary>
        public void PolylineLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (CurrentTool)
            {
                case ToolsEnum.Polyline: break;
                case ToolsEnum.Selection:
                    selection.SelectPrimitive(sender, e);
                    break;
                case ToolsEnum.Delete: break;
            }
        }
        /// <summary>
        ///  Клик левой кнопкой мыши на узле ломаной линии
        /// </summary>        
        void PolylineNodeLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (CurrentTool)
            {
                case ToolsEnum.Polyline: break;
                case ToolsEnum.Selection:
                    selection.SelectNode(sender as Shape, e);
                    break;
                case ToolsEnum.Delete:
                    delete.DeleteNode(sender as Shape);
                    break;
            }
        }
        /// <summary>
        /// Создать узел ломаной линии
        /// </summary>       
        public Shape CreateNode(Point position)
        {
            var controlPoint = new Rectangle() { Height = 8, Width = 8, Fill = new SolidColorBrush(Colors.Gray) };
            Canvas.SetZIndex(controlPoint, 2);
            Canvas.SetLeft(controlPoint, position.X - 4);
            Canvas.SetTop(controlPoint, position.Y - 4);
            controlPoint.MouseLeftButtonDown += PolylineNodeLeftButtonDown;
            return controlPoint;
        }
    }
}
