using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One_To_One
{

    // все данные профиля, такие как имя, возраст и так далее, выделяются в класс профиля UserProfile.
    public class UserProfile
    {
        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }

        public string Name { get; set; }
        public int Age { get; set; }

        public User User { get; set; }

    }
}
