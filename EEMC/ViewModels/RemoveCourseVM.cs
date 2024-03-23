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
    public class RemoveCourseVM : ViewModelBase
    {
        private readonly MessageBus _messageBus;
        private Window? _window;

        private Explorer? _chosenCourse;

        public Explorer? ChosenCourse
        {
            get => _chosenCourse;
            set
            {
                _chosenCourse = value;
                RaisePropertyChanged(() => ChosenCourse);
            }
        }

        public RemoveCourseVM(MessageBus messageBus)
        {
            _messageBus = messageBus;

            _messageBus.Receive<ExplorerWindowMessage>(this, async (message) =>
                {
                    _window = message.Window;
                    ChosenCourse = message.Course;
                }
            );
        }

        public ICommand RemoveCourse_Click
        {
            get => new Commands.DelegateCommand((obj) =>
                {
                    try
                    {
                        ChosenCourse.Remove();

                        _window?.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            );
        }

        public ICommand CancelRemoving_Click
        {
            get => new Commands.DelegateCommand((obj) =>
                {
                        _window?.Close();
                }
            );
        }
    }
}
