﻿using DevExpress.Mvvm;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using EEMC.Views;
using System.Windows.Controls;
using System.Windows.Input;

namespace EEMC.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        private Course _courses;
        public Course Courses
        {
            get => _courses;

            set
            {
                _courses = value;
                RaisePropertyChanged(() => Courses);
            }
        }

        private Page _currentPage;
        public Page CurrentPage 
        {
            get => _currentPage;
            set 
            {
                _currentPage = value;
                RaisePropertyChanged(() => CurrentPage);
            }
        }

        private readonly MessageBus _messageBus;

        public MainWindowVM(Course courses, MessageBus messageBus)
        {
            _courses = courses;
            _messageBus = messageBus;
        }

        public ICommand bMenu_Click 
        {
            get => new Commands.DelegateCommand(async (ChosenCourse) => 
            {
                CurrentPage = new CourseWindow();

                await _messageBus.SendTo<CourseWindowVM>(new CourseMessage(ChosenCourse as Explorer));
            });
        }
    }
}