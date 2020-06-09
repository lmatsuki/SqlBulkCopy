using Demo.Inserters;
using System;
using System.Threading.Tasks;

namespace Demo
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Instantiate implementation
                IInserter inserter = new EntityFrameworkInserter();
                //IInserter inserter = new SqlBulkCopyInserter();

                // Specify record count
                await inserter.InsertRecords(25000);
                await inserter.DeleteRecords();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
