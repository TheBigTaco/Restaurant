using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Cuisine.Models;

namespace Cuisine.Controllers
{
    public class HomeController : Controller
    {
      [HttpGet("/")]
      public ActionResult Index()
      {
        List<Types> cuisines = Types.GetAll();
        return View(cuisines );
      }
      [HttpGet("/cuisine")]
      public ActionResult AddCuisine()
      {
        return View();
      }
      [HttpPost("/cuisine/add")]
      public ActionResult NewCuisine()
      {
        Types newTypes = new Types(Request.Form["new-cuisine"]);
        newTypes.Save();
        return Redirect("/");
      }
      [HttpGet("/cuisine/{id}")]
      public ActionResult CuisineDetail(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Types selectedCuisine = Types.Find(id);
        List<Restaurant> cuisineRestaurants = selectedCuisine.GetRestaurants();
        model.Add("cuisine", selectedCuisine);
        model.Add("restaurant", cuisineRestaurants);
        return View(model);
      }
      [HttpGet("/cuisine/{id}/restaurant")]
      public ActionResult AddRestaurant(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Types selectedCuisine = Types.Find(id);
        List<Restaurant> cuisineRestaurants = selectedCuisine.GetRestaurants();
        model.Add("cuisine", selectedCuisine);
        model.Add("restaurant", cuisineRestaurants);
        return View(model);
      }
      [HttpPost("/cuisine/{id}/restaurant/new")]
      public ActionResult NewRestaurant(int id)
      {
        Restaurant newRestaurant = new Restaurant(Request.Form["restaurant-name"], long.Parse(Request.Form["restaurant-phone"]), Request.Form["restaurant-address"], int.Parse(Request.Form["cuisine-id"]), Request.Form["restaurant-dish"]);
        newRestaurant.Save();
        return Redirect("/cuisine/"+id);
      }
      [HttpGet("/cuisine/{cid}/restaurant/{rid}")]
      public ActionResult RestaurantDetail(int cid, int rid)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Restaurant selectedRestaurant = Restaurant.Find(rid);
        List<Review> restaurantReviews = selectedRestaurant.GetReviews();
        model.Add("restaurant", selectedRestaurant);
        model.Add("review", restaurantReviews);
        return View(model);
      }
      [HttpGet("/cuisine/{cid}/restaurant/{rid}/review")]
      public ActionResult AddReview(int cid, int rid)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Restaurant selectedRestaurant = Restaurant.Find(rid);
        List<Review> restaurantReviews = selectedRestaurant.GetReviews();
        model.Add("restaurant", selectedRestaurant);
        model.Add("review", restaurantReviews);
        return View(model);
      }
      [HttpPost("/cuisine/{cid}/restaurant/{rid}/review/new")]
      public ActionResult NewReview(int cid, int rid)
      {
        Review newReview = new Review(Request.Form["review-title"], Request.Form["review-description"], int.Parse(Request.Form["restaurant-id"]));
        newReview.Save();
        return Redirect("/cuisine/"+cid+"/restaurant/"+rid);
      }
    }
}
