using System;
using System.Windows;
using UpdateControls;

namespace CAP
{
    public class PageNavigationModel
    {
        private double _zoomExponent = 0.0;
        private Point _position;

        private Independent _indZoomExponent = new Independent();
        private Independent _indPosition = new Independent();

        public double Scale
        {
            get { _indZoomExponent.OnGet(); return Math.Pow(2.0, (double)_zoomExponent); }
        }

        public Point Position
        {
            get { _indPosition.OnGet(); return _position; }
        }

        public void ZoomBy(double delta)
        {
            _indZoomExponent.OnSet();
            _zoomExponent += delta;
        }

        public void PanBy(Point delta)
        {
            _indPosition.OnSet();
            _position = new Point(_position.X + delta.X, _position.Y + delta.Y);
        }

        public void Reset()
        {
            _indPosition.OnSet();
            _position.X = 0.0;
            _position.Y = 0.0;

            _indZoomExponent.OnSet();
            _zoomExponent = 0.0;
        }
    }
}
