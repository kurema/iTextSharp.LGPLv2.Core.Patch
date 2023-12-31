﻿using System;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iTextSharp.LGPLv2.Core.FunctionalTests.Issues;

/// <summary>
///     https://github.com/VahidN/iTextSharp.LGPLv2.Core/pull/128
/// </summary>
[TestClass]
public class Issue128
{
	[TestMethod]
	public void Test_Issue128_Verify_TryPassword_Works()
	{
		var inputFile = TestUtils.GetPdfsPath("issue128.pdf");

		try
		{
			using var reader = new PdfReader(inputFile);
		}
		catch (BadPasswordExceptionTriable pe)
		{
			Assert.IsTrue(pe.CanTryPassword);
			Assert.AreEqual(pe.TryPassword(Encoding.UTF8.GetBytes("fail")), BadPasswordExceptionTriable.TryPasswordResult.Fail);
			Assert.AreEqual(pe.TryPassword(Encoding.UTF8.GetBytes("password")), BadPasswordExceptionTriable.TryPasswordResult.SuccessUserPassword);
			Assert.AreEqual(pe.TryPassword(Encoding.UTF8.GetBytes("owner")), BadPasswordExceptionTriable.TryPasswordResult.SuccessOwnerPassword);
		}

		{
			using var reader = new PdfReader(inputFile, Encoding.UTF8.GetBytes("password"));
			Assert.AreEqual(reader.NumberOfPages, 2);
		}
	}
}
