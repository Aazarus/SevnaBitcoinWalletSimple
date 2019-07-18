// <copyright file="WalletManager.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet
{
  using System.Collections.Generic;
  using System.Linq;
  using SevnaBitcoinWallet.Commands;
  using SevnaBitcoinWallet.Exceptions;
  using SevnaBitcoinWallet.Wrapper;

  /// <summary>
  /// The following class manages the Wallet.
  /// </summary>
  /// ToDo: Consider making this a singleton.
  /// ToDo: As this class will need to get a password from the user when dependency injection is submitted we will need to introduce an interface that supplies the user provided password.
  public sealed class WalletManager
  {
    /// <summary>
    /// Lock token.
    /// </summary>
    private readonly object threadLock = new object();

    /// <summary>
    /// Initializes a new instance of the <see cref="WalletManager"/> class.
    /// </summary>
    /// <param name="bitcoinLibrary">BitcoinLibrary to use.</param>
    public WalletManager(IBitcoinLibrary bitcoinLibrary)
    {
      this.BitcoinLibrary = bitcoinLibrary;
      this.Commands = new List<string>();
      Configuration.Load();
    }

    /// <summary>
    /// Gets or sets the BitcoinLibrary.
    /// </summary>
    public IBitcoinLibrary BitcoinLibrary { get; set; }

    /// <summary>
    /// Gets the number of commands to process.
    /// </summary>
    /// ToDo: Improve this properties name, it is not just the commands. It is the commands and their corresponding arguments.
    /// ToDo: Consider moving this to the CommandIdentifier, doesn't make much sense that it is in this class.
    public List<string> Commands { get; }

    /// <summary>
    /// Adds CommandIdentifier to the list of CommandIdentifier to process.
    /// </summary>
    /// <param name="argumentsToAdd">Arguments to add.</param>
    /// <exception cref="CommandArgumentNullOrEmptyException">Null or Empty arguments were provided.</exception>
    public void AddCommands(string[] argumentsToAdd)
    {
      if (ConfirmArgumentsAreValid(argumentsToAdd))
      {
        lock (this.threadLock)
        {
          this.Commands.AddRange(argumentsToAdd);
        }
      }
    }

    /// <summary>
    /// Processes each command in the List.
    /// </summary>
    /// <returns>Collection of results from running commands.</returns>
    /// <exception cref="CommandNotFoundException">CommandIdentifier not found.</exception>
    public IEnumerable<string> ProcessCommands()
    {
      var result = new List<string>();
      while (this.CanContinueToProcessCommands())
      {
        try
        {
          result.Add(this.Commands.First());
          result.Add(this.ProcessNextCommand());
        }
        catch (CommandNotFoundException)
        {
          throw new CommandNotFoundException("No CommandIdentifier found.");
        }
      }

      return result;
    }

    /// <summary>
    /// Processes the next command.
    /// </summary>
    /// <returns>Result of processing command.</returns>
    /// <exception cref="CommandNotFoundException">Command not found for index.</exception>
    public string ProcessNextCommand()
    {
      var result = string.Empty;

      if (this.CanContinueToProcessCommands())
      {
        var nextCommandToProcess = this.GetNextCommandToProcess();
        var commandWithArguments =
          CommandIdentifier.FindMatchingCommandWithArguments(nextCommandToProcess, this.Commands);

        result = this.BitcoinLibrary.ProcessCommand(commandWithArguments);
        this.CleanUpPostCommandProcess(commandWithArguments);
      }

      return result;
    }

    /// <summary>
    /// Confirms the arguments are valid.
    /// </summary>
    /// <param name="argumentsToAdd">Arguments to check.</param>
    /// <returns>True if valid else false.</returns>
    /// <exception cref="CommandArgumentNullOrEmptyException">Null or Empty arguments were provided.</exception>
    private static bool ConfirmArgumentsAreValid(IEnumerable<string> argumentsToAdd)
    {
      ConfirmArgumentCollectionNotNull(argumentsToAdd);
      ConfirmArgumentCollectionNotEmpty(argumentsToAdd);
      ConfirmArgumentsNotEmpty(argumentsToAdd);
      ConfirmArgumentsNotNull(argumentsToAdd);

      return true;
    }

    /// <summary>
    /// Throws exception of any of the arguments in collection are empty strings.
    /// </summary>
    /// <param name="argumentsToCheck">Arguments to check.</param>
    private static void ConfirmArgumentsNotEmpty(IEnumerable<string> argumentsToCheck)
    {
      if (argumentsToCheck.Any(argument => argument == string.Empty))
      {
        throw new CommandArgumentNullOrEmptyException("Received empty Argument.");
      }
    }

    /// <summary>
    /// Throws exception of any of the arguments in collection are null.
    /// </summary>
    /// <param name="argumentsToCheck">Arguments to check.</param>
    private static void ConfirmArgumentsNotNull(IEnumerable<string> argumentsToCheck)
    {
      if (argumentsToCheck.Any(argument => argument == null))
      {
        throw new CommandArgumentNullOrEmptyException("Received null Argument.");
      }
    }

    /// <summary>
    /// Throws exception if argument collection is empty.
    /// </summary>
    /// <param name="argumentCollectionToCheck">Collection to check.</param>
    /// <exception cref="CommandArgumentNullOrEmptyException">Empty collection provided.</exception>
    private static void ConfirmArgumentCollectionNotEmpty(IEnumerable<string> argumentCollectionToCheck)
    {
      var argumentsAsList = argumentCollectionToCheck.ToList();
      if (argumentsAsList.Count == 0)
      {
        throw new CommandArgumentNullOrEmptyException("Argument collection is empty.");
      }
    }

    /// <summary>
    /// Throws exception if argument collection is null.
    /// </summary>
    /// <param name="argumentCollectionToCheck">Collection to check.</param>
    /// <exception cref="CommandArgumentNullOrEmptyException">Null collection provided.</exception>
    private static void ConfirmArgumentCollectionNotNull(IEnumerable<string> argumentCollectionToCheck)
    {
      if (argumentCollectionToCheck == null)
      {
        throw new CommandArgumentNullOrEmptyException("Argument contains null or empty values.");
      }
    }

    /// <summary>
    /// Returns the string for the command at the requested index.
    /// </summary>
    /// <returns>Command to process as string.</returns>
    /// <exception cref="CommandNotFoundException">Command not found for index.</exception>
    private string GetNextCommandToProcess()
    {
      return this.Commands.First() ?? throw new CommandNotFoundException("No Command found.");
    }

    /// <summary>
    /// Checks if there are commands still available to process.
    /// </summary>
    /// <returns>Whether commands are still present.</returns>
    private bool CanContinueToProcessCommands()
    {
      lock (this.threadLock)
      {
        if (this.Commands.Count > 0)
        {
          return true;
        }

        throw new CommandNotFoundException("No CommandIdentifier Available.");

      }
    }

    /// <summary>
    /// Removes the processed command and arguments from the Command collection.
    /// </summary>
    /// <param name="commandWithArguments">Command with arguments to remove.</param>
    private void CleanUpPostCommandProcess(IEnumerable<string> commandWithArguments)
    { // ToDo: Cleaner way to do this with LINQ?
      foreach (var element in commandWithArguments)
      {
        if (this.Commands.First() == element)
        {
          this.Commands.RemoveAt(0);
        }
      }
    }
  }
}
