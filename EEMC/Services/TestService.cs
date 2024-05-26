using EEMC.Models;
using Ionic.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace EEMC.Services
{
    public class ValidateResponse
    {
        public bool IsValid { get; set; }
        /// <summary>
        /// Текст ошибки валидации
        /// </summary>
        public string ValidErrorText { get; set; }
    }

    public static class TestService
    {
        /// <summary>
        /// Делает валидацию теста на возможность его сохранения
        /// </summary>
        /// <returns>True - если тест прошёл валидацию, False - иначе</returns>
        public static ValidateResponse ValidateForSave(Test test)
        {
            ValidateResponse response = new ValidateResponse();

            if (test == default)
            {
                response.IsValid = false;
                response.ValidErrorText = "Тест не создан";

                return response;
            }

            if (!test.Questions.Any())
            {
                response.IsValid = false;
                response.ValidErrorText = "В тесте отсутствуют вопросы";

                return response;
            }

            foreach (var question in test.Questions)
            {
                if (String.IsNullOrWhiteSpace(question.ShortQuestionText))
                {
                    response.IsValid = false;
                    response.ValidErrorText = $"У вопроса под номером {question.QuestionNumber} не заполнен краткий текст";

                    return response;
                }

                if (question.QuestionText == null || question.QuestionText.Blocks.Count == 0)
                {
                    response.IsValid = false;
                    response.ValidErrorText = $"Полный текст вопроса \"{question.DisplayedShortQuestionText}\" не заполнен";

                    return response;
                }

                if (String.IsNullOrWhiteSpace(question.Answer))
                {
                    response.IsValid = false;
                    response.ValidErrorText = $"У вопроса \"{question.DisplayedShortQuestionText}\" не введён ответ на него";

                    return response;
                }
            }

            response.IsValid = true;

            return response;
        }

        public static string Save(Test test)
        {
            string savePath = Path.Combine(Environment.CurrentDirectory, test.TestName);
            string resultFilePath = Path.Combine(Environment.CurrentDirectory, $"{test.TestName}.ctt");

            //Удаляем возможно имеющуюся временную папку и создаём заново
            if (Directory.Exists(savePath))
                Directory.Delete(savePath, true);
            if (File.Exists(savePath + ".ctt"))
                File.Delete(savePath + ".ctt");

            Directory.CreateDirectory(savePath);

            //Сериализуем вопросы
            string json = JsonConvert.SerializeObject(test);

            File.WriteAllText(Path.Combine(savePath, "test.json"), json);

            //Сохраняем FlowDocument'ы каждого вопроса
            foreach (var question in test.Questions)
            {
                var content = new TextRange(question.QuestionText.ContentStart, question.QuestionText.ContentEnd);

                if (content.CanSave(DataFormats.Rtf))
                {
                    using (var stream = new MemoryStream())
                    {
                        //Если в тексте были картинки, то здесь будет выбрасываться исключение, но оно ни на что не влияет: это WPF, это норма:)
                        content.Save(stream, DataFormats.Rtf);

                        File.WriteAllBytes(Path.Combine(savePath, question.TextFileName), stream.ToArray());
                    }
                }
                else
                {
                    throw new Exception($"Не удаётся сохранить содержимое вопроса \"{question.DisplayedShortQuestionText}\"");
                }
            }

            //Сохраняем получившееся в архив в кастомном .tt формате
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (ZipFile zip = new ZipFile(Encoding.UTF8))
            {
                zip.AddDirectory(savePath, "./");

                zip.Save(resultFilePath);
            }

            return resultFilePath;
        }
    }
}
