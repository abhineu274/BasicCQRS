namespace BasicCQRS.Miscellaneous
{
    public class NotFoundException : Exception // Creating a custom exception class
    {
        public NotFoundException(string message) : base(message) { }
    }
}
