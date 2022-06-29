using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private Context _context;
        public UserRepository(Context context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> Get(int id)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id.ToString());
            return user;

        }

        public async Task<IEnumerable<Product>> Favourites(int id)
        {
            var user = await Get(id);
            return user.Favourite;

        }

        public async Task<IEnumerable<DayIntake>> Statistics(int id)
        {
            var user = await Get(id);
            return user.Statistics;

        }
        public async Task<IEnumerable<DayIntake>> Consume(int id, double amount)
        {
            var user = await Get(id);
            user.Statistics.Add(new DayIntake() { DayCalories = amount });
            Update(user);
            await _context.SaveChangesAsync();

            return user.Statistics;


        }

        public void Create(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChangesAsync();
        }




        public void Update(User entity, Product product, bool ToAdd)
        {
            if (_context.Set<User>().Find(entity) == null)
            {
                throw new KeyNotFoundException(typeof(User).ToString());
            }
            if (ToAdd)
            {
                entity.Favourite.Add(product);
                _context.SaveChangesAsync();

            }
            else
            {
                entity.RemoveProduct(product);
                _context.SaveChangesAsync();
            }

        }
        public void Update(User user)
        {
            var id = Convert.ToInt32(user.Id);
            Delete(id);
            Create(user);
        }

        public async void Delete(int id)
        {
            var user = await Get(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
