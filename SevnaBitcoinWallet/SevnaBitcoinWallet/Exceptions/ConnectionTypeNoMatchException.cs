// <copyright file="ConnectionTypeNoMatchException.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Exceptions
{
  using System;

  /// <summary>
  /// Custom exception for no matching ConnectionType.
  /// </summary>
  public class ConnectionTypeNoMatchException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionTypeNoMatchException"/> class.
    /// </summary>
    public ConnectionTypeNoMatchException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionTypeNoMatchException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public ConnectionTypeNoMatchException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionTypeNoMatchException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="innerException">Next Exception.</param>
    public ConnectionTypeNoMatchException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}