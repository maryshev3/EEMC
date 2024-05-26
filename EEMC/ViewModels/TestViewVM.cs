using DevExpress.Mvvm;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EEMC.ViewModels
{
    public class TestViewVM : ViewModelBase
    {
        private readonly MessageBus _messageBus;
        private Test? _test;

        public IOrderedEnumerable<Question> Questions { get => _test?.Questions?.OrderBy(x => x.QuestionNumber); }

        private Question _selectedQuestion;
        public Question SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                _selectedQuestion = value;
                RaisePropertyChanged(() => SelectedQuestion);
            }
        }

        public TestViewVM(MessageBus messageBus) 
        {
            _messageBus = messageBus;

            _messageBus.Receive<TestMessage>(this, async (message) =>
            {
                _test = message.Test;
                SelectedQuestion = _test.Questions.First();

                RaisePropertyChanged(() => Questions);
            }
            );
        }

        public ICommand OpenQuestion
        {
            get => new Commands.DelegateCommand((question) =>
            {
                Question questionConverted = question as Question;

                if (SelectedQuestion == questionConverted)
                    return;

                SelectedQuestion = questionConverted;
            });
        }

        private bool _isProcessingTest;
        public bool IsProcessingTest
        {
            get => _isProcessingTest;
            set
            {
                _isProcessingTest = value;
            }
        }
        public Visibility ProcessingVisibility
        {
            get => IsProcessingTest ? Visibility.Visible : Visibility.Collapsed;
        }
        public Visibility ResultingVisibility
        {
            get => IsProcessingTest ? Visibility.Collapsed : Visibility.Visible;
        }

        public ICommand EndTest
        {
            get => new Commands.DelegateCommand((obj) =>
            {
                
            });
        }
    }
}
