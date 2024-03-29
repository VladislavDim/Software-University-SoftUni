﻿using _01.DBFirst.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Text;

namespace _14.DeleteProjectById
{
    public class StartUp
    {
        public static async Task Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            await Console.Out.WriteLineAsync("Which Project do you want to delete?");

            int id = int.Parse(Console.ReadLine());
            string result = await DeleteProjectById(context, id);
        }
        private static async Task<string> DeleteProjectById(SoftUniContext context, int id)
        {
            StringBuilder sb = new StringBuilder();

            using (context.Database.BeginTransaction())
            {
                var employeeProjects = context.EmployeesProjects
                    .Where(p => p.ProjectId == id);

                var project = await context.Projects
                    .Where(p => p.ProjectId == id)
                    .FirstOrDefaultAsync();

                context.EmployeesProjects.RemoveRange(employeeProjects);

                context.Projects.Remove(project);

                context.SaveChanges();

                var projects = await context.Projects
                    .ToListAsync();

                sb.AppendLine($"{project.Name} (DELETED)");

                foreach (var p in projects)
                {
                    sb.AppendLine($"{p.Name}");
                }

                PrintResult(sb.ToString().Trim());

                await Console.Out.WriteLineAsync("Do you want to save changes? Y/N");
                char choice = char.Parse(Console.ReadLine());

                if (choice == 'Y')
                {
                    context.Database.CommitTransactionAsync();
                }
                else
                {
                    context.Database.RollbackTransactionAsync();
                }
            }

            return sb.ToString().Trim();

        }

        private static void PrintResult(string output)
        {
            Console.WriteLine(output);
        }
    }

}