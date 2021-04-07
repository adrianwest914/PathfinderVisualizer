using System;
using System.Windows.Controls;
using PathfinderVisualizer.WPF.UIElements;
using PathfinderVisualizer.WPF.AStarAlgorithm;

namespace PathfinderVisualizer.WPF.Controllers
{
    public class DelayController : IController
    {
        private Label LblDelay;
        private Slider NumDelay;

        public DelayController(UIElements.UIControl uiElements)
        {
            LblDelay = (Label)uiElements.AStarControls[ControlNames.DelaySliderDisplay];
            NumDelay = (Slider)uiElements.AStarControls[ControlNames.DelaySlider];
        }

        public void StartControlling()
        {
            NumDelay.ValueChanged += ChangeDelayValue;
            StateObserver.AlgorithmIsDone += DisableControl;
            StateObserver.ResetAlgorithm += EnableControl;
        }
        public void StopControlling()
        {
            NumDelay.ValueChanged -= ChangeDelayValue;
        }

        private void ChangeDelayValue(object sender, EventArgs args)
        {
            uint value = (uint)NumDelay.Value;

            LblDelay.Content = $"Delay: {value} ms";
            AStarValues.Delay = value;
        }
        private void EnableControl(object sender, EventArgs args)
        {
            NumDelay.IsEnabled = true;
        }
        private void DisableControl(object sender, EventArgs args)
        {
            NumDelay.IsEnabled = false;
        }
    }
}
