using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Models
{
    public class Test
    {
        public string TestName { get; set; }
        public string TestDescription { get; set; }
        public ObservableCollection<Question> Questions { get; set; }

        public Test() 
        {
            Questions = new ObservableCollection<Question>();

            AddEmptyQuestion();
        }

        public void AddEmptyQuestion()
        {
            Question newQuestion = new();

            int lastQuestionNumber = Questions.Count;

            newQuestion.QuestionNumber = lastQuestionNumber + 1;

            Questions.Add(newQuestion);
        }

        public bool IsEnabledDown(Question question)
        {
            return question.QuestionNumber < Questions.Count;
        }

        public bool IsEnabledUp(Question question)
        {
            return question.QuestionNumber > 1;
        }

        /// <summary>
        /// Повышает номер вопроса в общем списке (понижает в визуальном плане)
        /// </summary>
        public void Down(Question question)
        {
            if (!IsEnabledDown(question))
                throw new Exception("Невозможно поднять вверх по списку");

            Question upper = Questions.First(x => x.QuestionNumber == question.QuestionNumber + 1);

            upper.QuestionNumber--;
            question.QuestionNumber++;
        }

        /// <summary>
        /// Понижает номер вопроса в общем списке (повышает в визуальном плане)
        /// </summary>
        public void Up(Question question)
        {
            if (!IsEnabledUp(question))
                throw new Exception("Невозможно спустить вниз по списку");

            Question downer = Questions.First(x => x.QuestionNumber == question.QuestionNumber - 1);

            downer.QuestionNumber++;
            question.QuestionNumber--;
        }

        /// <summary>
        /// Удаляет переданный вопрос из теста
        /// </summary>
        public void Remove(Question question)
        {
            Questions.Remove(question);

            var afterQuestions = Questions.Where(x => x.QuestionNumber > question.QuestionNumber);
            foreach (var afterQuestion in afterQuestions)
                afterQuestion.QuestionNumber--;
        }
    }
}
