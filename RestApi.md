# REST API

Visual Studio 2019</br>
.Net Core 3.1</br>
포트 번호 맞춤 필요

## Server (ASP.NET Core Web API)

1. 프로젝트 생성
     - Visual Studio 2019를 엽니다.
     - `File` > `New` > `Project`를 선택합니다.
    - ASP.NET Core Web Application을 선택하고 `Next`를 클릭합니다.
    - 프로젝트 이름과 위치를 설정한 후 `Create`를 클릭합니다.
    - 템플릿으로 `API`를 선택하고 `Create`를 클릭합니다.

2. 모델 클래스 추가</br>
`Models` 폴더를 만들고 `TodoItem.cs` 파일을 추가합니다.
```cs
namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
```

3. 컨트롤러 추가</br>
`Controllers` 폴더에 `TodoItemsController.cs` 파일을 추가합니다.
```cs
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private static readonly List<TodoItem> TodoItems = new List<TodoItem>();

        // GET: api/TodoItems
        [HttpGet]
        public ActionResult<IEnumerable<TodoItem>> GetTodoItems()
        {
            return TodoItems;
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public ActionResult<TodoItem> GetTodoItem(long id)
        {
            var todoItem = TodoItems.FirstOrDefault(item => item.Id == id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // POST: api/TodoItems
        [HttpPost]
        public ActionResult<TodoItem> PostTodoItem(TodoItem todoItem)
        {
            todoItem.Id = TodoItems.Count > 0 ? TodoItems.Max(item => item.Id) + 1 : 1;
            TodoItems.Add(todoItem);
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        public IActionResult PutTodoItem(long id, TodoItem todoItem)
        {
            var index = TodoItems.FindIndex(item => item.Id == id);
            if (index == -1)
            {
                return NotFound();
            }

            TodoItems[index] = todoItem;
            return NoContent();
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public IActionResult DeleteTodoItem(long id)
        {
            var todoItem = TodoItems.FirstOrDefault(item => item.Id == id);

            if (todoItem == null)
            {
                return NotFound();
            }

            TodoItems.Remove(todoItem);
            return NoContent();
        }
    }
}
```

4. 서비스 실행
- `F5` 키를 눌러 프로젝트를 실행합니다.
- 브라우저에서 `https://localhost:5001/api/todoitems`로 접근하면 빈 배열이 반환됩니다.

## 클라이언트 (Console Application)

1. 프로젝트 생성
    - Visual Studio 2019를 엽니다.
    - `File` > `New` > `Project`를 선택합니다.
    - `Console App (.NET Core)`를 선택하고 `Next`를 클릭합니다.
    - 프로젝트 이름과 위치를 설정한 후 `Create`를 클릭합니다.

2. Nuget 설치: System.Net.Http.Json

3. HttpClient 사용</br>
`Program.cs` 파일을 수정합니다.
```cs
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace TodoApiClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var client = new HttpClient { BaseAddress = new Uri("https://localhost:5001/") };

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
```

4. 프로젝트 실행

`F5` 키를 눌러 프로젝트를 실행합니다.
콘솔 창에 API의 결과가 출력됩니다.