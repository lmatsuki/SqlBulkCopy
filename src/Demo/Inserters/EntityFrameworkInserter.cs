using Demo.Domain.Entities;
using Demo.Infrastructure;
using Demo.Infrastructure.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Inserters
{
    public class EntityFrameworkInserter : IInserter
    {
        public async Task InsertRecords(int recordCount)
        {
            var stopwatch = new Stopwatch();
            IList<Player> players = InsertExtensions.GetPlayers(recordCount);

            stopwatch.Start();

            using (var context = new GameContext())
            {
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                context.Players.AddRange(players);
                await context.SaveChangesAsync();
            }

            stopwatch.Stop();
            Console.WriteLine("Inserting {0} players with related skills using Entity Framework took: {1}!", players.Count, stopwatch.Elapsed);
        }

        public async Task DeleteRecords()
        {
            // We use SQL query instead of EF to delete here as it can kill your SQL Server process if there are too many records
            string connnectionString = new GameContext().Database.GetDbConnection().ConnectionString;
            string deleteRecordsSql = "DELETE Skills; DELETE Players";
            using (SqlConnection connection = new SqlConnection(connnectionString))
            using (SqlCommand command = new SqlCommand(deleteRecordsSql, connection))
            {
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
