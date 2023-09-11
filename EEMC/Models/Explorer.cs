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

    //public struct Content 
    //{
    //    public string Name;
    //    public ContentType Type;
    //} 

    public class Explorer
    {
        private string _name;
        public string Name 
        { 
            get => _name;
            set => _name = value;
        }

        public ContentType Type;

        public ObservableCollection<Explorer>? Content;
    }
}
