using EEMC.Models;
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
}

ExplorerBuilderTest();