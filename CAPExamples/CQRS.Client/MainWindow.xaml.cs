/**********************************************************************
 * 
 * The CAP Theorem and its Implications
 * 
 * Michael L Perry
 * http://qedcode.com
 * 
 * Copyright 2010
 * Creative Commons Attribution 3.0
 * 
 **********************************************************************/

using System.Windows;
using UpdateControls.XAML;

namespace CQRS.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ForView.Wrap(new MainViewModel(new Model(), new LoadTester()));
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            ForView.Unwrap<MainViewModel>(DataContext).ThreadCount = 0;
        }
    }
}
