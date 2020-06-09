using Demo.Domain.Entities;
using Demo.Infrastructure;
using Demo.Infrastructure.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Inserters
{
    public class SqlBulkCopyInserter : IInserter
    {
        public async Task InsertRecords(int recordCount)
        {
            var stopwatch = new Stopwatch();
            IList<Player> players = InsertExtensions.GetPlayers(recordCount);

            stopwatch.Start();

            // Specify fields for Players table
            var playersDataTable = new DataTable();
            playersDataTable.Columns.Add("Id");
            playersDataTable.Columns.Add("Name");
            playersDataTable.Columns.Add("NickName");
            playersDataTable.Columns.Add("Hp");
            playersDataTable.Columns.Add("Mp");
            playersDataTable.Columns.Add("Attack");
            playersDataTable.Columns.Add("Defense");
            playersDataTable.Columns.Add("MagicAttack");
            playersDataTable.Columns.Add("MagicDefense");
            playersDataTable.Columns.Add("Level");
            playersDataTable.Columns.Add("CorrelationId");

            // Populate players DataTable
            for (int i = 0; i < players.Count; i++)
            {
                playersDataTable.Rows.Add(players[i].Id, players[i].Name, players[i].NickName,
                    players[i].Hp, players[i].Mp, players[i].Attack, players[i].Defense,
                    players[i].MagicAttack, players[i].MagicDefense, players[i].Level,
                    players[i].CorrelationId);
            }

            // Write Players to SQL database
            string connnectionString = new GameContext().Database.GetDbConnection().ConnectionString;
            using (var sqlBulkCopy = new SqlBulkCopy(connnectionString))
            {
                sqlBulkCopy.DestinationTableName = "Players";
                await sqlBulkCopy.WriteToServerAsync(playersDataTable);
            }

            // Specify fields for Skills table
            var skillsDataTable = new DataTable();
            skillsDataTable.Columns.Add("Id");
            skillsDataTable.Columns.Add("PlayerId");
            skillsDataTable.Columns.Add("Name");
            skillsDataTable.Columns.Add("Cost");
            skillsDataTable.Columns.Add("Cooldown");
            skillsDataTable.Columns.Add("Level");
            skillsDataTable.Columns.Add("IsLearned");
            skillsDataTable.Columns.Add("CorrelationId");

            // Populate skills DataTable
            IList<Skill> skills = players.SelectMany(player => player.Skills).ToList();
            for (int i = 0; i < skills.Count; i++)
            {
                skillsDataTable.Rows.Add(skills[i].Id, skills[i].CorrelationId,
                    skills[i].Name, skills[i].Cost, skills[i].Cooldown, skills[i].Level,
                    skills[i].IsLearned, skills[i].CorrelationId);
            }

            // Write Skills to SQL database
            using (var sqlBulkCopy = new SqlBulkCopy(connnectionString))
            {
                sqlBulkCopy.DestinationTableName = "Skills";
                await sqlBulkCopy.WriteToServerAsync(skillsDataTable);
            }

            // Run following Update statement to sync the parent IDs with children
            string updateMappingSql = "UPDATE Skills SET PlayerId = Players.Id FROM Skills INNER JOIN Players ON Skills.CorrelationId = Players.CorrelationId";
            using (SqlConnection connection = new SqlConnection(connnectionString))
            using (SqlCommand command = new SqlCommand(updateMappingSql, connection))
            {
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }

            stopwatch.Stop();
            Console.WriteLine("Inserting {0} players and {1} skills with SqlBulkCopy took: {2}!", players.Count, skills.Count, stopwatch.Elapsed);
        }

        public async Task DeleteRecords()
        {
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
