using BAL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class UserService : IUserService
    {

        IUnitOfWork unitOfWork;


        public UserService(IUnitOfWork uow)
        {
            this.unitOfWork = uow;
        }
        public void CreateUser(User User)
        {

            // validation
            if (User.Id == null || User.Callories == 0)
                throw new ValidationException("Not enough info provided");


            // creating
            unitOfWork.Users.Create(User);
            unitOfWork.Save();
        }

        public async Task<IEnumerable<Product>> GetFavourites(User User)
        {
            int id = Convert.ToInt32(User.Id);
            //validation
            if (unitOfWork.Users.Get(id) != null)
            {
                var result = await unitOfWork.Users.Favourites(id);
                return result;
            }
            else { throw new EntryPointNotFoundException(); }
        }

        //getting all users
        public async Task<IEnumerable<User>> GetUsers()
        {

            return await unitOfWork.Users.GetAll();
        }

        //getting user info by id
        public async Task<User> GetUser(int user_id)
        {
            // validation
            if (user_id == 0)
            {
                throw new ValidationException("Id not provided");
            }
            else
            {// reading

                var user = await unitOfWork.Users.Get(user_id);
                return user;
            }
        }


        //Getting Statistics of user per days
        public async Task<ICollection<DayIntake>> GetStatistics(string user_id, int days)
        {

            var id = Convert.ToInt32(user_id);
            var userStatistics = await unitOfWork.Users.Statistics(id);
            if (userStatistics != null)
            {
                List<DayIntake> result = new List<DayIntake>();

                foreach (var item in userStatistics)
                {
                    result.Add(item);
                }
                if (days > result.Count)
                {
                    return result;
                }
                else
                {
                    List<DayIntake> resultcutted = new List<DayIntake>();
                    for (int i = 0; i < days; i++)
                    {
                        resultcutted.Add(result[0]);
                    }
                    return resultcutted;

                }


            }
            else
            {
                throw new Exception($"user {id} has no 'statistics' list");
            }


        }


        //adding product to list of Favourite users products
        public async Task<bool> AddProduct(int user_id, Product prdct)
        {
            if (!IsAnyNullOrEmpty(prdct))
            {
                var user = await unitOfWork.Users.Get(user_id);
                unitOfWork.Users.Update(user, prdct, true);
                return true;
            }
            else
            {
                return false;
            }

        }


        //removing product from list of Favourite users products
        public async Task<bool> RemoveProduct(int user_id, Product prdct)
        {
            if (!IsAnyNullOrEmpty(prdct))
            {
                var user = await unitOfWork.Users.Get(user_id);
                unitOfWork.Users.Update(user, prdct, false);
                return true;
            }
            else
            {
                return false;
            }
        }


        //adding product to list of Favourite users products
        public async Task<bool> Consume(int user_id, double amount)
        {
            if (amount > 0)
            {
                await unitOfWork.Users.Consume(user_id, amount);
                return true;
            }
            else
            {
                return false;
            }


        }

        //removing users
        public async Task<bool> RemoveUser(int user_id)
        {
            if (user_id != 0)
            {
                var findet = await unitOfWork.Users.Get(user_id);
                if (findet != null)
                {
                    unitOfWork.Users.Delete(user_id);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new Exception("id not provided");
            }

        }


        private bool IsAnyNullOrEmpty(object myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if (string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }

}
