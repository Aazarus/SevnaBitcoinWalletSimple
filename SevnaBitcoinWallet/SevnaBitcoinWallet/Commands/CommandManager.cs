// <copyright file="CommandManager.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Commands
{
  using System;
  using System.Collections.Generic;
  using SevnaBitcoinWallet.Exceptions;

  /// <summary>
  /// Defines commandline commands.
  /// </summary>
  public class CommandManager
  {
    /// <summary>
    /// Represents the Approved CommandManager.
    /// </summary>
    public enum ApprovedCommands
    {
      /// <summary>
      /// Represents the Generate Wallet command
      /// </summary>
      GenerateWallet,

      /// <summary>
      /// Represents the Help command
      /// </summary>
      Help,

      /// <summary>
      /// Represents the Help command
      /// </summary>
      Receive,

      /// <summary>
      /// Represents the Recover Wallet command
      /// </summary>
      RecoverWallet,

      /// <summary>
      /// Represents the Send command
      /// </summary>
      Send,

      /// <summary>
      /// Represents the Show Balances command
      /// </summary>
      ShowBalances,

      /// <summary>
      /// Represents the Show History command
      /// </summary>
      ShowHistory,
    }

    /// <summary>
    /// Gets the accepted Commandline commands.
    /// </summary>
    public static HashSet<string> ApprovedCommandSet { get; } = new HashSet<string>()
    {
      "generate-wallet",
      "help",
      "receive",
      "recover-wallet",
      "send",
      "show-balances",
      "show-history",
    };

    /// <summary>
    /// Processes current command and returns collection with command and arguments.
    /// </summary>
    /// <param name="commandToProcess">Command to process.</param>
    /// <param name="commands">List of Commands.</param>
    /// <returns>Collection with Command and Arguments.</returns>
    /// <exception cref="CommandNotFoundException">Command to process is unknown.</exception>
    public static List<string> FindMatchingCommandWithArguments(string commandToProcess, List<string> commands)
    {
      switch (commandToProcess.ToLower())
      {
        case "generate-wallet":
          return new List<string>
          {
            commands[0],
            ProcessStringForValidWalletFileArgument(commands[1]),
          };
        case "help":
          return new List<string>
          {
            "help",
          };
        case "receive":
          return new List<string>
          {
            commands[0],
            ProcessStringForValidWalletFileArgument(commands[1]),
          };
        case "recover-wallet":
          return new List<string>
          {
            commands[0],
            ProcessStringForValidWalletFileArgument(commands[1]),

          };
        case "send":
          return new List<string>
          {
            commands[0],
            ProcessStringForValidBtcArgument(commands[1]),
            ProcessStringForValidAddressArgument(commands[2]),
            ProcessStringForValidWalletFileArgument(commands[3]),
          };
        case "show-balances":
          return new List<string>
          {
            commands[0],
            ProcessStringForValidWalletFileArgument(commands[1]),
          };
        case "show-history":
          return new List<string>
          {
            commands[0],
            ProcessStringForValidWalletFileArgument(commands[1]),
          };
        default:
          throw new CommandNotFoundException($"Unknown command provided: {commandToProcess}");
      }
    }

    /// <summary>
    /// Removes matching command in collection.
    /// </summary>
    /// <param name="commandToProcess">Command to remove.</param>
    /// <param name="commands">List of Commands.</param>
    /// <returns>Collection with Command and Arguments.</returns>
    public static List<string> RemoveMatchingCommandInCollection(string commandToProcess, ref List<string> commands)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Ensures argument is a valid wallet-file argument.
    /// </summary>
    /// <param name="argumentToProcess">Argument to process.</param>
    /// <returns>Argument if valid.</returns>
    /// <exception cref="InvalidCommandArgumentFound">Command argument is not valid.</exception>
    private static string ProcessStringForValidWalletFileArgument(string argumentToProcess)
    {
      if (argumentToProcess.Contains("wallet-file="))
      {
        return argumentToProcess;
      }

      throw new InvalidCommandArgumentFound($"Argument should be 'wallet-file=$NameOfWalletFile' actual is: {argumentToProcess}");
    }

    /// <summary>
    /// Ensures argument is a valid btc argument.
    /// </summary>
    /// <param name="argumentToProcess">Argument to process.</param>
    /// <returns>Argument if valid.</returns>
    /// <exception cref="InvalidCommandArgumentFound">Command argument is not valid.</exception>
    private static string ProcessStringForValidBtcArgument(string argumentToProcess)
    {
      if (argumentToProcess.Contains("btc="))
      {
        return argumentToProcess;
      }

      throw new InvalidCommandArgumentFound($"Argument should be 'btc=$AmountOfBitcoin' actual is: {argumentToProcess}");
    }

    /// <summary>
    /// Ensures argument is a valid address argument.
    /// </summary>
    /// <param name="argumentToProcess">Argument to process.</param>
    /// <returns>Argument if valid.</returns>
    /// <exception cref="InvalidCommandArgumentFound">Command argument is not valid.</exception>
    private static string ProcessStringForValidAddressArgument(string argumentToProcess)
    {
      if (argumentToProcess.Contains("address="))
      {
        return argumentToProcess;
      }

      throw new InvalidCommandArgumentFound($"Argument should be 'address=$BitcoinAddress' actual is: {argumentToProcess}");
    }
  }
}
