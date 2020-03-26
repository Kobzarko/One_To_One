using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One_To_One
{
    class Program
    {
        static void Main(string[] args)
        {

            //  Добавление и получение

            using (UserContext db = new UserContext())
            {
                User user1 = new User { Login = "login1", Password = "pass1234" };
                User user2 = new User { Login = "login2", Password = "5678word2" };
                db.Users.AddRange(new List<User> { user1, user2 });
                db.SaveChanges();
                UserProfile profile1 = new UserProfile { Id = user1.Id, Age = 22, Name = "Tom" };
                UserProfile profile2 = new UserProfile { Id = user2.Id, Age = 27, Name = "Alice" };
                db.UserProfiles.AddRange(new List<UserProfile> { profile1, profile2 });
                db.SaveChanges();

                foreach (User user in db.Users.Include("Profile").ToList())
                    Console.WriteLine("Name: {0}  Age: {1}  Login: {2}  Password: {3}",
                            user.Profile.Name, user.Profile.Age, user.Login, user.Password);
            }

            // Редактирование

            using (UserContext db = new UserContext())
            {
                User user1 = db.Users.FirstOrDefault();
                if (user1 != null)
                {
                    user1.Password = "dsfvbggg";
                    db.Entry(user1).State = EntityState.Modified;
                    db.SaveChanges();
                }

                UserProfile profile2 = db.UserProfiles.FirstOrDefault(p => p.User.Login == "login2");
                if (profile2 != null)
                {
                    profile2.Name = "Alice II";
                    db.Entry(profile2).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            // При удалении надо учитывать следующее: 
            //так как объект UserProfile требует наличие объекта User и зависит от этого объекта,
            //то при удалении связанного объекта User надо будет удалить и связанный с ним объект UserProfile.
            //    Поскольку по молчанию у нас не предусмотрено каскадное даление при данной связи.
            //    Если же будет удален объект UserProfile, на объект User это никак не повлияет:

            using (UserContext db = new UserContext())
            {
                User user1 = db.Users.Include("Profile").FirstOrDefault();
                if (user1 != null)
                {
                    db.UserProfiles.Remove(user1.Profile);
                    db.Users.Remove(user1);
                    db.SaveChanges();
                }

                UserProfile profile2 = db.UserProfiles.FirstOrDefault(p => p.User.Login == "login2");
                if (profile2 != null)
                {
                    db.UserProfiles.Remove(profile2);
                    db.SaveChanges();
                }
            }

        }
    }
}
