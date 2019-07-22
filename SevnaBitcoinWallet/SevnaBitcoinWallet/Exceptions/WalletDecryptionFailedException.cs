// <copyright file="WalletDecryptionFailedException.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Exceptions
{
  using System;

  /// <summary>
  /// Custom exception for when Wallet fails to decrypt.
  /// </summary>
  public class WalletDecryptionFailedException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="WalletDecryptionFailedException"/> class.
    /// </summary>
    public WalletDecryptionFailedException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WalletDecryptionFailedException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public WalletDecryptionFailedException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WalletDecryptionFailedException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="innerException">Next Exception.</param>
    public WalletDecryptionFailedException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
