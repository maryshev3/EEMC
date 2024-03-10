using EEMC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EEMC.Messages
{
    public class ExplorerWindowMessage : IMessage
    {
        public Window? Window { get; set; }
        public Explorer? Course { get; set; }

        public ExplorerWindowMessage(Window? window, Explorer? course)
        {
            Window = window;
            Course = course;
        }
    }
}
