using Microsoft.AspNetCore.Http;
using MyTestProgForPractice.Classes;

namespace Tests
{
    [TestClass]
    public sealed class Tests_for_API
    {
        [TestClass]
        public class CsvParserTests
        {
            [TestMethod]
            public async Task ParseF_ValidFile_ReturnsValues()
            {
                // Arrange
                var csv = """
                      Date;ExecutionTime;Value
                      2026-07-26T10:00:00Z;1.5;10.5
                      2026-07-26T10:01:00Z;2.0;20.5
                      """;

                var file = CreateFile(csv);
                var parser = new CsvParser();

                // Act
                var result = await parser.ParseF(file);

                // Assert
                Assert.AreEqual(2, result.Count);
                Assert.AreEqual(1.5, result[0].Execution_time);
                Assert.AreEqual(10.5, result[0].ValueData);
            }


            [TestMethod]
            public async Task ParseF_InvalidColumnCount_ThrowsException()
            {
                // Arrange
                var csv = """
                      Date;ExecutionTime;Value
                      2026-07-26T10:00:00Z;1.5
                      """;

                var file = CreateFile(csv);
                var parser = new CsvParser();

                // Act & Assert
                var exception = await Assert.ThrowsExceptionAsync<Exception>(
                    () => parser.ParseF(file));

                StringAssert.Contains(
                    exception.Message,
                    "Неверное количество столбцов");
            }


            private static IFormFile CreateFile(string content)
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(content);
                var stream = new MemoryStream(bytes);

                return new FormFile(
                    stream,
                    0,
                    bytes.Length,
                    "file",
                    "test.csv");
            }
        }
    }
}
