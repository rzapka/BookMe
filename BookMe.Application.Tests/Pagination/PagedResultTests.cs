using System;
using System.Collections.Generic;
using BookMe.Application.Pagination;
using Xunit;

namespace BookMe.Application.Pagination.Tests
{
    public class PagedResultTests
    {
        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly_WhenInputIsValid()
        {
            // Arrange
            var items = new List<string> { "Item1", "Item2", "Item3" };
            int totalCount = 10;
            int pageSize = 5;
            int pageNumber = 2;

            // Act
            var pagedResult = new PagedResult<string>(items, totalCount, pageSize, pageNumber);

            // Assert
            Assert.Equal(items, pagedResult.Items);
            Assert.Equal(totalCount, pagedResult.TotalItemsCount);
            Assert.Equal(2, pagedResult.TotalPages); // 10 / 5 = 2
            Assert.Equal(6, pagedResult.ItemsFrom); // (5 * (2 - 1)) + 1 = 6
            Assert.Equal(10, pagedResult.ItemsTo);  // 6 + 5 - 1 = 10
        }

        [Fact]
        public void Constructor_ShouldHandleSinglePageCorrectly()
        {
            // Arrange
            var items = new List<string> { "Item1", "Item2" };
            int totalCount = 2;
            int pageSize = 10;
            int pageNumber = 1;

            // Act
            var pagedResult = new PagedResult<string>(items, totalCount, pageSize, pageNumber);

            // Assert
            Assert.Equal(items, pagedResult.Items);
            Assert.Equal(totalCount, pagedResult.TotalItemsCount);
            Assert.Equal(1, pagedResult.TotalPages);
            Assert.Equal(1, pagedResult.ItemsFrom);
            Assert.Equal(2, pagedResult.ItemsTo);
        }

        [Fact]
        public void Constructor_ShouldHandleEmptyItemsCorrectly()
        {
            // Arrange
            var items = new List<string>();
            int totalCount = 0;
            int pageSize = 10;
            int pageNumber = 1;

            // Act
            var pagedResult = new PagedResult<string>(items, totalCount, pageSize, pageNumber);

            // Assert
            Assert.Equal(items, pagedResult.Items);
            Assert.Equal(totalCount, pagedResult.TotalItemsCount);
            Assert.Equal(0, pagedResult.TotalPages);
            Assert.Equal(0, pagedResult.ItemsFrom);
            Assert.Equal(0, pagedResult.ItemsTo);
        }

        [Fact]
        public void Constructor_ShouldHandleEdgeCases_WhenPageNumberIsOutOfRange()
        {
            // Arrange
            var items = new List<string> { "Item1", "Item2" }; // Zakładamy, że na stronie są tylko dwa elementy
            int totalCount = 5; // Całkowita liczba elementów w systemie
            int pageSize = 2; // Rozmiar jednej strony
            int pageNumber = 3; // Trzecia strona (wykracza poza pełny zakres danych)

            // Act
            var pagedResult = new PagedResult<string>(items, totalCount, pageSize, pageNumber);

            // Assert
            Assert.Equal(items, pagedResult.Items);
            Assert.Equal(totalCount, pagedResult.TotalItemsCount);
            Assert.Equal(3, pagedResult.TotalPages); // 5 / 2 = 2.5 -> 3 strony
            Assert.Equal(5, pagedResult.ItemsFrom); // (2 * (3 - 1)) + 1 = 5
            Assert.Equal(5, pagedResult.ItemsTo);   // Math.Min(5 + 2 - 1, 5) = 5
        }


        [Fact]
        public void Constructor_ShouldHandleInvalidPageSizeGracefully()
        {
            // Arrange
            var items = new List<string> { "Item1" };
            int totalCount = 10;
            int pageSize = 0; // Invalid page size
            int pageNumber = 1;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new PagedResult<string>(items, totalCount, pageSize, pageNumber));
        }
    }
}
