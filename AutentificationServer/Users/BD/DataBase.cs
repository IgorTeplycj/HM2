using AutentificationServer.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutentificationServer.Users.BD
{
    /// <summary>
    /// Имитация базы данных
    /// </summary>
    public static class DataBase
    {
        static List<Account> accounts = new List<Account>
        {
            new Account
            {
                Email = "igorvrt@mail.ru",
                Name = "Igor",
                Password  = "12345",
                Role = Roles.Admin
            },
            new Account
            {
                Email = "user1@mail.ru",
                Name  = "User1",
                Password = "123456",
                Role = Roles.User
            },
            new Account
            {
                Email = "user2@mail.ru",
                Name  = "User2",
                Password = "1234567",
                Role = Roles.User
            },
            new Account
            {
                Email = "user3@mail.ru",
                Name  = "User3",
                Password = "1234568",
                Role = Roles.User
            }
        };

        public static List<Account> GetAccounts()
        {
            return accounts;
        }
    }
}
