using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cuisine.Models;

namespace Cuisine.Models.Tests
{
  [TestClass]
  public class TypeTests : IDisposable
  {
    public void Dispose()
    {
      Restaurant.DeleteAll();
      Type.DeleteAll();
    }
    public TypeTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=best_restaurants_tests;";
    }
    [TestMethod]
    public void GetAll_CuisineEmptyAtFirst_0()
    {
      int result = Type.GetAll().Count;
      Assert.AreEqual(0, result);
    }
    [TestMethod]
    public void Equals_ReturnsTrueForSameName_Type()
    {
      Type firstType = new Type("Mexican");
      Type secondType = new Type("Mexican");
      Assert.AreEqual(firstType, secondType);
    }
    [TestMethod]
    public void Save_SavesTypeToDatabase_TypeList()
    {
      Type testType = new Type("Mexican");
      testType.Save();
      List<Type> result = Type.GetAll();
      List<Type> testList = new List<Type>{testType};

      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Save_DatabaseAssignsIdToType_Id()
    {
      Type testType = new Type("Mexican");
      testType.Save();
      Type savedType = Type.GetAll()[0];
      int result = savedType.Id;
      int testId = testType.Id;
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_FindsTypeInDatabase_Type()
    {
      Type testType = new Type("Mexican");
      testType.Save();
      Type foundType = Type.Find(testType.Id);
      Assert.AreEqual(testType, foundType);
    }
  }
}
