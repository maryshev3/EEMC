using DevExpress.Mvvm;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using EEMC.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace EEMC.ViewModels
{
    public class SwitcherCourseViewVM : ViewModelBase
    {
        private static Page _currentPage = null;
        public Page CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                RaisePropertyChanged(() => CurrentPage);
            }
        }

        private Explorer? _currentCourse;

        public Explorer? CurrentCourse
        {
            get => _currentCourse;
            set
            {
                _currentCourse = value;
                RaisePropertyChanged(() => CurrentCourse);
            }
        }

        private readonly MessageBus _messageBus;

        public SwitcherCourseViewVM(MessageBus messageBus)
        {
            _messageBus = messageBus;

            _messageBus.Receive<CourseMessage>(this, async (message) =>
            {
                CurrentCourse = message.Course;
            });
        }

        public ICommand OpenFiles_Click
        {
            get => new Commands.DelegateCommand(async (obj) =>
                {
                    await OpenCourseWindow();
                }
            );
        }

        public ICommand OpenThemes_Click
        {
            get => new Commands.DelegateCommand(async (obj) =>
            {
                await OpenThemesWindow();
            }
            );
        }

        public async Task OpenCourseWindow()
        {
            CurrentPage = new CourseWindow();

            await _messageBus.SendTo<CourseWindowVM>(new CourseMessage(_currentCourse));
        }

        public async Task OpenThemesWindow()
        {
            CurrentPage = new ThemesWindow();

            await _messageBus.SendTo<ThemesWindowVM>(new CourseMessage(_currentCourse));
        }
    }
}
