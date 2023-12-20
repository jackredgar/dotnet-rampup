using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnet_rampup.Models;

namespace dotnet_rampup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems([FromQuery(Name = "ShowCompleted")] bool ShowCompleted = true)
        {
          if (_context.TodoItems == null)
          {
              return NotFound();
          }

          if (!ShowCompleted)
            {
                return await _context.TodoItems.Where(n => !n.IsComplete).ToListAsync();
            }
          else
            {
                return await _context.TodoItems.ToListAsync();
            }
            
        }

        [HttpGet("completed")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetCompletedTodoItems()
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }

            return await _context.TodoItems.Where(n => n.IsComplete).ToListAsync();

        }

        [HttpGet("uncompleted")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetUncompletedTodoItems()
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }

            return await _context.TodoItems.Where(n => !n.IsComplete).ToListAsync();

        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(Guid id)
        {
          if (_context.TodoItems == null)
          {
              return NotFound();
          }
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoCreate todoCreate)
        {
            TodoItem todoItem = new TodoItem();
            todoItem.Id = id;
            todoItem.Name = todoCreate.Name;
            todoItem.IsComplete = todoCreate.IsComplete;

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoCreate todoCreate)
        {
            TodoItem todoItem = new TodoItem();
            todoItem.Name = todoCreate.Name;
            todoItem.IsComplete = todoCreate.IsComplete;
            // todoItem.Id = Guid.NewGuid();

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            //    return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return CreatedAtAction(nameof(GetTodoItem), new { id = Guid.NewGuid() }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(Guid id)
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool TodoItemExists(Guid id)
        {
            return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
