using EEMC.Models;
using Xunit;

void JSONParserCoursesTest() 
{
    //
    JSONCoursesParser parser = new JSONCoursesParser();

    //
    parser.Parse(Path.Combine(Environment.CurrentDirectory, "../../.." ,"Courses.json"));
    List<Course> result = parser.Courses;

    //
    Assert.Equal("Математический анализ", result[0].name);
    Assert.Equal("Дифференциальные уравнения", result[1].name);
    Assert.Equal("Комплексный анализ", result[2].name);
    Assert.Equal("Уравнения математической физики", result[3].name);
}

JSONParserCoursesTest();