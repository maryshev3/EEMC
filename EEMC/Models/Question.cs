using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace EEMC.Models
{
    public enum ProcessingQuestionStatus
    {
        NotAnswered,
        Answered
    }

    public enum ResultQuestionStatus
    {
        CorrectAnswered,
        IncorrectAnswered
    }

    public class Question : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public int? QuestionWeight { get; set; }
        public int QuestionNumber { get; set; }

        private string _shortQuestionText;
        public string ShortQuestionText 
        {
            get => _shortQuestionText;
            set
            {
                _shortQuestionText = value;
                OnPropertyChanged("DisplayedShortQuestionText");
            }
        }

        [JsonIgnore]
        public string TextFileName
        {
            get => $"{QuestionNumber}_{ShortQuestionText}.rtf";
        }

        [JsonIgnore]
        public string DisplayedShortQuestionText 
        {
            get 
            {
                string txt = String.IsNullOrWhiteSpace(ShortQuestionText) ? "Краткий текст вопроса отсутствует" : ShortQuestionText;

                StringBuilder result = new();

                result.Append(QuestionNumber.ToString());
                result.Append(". ");
                result.Append(txt);

                return result.ToString();
            }
        }
        [JsonIgnore]
        public FlowDocument QuestionText { get; set; }
        public string Answer { get; set; }

        //Этот блок для прохождения теста
        private string _userAnswer;
        [JsonIgnore]
        public string UserAnswer
        {
            get => _userAnswer;
            set
            {
                _userAnswer = value;
                OnPropertyChanged("ProcessingStatus");
                OnPropertyChanged("ResultStatus");
                OnPropertyChanged("ProcessingImage");
                OnPropertyChanged("ResultImage");
            }
        }
        [JsonIgnore]
        public ProcessingQuestionStatus ProcessingStatus
        {
            get => String.IsNullOrWhiteSpace(UserAnswer)
                ? ProcessingQuestionStatus.NotAnswered
                : ProcessingQuestionStatus.Answered;
        }
        [JsonIgnore]
        public ResultQuestionStatus ResultStatus
        {
            get => Answer.Trim().ToLower() == UserAnswer.Trim().ToLower()
                ? ResultQuestionStatus.CorrectAnswered
                : ResultQuestionStatus.IncorrectAnswered;
        }
        [JsonIgnore]
        public string ProcessingImage
        {
            get => ProcessingStatus switch
            {
                ProcessingQuestionStatus.NotAnswered => "/Resources/wait_answer_icon.png",
                ProcessingQuestionStatus.Answered => "/Resources/answered_unknown_icon.png"
            };
        }
        [JsonIgnore]
        public string ResultImage
        {
            get => ResultStatus switch
            {
                ResultQuestionStatus.CorrectAnswered => "/Resources/answered_good_icon.png",
                ResultQuestionStatus.IncorrectAnswered => "/Resources/answered_incorrect_icon.png"
            };
        }
    }
}
