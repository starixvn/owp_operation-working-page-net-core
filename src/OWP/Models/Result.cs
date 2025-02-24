using System.Text.Json.Serialization;

namespace OWP.Models;

public enum ErrorType
{
    Failure = 0,
    NotFound = 1,
    Validation = 2,
}

public class Error
{
    protected Error(
        ErrorCode code,
        string description,
        int? row,
        ErrorType errorType
    )
    {
        Code = code;
        Description = description;
        Row = row;
        ErrorType = errorType;
    }

    public ErrorCode Code { get; }

    public string Description { get; }

    public ErrorType ErrorType { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Row { get; set; }

    public static Error Failure(ErrorCode code, string description, int? row) =>
        new(code, description, row,ErrorType.Failure);


    public static Error Validation(ErrorCode errorCode, string description, int? row) =>
        new(errorCode, description, row, ErrorType.Validation);


}

public class Result
{
    protected Result()
    {
        IsSuccess = true;
        Errors = null;
    }

    protected Result(IList<Error> errors)
    {
        IsSuccess = false;
        Errors = errors;
    }

    public bool IsSuccess { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<Error> Errors { get; }

    public static Result Success() =>
        new();

    public static Result Failure(IList<Error> errors) =>
        new(errors);
}

public sealed class ResultT<TValue> : Result
{
    private readonly TValue _value;

    private ResultT(
        TValue value
    )
    {
        _value = value;
    }

    private ResultT(
        IList<Error> errors
    ) : base(errors)
    {
        _value = default;
    }

    public TValue Value =>
        IsSuccess ? _value! : throw new InvalidOperationException("Value can not be accessed when IsSuccess is false");


    public static ResultT<TValue> Success(TValue value) =>
        new(value);

    public new static ResultT<TValue> Failure(IList<Error> errors) =>
        new(errors);
}





//public class UploadExcelResult
//{
//    protected UploadExcelResult()
//    {
//        IsSuccess = true;
//        Error = null;
//    }

//    protected UploadExcelResult(ExcelError error)
//    {
//        IsSuccess = false;
//        Error = error;
//    }

//    public bool IsSuccess { get; }

//    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
//    public ExcelError Error { get; }

//    public static implicit operator UploadExcelResult(ExcelError error) =>
//        new(error);

//    public static UploadExcelResult Success() =>
//        new();

//    public static UploadExcelResult Failure(ExcelError error) =>
//        new(error);
//}

//public sealed class UploadExcelResultT<TValue> : UploadExcelResult
//{
//    private readonly TValue _value;

//    private UploadExcelResultT(
//        TValue value
//    )
//    {
//        _value = value;
//    }

//    private UploadExcelResultT(
//        ExcelError error
//    ) : base(error)
//    {
//        _value = default;
//    }

//    public TValue Value =>
//        IsSuccess ? _value! : throw new InvalidOperationException("Value can not be accessed when IsSuccess is false");

//    public static implicit operator UploadExcelResultT<TValue>(ExcelError error) =>
//        new(error);

//    public static implicit operator UploadExcelResultT<TValue>(TValue value) =>
//        new(value);

//    public static UploadExcelResultT<TValue> Success(TValue value) =>
//        new(value);

//    public new static UploadExcelResultT<TValue> Failure(ExcelError error) =>
//        new(error);
//}
