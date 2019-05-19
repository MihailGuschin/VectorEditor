using System;
using System.Windows.Shapes;
using VectorEditor.Helpers;

namespace VectorEditor.Model
{
    class DeleteTool : VectorShapeTool
    {
        public DeleteTool(ToolHandler workspace):base(ToolsEnum.Delete)        
        {
            this.workspace = workspace;
        }        
        public void DeleteNode(Shape sender)
        {
            var polyline = PolylineConfigurator.GetPolyline(sender);
            var nodes = PolylineConfigurator.GetNodes(polyline);
            var index = nodes.FindIndex(p => p == sender);
            nodes.RemoveAt(index);
            polyline.Points.RemoveAt(index);
            workspace.Canvas.Children.Remove(sender);

            if (nodes.Count == 0)
            {
                workspace.Canvas.Children.Remove(polyline);
                workspace.selectedPrimitive = null;
            }
        }
    }
}
