using DevExpress.Mvvm;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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


        private Timer _timer;
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

            this._timer = new Timer(
            new TimerCallback((s) => RaisePropertyChanged(() => StopwatchString)),
            null, 1000, 1000);
            _stopwatch.Start();
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

        private bool _isProcessingTest = true;
        public bool IsProcessingTest
        {
            get => _isProcessingTest;
            set
            {
                _isProcessingTest = value;
                RaisePropertyChanged(() => IsProcessingTest);
                RaisePropertyChanged(() => IsNotProcessingTest);
                RaisePropertyChanged(() => AnswerText);
                RaisePropertyChanged(() => ProcessingVisibility);
                RaisePropertyChanged(() => ResultingVisibility);
            }
        }
        public bool IsNotProcessingTest
        {
            get => !_isProcessingTest;
        }
        public string AnswerText
        {
            get => IsProcessingTest ? "Верный ответ на вопрос:" : "Ваш ответ на вопрос:";
        }
        public Visibility ProcessingVisibility
        {
            get => IsProcessingTest ? Visibility.Visible : Visibility.Collapsed;
        }
        public Visibility ResultingVisibility
        {
            get => IsProcessingTest ? Visibility.Collapsed : Visibility.Visible;
        }

        private readonly Stopwatch _stopwatch = new Stopwatch();
        public string StopwatchString
        {
            get 
            {
                TimeSpan ts = _stopwatch.Elapsed;

                return String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            } 
        }

        public ICommand EndTest
        {
            get => new Commands.DelegateCommand((obj) =>
            {
                var notInputedQuestions = _test.Questions.Where(x => String.IsNullOrWhiteSpace(x.UserAnswer));
                if (notInputedQuestions.Any())
                {
                    StringBuilder errorMessage = new();
                    errorMessage.AppendLine("У следующих вопросов забыли написать ответ:");

                    foreach (var item in notInputedQuestions)
                        errorMessage.AppendLine(item.DisplayedShortQuestionText);

                    MessageBox.Show(errorMessage.ToString());

                    return;
                }

                _timer.Change(0, 0);
                _stopwatch.Stop();

                IsProcessingTest = false;

                MessageBox.Show(ConclusionString());
            });
        }

        private string ConclusionString()
        {
            int totalCount = _test.Questions.Sum(x => x.QuestionWeight.Value);
            int rightCount = _test.Questions.Where(x => x.ResultStatus == ResultQuestionStatus.CorrectAnswered).Sum(x => x.QuestionWeight.Value);

            int precission = (rightCount * 100) / totalCount;

            StringBuilder resultString = new StringBuilder();

            resultString.AppendLine("Статистика по прохождению теста:");

            resultString.Append($"Вы набрали {rightCount} баллов из {totalCount}. ");
            resultString.AppendLine(
                precission < 60 
                    ? "Очень плохой результат:("
                    : (
                        precission < 70 
                            ? "На тройку пойдёт"
                            : (
                                precission < 90 
                                    ? "Хороший результат"
                                    : "Молодец!"
                            )
                    )
            );

            resultString.AppendLine($"Общее затраченное время: {StopwatchString}");

            return resultString.ToString();
        }

        public ICommand Conclusion
        {
            get => new Commands.DelegateCommand((obj) =>
            {
                MessageBox.Show(ConclusionString());
            });
        }
    }
}
