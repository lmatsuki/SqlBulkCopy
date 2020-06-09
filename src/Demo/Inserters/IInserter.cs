using System.Threading.Tasks;

namespace Demo.Inserters
{
    public interface IInserter
    {
        Task InsertRecords(int recordCount);

        Task DeleteRecords();
    }
}
