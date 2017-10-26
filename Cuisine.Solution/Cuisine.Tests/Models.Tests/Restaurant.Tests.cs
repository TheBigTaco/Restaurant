using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cuisine.Models;

namespace Cuisine.Models.Tests
{
  [TestClass]
  public class RestaurantTests : IDisposable
  {
    public void Dispose()
    {
      Restaurant.DeleteAll();
      Types.DeleteAll();
      Review.DeleteAll();
    }
    public RestaurantTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=best_restaurants_tests;";
    }
    [TestMethod]
    public void Equals_OverrideTrueForSameParameters_Restaurant()
    {
      Restaurant firstRestaurant = new Restaurant("Shigezo", 5555555555, "SW Salmon Street", 3, "Fire Cracker Roll");
      Restaurant secondRestaurant = new Restaurant("Shigezo", 5555555555, "SW Salmon Street", 3, "Fire Cracker Roll");

      Assert.AreEqual(firstRestaurant, secondRestaurant);
    }
    [TestMethod]
    public void Save_SaveRestaurantToDatabase_RestaurantList()
    {
      Restaurant firstRestaurant = new Restaurant("Shigezo", 5555555555, "SW Salmon Street", 3, "Fire Cracker Roll");
      firstRestaurant.Save();

      List<Restaurant> result = Restaurant.GetAll();
      List<Restaurant> testList = new List<Restaurant> {firstRestaurant};

      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Save_DatabaseAssignsIdToObject_Id()
    {
      Restaurant firstRestaurant = new Restaurant("Shigezo", 5555555555, "SW Salmon Street", 3, "Fire Cracker Roll");
      firstRestaurant.Save();

      Restaurant savedRestaurant = Restaurant.GetAll()[0];

      int result = savedRestaurant.Id;
      int testId = firstRestaurant.Id;

      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void Find_FindsRestaurantInDatabase_Restaurant()
    {
      Restaurant firstRestaurant = new Restaurant("Shigezo", 5555555555, "SW Salmon Street", 3, "Fire Cracker Roll");
      firstRestaurant.Save();

      Restaurant foundRestaurant = Restaurant.Find(firstRestaurant.Id);

      Assert.AreEqual(firstRestaurant, foundRestaurant);
    }
    [TestMethod]
    public void GetRestaurants_RetrievesAllRestaurantsWithTypes_RestaurantList()
    {
      Types testTypes = new Types("Japanese");
      testTypes.Save();
      Restaurant firstRestaurant = new Restaurant("Shigezo", 5555555555, "SW Salmon Street", testTypes.Id, "Fire Cracker Roll");
      Restaurant secondRestaurant = new Restaurant("Shigezo", 5555555555, "SW Salmon Street", testTypes.Id, "Fire Cracker Roll");
      firstRestaurant.Save();
      secondRestaurant.Save();


      List<Restaurant> testRestaurantList = new List<Restaurant> {firstRestaurant, secondRestaurant};
      List<Restaurant> resultRestaurantList = testTypes.GetRestaurants();

      CollectionAssert.AreEqual(testRestaurantList, resultRestaurantList);
    }
    [TestMethod]
    public void GetRestaurants_MakeSureListOrderedByName_RestaurantList()
    {
      Types testTypes = new Types("Japanese");
      testTypes.Save();
      Restaurant firstRestaurant = new Restaurant("Shigezo", 5555555555, "SW Salmon Street", testTypes.Id, "Fire Cracker Roll");
      Restaurant secondRestaurant = new Restaurant("Andina", 5555555555, "SW Salmon Street", testTypes.Id, "Fire Cracker Roll");
      firstRestaurant.Save();
      secondRestaurant.Save();


      List<Restaurant> testRestaurantList = new List<Restaurant> {secondRestaurant, firstRestaurant};
      List<Restaurant> resultRestaurantList = testTypes.GetRestaurants();

      CollectionAssert.AreEqual(testRestaurantList, resultRestaurantList);
    }
  }
}
