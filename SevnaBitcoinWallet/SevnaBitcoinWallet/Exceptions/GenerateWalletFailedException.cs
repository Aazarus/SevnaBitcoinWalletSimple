// <copyright file="GenerateWalletFailedException.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Exceptions
{
  using System;

  /// <summary>
  /// Custom exception for when wallet generation fails.
  /// </summary>
  public class GenerateWalletFailedException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="GenerateWalletFailedException"/> class.
    /// </summary>
    public GenerateWalletFailedException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GenerateWalletFailedException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public GenerateWalletFailedException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GenerateWalletFailedException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="inner">Next Exception.</param>
    public GenerateWalletFailedException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}