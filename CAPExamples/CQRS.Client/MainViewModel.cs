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

using System.Windows.Input;
using UpdateControls.XAML;

namespace CQRS.Client
{
    public class MainViewModel
    {
        private Model _model;
        private LoadTester _loadTester;

        public MainViewModel(Model model, LoadTester loadTester)
        {
            _model = model;
            _loadTester = loadTester;
        }

        public string FromAccount
        {
            get { return _model.FromAccount; }
            set { _model.FromAccount = value; }
        }

        public string ToAccount
        {
            get { return _model.ToAccount; }
            set { _model.ToAccount = value; }
        }

        public decimal AccountBalance
        {
            get { return _model.AccountBalance; }
        }

        public decimal TransferAmount
        {
            get { return _model.TransferAmount; }
            set { _model.TransferAmount = value; }
        }

        public string LastError
        {
            get { return _model.LastError; }
        }

        public int ThreadCount
        {
            get { return _loadTester.ThreadCount; }
            set { _loadTester.ThreadCount = value; }
        }

        public ICommand Transfer
        {
            get
            {
                return MakeCommand
                    .When(() =>
                        !string.IsNullOrEmpty(_model.FromAccount) &&
                        !string.IsNullOrEmpty(_model.ToAccount) &&
                        _model.TransferAmount > 0.0m)
                    .Do(() => _model.Transfer());
            }
        }

        public ICommand Refresh
        {
            get
            {
                return MakeCommand
                    .Do(() => _model.Refresh());
            }
        }
    }
}
