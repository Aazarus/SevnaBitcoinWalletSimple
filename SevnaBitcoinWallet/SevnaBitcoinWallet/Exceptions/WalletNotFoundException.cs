// <copyright file="WalletNotFoundException.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Exceptions
{
  using System;

  /// <summary>
  /// Custom exception for when a given wallet isn't found.
  /// </summary>
  public class WalletNotFoundException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="WalletNotFoundException"/> class.
    /// </summary>
    public WalletNotFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WalletNotFoundException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public WalletNotFoundException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WalletNotFoundException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="inner">Next Exception.</param>
    public WalletNotFoundException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}