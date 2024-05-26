using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace EEMC.Models
{
    public class Question : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
        public FlowDocument QuestionText { get; set; }
        public string Answer { get; set; }
    }
}
