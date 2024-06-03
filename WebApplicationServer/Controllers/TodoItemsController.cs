using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationServer.Models;

namespace WebApplicationServer.Controllers
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
