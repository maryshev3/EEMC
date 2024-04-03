﻿using DevExpress.Mvvm;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace EEMC.ViewModels
{
    public class RemoveThemeVM : ViewModelBase
    {
        private readonly MessageBus _messageBus;
        private Window? _window;
        private Theme? _chosenTheme;

        public Theme? ChosenTheme 
        {
            get => _chosenTheme;
            set
            {
                _chosenTheme = value;
                RaisePropertyChanged(() => ChosenTheme);
            }
        }

        public RemoveThemeVM(MessageBus messageBus)
        {
            _messageBus = messageBus;

            _messageBus.Receive<ThemeWindowMessage>(this, async (message) =>
            {
                _window = message.Window;
                _chosenTheme = message.Theme;
            }
            );
        }

        public ICommand RemoveTheme_Click
        {
            get => new Commands.DelegateCommand((themeName) =>
            {
                try
                {
                    _chosenTheme.RemoveTheme();

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