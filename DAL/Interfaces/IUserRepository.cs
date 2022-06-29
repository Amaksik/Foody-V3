using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();

        Task<User> Get(int id);

        Task<IEnumerable<Product>> Favourites(int id);
        Task<IEnumerable<DayIntake>> Statistics(int id);
        Task<IEnumerable<DayIntake>> Consume(int id, double amount);

        void Create(User item);
        void Update(User item);
        void Update(User item, Product product, bool ToAdd);
        void Delete(int id);

        void SaveChanges();
        void Dispose();


    }
}
