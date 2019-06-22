// <copyright file="Commands.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Commands
{
  using System.Collections.Generic;

  /// <summary>
  /// Defines commandline commands.
  /// </summary>
  public class Commands
  {
    /// <summary>
    /// Gets the accepted Commandline commands.
    /// </summary>
    public static HashSet<string> ApprovedCommands { get; } = new HashSet<string>()
    {
      "help",
      "generate-wallet",
      "recover-wallet",
      "show-balances",
      "show-history",
      "receive",
      "send",
    };
  }
}
