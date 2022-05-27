namespace WorkerManagementAPI.Exceptions
{
    public class PasswordNotMatchException : Exception
    {
        public PasswordNotMatchException(string message) : base(message)
        {
        }
    }
}
