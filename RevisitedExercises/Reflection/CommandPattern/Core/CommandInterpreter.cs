﻿using CommandPattern.Commands;
using CommandPattern.Core.Contracts;
using System;
using System.Linq;

namespace CommandPattern.Core
{
    public class CommandInterpreter : ICommandInterpreter
    {
        private readonly ICommandFactory commandFactory;

        public CommandInterpreter(ICommandFactory commandFactory)
        {
            this.commandFactory = commandFactory;
        }
        public string Read(string args)
        {
            string[] parts = args.Split();
            string commandType = parts[0];
            string[] commandArgs = parts.Skip(1).ToArray();

            ICommand command = this.commandFactory.CreateCommand(commandType);

            return command.Execute(commandArgs);

        }
    }
}
