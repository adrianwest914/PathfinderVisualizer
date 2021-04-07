using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace PathfinderVisualizer.WPF.UIElements
{
    public class UIControl
    {
        public Dictionary<string, UIElement> AStarControls;

        public UIControl(Panel parent, Canvas drawingCanvas)
        {
            AStarControls = new Dictionary<string, UIElement>();

            AStarControls.Add(drawingCanvas.Name, drawingCanvas);
            List<Control> allControls = GetAllControls(parent);
            List<Control> controls = RemoveNoNameControls(allControls);

            foreach (Control ctrl in controls)
                AStarControls.Add(ctrl.Name, ctrl);
        }

        private List<Control> GetAllControls(Panel parent)
        {
            var result = new List<Control>();

            //get all controls from children that are also panels
            var childPanels = parent.Children.OfType<Panel>();
            foreach (Panel p in childPanels)
                result.AddRange(GetAllControls(p));

            //add all children that are controls 
            var childControls = parent.Children.OfType<Control>();
            foreach (Control child in childControls)
                result.Add(child);

            return result;
        }
        private List<Control> RemoveNoNameControls(List<Control> controls)
        {
            var result = new List<Control>();

            foreach (Control ctrl in controls)
                if (ctrl.Name.Length > 0) //if a control isnt given a name the default name will be ""
                    result.Add(ctrl);

            return result;
        }
    }
}
