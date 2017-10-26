using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cuisine.Models;

namespace Cuisine.Models.Tests
{
  [TestClass]
  public class ReviewTests : IDisposable
  {
    public void Dispose()
    {
      Restaurant.DeleteAll();
      Types.DeleteAll();
      Review.DeleteAll();
    }
    public ReviewTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=best_restaurants_tests;";
    }
    [TestMethod]
    public void Equals_OverrideTrueForSameParameters_Review()
    {
      Review firstReview = new Review("I love this", "no really it's great", 0);
      Review secondReview = new Review("I love this", "no really it's great", 0);

      Assert.AreEqual(firstReview, secondReview);
    }
    [TestMethod]
    public void Save_SaveReviewToDatabase_ReviewList()
    {
      Review firstReview = new Review("I love this", "no really it's great", 0);
      firstReview.Save();

      List<Review> result = Review.GetAll();
      List<Review> testList = new List<Review> {firstReview};

      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Save_DatabaseAssignsIdToObject_Id()
    {
      Review firstReview = new Review("I love this", "no really it's great", 0);
      firstReview.Save();

      Review savedReview = Review.GetAll()[0];

      int result = savedReview.Id;
      int testId = firstReview.Id;

      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void Find_FindsReviewInDatabase_Review()
    {
      Review firstReview = new Review("I love this", "no really it's great", 0);
      firstReview.Save();

      Review foundReview = Review.Find(firstReview.Id);

      Assert.AreEqual(firstReview, foundReview);
    }
    [TestMethod]
    public void GetReviews_RetrievesAllReviewsWithRestaurants_ReviewList()
    {
      Types testTypes = new Types("Japanese");
      testTypes.Save();
      Restaurant testRestaurant = new Restaurant("Shigezo", 5555555555, "SW Salmon Street", testTypes.Id, "Fire Cracker Roll");
      testRestaurant.Save();
      Review firstReview = new Review("I love this", "no really it's great", testRestaurant.Id);
      Review secondReview = new Review("I love this", "no really it's great", testRestaurant.Id);
      firstReview.Save();
      secondReview.Save();


      List<Review> testReviewList = new List<Review> {firstReview, secondReview};
      List<Review> resultReviewList = testRestaurant.GetReviews();

      CollectionAssert.AreEqual(testReviewList, resultReviewList);
    }
  }
}
