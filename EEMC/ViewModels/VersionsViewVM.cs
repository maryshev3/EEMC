using DevExpress.Mvvm;
using EEMC.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.ViewModels
{
    public class VersionsViewVM : ViewModelBase
    {
        private Version[] _versions;
        public Version[] Versions
        {
            get => _versions;
            set
            {
                _versions = value;
                RaisePropertyChanged(() => Versions);
            }
        }

        public VersionsViewVM()
        {
            if (File.Exists("./versions.json"))
            {
                string json = File.ReadAllText("./versions.json");

                Versions = JsonConvert.DeserializeObject<Version[]>(json);
            }
        }
    }
}
