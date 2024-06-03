using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ConsoleAppClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var client = new HttpClient { BaseAddress = new Uri("https://localhost:44358/") };

            // GET
            var todoItems = await client.GetFromJsonAsync<TodoItem[]>("api/todoitems");
            Console.WriteLine("Todo Items:");
            foreach (var item in todoItems)
            {
                Console.WriteLine($"- {item.Name} (Completed: {item.IsComplete})");
            }

            // POST
            var newItem = new TodoItem { Name = "Learn C#", IsComplete = false };
            var response = await client.PostAsJsonAsync("api/todoitems", newItem);
            var createdItem = await response.Content.ReadFromJsonAsync<TodoItem>();
            Console.WriteLine($"Created: {createdItem.Name} (ID: {createdItem.Id})");

            // PUT
            createdItem.IsComplete = true;
            await client.PutAsJsonAsync($"api/todoitems/{createdItem.Id}", createdItem);

            // DELETE
            await client.DeleteAsync($"api/todoitems/{createdItem.Id}");
        }
    }

    public class TodoItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
