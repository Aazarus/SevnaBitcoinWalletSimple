// <copyright file="Program.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet
{
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
      var walletManager = new WalletManager();
      walletManager.AddCommands(args);
    }
  }
}
