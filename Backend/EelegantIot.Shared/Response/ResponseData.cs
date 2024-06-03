namespace EelegantIot.Shared.Response;

public struct ResponseData<T>
{
    public ResponseData(ErrorModel error)
    {
        Error = error;
    }

    public ResponseData(T data)
    {
        Data = data;
    }

    public ErrorModel? Error { get; set; }
    public T? Data { get; set; }
    public bool Success => Error is null;
}