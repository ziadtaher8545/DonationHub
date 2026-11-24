using DonationHub.Domain.Common.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Common.Results;

public class Result
{
    public bool IsSuccess { get; }
    public Error? Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }
    public static Result Ok() => new(true, null);

    public static Result Fail(Error error) => new(false, error);
    public static implicit operator Result(Error error) => Fail(error);

}
public class Result<TValue> : Result
{
    private readonly TValue _value;

    public TValue? Data => IsSuccess ? _value : throw new ArgumentException("Can't Access Data with errors", nameof(Data));

    private Result(TValue value) : base(true, null)
    {
        _value = value;
    }

    private Result(Error error) : base(false, error)
    {
        _value = default!;
    }
    public static Result<TValue> Ok(TValue value) => new(value);
    public static new Result<TValue> Fail(Error error) => new(error);

    public static implicit operator Result<TValue>(TValue value) => Ok(value);
    public static implicit operator Result<TValue>(Error error) => Fail(error);
}
