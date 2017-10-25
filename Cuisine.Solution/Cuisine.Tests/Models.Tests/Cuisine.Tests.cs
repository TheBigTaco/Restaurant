using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cuisine.Models;

namespace Cuisine.Models.Tests
{
  [TestClass]
  public class TypesTests : IDisposable
  {
    public void Dispose()
    {
      Restaurant.DeleteAll();
      Types.DeleteAll();
    }
    public TypesTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=best_restaurants_tests;";
    }
    [TestMethod]
    public void GetAll_CuisineEmptyAtFirst_0()
    {
      int result = Types.GetAll().Count;
      Assert.AreEqual(0, result);
    }
    [TestMethod]
    public void Equals_ReturnsTrueForSameName_Types()
    {
      Types firstTypes = new Types("Mexican");
      Types secondTypes = new Types("Mexican");
      Assert.AreEqual(firstTypes, secondTypes);
    }
    [TestMethod]
    public void Save_SavesTypesToDatabase_TypesList()
    {
      Types testTypes = new Types("Mexican");
      testTypes.Save();
      List<Types> result = Types.GetAll();
      List<Types> testList = new List<Types>{testTypes};

      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Save_DatabaseAssignsIdToTypes_Id()
    {
      Types testTypes = new Types("Mexican");
      testTypes.Save();
      Types savedTypes = Types.GetAll()[0];
      int result = savedTypes.Id;
      int testId = testTypes.Id;
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_FindsTypesInDatabase_Types()
    {
      Types testTypes = new Types("Mexican");
      testTypes.Save();
      Types foundTypes = Types.Find(testTypes.Id);
      Assert.AreEqual(testTypes, foundTypes);
    }
  }
}
