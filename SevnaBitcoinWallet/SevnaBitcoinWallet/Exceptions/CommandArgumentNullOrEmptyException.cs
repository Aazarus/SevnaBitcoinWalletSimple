// <copyright file="CommandArgumentNullOrEmptyException.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Exceptions
{
  using System;

  /// <summary>
  /// Custom exception for null or empty Command arguments.
  /// </summary>
  public class CommandArgumentNullOrEmptyException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandArgumentNullOrEmptyException"/> class.
    /// </summary>
    public CommandArgumentNullOrEmptyException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandArgumentNullOrEmptyException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public CommandArgumentNullOrEmptyException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandArgumentNullOrEmptyException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="innerException">Next Exception.</param>
    public CommandArgumentNullOrEmptyException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
