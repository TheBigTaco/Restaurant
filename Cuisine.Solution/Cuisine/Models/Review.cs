using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Cuisine.Models
{
  public class Review
  {
    public string Title {get;}
    public string Description {get;}
    public int RestaurantId {get;}
    public int Id {get;set;}

    public Review(string title, string description, int restaurantId, int id = 0)
    {
      Title = title;
      Description = description;
      RestaurantId = restaurantId;
      Id = id;
    }
    public override bool Equals(System.Object otherReview)
    {
      if (!(otherReview is Review))
      {
        return false;
      }

      else
      {
        Review newReview = (Review) otherReview;
        return this.Id.Equals(newReview.Id);
      }
    }

    public override int GetHashCode()
    {
      return this.Id.GetHashCode();
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO review (title, description, restaurant_id) VALUE (@title, @description, @restaurant_id);";

      MySqlParameter title = new MySqlParameter();
      title.ParameterName = "@title";
      title.Value = this.Title;
      cmd.Parameters.Add(title);

      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@description";
      description.Value = this.Description;
      cmd.Parameters.Add(description);

      MySqlParameter restaurantId = new MySqlParameter();
      restaurantId.ParameterName = "@restaurant_id";
      restaurantId.Value = this.RestaurantId;
      cmd.Parameters.Add(restaurantId);

      cmd.ExecuteNonQuery();
      Id = (int)cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Review> GetAll()
    {
      List<Review> allReviews = new List<Review> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM review;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int reviewId = rdr.GetInt32(0);
        string reviewTitle = rdr.GetString(1);
        string reviewDescription = rdr.GetString(2);
        int reviewRestaurantId = rdr.GetInt32(3);

        Review newReview = new Review(reviewTitle, reviewDescription, reviewRestaurantId, reviewId);
        allReviews.Add(newReview);
      }
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return allReviews;
    }

    public static Review Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM review WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int reviewId = 0;
      string reviewTitle = "";
      string reviewDescription = "";
      int reviewRestaurantId = 0;

      while(rdr.Read())
      {
        reviewId = rdr.GetInt32(0);
        reviewTitle = rdr.GetString(1);
        reviewDescription = rdr.GetString(2);
        reviewRestaurantId = rdr.GetInt32(3);
      }

      Review newReview = new Review(reviewTitle, reviewDescription, reviewRestaurantId, reviewId);

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return newReview;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM review;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
