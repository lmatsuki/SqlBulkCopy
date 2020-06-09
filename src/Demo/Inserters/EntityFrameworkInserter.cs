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
            IEnumerable<IEnumerable<Player>> players = InsertExtensions.GetPlayers(recordCount).Chunk(10000);

            stopwatch.Start();

            List<Task> tasks = new List<Task>();
            using (SemaphoreSlim throttler = new SemaphoreSlim(2))
            {
                foreach (var element in players)
                {
                    await throttler.WaitAsync().ConfigureAwait(false);
                    tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            await InsertPlayers(element).ConfigureAwait(false);
                        }
                        finally
                        {
                            // ReSharper disable once AccessToDisposedClosure
                            throttler.Release();
                        }
                    }));
                }

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }

            stopwatch.Stop();
            Console.WriteLine("Inserting {0} players with related skills using Entity Framework took: {1}!", recordCount, stopwatch.Elapsed);
        }

        public async Task InsertPlayers(IEnumerable<Player> players)
        {
            using (var context = new GameContext())
            {
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                context.Players.AddRange(players);
                await context.SaveChangesAsync();
            }
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
