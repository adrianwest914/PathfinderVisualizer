using System;
using System.Windows;
using System.Windows.Controls;
using PathfinderVisualizer.WPF.UIElements;
using PathfinderVisualizer.WPF.AStarAlgorithm;
using PathfinderVisualizer.WPF.Observers.Helpers;

namespace PathfinderVisualizer.WPF.Observers
{
    public class DiagonalPathObserver : IObserver
    {
        CheckBox checkBox;

        public DiagonalPathObserver(UIElements.UIControl uiElements)
        {
            checkBox = (CheckBox)uiElements.AStarControls[ControlNames.DiagonalPathCheckbox];
        }

        public void StartObserving()
        {
            checkBox.Click += DiagonalPathCheckbox_Clicked;
            AStarValues.AlgorithmStateChanged += AlgorithmStateChanged;
        }
        public void StopObserving()
        {
            checkBox.Click -= DiagonalPathCheckbox_Clicked;
            AStarValues.AlgorithmStateChanged -= AlgorithmStateChanged;
        }

        private void DiagonalPathCheckbox_Clicked(object sender, EventArgs e)
        {
            AStarValues.DiagonalPathsEnabled = (bool)checkBox.IsChecked;
        }
        private void AlgorithmStateChanged(object sender, EventArgs e)
        {
            if (AStarValues.AStarState == State.HasNotStarted)
                checkBox.IsEnabled = true;
            else
                checkBox.IsEnabled = false;
        }
    }
}
