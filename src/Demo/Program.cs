using Demo.Inserters;
using System;

namespace Demo
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Instantiate implementation
                IInserter inserter = new EntityFrameworkInserter();
                //IInserter inserter = new SqlBulkCopyInserter();

                // Specify record count
                inserter.InsertRecords(100000);
                inserter.DeleteRecords();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
