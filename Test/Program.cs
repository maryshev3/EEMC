using EEMC.Models;
using Xunit;

void JSONCoursesManagerParseTest() 
{
    JSONCoursesManager parser = new JSONCoursesManager(Path.Combine(Environment.CurrentDirectory, "../../..", "Courses.json"));

    List<Course> result = parser.Parse();

    Assert.Equal("Математический анализ", result[0].Name);
    Assert.Equal("Дифференциальные уравнения", result[1].Name);
    Assert.Equal("Комплексный анализ", result[2].Name);
    Assert.Equal("Уравнения математической физики", result[3].Name);
}

void JSONCoursesManagerUpdateTest() 
{
    JSONCoursesManager saver = new JSONCoursesManager(Path.Combine(Environment.CurrentDirectory, "../../..", "CoursesSave.json"));
    List<Course> courses = new List<Course>() 
        { 
            new Course() { Name = "Математический анализ" },
            new Course() { Name = "Дифференциальные уравнения" } 
        };

    saver.Save(courses);

    List<Course> result = saver.Parse();
    Assert.Equal("Математический анализ", result[0].Name);
    Assert.Equal("Дифференциальные уравнения", result[1].Name);
}

JSONCoursesManagerParseTest();
JSONCoursesManagerUpdateTest();