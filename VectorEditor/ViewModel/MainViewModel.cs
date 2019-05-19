using System;
using System.Windows;
using System.Windows.Shapes;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using VectorEditor.Helpers;
using System.Windows.Media;
using VectorEditor.Model;
using Microsoft.Win32;
using System.Windows.Markup;
using System.IO;
using System.Collections.Generic;

namespace VectorEditor.ViewModel
{
    class MainViewModel: BindableBase
    {
        public MainViewModel()
        {
            canvas = App.Current.Resources["Canvas"] as Canvas;
            workspace = new ToolHandler(canvas);            
            lineChecked = true;
            workspace.CurrentTool = ToolsEnum.Polyline;
            Color = Colors.Gray;
            thickness = "4";
        }
        ToolHandler workspace;

        string thickness;
        public string Thickness
        {
            get => thickness;
            set
            {
                thickness = value;
                workspace.selectedPrimitive.StrokeThickness = double.Parse(value);
                OnPropertyChanged();
            }
        }

        Color? color;
        public Color? Color
        {
            get => color;
            set
            {
                workspace.SelectedColor = new SolidColorBrush(value.Value);
                color = value;
                OnPropertyChanged();
            }
        }

        Canvas canvas;
        public Canvas Canvas
        {
            get => canvas;
            set
            {
                canvas = value;
                OnPropertyChanged();
            }
        }

        bool? lineChecked;
        public bool? LineChecked
        {
            get => lineChecked;
            set
            {
                lineChecked = value;
                OnPropertyChanged();
                if (value == true)
                    workspace.CurrentTool = ToolsEnum.Polyline;
            }
        }

        bool? selectionChecked;
        public bool? SelectionChecked
        {
            get => selectionChecked;
            set
            {                
                selectionChecked = value;                
                OnPropertyChanged();
                if (value == true)
                    workspace.CurrentTool = ToolsEnum.Selection;
            }
        }

        private bool? deleteChecked;
        public bool? DeleteChecked
        {
            get => deleteChecked;
            set
            {
                deleteChecked = value;
                OnPropertyChanged();
                if (value == true)
                    workspace.CurrentTool = ToolsEnum.Delete;
            }
        }

        ICommand save;
        public ICommand Save
        {
            get
            {
                if (save == null)
                    save = new Command(ExecuteSave);
                return save;
            }            
        }

        ICommand load;
        public ICommand Load
        {
            get
            {
                if (load == null)
                    load = new Command(ExecuteLoad);
                return load;
            }
        }

        void ExecuteLoad()
        {
            OpenFileDialog load = new OpenFileDialog();
            load.Filter = "Файл XML|*.xml";

            if (load.ShowDialog().Value)
            {                
                FileStream fs = new FileStream(load.FileName, FileMode.Open);
                var canvas = (Canvas)XamlReader.Load(fs);
                fs.Close();
                var polylines = canvas.Children.OfType<Shape>().ToArray();
                Canvas.Children.Clear();
                foreach (var s in polylines)
                {
                    if (s is Polyline)
                    {
                        s.MouseLeftButtonDown += workspace.PolylineLeftButtonDown;
                        var nodes = new List<Shape>();
                        foreach (var point in ((Polyline)s).Points)
                        {
                            var node = workspace.CreateNode(point);
                            nodes.Add(node);
                            PolylineConfigurator.SetPolyline(node, s as Polyline);
                            Canvas.Children.Add(node);
                        }
                        PolylineConfigurator.SetNodes(s as Polyline, nodes);
                        canvas.Children.Remove(s);
                        Canvas.Children.Add(s);
                        workspace.selectedPrimitive = s as Polyline;
                    }                    
                }
            }
        }

        void ExecuteSave()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.CheckPathExists = true;
            save.ValidateNames = true;
            save.AddExtension = true;
            save.OverwritePrompt = true;
            save.DefaultExt = ".xml";
            save.Filter = "Файл XML|*.xml";
            save.FilterIndex = 0;

            if (save.ShowDialog().Value)
            {
                var s = XamlWriter.Save(canvas );
                FileStream fs = File.Create(save.FileName);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(s);
                sw.Close();
                fs.Close();
            }
        }
    }
}
