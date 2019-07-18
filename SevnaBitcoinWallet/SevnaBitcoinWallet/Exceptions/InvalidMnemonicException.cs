// <copyright file="InvalidMnemonicException.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Exceptions
{
  using System;

  /// <summary>
  /// Custom exception for an invalid Mnemonic value.
  /// </summary>
  public class InvalidMnemonicException : Exception
  {/// <summary>
   /// Initializes a new instance of the <see cref="InvalidMnemonicException"/> class.
   /// </summary>
    public InvalidMnemonicException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidMnemonicException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public InvalidMnemonicException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidMnemonicException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="innerException">Next Exception.</param>
    public InvalidMnemonicException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}