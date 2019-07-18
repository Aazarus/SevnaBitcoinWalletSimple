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
  using NBitcoin;
  using SevnaBitcoinWallet.Exceptions;

  /// <summary>
  /// The following class adds a layer of abstraction between SevnaBitcoinWallet
  /// and the in use 3rd party Bitcoin Library.
  /// </summary>
  // ToDo: BitcoinLibrary should be an interface, with the concrete implementation injected into clients.
  public class BitcoinLibrary : IBitcoinLibrary
  {
    /// <summary>
    /// Identifies the command and forwards to correct method for processing.
    /// </summary>
    /// <param name="commandWithArguments">The command to process with any available arguments.</param>
    /// <param name="additionalCommandArguments">Any additional arguments required by a command.</param>
    /// <returns>Result of running command.</returns>
    public string ProcessCommand(List<string> commandWithArguments, object additionalCommandArguments = null)
    {
      switch (commandWithArguments.First())
      {
        case "generate-wallet":
          return this.GenerateWallet(commandWithArguments.Skip(1).ToArray(), additionalCommandArguments as SecureString);
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
    public string GenerateWallet(string[] args, SecureString password)
    {
      try
      {
        var walletFilePath = this.GetWalletFilePath(args);
        CheckIfWalletExists(walletFilePath);

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
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public string ProcessShowWallet(string[] args)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Recreates a wallet based on the provided mnemonic.
    /// </summary>
    /// <param name="args">[0]"wallet-file=*Wallet file name*", [1]"mnemonic=*Comma separated mnemonic*.</param>
    /// <param name="password">SecureString password.</param>
    /// <returns>Location of generated wallet.</returns>
    /// <exception cref="WalletAlreadyExistsException">The requested wallet name already exists.</exception>
    /// <exception cref="GenerateWalletFailedException">Failed to generate wallet.</exception>
    public string ProcessRecoverWallet(string[] args, SecureString password)
    {
      try
      {
        var walletFilePath = this.GetWalletFilePath(args);

        var userMnemonic = this.GetArgumentValue(args, "mnemonic");
        CheckIfWalletExists(walletFilePath);
        ConfirmMnemonicIsValid(userMnemonic);

        var mnemonic = new Mnemonic(userMnemonic);

        Safe.Recover(
          mnemonic,
          /* Note: From the Sevna boundary we want the password to be secure the library however takes a string so we must convert.*/
          new System.Net.NetworkCredential(string.Empty, password).Password,
          walletFilePath,
          Configuration.DefaultNetwork);

        return walletFilePath;
      }
      catch (InvalidCommandArgumentFoundException)
      {
        // ToDo: Log
        throw new GenerateWalletFailedException("Could not generate wallet 'wallet-file' argument is invalid.");
      }
      catch (InvalidMnemonicException)
      {
        // Because we don't know the exceptions thrown by Safe.Recover() we must rethrow.
        throw;
      }
      catch (WalletAlreadyExistsException ex)
      {
        // Because we don't know the exceptions thrown by Safe.Recover() we must rethrow.
        throw;
      }
      catch (Exception)
      {
        // Safe.Recover() may throw an exception but we're not told what the exception may be, so using a catch all.
        // HBitcoin code does not help.
        // ToDo: Log
        throw new GenerateWalletFailedException("Failed to generate wallet.");
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
    private string GetArgumentValue(IEnumerable<string> args, string argumentOfInterest, bool required = true)
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
    private string GetWalletFilePath(IEnumerable<string> args)
    {
      try
      {
        var walletFileName = this.GetArgumentValue(args, "wallet-file");

        if (string.IsNullOrEmpty(walletFileName))
        {
          return Configuration.DefaultWalletFileName;
        }

        const string walletDirectoryName = "Wallets";
        Directory.CreateDirectory(walletDirectoryName);
        return Path.Combine(walletDirectoryName, walletFileName);
      }
      catch (InvalidCommandArgumentFoundException)
      {
        // Log exception
        throw new GenerateWalletFailedException("Could not generate wallet 'wallet-file' argument is invalid.");
      }
    }

    /// <summary>
    /// Checks if a given wallet exists.
    /// </summary>
    /// <param name="walletFilePath">Path to wallet.</param>
    /// <exception cref="WalletAlreadyExistsException">The requested wallet name already exists.</exception>
    private static void CheckIfWalletExists(string walletFilePath)
    {
      if (FileHelper.CheckFileExists(walletFilePath))
      {
        throw new WalletAlreadyExistsException(
          "The request to generate a wallet failed because the wallet already exists.");
      }
    }

    /// <summary>
    /// Confirms a given mnemonic string is valid.
    /// </summary>
    /// <param name="mnemonic">Mnemonic to check.</param>
    /// <exception cref="InvalidMnemonicException">The provided mnemonic is invalid.</exception>
    private static void ConfirmMnemonicIsValid(string mnemonic)
    {
      try
      {
        if (new Mnemonic(mnemonic).IsValidChecksum == false)
        {
          throw new InvalidMnemonicException("Mnemonic is invalid.");
        }
      }
      catch (FormatException)
      {
        throw new InvalidMnemonicException("Mnemonic is invalid.");
      }
      catch (NotSupportedException)
      {
        throw new InvalidMnemonicException("Mnemonic is invalid.");
      }
      catch (Exception)
      {
        throw new InvalidMnemonicException("Mnemonic is invalid.");
      }
    }
  }
}
