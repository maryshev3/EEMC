using EEMC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Messages
{
    public class CourseMessage : IMessage
    {
        public Explorer Course { get; set; }

        public CourseMessage(Explorer course) 
        {
            Course = course;
        }
    }
}
