using System;
using Week2_Homework.Records;

namespace Week2_Homework.Methods;

public class ObjectInspector
{
    public static void Inspect(object item)
    {
        switch (item)
        {
            case Student s:
                Console.WriteLine($"Student: '{s.Name}' with {s.Courses.Count} courses.");
                break;

            case Course c:
                Console.WriteLine($"Course: {c.Title} has {c.Credits} credits.");
                break;
            
            default:
                Console.WriteLine("Object is of an unknown type.");
                break;
        }
    }
}