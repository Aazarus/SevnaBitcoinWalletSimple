// <copyright file="InvalidCommandArgumentFound.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Exceptions
{
  using System;

  /// <summary>
  /// Custom exception for an invalid command argument.
  /// </summary>
  public class InvalidCommandArgumentFound : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCommandArgumentFound"/> class.
    /// </summary>
    public InvalidCommandArgumentFound()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCommandArgumentFound"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public InvalidCommandArgumentFound(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCommandArgumentFound"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="inner">Next Exception.</param>
    public InvalidCommandArgumentFound(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}