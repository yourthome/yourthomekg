using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Yourthome.Models
{
    public class Rental
    {
        public int RentalID { get; set; }
        public Region Region { get; set; }
        public string Street { get; set; }
        public int Rooms { get; set; }
        public int Cost { get; set; }
        public PropertyType PropertyType { get; set; }
        public string Description { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public List<Photo> Photos { get; set; }
    }
    public enum PropertyType
    {
        House,
        Apartment
    }
    public enum Region
    {
        [Display(Name = "Бишкек")]
        Bishkek,
        [Display(Name = "Чуй")]
        Chuy,
        [Display(Name = "Ысык-Кол")]
        Ik,
        [Display(Name = "Нарын")]
        Naryn,
        [Display(Name = "Талас")]
        Talas,
        [Display(Name = "Джалал-Абад")]
        Jalalabad,
        [Display(Name = "Ош")]
        Osh,
        [Display(Name = "Баткен")]
        Batken
    }
    public enum Sort
    {
        ASC,
        DESC
    }
}
