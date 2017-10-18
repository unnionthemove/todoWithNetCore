using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;


namespace TodoApi.Controllers
{
    [Route("api/todo")]
    public class TodoController : Controller
    {
        private readonly ITodoItemService _itemService;

        public TodoController(ITodoItemService itemService)
        {
            _itemService = itemService;

            _itemService.GetAllItems();
        }

        [HttpGet]
        public IEnumerable<TodoItem> GetAll(bool includeAll =  true)
        {
            if (includeAll)
                return _itemService.GetAllItems();
            else
                return _itemService.GetAllItems().Where(t => t.IsComplete == false).ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(string id)
        {       
            var item = _itemService.GetItem(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

  
        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _itemService.CreateItem(item);
            
            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] TodoItem item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var todo = _itemService.GetItem(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _itemService.EditItem(id, item);
            
            return new ObjectResult(todo);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var todo = _itemService.GetItem(id);
            if (todo == null)
            {
                return NotFound();
            }

            _itemService.DeleteItem(id);
            return new NoContentResult();
        }
    }
}