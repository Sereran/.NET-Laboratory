using System;
using System.Collections.Generic;
using System.Linq;
using Week2_Homework.Methods;
using Week2_Homework.Records;

var Students = new List<Student>
{
    new Student(1, "Alice", 20, new List<Course>
    {
        new Course("Mathematics", 3),
        new Course("Physics", 4)
    }),
    new Student(2, "Bob", 22, new List<Course>
    {
        new Course("Chemistry", 3),
        new Course("Biology", 4)
    })
};

var Instructor_1 = new Instructor
{
    Name = "Dr. Smith",
    Department = "Computer Science",
    Email = "johnsmith@gmail.com"
};

var courses1 = new List<Course>
{
    new Course("Mathematics", 3),
    new Course("Physics", 4),
    new Course("Chemistry", 3),
    new Course("Biology", 4),
    new Course("Computer Science", 2),
    new Course("History", 2)
};

var updatedStudents = Students[0] with
{
    Courses = Students[0].Courses.Append(new Course("Computer Science", 3)).ToList()
};

Console.WriteLine(Instructor_1);
Console.WriteLine();

Func<List<Course>, List<Course>> filterByCredits = courses =>
{
    return courses.Where(c => c.Credits > 3).ToList();
};

Console.WriteLine("--- Filtering Courses ---");
foreach (var course in filterByCredits(courses1))
{
    Console.WriteLine(course);
}
Console.WriteLine();

Console.WriteLine("--- Inspecting Objects ---");
ObjectInspector.Inspect(Students[0]);
ObjectInspector.Inspect(Students[0].Courses[0]);
ObjectInspector.Inspect(Instructor_1);

while (true)
{
    Console.Write("Enter Student's Name (or 'done'): ");
    string name = Console.ReadLine();

    if (name.ToLower() == "done")
    {
        break;
    }

    Console.Write("Enter Age: ");
    int age = int.Parse(Console.ReadLine());
    
    int Id = Students.Count + 1;
    
    var courses = new List<Course>();
    while (true)
    {
        Console.Write("Enter Course Title (or 'done'): ");
        string courseTitle = Console.ReadLine();
        if (courseTitle.ToLower() == "done")
        {
            break;
        }
    }

    Students.Add(new Student(Id, name, age, courses));
    Console.WriteLine("Student added!");
    Console.WriteLine();
}

Console.WriteLine("\n--- Here Are The Students ---");
if (Students.Count == 0)
{
    Console.WriteLine("No students were added.");
}
else
{
    foreach (var student in Students)
    {
        Console.WriteLine(student);
    }
}