using AutoMapper;
using NUnit.Framework;
using SchoolSystem.Infrastructure;
using SchoolSystem.Infrastructure.Common;
using SchoolSystem.Tests.Common;
using SchoolSystem.Tests.Common.Mocks;

namespace SchoolSystem.Tests;

public class UnitTestBase
{
    private SchoolLogContext dbContext;
    protected SchoolLogTestDb testDb;
    protected IMapper mapper;
    protected IRepository repo;

    [SetUp]
    public void OneTimeSetUp()
    {
        this.dbContext = DatabaseMock.MockDatabase();
        this.testDb = new SchoolLogTestDb(this.dbContext);
        this.mapper = MapperMock.MockMapper();
        this.repo = new RepoMock(this.dbContext);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        this.dbContext.Dispose();
    }
}
