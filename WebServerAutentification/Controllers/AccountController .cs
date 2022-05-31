using EndPointMessage;
using HM2.EndPoint.Commands;
using HM2.Threads;
using HM2.Threads.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TokenGenerateLibrary;
using WebServer.IoCs;
using WebServer.Models;
using WebServer.Models.Secure;

namespace WebServer.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public string Index()
        {
            return "index";
        }

        [HttpGet("/idgame")]
        public IActionResult IdGame(string jsonListUsers)
        {
            var autorizedAccounts = JsonSerializer.Deserialize<List<Account>>(jsonListUsers);

            string id = "";
            foreach (var item in autorizedAccounts)
            {
                id += item.Name;
            }

            var idGame = Hash.GetMd5Hash(id);
            IoC<List<Account>>.Resolve("IoC.Registration", idGame, autorizedAccounts);
            return Json(idGame);
        }

        [HttpGet("/token")]
        public IActionResult Token(string user, string idgame)
        {
            //получаем из IoC список пользоваиелей, которым разрешен доступ к игре
            var users = IoC<List<Account>>.Resolve(idgame);
            //получаем пользователя, который запросил доступ к игре
            var reqUser = JsonSerializer.Deserialize<Account>(user);

            //проверяем пользовател на право участия в игре
            List<Account> authorizedUsers = IoC<List<Account>>.Resolve(idgame);
            var userIsFound = authorizedUsers.FirstOrDefault(x => x.Name == reqUser.Name && x.HashOfPassword == reqUser.HashOfPassword);
            if (userIsFound == null)
            {
                //если пользователь не найден в списке авторизованных лиц, то возвращаем ошибку
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            Token tokengen = new Token();
            var token = tokengen.GenerateToken(idgame, reqUser.Name, reqUser.Role.ToString());
            return Json(token);
        }

        [HttpGet("/command")]
        public IActionResult Command(string token, string message)
        {
            //проверка валидности токена
            Token tokengen = new Token();
            try
            { 
                tokengen.ValidateToken(token); 
            }
            catch(Exception ex)
            {
                return BadRequest(new { errorText = "Invalid token." });
            }
            
            CreateClientAndSendMessage(message);
            return Json("Ok");
        }

        void CreateClientAndSendMessage(string msg)
        {
            const string ipAddr = "127.0.0.1";
            const int port = 8080;
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ipAddr), port);
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            var data = Encoding.UTF8.GetBytes(msg);

            tcpSocket.Connect(tcpEndPoint);
            tcpSocket.Send(data);

            var buffer = new byte[2048];
            var size = 0;
            var answer = new StringBuilder();

            do
            {
                size = tcpSocket.Receive(buffer);
                answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
            }
            while (tcpSocket.Available > 0);

            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();
        }
    }
}
