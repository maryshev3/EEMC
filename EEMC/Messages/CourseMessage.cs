using EEMC.Models;
using System.Windows;

namespace EEMC.Messages
{
    public class CourseMessage : IMessage
    {
        public Explorer? Course { get; set; }

        public CourseMessage(Explorer? course) 
        {
            Course = course;
        }
    }
}
