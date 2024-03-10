using DevExpress.Mvvm;
using EEMC.Models;
using EEMC.Services;
using EEMC.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using EEMC.Messages;

namespace EEMC.ViewModels
{
    public class AddCourseVM : ViewModelBase
    {
        private readonly MessageBus _messageBus;
        private Window? _window;
        private readonly Course _course;

        public AddCourseVM(MessageBus messageBus, Course course)
        {
            _messageBus = messageBus;

            _messageBus.Receive<WindowMessage>(this, async (message) =>
            {
                _window = message.Window;
            });

            _course = course;
        }

        public ICommand AddCourse_Click
        {
            get => new Commands.DelegateCommand((courseName) =>
                {
                    try
                    {
                        _course.CreateCourse(courseName as string);

                        _window?.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            );
        }
    }
}
