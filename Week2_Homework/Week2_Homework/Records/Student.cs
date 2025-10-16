namespace Week2_Homework.Records;

public record Student(int Id, string Name, int Age, List<Course> Courses)
{
    public override string ToString()
    {
        string coursesStr = string.Join(", ", Courses.Select(c => c.Title));
        return $"Student(Id: {Id}, Name: {Name}, Age: {Age}, Courses: [{coursesStr}])";
    }
}