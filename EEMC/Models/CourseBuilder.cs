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
        private static readonly string _imagePathTemplate = "/Resources/course_img{NUM_OF_IMG}.png";
        private static readonly int _imagesCount = 9;
        private static readonly Random _random = new Random();

        private static string GenerateImagePath()
        {
            int numOfImg = _random.Next(0, _imagesCount - 1);

            return _imagePathTemplate.Replace("{NUM_OF_IMG}", numOfImg.ToString());
        }

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

                var coursesMap = course.Courses.ToDictionary(x => x.Name);
                Explorer[] coursesWithoutImage = default;
                CourseImage[] courseImages = default;

                //Заполняем данные об изображениях курсов
                if (File.Exists("./course_img.json"))
                {
                    string courseImgJson = File.ReadAllText("./course_img.json");

                    courseImages = JsonConvert.DeserializeObject<CourseImage[]>(courseImgJson);

                    foreach (var courseImage in courseImages)
                    {
                        if (coursesMap.ContainsKey(courseImage.CourseName))
                        {
                            coursesMap[courseImage.CourseName].ImagePath = courseImage.ImagePath;
                        }
                    }

                    coursesWithoutImage = course.Courses.Where(x => x.ImagePath == default).ToArray();
                }
                else
                {
                    coursesWithoutImage = course.Courses.ToArray();
                }

                if (
                    coursesWithoutImage.Any()
                    || !coursesWithoutImage.Any() && courseImages != default && courseImages.Length != course.Courses.Count
                )
                {
                    foreach (var courseWithoutImage in coursesWithoutImage)
                        courseWithoutImage.ImagePath = GenerateImagePath();

                    string courseImgJson = JsonConvert.SerializeObject(
                        course.Courses.Select(
                            x => new CourseImage()
                            {
                                CourseName = x.Name,
                                ImagePath = x.ImagePath
                            }
                        )
                    );

                    File.WriteAllText("./course_img.json", courseImgJson);
                }

                //Формируем "темы" курса
                //Считываем из json существующие темы
                string json = File.ReadAllText("./themes.json");

                Theme[] themes = JsonConvert.DeserializeObject<Theme[]>(json);
                var themesGrouped = themes.GroupBy(x => x.CourseName);

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
