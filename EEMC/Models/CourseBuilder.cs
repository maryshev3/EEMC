using System;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EEMC.Models
{
    public static class CourseBuilder
    {
        public static Course Build(ExplorerBuilder ExplorerBuilder) 
        {
            try
            {
                //Формируем статические файлы курса
                Course course = new Course
                    (
                        new ObservableCollection<Explorer>
                        (
                            ExplorerBuilder.Build(Path.Combine(Environment.CurrentDirectory, "Курсы")).Content
                                .Where(x => x.Type == ContentType.Folder)
                        )
                    );

                //Формируем "темы" курса
                //Считываем из json существующие темы
                string json = File.ReadAllText("./themes.json");

                Theme[] themes = JsonConvert.DeserializeObject<Theme[]>(json);
                var themesGrouped = themes.GroupBy(x => x.CourseName);

                var coursesMap = course.Courses.ToDictionary(x => x.Name);

                foreach (var item in themesGrouped)
                {
                    if (coursesMap.ContainsKey(item.Key))
                    {
                        coursesMap[item.Key].Themes = new ObservableCollection<Theme>(item);
                    }
                }

                return course;

            }
            catch (System.IO.DirectoryNotFoundException) 
            {
                return new Course
                    (
                        new ObservableCollection<Explorer>()
                    );
            }
        }
    }
}
