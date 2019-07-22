// <copyright file="IncorrectWalletPasswordException.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Exceptions
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class IncorrectWalletPasswordException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="IncorrectWalletPasswordException"/> class.
    /// </summary>
    public IncorrectWalletPasswordException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IncorrectWalletPasswordException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public IncorrectWalletPasswordException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IncorrectWalletPasswordException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="innerException">Next Exception.</param>
    public IncorrectWalletPasswordException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}