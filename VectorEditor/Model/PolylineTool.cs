using System;
using System.Windows.Input;
using System.Windows.Shapes;
using VectorEditor.Helpers;

namespace VectorEditor.Model
{
    class PolylineTool : VectorShapeTool
    {
        public PolylineTool(ToolHandler workspace): base(ToolsEnum.Polyline)
        {
            this.workspace = workspace;
        }        
        public void AddPoint(object sender)
        {
            var position = Mouse.GetPosition(workspace.Canvas);
                        
            if (workspace.selectedPrimitive == null)
            {
                Polyline line = new Polyline();
                line.MouseLeftButtonDown += workspace.PolylineLeftButtonDown;
                workspace.Canvas.Children.Add(line);
                line.Stroke = workspace.SelectedColor;
                line.StrokeThickness = 4;
                workspace.selectedPrimitive = line;
            }
            var controlPoint = workspace.CreateNode(position);
            workspace.Canvas.Children.Add(controlPoint);
            PolylineConfigurator.SetPolyline(controlPoint, workspace.selectedPrimitive);
            workspace.selectedPrimitive.Points.Add(position);
            PolylineConfigurator.GetNodes(workspace.selectedPrimitive).Add(controlPoint);
        }
    }
}
