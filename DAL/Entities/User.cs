using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public double Callories { get; set; }


        public ICollection<DayIntake> Statistics { get; set; }

        public ICollection<Product> Favourite { get; set; }
        public User()
        {
            Favourite = new List<Product>();
            Statistics = new List<DayIntake>();
        }


















        public void RemoveProduct(Product p)
        {
            Favourite.Remove(p);
        }

    }
}
