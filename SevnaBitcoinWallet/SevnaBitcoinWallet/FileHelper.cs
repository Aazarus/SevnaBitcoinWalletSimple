// <copyright file="FileHelper.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet
{
  using System.IO;

  /// <summary>
  /// The following class provides helper method for acting on files.
  /// </summary>
  internal static class FileHelper
  {
    /// <summary>
    /// Checks if the file exists with a given name.
    /// </summary>
    /// <param name="fileName">The name of the file to check.</param>
    /// <returns>True if file exists or false if it does not.</returns>
    public static bool CheckFileExists(string fileName)
    {
      return File.Exists(fileName);
    }
  }
}