using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class AddTodoViewModel
    {
        [Required]
        public string Text { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateDue { get; set; }

        public string Label { get; set; }
    }
}
