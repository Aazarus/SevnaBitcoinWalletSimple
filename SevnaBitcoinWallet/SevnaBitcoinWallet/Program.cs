// <copyright file="Program.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet
{
  using SevnaBitcoinWallet.Wrapper;

  /// <summary>
  /// App entry class.
  /// </summary>
  public class Program
  {
    /// <summary>
    /// App entry method.
    /// </summary>
    /// <param name="args">Arguments passed into app.</param>
    public static void Main(string[] args)
    {
      IBitcoinLibrary bitcoinLibrary = new BitcoinLibrary();
      var walletManager = new WalletManager(bitcoinLibrary);
      walletManager.AddCommands(args);
    }
  }
}
