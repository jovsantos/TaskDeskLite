using System.Security.Cryptography.X509Certificates;
using TaskDeskLite.Core;

namespace TaskDeskLite.Tests
{
    public class UnitTest1
    {
        [Fact(DisplayName = "Teste De Limite: Titulo muito curto." )] // Fact serve para indicar que o teste é verdadeiro
        public void TesteDeLimite_TituloMuitoCurto()
        {
            // Arrange
            var task = new TaskItem { Title = "Oi", Priority = TaskPriority.Medium };
            var service = new TaskService();

            // Act e Assert
            var ex = Assert.Throws<ArgumentException>(() => service.Create(task));
           

            Assert.Contains("3 a 40 caracteres", ex.Message);
        }
    }
}
