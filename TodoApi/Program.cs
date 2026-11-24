using Microsoft.EntityFrameworkCore;   // נדרש עבור UseMySql ו־ToListAsync
using TodoApi;                         // ה־namespace שבו נמצאות Item ו־ToDoDbContext

var builder = WebApplication.CreateBuilder(args);

// ===== 1. רישום שירותים (Services) – לפני Build =====

// 1.א הזרקת ה־DbContext
var connectionString = builder.Configuration.GetConnectionString("ToDoDB");
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// 1.ב הגדרת CORS – פתוח לכולם
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// 1.ג Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ===== 2. Middlewares – אחרי Build =====

app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI();

// ===== 3. Routes =====

app.MapGet("/", () => "Server is running. Go to /items");

// 3.א שליפת כל המשימות
app.MapGet("/items", async (ToDoDbContext db) =>
    await db.Items.ToListAsync());

// 3.ב הוספת משימה חדשה
app.MapPost("/items", async (Item item, ToDoDbContext db) =>
{
    db.Items.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/items/{item.Id}", item);
});

// 3.ג עדכון משימה
app.MapPut("/items/{id}", async (int id, Item input, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();

    if (input.Name is not null) item.Name = input.Name;
    item.IsComplete = input.IsComplete;

    await db.SaveChangesAsync();
    return Results.Ok(item);
});

// 3.ד מחיקת משימה
app.MapDelete("/items/{id}", async (int id, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();

    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
