using System;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;

namespace EEMC.Models
{
    public static class CourseBuilder
    {
        public static Course Build(ExplorerBuilder ExplorerBuilder) 
        {
            return new Course
                (
                    new ObservableCollection<Explorer>
                    (
                        ExplorerBuilder.Build(Path.Combine(Environment.CurrentDirectory, "Курсы")).Content
                            .Where(x => x.Type == ContentType.Folder)
                    )
                );
        }
    }
}
