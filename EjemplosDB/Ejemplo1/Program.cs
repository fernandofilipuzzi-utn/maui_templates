using Ejemplo1.Config;
using Ejemplo1.Models;


using (var db = new AppDbContext())
{
    db.Database.EnsureCreated();

    // Agregar datos
    if (!db.Users.Any())
    {
        db.Users.AddRange(new User { Name = "Alice", Age = 30 },
                            new User { Name = "Bob", Age = 25 });
        db.SaveChanges();
    }

    // Consultar datos
    var users = db.Users.ToList();
    foreach (var user in users)
    {
        Console.WriteLine($"Id: {user.Id}, Name: {user.Name}, Age: {user.Age}");
    }
}


