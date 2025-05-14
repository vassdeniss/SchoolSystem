using Microsoft.EntityFrameworkCore;
using SchoolSystem.Infrastructure;

namespace SchoolSystem.Tests.Common.Mocks;

public class DatabaseMock
{
    public static SchoolLogContext MockDatabase()
    {
        DbContextOptionsBuilder<SchoolLogContext> optionsBuilder = new();
        optionsBuilder.UseInMemoryDatabase($"SchoolLog-TestDb-{DateTime.Now.Ticks}");
        return new SchoolLogContext(optionsBuilder.Options);
    }
}
