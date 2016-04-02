using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using DSA.Common.Infrastructure.ViewModel;

namespace DSA.Database.Model.Entities
{
    public class LocalPatient:ViewModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string Block { get; set; }
        public string StreetNumber { get; set; }
        public string Job {get; set; }
        public string Ocupation { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Brush Brush { get; set; }
        public bool NewlyAdded { get; set; }
        //public string EntireName { get; set; }
        public List<LocalIntervention> Interventions =new List<LocalIntervention>();

        private string allName;
        public string AllName
        {
            get { return allName; }
            set
            {
                if (value == allName)
                    return;
                allName = value;
                OnPropertyChanged();
            }
        }        
    }
}
