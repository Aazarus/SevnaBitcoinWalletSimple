// <copyright file="CommandNotFoundException.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Exceptions
{
  using System;

  /// <summary>
  /// Custom exception for Command not found.
  /// </summary>
  public class CommandNotFoundException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandNotFoundException"/> class.
    /// </summary>
    public CommandNotFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandNotFoundException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public CommandNotFoundException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandNotFoundException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="innerException">Next Exception.</param>
    public CommandNotFoundException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
