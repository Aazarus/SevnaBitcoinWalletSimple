namespace SevnaBitcoinWallet.Exceptions
{
  using System;

  /// <summary>
  /// Custom exception for no matching network.
  /// </summary>
  public class NetworkNoMatchException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="NetworkNoMatchException"/> class.
    /// </summary>
    public NetworkNoMatchException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NetworkNoMatchException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public NetworkNoMatchException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NetworkNoMatchException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="inner">Next Exception.</param>
    public NetworkNoMatchException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}