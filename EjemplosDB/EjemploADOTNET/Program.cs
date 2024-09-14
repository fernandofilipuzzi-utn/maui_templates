using Microsoft.Data.Sqlite;

Console.WriteLine("Hello, World!");


Console.WriteLine("Querying for blogs");
string DbPath = "../../../blogging.db";
Console.WriteLine($"Database path: {DbPath}");

await using var conn = new SqliteConnection($"Data Source={DbPath}");

await conn.OpenAsync();

await using var cmd1 = new SqliteCommand("INSERT INTO Blogs(Url) VALUES('http://www.google.com')", conn);
await using var dataReader1 = await cmd1.ExecuteReaderAsync();

while (await dataReader1.ReadAsync())
{
    Console.WriteLine(dataReader1["BlogId"]);
    Console.WriteLine(dataReader1["Url"]);
}


await using var cmd2 = new SqliteCommand("SELECT * FROM Blogs WHERE BlogId=1", conn);
await using var dataReader2 = await cmd2.ExecuteReaderAsync();

while (await dataReader2.ReadAsync())
{ 
    Console.WriteLine(dataReader2["BlogId"]);
    Console.WriteLine(dataReader2["Url"]);
}