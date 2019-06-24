namespace SevnaBitcoinWallet.Exceptions
{
  using System;

  /// <summary>
  /// Custom exception for unknown ApprovedCommand value.
  /// </summary>
  public class ApprovedCommandValueUnknownException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ApprovedCommandValueUnknownException"/> class.
    /// </summary>
    public ApprovedCommandValueUnknownException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApprovedCommandValueUnknownException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public ApprovedCommandValueUnknownException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApprovedCommandValueUnknownException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="innerException">Next Exception.</param>
    public ApprovedCommandValueUnknownException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}