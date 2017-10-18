using System;
using TodoApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Services
{
    public interface ITodoItemService
    {
        List<TodoItem> GetAllItems();

        TodoItem GetItem(string Id);

        void DeleteItem(string Id);

        string CreateItem(TodoItem NewItem);

        void EditItem(string Id, TodoItem EditedItem);
    }
}