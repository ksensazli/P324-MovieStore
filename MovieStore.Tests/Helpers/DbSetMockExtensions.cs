using Microsoft.EntityFrameworkCore;
using Moq;

namespace MovieStore.Tests.Helpers;

public static class DbSetMockExtensions
{
    public static Mock<DbSet<T>> CreateDbSetMock<T>(this IEnumerable<T> elements) where T : class
    {
        var elementsAsQueryable = elements.AsQueryable();
        var dbSetMock = new Mock<DbSet<T>>();

        dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elementsAsQueryable.GetEnumerator());

        dbSetMock.Setup(x => x.Add(It.IsAny<T>())).Callback<T>((s) => elements.ToList().Add(s));
        dbSetMock.Setup(x => x.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>((s) => elements.ToList().AddRange(s));

        return dbSetMock;
    }
}