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
  using DotNetWallet.QBitNinjaJutsus;
  using HBitcoin.KeyManagement;
  using NBitcoin;
  using QBitNinja.Client.Models;
  using SevnaBitcoinWallet.Exceptions;
  using static DotNetWallet.QBitNinjaJutsus.QBitNinjaJutsus;

  /// <summary>
  /// The following class adds a layer of abstraction between SevnaBitcoinWallet
  /// and the in use 3rd party Bitcoin Libraries, NBitcoin and HBitcoin.
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
    /// Recreates a wallet based on the provided mnemonic.
    /// </summary>
    /// <param name="args">[0]"wallet-file=*Wallet file name*", [1]"mnemonic=*Comma separated mnemonic*.</param>
    /// <param name="password">Wallet password.</param>
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
      catch (WalletAlreadyExistsException)
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
    /// Provides the balances for a wallet.
    /// </summary>
    /// <param name="args">Wallet to check.</param>
    /// <param name="password">Wallet password.</param>
    /// <returns>Wallet balances.</returns>
    /// <exception cref="WalletNotFoundException">The request Wallet was not found.</exception>
    /// <exception cref="IncorrectWalletPasswordException">Provided password is incorrect.</exception>
    /// <exception cref="WalletDecryptionFailedException">Failed to decrypt wallet.</exception>
    public string ShowBalances(string[] args, SecureString password)
    {
      var balances = string.Empty;
      try
      {
        var walletFilePath = this.GetWalletFilePath(args);
        CheckIfWalletExists(walletFilePath, true);

        Safe safe = this.DecryptWallet(walletFilePath, password);

        if (Configuration.DefaultConnectionType == ConnectionType.Http)
        {
          // 1. Query all operations grouped by address.
          Dictionary<BitcoinAddress, List<BalanceOperation>> operationPerAddresses = QueryOperationsPerSafeAddresses(safe, 7);

          // 2. Get all address history record with a wrapper class
          List<AddressHistoryRecord> addressHistoryRecords = GetAllAddressHistoryRecords(operationPerAddresses);

          // 3. Calculate wallet balances
          GetBalances(addressHistoryRecords, out Money confirmedWalletBalance, out Money unconfirmedWalletBalance);

          // 4. Group all address history records by addresses
          Dictionary<BitcoinAddress, HashSet<AddressHistoryRecord>> addressHistoryRecordsPerAddress = GroupAllHistoryRecordsByAddresses(operationPerAddresses, addressHistoryRecords);

          // 5. Calculate address balances
          balances += CalculateAddressBalances(addressHistoryRecordsPerAddress);
        }
      }
      catch (WalletNotFoundException)
      {
        // ToDo: Log
        throw;
      }

      return balances;
    }

    /// <summary>
    /// Calculates the confirmed and unconfirmed balance for a collection of addresses.
    /// </summary>
    /// <param name="addressHistoryRecordsPerAddress">Collection of addresses.</param>
    /// <returns>A String containing each Address and it's Confirmed and Unconfirmed balance.</returns>
    private static string CalculateAddressBalances(Dictionary<BitcoinAddress, HashSet<AddressHistoryRecord>> addressHistoryRecordsPerAddress)
    {
      string balances = string.Empty;

      foreach (var elem in addressHistoryRecordsPerAddress)
      {
        GetBalances(elem.Value, out Money confirmedBalance, out Money unconfirmedBalance);

        balances += $"Address Key - {elem.Key.ToString()}: ";

        balances += $" Confirmed Balance: ";
        if (confirmedBalance != Money.Zero)
        {
          balances += $"{confirmedBalance.ToDecimal(MoneyUnit.BTC).ToString("0.#############################")}. ";
        }
        else
        {
          balances += $"0. ";
        }

        balances += $" Unconfirmed Balance: ";
        if (unconfirmedBalance != Money.Zero)
        {
          balances += $"{unconfirmedBalance.ToDecimal(MoneyUnit.BTC).ToString("0.#############################")}! ";
        }
        else
        {
          balances += $"0! ";
        }
      }

      return balances;
    }

    /// <summary>
    /// Groups all corresponding history records to a bitcoin address.
    /// </summary>
    /// <param name="operationPerAddresses">Bitcoin addresses.</param>
    /// <param name="addressHistoryRecords">History records.</param>
    /// <returns>A dictionary of BitcoinAddresses and the corresponding history records.</returns>
    private static Dictionary<BitcoinAddress, HashSet<AddressHistoryRecord>> GroupAllHistoryRecordsByAddresses(
      Dictionary<BitcoinAddress, List<BalanceOperation>> operationPerAddresses,
      List<AddressHistoryRecord> addressHistoryRecords)
    {
      var addressHistoryRecordsPerAddress = new Dictionary<BitcoinAddress, HashSet<AddressHistoryRecord>>();
      foreach (var address in operationPerAddresses.Keys)
      {
        var recs = new HashSet<AddressHistoryRecord>();
        foreach (var record in addressHistoryRecords)
        {
          if (record.Address == address)
          {
            recs.Add(record);
          }
        }

        addressHistoryRecordsPerAddress.Add(address, recs);
      }

      return addressHistoryRecordsPerAddress;
    }

    /// <summary>
    /// Get the history records for addresses.
    /// </summary>
    /// <param name="operationPerAddresses">Dictionary containing BitcoinAddress and collection of Operations.</param>
    /// <returns>A collection of AddressHistoryRecords.</returns>
    private static List<AddressHistoryRecord> GetAllAddressHistoryRecords(Dictionary<BitcoinAddress, List<BalanceOperation>> operationPerAddresses)
    {
      var addressHistoryRecords = new List<AddressHistoryRecord>();

      foreach (var elem in operationPerAddresses)
      {
        foreach (var op in elem.Value)
        {
          addressHistoryRecords.Add(new AddressHistoryRecord(elem.Key, op));
        }
      }

      return addressHistoryRecords;
    }

    /// <summary>
    /// Checks if a given wallet exists and throws an exception based on whether it should or not.
    /// </summary>
    /// <param name="walletFilePath">Path to wallet.</param>
    /// <param name="shouldWalletExist">Should the wallet exist.</param>
    /// <exception cref="WalletAlreadyExistsException">The requested wallet name already exists.</exception>
    /// <exception cref="WalletNotFoundException">The request Wallet was not found.</exception>
    private static void CheckIfWalletExists(string walletFilePath, bool shouldWalletExist = false)
    {
      bool walletExists = FileHelper.CheckFileExists(walletFilePath);
      if (walletExists && shouldWalletExist == false)
      {
        throw new WalletAlreadyExistsException(
          "The request to generate a wallet failed because the wallet already exists.");
      }

      if (!walletExists && shouldWalletExist == true)
      {
        throw new WalletNotFoundException($"Wallet not found: {walletFilePath}");
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
    /// <exception cref="InvalidCommandArgumentFoundException">Command argument is missing or missing a value.</exception>
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
        throw;
      }
    }

    /// <summary>
    /// Decrypts a wallet.
    /// </summary>
    /// <param name="walletFilePath">Path to wallet.</param>
    /// <param name="password">Wallet Password.</param>
    /// <returns>A Safe object.</returns>
    /// <exception cref="IncorrectWalletPasswordException">Provided password is incorrect.</exception>
    /// <exception cref="WalletDecryptionFailedException">Failed to decrypt wallet.</exception>
    private Safe DecryptWallet(string walletFilePath, SecureString password)
    {
      Safe safe;

      try
      {
        safe = Safe.Load(
          new System.Net.NetworkCredential(string.Empty, password).Password, /* Note: From the Sevna boundary we want the password to be secure the library however takes a string so we must convert.*/
          walletFilePath);
      }
      catch (SecurityException)
      {
        throw new IncorrectWalletPasswordException("Provided password is incorrect.");
      }

      if (safe == null)
      {
        throw new WalletDecryptionFailedException("Wallet could not be decrypted.");
      }

      return safe;
    }
  }
}
