using HM2.GameSolve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.IoCs
{
    class RegisterIoCCommand : ICommand
    {
        IDictionary<string, ICommand> _commCol;
        string _key;
        ICommand _command;
        public RegisterIoCCommand(string key, ICommand command, IDictionary<string, ICommand> commCol)
        {
            _commCol = commCol;
            _key = key;
            _command = command;
        }
        public void Execute()
        {
            _commCol.Add(_key, _command);
        }
    }
    public class IoC
    {
        IDictionary<string, ICommand> _ioc = new Dictionary<string, ICommand>();

        public ICommand Resolve(string key, params object[] obj)
        {
            if (key.Contains("IoC.Register"))
            {
                ICommand command = new RegisterIoCCommand((string)obj[0], (ICommand)obj[1], _ioc);
                return command;
            }
            else
            {
                return _ioc[key];
            }
        }
    }
}