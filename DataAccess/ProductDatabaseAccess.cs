using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DatabaseAccess.Repositorymodel;
using MySqlConnector;

namespace DatabaseAccess;

public class ProductDatabaseAccess : IProductDatabaseAccess
{
    private string Server = "127.0.0.1";
    private string Port = "3307";
    private string DatabaseName = "product";
    private string User = "root";
    private string Password = "1234";
    private MySqlConnection _connection;


    public ProductDatabaseAccess()
    {
        try {
            string connectionString = $"Server={Server};Port={Port};Database={DatabaseName};Uid={User};Pwd={Password};";
            _connection = new MySqlConnection(connectionString);
        } catch (MySqlException ex){
            Console.WriteLine($"DB Fehler: {ex.Message}");
            throw;
        }

    }

    public async Task<Repositorymodel.ProductRepositoryModel> GetAllProducts()
    {
        var productsList = new Repositorymodel.ProductRepositoryModel();
        productsList.Products = new List<Repositorymodel.Product>();

        string query = @"
            SELECT 
                p.Id AS ProductId, 
                p.Name AS ProductName, 
                p.Price,
                c.Id AS CategoryId, 
                c.Name AS CategoryName,
                d.Id AS DescriptionId, 
                d.ShortSummary, 
                d.DetailedText, 
                d.WeightInGrams
            FROM products p
            INNER JOIN categories c ON p.CategoryId = c.Id
            INNER JOIN descriptions d ON p.DescriptionId = d.Id;";

        try {
            if (_connection.State != ConnectionState.Open) {
                await _connection.OpenAsync();
            }

            using var command = new MySqlCommand(query, _connection);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync()) {

                var product = new Repositorymodel.Product
                (
                    reader.GetInt32("ProductId"),
                    reader.GetString("ProductName"),

                    reader.GetDouble("Price"),

                    new Repositorymodel.Description(
                        reader.GetInt32("DescriptionId"),
                        reader.GetString("ShortSummary"),
                        "",
                        0),

                    new Repositorymodel.Category(
                        reader.GetInt32("CategoryId"),
                        reader.GetString("CategoryName"))

                );

                productsList.Products.Add(product);
            }
        } catch (MySqlException ex) {
            Console.WriteLine($"Fehler beim Abrufen der Produkte: {ex.Message}");
            throw;
        }

        return productsList;
    }
}
