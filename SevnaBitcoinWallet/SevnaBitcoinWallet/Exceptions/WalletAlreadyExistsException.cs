// <copyright file="WalletAlreadyExistsException.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Exceptions
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  /// <summary>
  /// Custom exception for when a Wallet File already exists.
  /// </summary>
  public class WalletAlreadyExistsException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="WalletAlreadyExistsException"/> class.
    /// </summary>
    public WalletAlreadyExistsException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WalletAlreadyExistsException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public WalletAlreadyExistsException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WalletAlreadyExistsException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="inner">Next Exception.</param>
    public WalletAlreadyExistsException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}