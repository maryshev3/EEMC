using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Models
{
    interface IManager
    {
        List<Course> Parse();

        void Save(List<Course> Courses);
    }
}
