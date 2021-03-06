﻿using NUnit.Framework;
using System;
using MonoDevelop.VersionControl.Mercurial;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace MonoDevelop.VersionControl.Tests
{
	[TestFixture]
	public class MercurialClientTests
	{
		private string _currentPath;
		private string _testRepoPath;
		private MercurialClient _mc;
		private string _testFilePath;
		private const string _testFileContent = "qweqwe";


		[TestFixtureSetUp]
		public void Init()
		{
			_currentPath = Directory.GetCurrentDirectory();
			_testRepoPath = Path.Combine(_currentPath, "testRepo");
			if (Directory.Exists(_testRepoPath))
			{
				Directory.Delete(_testRepoPath, true);
			}

			Directory.CreateDirectory(_testRepoPath);

			var os = Environment.OSVersion.VersionString.ToLower();
			var hgPath = (os.Contains("win")) ? "hg" : (TestsUtils.IsRunningOnMac()) ? "/usr/local/bin/hg" : "/usr/bin/hg";

			MercurialClient.Init(_testRepoPath, hgPath);

			_mc = new MercurialClient(_testRepoPath, hgPath);

			_testFilePath = Path.Combine(_testRepoPath, "qwe.txt");
			File.AppendAllText(_testFilePath, _testFileContent);

			_mc.Add(new string[] { _testFilePath });
			_mc.Commit("first commit");
		}

		[TestFixtureTearDown]
		public void CleanUp()
		{
			Directory.Delete(_testRepoPath, true);
		}

		[Test]
		public void CatWithoutRevision()
		{
			var res = _mc.Cat(_testFilePath, null);
			Assert.AreEqual(_testFileContent, res);
		}
	}
}

