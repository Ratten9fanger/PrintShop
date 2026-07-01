using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShop.DataAccess.Entities
{
    public class UserEntity //описываем уровень хранения, никакой валидации
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        //поведение поля userId в таблице Carts
        public CartEntity? Cart { get; set; }

    }
}
