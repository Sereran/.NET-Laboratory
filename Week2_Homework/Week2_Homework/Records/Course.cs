namespace Week2_Homework.Records;

public record Course(string Title, int Credits)
{
    public override string ToString()
    {
        return $"Course(Title: {Title}, Credits: {Credits})";
    }
}