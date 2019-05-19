using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorEditor.Helpers;

namespace VectorEditor.Model
{
    abstract class VectorShapeTool
    {
        public VectorShapeTool(ToolsEnum toolType)
        {
            this.toolType = toolType;
        }
        protected ToolHandler workspace;
        protected readonly ToolsEnum toolType;
        public ToolsEnum ToolType => toolType;
    }
}
