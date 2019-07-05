// <copyright file="BitcoinLibrary.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Wrapper
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Security;
  using HBitcoin.KeyManagement;
  using SevnaBitcoinWallet.Exceptions;

  /// <summary>
  /// The following class adds a layer of abstraction between SevnaBitcoinWallet
  /// and the in use 3rd party Bitcoin Library.
  /// </summary>
  public class BitcoinLibrary
  {
    /// <summary>
    /// Identifies the command and forwards to correct method for processing.
    /// </summary>
    /// <param name="commandWithArguments">The command to process with any available arguments.</param>
    /// <param name="additionalCommandArguments">Any additional arguments required by a command. i.e. SecureString for GenerateWallet.</param>
    /// <returns>Result of running command.</returns>
    public static string ProcessCommand(List<string> commandWithArguments, object additionalCommandArguments = null)
    {
      switch (commandWithArguments.First())
      {
        case "generate-wallet":
          return GenerateWallet(commandWithArguments.Skip(1).ToArray(), additionalCommandArguments as SecureString);
        default:
          throw new CommandNotFoundException($"Requested command to process '{commandWithArguments.First()}' not found.");
      }
    }

    /// <summary>
    /// Generates a wallet using the provided argument.
    /// </summary>
    /// <param name="args">The arguments for the command.</param>
    /// <param name="password">Bitcoin wallet password.</param>
    /// <returns>Comma separated wallet mnemonic.</returns>
    /// <exception cref="WalletAlreadyExistsException">The requested wallet name already exists.</exception>
    /// <exception cref="GenerateWalletFailedException">Failed to generate wallet.</exception>
    public static string GenerateWallet(string[] args, SecureString password)
    {
      try
      {
        var walletFilePath = GetWalletFilePath(args);

        if (FileHelper.CheckFileExists(walletFilePath))
        {
          throw new WalletAlreadyExistsException("The request to generate a new wallet failed because the wallet already exists.");
        }

        Safe.Create(
          out var mnemonic,
          /* Note: From the Sevna boundary we want the password to be secure the library however takes a string so we must convert.*/
          new System.Net.NetworkCredential(string.Empty, password).Password,
          walletFilePath,
          Configuration.DefaultNetwork);

        return string.Join(",", mnemonic.Words);
      }
      catch (InvalidCommandArgumentFoundException)
      {
        // ToDo: Log
        throw new GenerateWalletFailedException("Could not generate wallet 'wallet-file' argument is invalid.");
      }
    }

    /// <summary>
    /// Pulls the arguments value out of the full argument string.
    /// </summary>
    /// <param name="args">Arguments to search.</param>
    /// <param name="argumentOfInterest">Name of the argument of interest.</param>
    /// <param name="required">Is the argument required.</param>
    /// <returns>The argument value.</returns>
    /// <exception cref="InvalidCommandArgumentFoundException">Command argument is missing or missing a value.</exception>
    // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
    private static string GetArgumentValue(IEnumerable<string> args, string argumentOfInterest, bool required = true)
    {
      var arg = args.FirstOrDefault(x => x.StartsWith(argumentOfInterest));
      var argValue = arg?.Substring(arg.IndexOf("=", StringComparison.Ordinal) + 1);

      if (required && string.IsNullOrEmpty(argValue))
      {
        throw new InvalidCommandArgumentFoundException($"Expected an argument value but was not received. Invalid for argument: {arg}");
      }

      return argValue;
    }

    /// <summary>
    /// Ensures the Wallet File Path is suitable for the Library.
    /// </summary>
    /// <param name="args">The wallets file path.</param>
    /// <returns>The wallets full file path.</returns>
    private static string GetWalletFilePath(IEnumerable<string> args)
    {
      var walletFileName = GetArgumentValue(args, "wallet-file", false);

      if (string.IsNullOrEmpty(walletFileName))
      {
        return Configuration.DefaultWalletFileName;
      }

      const string walletDirectoryName = "Wallets";
      Directory.CreateDirectory(walletDirectoryName);
      return Path.Combine(walletDirectoryName, walletFileName);
    }
  }
}
