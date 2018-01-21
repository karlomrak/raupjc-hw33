using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ToDoRepository;

namespace WebApplication.Models
{
    public class TodoViewModel
    {
      
        public Guid Id { get; set; }
        public string Text { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DateCompleted { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DateDue { get; set; }

        public string Labels { get; set; }


        public TodoViewModel(TodoItem item)
        {
            Id = item.Id;
            Text = item.Text;
            DateCompleted = item.DateCompleted;
            DateDue = item.DateDue;
        }

        public TodoViewModel()
        {
        }

        public string DaysRemaining()
        {
            if (DateDue == null)
            {
                return " ";
            }

            else
            {
                int nmb = Int32.Parse(((DateTime)DateDue - DateTime.Now).Days.ToString());
                return "za " + NumberToWords(nmb) + " dana!";
            }

        }
        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "nula";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " milijuna ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " tisuća ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " sto ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "i ";

                var unitsMap = new[] { "nula", "jedan", "dva", "tri", "četiri", "pet", "šest", "sedam", "osam", "devet", "deset", "jedanaest", "dvanaest", "trinaest", "četrnaest", "petnaest", "šestnaest", "sedamnaest", "osamnaest", "devetnaest" };
                var tensMap = new[] { "nula", "deset", "dvadeset", "trideset", "četrdeset", "pedeset", "šezdeset", "sedamdeset", "osamdeset", "devedeset" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }

            return words;
        }
    }

}
