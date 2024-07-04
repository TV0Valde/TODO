namespace TODO.Application.Common.Exceptions
{
    public class DuplecateTitleException : Exception
    {
        public DuplecateTitleException(string title) : base($"Сущность с названием/заголовком {title} уже существует") { }
    }
}
