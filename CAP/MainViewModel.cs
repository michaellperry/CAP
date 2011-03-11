using System;
using System.Windows;
using CAP.Pages;
using UpdateControls;
using System.Windows.Threading;

namespace CAP
{
    public class MainViewModel
    {
        private PresentationNavigationModel _presentationNavigation;

        private PageTitle _title;
        private PageEricBrewer _ericBrewer;
        private PageUdiDahan _udiDahan;
        private PageGregYoung _gregYoung;
        private PageDefinitions _definitions;
        private PageProof _proof;
        private PageChoices _choices;
        private PageCentralDatabase _centralDatabase;
        private PageCQRS _cqrs;
        private PageEventSourcing _eventSourcing;
        private PageConclusion _conclusion;

        private IPage[] _pages;

        private InertialProperty _inertialScale;
        private InertialProperty _positionX;
        private InertialProperty _positionY;

        private DispatcherTimer _timer;

        public MainViewModel(PageNavigationModel pageNavigation)
        {
            _presentationNavigation = new PresentationNavigationModel();

            _title = new PageTitle(_presentationNavigation);
            _ericBrewer = new PageEricBrewer(_presentationNavigation);
            _udiDahan = new PageUdiDahan(_presentationNavigation);
            _gregYoung = new PageGregYoung(_presentationNavigation);
            _definitions = new PageDefinitions(_presentationNavigation);
            _proof = new PageProof(_presentationNavigation);
            _choices = new PageChoices(_presentationNavigation);
            _centralDatabase = new Pages.PageCentralDatabase(_presentationNavigation);
            _cqrs = new Pages.PageCQRS(_presentationNavigation);
            _eventSourcing = new Pages.PageEventSourcing(_presentationNavigation);
            _conclusion = new PageConclusion(_presentationNavigation);

            _pages = new IPage[]
            {
                _title,
                _ericBrewer,
                _udiDahan,
                _gregYoung,
                _definitions,
                _proof,
                _choices,
                _centralDatabase,
                _cqrs,
                _eventSourcing,
                _conclusion
            };

            _inertialScale = new InertialProperty(() => (float)PageNavigation.Scale);
            _positionX = new InertialProperty(() => (float)PageNavigation.Position.X);
            _positionY = new InertialProperty(() => (float)PageNavigation.Position.Y);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.05);
            _timer.Tick += delegate(object sender, EventArgs e)
            {
                _inertialScale.OnTimer();
                _positionX.OnTimer();
                _positionY.OnTimer();
            };
            _timer.Start();
        }

        public string CurrentTitle
        {
            get { return _pages[_presentationNavigation.CurrentPageIndex].Title; }
        }

        public PageTitle Title
        {
            get { return _title; }
        }

        public PageEricBrewer EricBrewer
        {
            get { return _ericBrewer; }
        }

        public PageUdiDahan UdiDahan
        {
            get { return _udiDahan; }
        }

        public PageGregYoung GregYoung
        {
            get { return _gregYoung; }
        }

        public PageDefinitions Definitions
        {
            get { return _definitions; }
        }

        public PageProof Proof
        {
            get { return _proof; }
        }

        public PageChoices Choices
        {
            get { return _choices; }
        }

        public PageCentralDatabase CentralDatabase
        {
            get { return _centralDatabase; }
        }

        public PageCQRS Cqrs
        {
            get { return _cqrs; }
        }

       public PageEventSourcing EventSourcing
        {
            get { return _eventSourcing; }
        }

       public PageConclusion Conclusion
       {
           get { return _conclusion; }
       }

       public IPage GetCurrentPage()
       {
           return _pages[_presentationNavigation.CurrentPageIndex];
       }

        private PageNavigationModel PageNavigation
        {
            get { return GetCurrentPage().PageNavigation; }
        }

        public decimal Scale
        {
            get { return (decimal)_inertialScale.Value; }
        }

        public double PositionX
        {
            get { return (double)_positionX.Value; }
        }

        public double PositionY
        {
            get { return (double)_positionY.Value; }
        }

        public void ZoomBy(int delta, Point relativeTo)
        {
            double ticks = delta / 120.0;

            Point adjust = relativeTo;
            double scaleStart = PageNavigation.Scale;
            PageNavigation.ZoomBy(ticks / 3.0);
            double scaleEnd = PageNavigation.Scale;
            double scalar = 1 / scaleEnd - 1 / scaleStart;
            PageNavigation.PanBy(new Point(adjust.X * scalar, adjust.Y * scalar));
        }

        public void PanBy(Point offset)
        {
            PageNavigation.PanBy(new Point(offset.X / PageNavigation.Scale, offset.Y / PageNavigation.Scale));
        }

        public void ResetZoom()
        {
            PageNavigation.Reset();
        }

        public void MovePage(int offset)
        {
            int nextPageIndex = _presentationNavigation.CurrentPageIndex + offset;
            if (0 <= nextPageIndex && nextPageIndex < _pages.Length)
            {
                _presentationNavigation.CurrentPageIndex = nextPageIndex;
            }
        }

        public string CurrentPageTag
        {
            get { return GetCurrentPage().Tag; }
        }

        public string Notes
        {
            get { return GetCurrentPage().Notes; }
            set { GetCurrentPage().Notes = value; }
        }
    }
}
