using System;
using System.Threading;
using System.Windows.Threading;
using PathfinderVisualizer.WPF.Controllers;
using PathfinderVisualizer.WPF.AStarAlgorithm.AStarImplementation.AlgorithmThread;

namespace PathfinderVisualizer.WPF.AStarAlgorithm.AStarImplementation
{
    public class AlgorithmThreadController : IController
    {
        private static Dispatcher MainDispatcher;
        private Thread AlgorithmThread;
        private AlgorithmController algorithmController;

        public AlgorithmThreadController(Dispatcher mainThreadDispatcher)
        {
            MainDispatcher = mainThreadDispatcher;
        }
        public void StartControlling()
        {
            StateObserver.StartAlgorithm += StartThread;
            StateObserver.ResetAlgorithm += ResetThread;
            AStarValues.GridDimensionChanged += ResetThread;
        }
        public void StopControlling()
        {
            StateObserver.StartAlgorithm -= StartThread;
            StateObserver.ResetAlgorithm -= ResetThread;
            AStarValues.GridDimensionChanged -= ResetThread;
        }
        
        private void StartThread(object sender, EventArgs args)
        {
            if (AlgorithmThread != null && AlgorithmThread.IsAlive)
                return;

            AlgorithmThread = new Thread(() => 
            {
                algorithmController = new AlgorithmController(MainDispatcher);
            });
            AlgorithmThread.Start();
        }
        private void ResetThread(object sender, EventArgs args)
        {
            CallEvent(StopThread, EventArgs.Empty);
            AlgorithmThread?.Abort();
            algorithmController?.Reset();

            AStarValues.AStarState = State.HasNotStarted;
        }
        private void CallEvent(Delegate handler, EventArgs args)
        {
            if (handler != null)
                handler.DynamicInvoke(null, args);
        }
        public static event EventHandler StopThread;
    }
}