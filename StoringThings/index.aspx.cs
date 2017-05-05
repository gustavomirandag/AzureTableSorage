using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types

namespace StoringThings
{
    public class EntidadeEstudante : TableEntity
    {
        public EntidadeEstudante(string firstName)
        {
            this.PartitionKey = "EDS"; //Engenharia de Software
            this.RowKey = firstName;
        }

        public EntidadeEstudante() { }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }

    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnCriar_Click(object sender, EventArgs e)
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                StoringThings.Properties.Settings.Default.StorageConnectionString);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("alunos");

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            // Create the batch operation.
            TableBatchOperation batchOperation = new TableBatchOperation();

            // Create a customer entity and add it to the table.
            EntidadeEstudante cliente = new EntidadeEstudante("Guilherme");
            cliente.Email = "gulherme@jk.com";
            cliente.PhoneNumber = "21 23165465";

            // Create another customer entity and add it to the table.
            EntidadeEstudante cliente2 = new EntidadeEstudante("Patrick");
            cliente2.Email = "patrick@mitchel.com";
            cliente2.PhoneNumber = "21 85854621";

            // Add both customer entities to the batch insert operation.
            batchOperation.Insert(cliente);
            batchOperation.Insert(cliente2);

            // Execute the batch operation.
            table.ExecuteBatch(batchOperation);
        }

        protected void btnObterTodos_Click(object sender, EventArgs e)
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                StoringThings.Properties.Settings.Default.StorageConnectionString);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference("alunos");

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<EntidadeEstudante> query = new TableQuery<EntidadeEstudante>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "EDS"));

            IEnumerable<EntidadeEstudante> estudantes = table.ExecuteQuery(query);
            gvAlunos.DataSource = estudantes;
            gvAlunos.DataBind();

            // Print the fields for each customer.
            /*foreach (EntidadeEstudante entity in table.ExecuteQuery(query))
            {
                Console.WriteLine("{0}, {1}\t{2}\t{3}", entity.PartitionKey, entity.RowKey,
                    entity.Email, entity.PhoneNumber);
            }*/
        }

        protected void btnDeletarTabela_Click(object sender, EventArgs e)
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                StoringThings.Properties.Settings.Default.StorageConnectionString);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable that represents the "people" table.
            CloudTable table = tableClient.GetTableReference("alunos");

            // Delete the table it if exists.
            table.DeleteIfExists();
        }
    }
}