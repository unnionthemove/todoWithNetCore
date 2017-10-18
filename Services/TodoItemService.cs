using System;
using TodoApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using System.Net;

namespace TodoApi.Services
{
    public class TodoItemService : ITodoItemService
    {
        private DocDbConfig _config = new DocDbConfig();
        private DocumentClient client;
        private DocDbConfig config;
        private Uri collectionUri;

        public TodoItemService(IOptions<DocDbConfig> ConfigAccessor)
        {
            config = ConfigAccessor.Value;
            client = new DocumentClient(new Uri(config.Endpoint), config.AuthKey);
            collectionUri = UriFactory.CreateDocumentCollectionUri(config.Database, config.Collection);
            this.CreateDatabaseIfNotExists(config.Database).Wait();
            this.CreateDocumentCollectionIfNotExists(config.Database, config.Collection).Wait();

        }
        public string CreateItem(TodoItem NewItem)
        {
            string NewID = Guid.NewGuid().ToString();
            NewItem.Id = NewID;

            client.CreateDocumentAsync(collectionUri, NewItem).Wait();
            return NewID;
        }

        public void DeleteItem(string Id)
        {
            client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(config.Database, config.Collection, Id)).Wait();
        }

        public void EditItem(string Id, TodoItem EditedItem)
        {
            client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(config.Database, config.Collection, Id), EditedItem).Wait();
        }

        public List<TodoItem> GetAllItems()
        {
            return client.CreateDocumentQuery<TodoItem>(collectionUri).ToList<TodoItem>();
        }

        public TodoItem GetItem(string Id)
        {
            try
            {
            TodoItem item = client.ReadDocumentAsync<TodoItem>(UriFactory.CreateDocumentUri(config.Database, config.Collection, Id).ToString()).Result;
                return item;
            }
            catch (AggregateException ae)
            {
                if (ae.InnerException is DocumentClientException &&
                    (ae.InnerException as DocumentClientException).StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw;
            }
        }

        private async Task CreateDatabaseIfNotExists(string databaseName)
        {
            try
            {
                await this.client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseName));
            }
            catch (DocumentClientException de)
            {
                // If the database does not exist, create a new database
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.client.CreateDatabaseAsync(new Database { Id = databaseName });
                }
                else
                {
                    throw;
                }
            }
        }


        private async Task CreateDocumentCollectionIfNotExists(string databaseName, string collectionName)
        {
            try
            {
                await this.client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName));
            }
            catch (DocumentClientException de)
            {
                // If the document collection does not exist, create a new collection
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    DocumentCollection collectionInfo = new DocumentCollection();
                    collectionInfo.Id = collectionName;

                    collectionInfo.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });

                    await this.client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(databaseName),
                        new DocumentCollection { Id = collectionName },
                        new RequestOptions { OfferThroughput = 400 });
                }
                else
                {
                    throw;
                }
            }
        }
    }
}