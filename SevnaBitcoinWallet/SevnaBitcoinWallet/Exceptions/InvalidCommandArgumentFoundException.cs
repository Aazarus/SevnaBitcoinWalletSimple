// <copyright file="InvalidCommandArgumentFoundException.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Exceptions
{
  using System;

  /// <summary>
  /// Custom exception for an invalid command argument.
  /// </summary>
  public class InvalidCommandArgumentFoundException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCommandArgumentFoundException"/> class.
    /// </summary>
    public InvalidCommandArgumentFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCommandArgumentFoundException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public InvalidCommandArgumentFoundException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCommandArgumentFoundException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="inner">Next Exception.</param>
    public InvalidCommandArgumentFoundException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}