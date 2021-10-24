using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.InputModel
{
    public class AccountInputModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Account's addres is required.")]
        [StringLength(42, MinimumLength = 42, ErrorMessage = "Invalid address length")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Account's username is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Invalid username length (1-20)")]
        public string Username { get; set; }
        [DefaultValue(false)]
        public bool Ban { get; set; }
    }
}
