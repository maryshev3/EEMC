﻿using DevExpress.Mvvm;
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
    public class AddFolderVM : ViewModelBase
    {
        private readonly MessageBus _messageBus;
        private Window? _window;
        private Explorer? _chosenCourse;

        public AddFolderVM(MessageBus messageBus)
        {
            _messageBus = messageBus;

            _messageBus.Receive<ExplorerWindowMessage>(this, async (message) =>
            {
                _window = message.Window;
                _chosenCourse = message.Course;
            }
            );
        }

        public ICommand Add
        {
            get => new Commands.DelegateCommand((newCourseName) =>
            {
                try
                {
                    _chosenCourse.AddFolder(newCourseName as string);

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
