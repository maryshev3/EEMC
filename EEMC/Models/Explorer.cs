using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Models
{
    public enum ContentType
    {
        File,
        Folder
    }

    public class Explorer
    {
        private string _name;
        public string Name 
        { 
            get => _name;
            set => _name = value;
        }

        public string NameWithPath;

        public ContentType Type;

        public ObservableCollection<Explorer>? Content;

        public Explorer(string Name, string NameWithPath, ContentType Type, ObservableCollection<Explorer>? Content) 
        {
            this._name = Name;
            this.NameWithPath = NameWithPath;
            this.Type = Type;
            this.Content = Content;
        }
    }
}
