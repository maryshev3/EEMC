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

void t() 
{
    string FoundedFolderCourses = Array.Find(Directory.GetDirectories(Environment.CurrentDirectory), (x) =>
    {
        return x == Path.Combine(Environment.CurrentDirectory, "Курсы");
    });
    Console.WriteLine(FoundedFolderCourses);
}

JSONCoursesManagerParseTest();
JSONCoursesManagerUpdateTest();
t();

string[] str1 = { "0", "1" }, str2 = { "2", "3" };

str1 = str2;

str2[1] = "sa";

Console.WriteLine(str1[0] + "" + str1[1]);