using EEMC.Models;
using System.Collections.ObjectModel;
using Xunit;

void ExplorerBuilderTest() 
{
    //Arrange
    Explorer exp;
    
    //Act
    exp = ExplorerBuilder.Build(Path.Combine(Environment.CurrentDirectory, "Курсы"));

    //Assert
    Assert.Equal("Курсы", exp.Name);
    Assert.Equal(ContentType.Folder, exp.Type);

    Assert.Equal("Комп анализ", exp.Content[0].Name);
    Assert.Equal(ContentType.Folder, exp.Content[0].Type);
    Assert.Equal("Мат анализ", exp.Content[1].Name);
    Assert.Equal(ContentType.Folder, exp.Content[1].Type);
    Assert.Equal("Положение.txt", exp.Content[2].Name);
    Assert.Equal(ContentType.File, exp.Content[2].Type);

    Assert.Empty(exp.Content[0].Content);
    Assert.Equal("Лекция.txt", exp.Content[1].Content[0].Name);
    Assert.Equal(ContentType.File, exp.Content[1].Content[0].Type);

    Assert.Equal("\\Курсы", exp.NameWithPath);

    Assert.Equal("\\Курсы\\Комп анализ", exp.Content[0].NameWithPath);
    Assert.Equal("\\Курсы\\Мат анализ", exp.Content[1].NameWithPath);
    Assert.Equal("\\Курсы\\Положение.txt", exp.Content[2].NameWithPath);

    Assert.Equal("\\Курсы\\Мат анализ\\Лекция.txt", exp.Content[1].Content[0].NameWithPath);
}

void CourseBuilderTest() 
{
    //Arrange
    Course course;

    //Act
    course = CourseBuilder.Build(new ExplorerBuilder());

    //Assert
    Assert.Equal(2, course.Courses.Count());

    Assert.Equal("Комп анализ", course.Courses.ElementAt(0).Name);
    Assert.Equal("Мат анализ", course.Courses.ElementAt(1).Name);

    Assert.Equal("Лекция.txt", course.Courses.ElementAt(1).Content[0].Name);
}

ExplorerBuilderTest();
CourseBuilderTest();