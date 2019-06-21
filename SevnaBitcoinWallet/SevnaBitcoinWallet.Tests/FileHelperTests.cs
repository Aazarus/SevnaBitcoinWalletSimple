// <copyright file="FileHelperTests.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Tests
{
  using System;
  using System.IO;
  using FluentAssertions;
  using Xunit;

  /// <summary>
  /// Tests the FileHelper class.
  /// </summary>
  public class FileHelperTests
  {
    /// <summary>
    /// Tests the CheckFileExists method correctly identifies an existing file.
    /// </summary>
    [Fact]
    public void CheckFileExists_ShouldReturnTrueIfAFileExists()
    {
      // Arrange
      const string fileName = "CheckFileExists_FileExists.json";
      var currentDirectory = Environment.CurrentDirectory;

      using (File.Create($"{currentDirectory}\\{fileName}"))
      {
        File.Exists($"{currentDirectory}\\{fileName}").Should().BeTrue();

        // Act
        var result = FileHelper.CheckFileExists(fileName);

        // Assert
        result.Should().Be(true);
      }
    }
  }
}
