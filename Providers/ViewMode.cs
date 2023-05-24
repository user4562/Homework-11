using System.Windows;

namespace Homework_11.Providers
{
    internal class ViewMode
    {
        public Visibility DepartamentDoneButton { get; set; }
        public Visibility DepartamentControlButton { get; set; }
        public Visibility DepartamentTreeView { get; set; }
        public Visibility DepartamentCreateView { get; set; }

        public Visibility WorkerDoneButton { get; set; }
        public Visibility WorkerControlButton { get; set; }
        public Visibility WorkerListView { get; set; }
        public Visibility WorkerCreateView { get; set; }

        public ViewMode()
        {
            DepartamentDoneButton = Visibility.Collapsed;
            DepartamentControlButton = Visibility.Visible;
            DepartamentTreeView = Visibility.Visible;
            DepartamentCreateView = Visibility.Collapsed;

            WorkerDoneButton = Visibility.Collapsed;
            WorkerControlButton = Visibility.Visible;
            WorkerListView = Visibility.Visible;
            WorkerCreateView = Visibility.Collapsed;
        }

        public ViewMode(ViewModeOptions options) : this()
        {
            if(options == ViewModeOptions.CreateWorker)
            {
                WorkerDoneButton = Visibility.Visible;
                WorkerControlButton = Visibility.Collapsed;
                WorkerListView = Visibility.Collapsed;
                WorkerCreateView = Visibility.Visible;
            }

            if(options == ViewModeOptions.CreateDepartament)
            {
                WorkerDoneButton = Visibility.Collapsed;
                WorkerControlButton = Visibility.Collapsed;
                WorkerListView = Visibility.Collapsed;
                WorkerCreateView = Visibility.Visible;

                DepartamentDoneButton = Visibility.Visible;
                DepartamentControlButton = Visibility.Collapsed;
                DepartamentTreeView = Visibility.Collapsed;
                DepartamentCreateView = Visibility.Visible;
            }
        }


    }

    internal enum ViewModeOptions
    {
        Default,
        CreateWorker,
        CreateDepartament,
    }
}
