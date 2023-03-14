namespace WorkHive.Common.HelperClasses
{
    public class MethodResult<TError> where TError : Enum
    {
        protected readonly List<TError> _errorCodes = new List<TError>();

        public bool IsSuccessful => _errorCodes.Count == 0;

        public MethodResult(params TError[] errorCodes)
        {
            _errorCodes.AddRange(errorCodes);
        }

        public void AddErrorCode(TError error)
        {
            _errorCodes.Add(error);
        }

        public List<string> GetErrors()
        {
            return _errorCodes.Select(c => c.ToString()).ToList();
        }

        public static MethodResult<TError> Success() => new();
        public static MethodResult<TError> Error(params TError[] errorCodes) => new(errorCodes);
    }

    public class MethodResult<TReturn, TError>
        : MethodResult<TError> where TError : Enum
    {
        public readonly TReturn Result;

        public MethodResult(TReturn result)
        {
            Result = result;
        }

        public MethodResult(params TError[] errorCodes)
        {
            _errorCodes.AddRange(errorCodes);
        }

        public static MethodResult<TReturn, TError> Success(TReturn result) => new(result);
        public static new MethodResult<TReturn, TError> Error(params TError[] errorCodes) => new(errorCodes);
    }
}