using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UpdateControls.XAML;

namespace CAP
{
	public partial class MainPage : UserControl
	{
        private static TimeSpan DoubleClickTime = TimeSpan.FromMilliseconds(500.0);

        private bool _panning;
        private bool _moved;
        private Point _panPosition;
        private DateTime? _firstClick = null;

        public MainPage()
		{
			// Required to initialize variables
			InitializeComponent();

            DataContext = ForView.Wrap(new MainViewModel(new PageNavigationModel()));
        
            VisualStateManager.GoToState(this, ViewModel.CurrentPageTag, false);
        }

        private MainViewModel ViewModel
        {
            get { return ForView.Unwrap<MainViewModel>(DataContext); }
        }

        private void Map_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ViewModel.ZoomBy(e.Delta, e.GetPosition(map));
        }

        private void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(map);
            _panning = true;
            _moved = false;
            _panPosition = position;
            //Mouse.Capture(map);
            e.Handled = true;
        }

        private void Map_MouseMove(object sender, MouseEventArgs e)
        {
            if (_panning)
            {
                Point newPosition = e.GetPosition(map);
                if (newPosition != _panPosition)
                {
                    ViewModel.PanBy(new Point(newPosition.X - _panPosition.X, newPosition.Y - _panPosition.Y));
                    _panPosition = newPosition;
                    _moved = true;
                }
                //e.Handled = true;
            }
        }

        private void Map_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_panning)
            {
                //Mouse.Capture(null);
                _panning = false;
                e.Handled = true;
            }
            if (!_moved)
            {
                DateTime now = DateTime.Now;
                if (_firstClick != null && (now - _firstClick) < DoubleClickTime)
                    ViewModel.ResetZoom();
                else
                    _firstClick = now;
            }
        }

        private void LayoutRoot_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.PageDown)
            {
                ViewModel.MovePage(1);
                VisualStateManager.GoToState(this, ViewModel.CurrentPageTag, true);
            }
            if (e.Key == Key.PageUp)
            {
                ViewModel.MovePage(-1);
                VisualStateManager.GoToState(this, ViewModel.CurrentPageTag, true);
            }
            if (e.Key == Key.Up)
            {
                ViewModel.GetCurrentPage().ChangeState(-1);
            }
            if (e.Key == Key.Down)
            {
                ViewModel.GetCurrentPage().ChangeState(1);
            }
        }
    }
}