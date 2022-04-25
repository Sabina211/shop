using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Client()
        {
        }
        public Client(string firstName, string surname, string patromymic, string phoneNumber, string email)
        {
            Id = 0;
            FirstName = firstName;
            Surname = surname;
            Patronymic = patromymic;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public Client(int id, string firstName, string surname, string patromymic, string phoneNumber, string email)
        {
            Id =id;
            FirstName = firstName;
            Surname = surname;
            Patronymic = patromymic;
            PhoneNumber = phoneNumber;
            Email = email;
        }
    }

}
