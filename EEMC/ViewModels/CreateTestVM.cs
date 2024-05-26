using DevExpress.Mvvm;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EEMC.ViewModels
{
    public class CreateTestVM : ViewModelBase
    {
        public Test ThisTest { get; set; }
        public IOrderedEnumerable<Question> Questions { get => ThisTest.Questions.OrderBy(x => x.QuestionNumber); }

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

        private readonly MessageBus _messageBus;
        private Window? _window;
        private Theme? _chosenTheme;

        public CreateTestVM(MessageBus messageBus)
        {
            ThisTest = new Test();
            SelectedQuestion = ThisTest.Questions.First();

            _messageBus = messageBus;

            _messageBus.Receive<ThemeWindowStringMessage>(this, async (message) =>
            {
                _window = message.Window;
                _chosenTheme = message.Theme;
                ThisTest.TestName = message.String;
            }
            );
        }

        private void NotifyWhenListModified()
        {
            RaisePropertyChanged(() => Questions);
            RaisePropertyChanged(() => EditVisibility);
            RaisePropertyChanged(() => UpQuestionVisibility);
            RaisePropertyChanged(() => DownQuestionVisibility);
        }

        public ICommand AddQuestion
        {
            get => new Commands.DelegateCommand((obj) =>
            {
                ThisTest.AddEmptyQuestion();
                SelectedQuestion = ThisTest.Questions.Last();

                NotifyWhenListModified();
            });
        }

        public ICommand OpenQuestion
        {
            get => new Commands.DelegateCommand((question) =>
            {
                Question questionConverted = question as Question;

                if (SelectedQuestion == questionConverted)
                    return;

                SelectedQuestion = questionConverted;

                RaisePropertyChanged(() => UpQuestionVisibility);
                RaisePropertyChanged(() => DownQuestionVisibility);
            });
        }

        public Visibility EditVisibility
        {
            get => SelectedQuestion == default ? Visibility.Collapsed : Visibility.Visible;
        }

        public ICommand RemoveQuestion
        {
            get => new Commands.DelegateCommand((obj) =>
            {
                var newSelected = ThisTest.Questions.FirstOrDefault(x => x.QuestionNumber == SelectedQuestion.QuestionNumber - 1);

                ThisTest.Remove(SelectedQuestion);

                SelectedQuestion = newSelected;

                NotifyWhenListModified();
            });
        }

        public Visibility UpQuestionVisibility
        {
            get => SelectedQuestion != default && ThisTest.IsEnabledUp(SelectedQuestion) ? Visibility.Visible : Visibility.Collapsed;
        }

        public Visibility DownQuestionVisibility
        {
            get => SelectedQuestion != default && ThisTest.IsEnabledDown(SelectedQuestion) ? Visibility.Visible : Visibility.Collapsed;
        }

        public ICommand UpQuestion
        {
            get => new Commands.DelegateCommand((obj) =>
            {
                ThisTest.Up(SelectedQuestion);

                RaisePropertyChanged(() => Questions);
            });
        }

        public ICommand DownQuestion
        {
            get => new Commands.DelegateCommand((obj) =>
            {
                ThisTest.Down(SelectedQuestion);

                RaisePropertyChanged(() => Questions);
            });
        }

        public ICommand SaveTest
        {
            get => new Commands.DelegateCommand((obj) =>
            {
                //Делаем валидацию текущего теста
                var validResponse = TestService.ValidateForSave(ThisTest);

                if (!validResponse.IsValid)
                {
                    MessageBox.Show(validResponse.ValidErrorText);

                    return;
                }

                //Создаём файл текста
                string filePath = TestService.Save(ThisTest);

                //Относим его к теме
                _chosenTheme?.AddFile(filePath);

                MessageBox.Show($"Тест \"{ThisTest.TestName}\" успешно создан");

                _window?.Close();
            });
        }
    }
}
