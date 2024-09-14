// Inicializar SQLitePCL
using Ejemplo2.Config;
using Ejemplo2.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

Batteries.Init();

using (var db = new AppDbContext())
{
    db.Database.EnsureCreated();

    // Agregar datos si la base de datos está vacía
    if (!db.Users.Any())
    {
        var user = new User { Name = "Alice" };
        user.Posts = new List<Post>
                {
                    new Post { Title = "First Post", Content = "This is my first post" },
                    new Post { Title = "Second Post", Content = "This is my second post" }
                };

        db.Users.Add(user);
        db.SaveChanges();
    }

    // Consultar datos
    var users = db.Users.Include(u => u.Posts).ToList();
    foreach (var user in users)
    {
        Console.WriteLine($"User: {user.Name}");
        foreach (var post in user.Posts)
        {
            Console.WriteLine($" - Post: {post.Title}, Content: {post.Content}");
        }
    }
}