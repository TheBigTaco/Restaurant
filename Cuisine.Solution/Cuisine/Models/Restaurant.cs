using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Cuisine.Models
{
  public class Restaurant
  {
    public string Name {get;}
    public long PhoneNumber {get;}
    public string Address {get;}
    public int CuisineId {get;}
    public string BestDish {get;}
    public int Id {get;set;}

    public Restaurant(string name, long phonenumber, string address, int cuisineid, string bestdish, int id = 0)
    {
      Name = name;
      PhoneNumber = phonenumber;
      Address = address;
      CuisineId = cuisineid;
      BestDish = bestdish;
      Id = id;
    }
    public override bool Equals(System.Object otherRestaurant)
    {
      if (!(otherRestaurant is Restaurant))
      {
        return false;
      }
      else
      {
         Restaurant newRestaurant = (Restaurant) otherRestaurant;
         bool nameEquality = this.Name == newRestaurant.Name;
         bool phonenumberEquality = this.PhoneNumber == newRestaurant.PhoneNumber;
         bool addressEquality = this.Address == newRestaurant.Address;
         bool cuisineEquality = this.CuisineId == newRestaurant.CuisineId;
         bool bestdishEquality = this.BestDish == newRestaurant.BestDish;
         bool idEquality = this.Id == newRestaurant.Id;
         return (nameEquality && phonenumberEquality && addressEquality && cuisineEquality && bestdishEquality && idEquality);
       }
    }
    public override int GetHashCode()
    {
      return this.Name.GetHashCode();
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO restaurant (name, phone_number, address, cuisine_id, best_dish) VALUES (@name, @phone_number, @address, @cuisine_id, @best_dish);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this.Name;
      cmd.Parameters.Add(name);

      MySqlParameter phoneNumber = new MySqlParameter();
      phoneNumber.ParameterName = "@phone_number";
      phoneNumber.Value = this.PhoneNumber;
      cmd.Parameters.Add(phoneNumber);

      MySqlParameter address = new MySqlParameter();
      address.ParameterName = "@address";
      address.Value = this.Address;
      cmd.Parameters.Add(address);

      MySqlParameter cuisineId = new MySqlParameter();
      cuisineId.ParameterName = "@cuisine_id";
      cuisineId.Value = this.CuisineId;
      cmd.Parameters.Add(cuisineId);

      MySqlParameter bestDish = new MySqlParameter();
      bestDish.ParameterName = "@best_dish";
      bestDish.Value = this.BestDish;
      cmd.Parameters.Add(bestDish);

      cmd.ExecuteNonQuery();
      Id = (int)cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public static List<Restaurant> GetAll()
    {
      List<Restaurant> allRestaurants = new List<Restaurant> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM restaurant;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int restaurantId = rdr.GetInt32(0);
        string restaurantName = rdr.GetString(1);
        long restaurantPhoneNumber = rdr.GetInt64(2);
        string restaurantAddress = rdr.GetString(3);
        int restaurantCuisineId = rdr.GetInt32(4);
        string restaurantBestDish = rdr.GetString(5);

        Restaurant newRestaurant = new Restaurant(restaurantName, restaurantPhoneNumber, restaurantAddress, restaurantCuisineId, restaurantBestDish, restaurantId);
        allRestaurants.Add(newRestaurant);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return allRestaurants;
    }

    public static Restaurant Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM restaurant WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int restaurantId = 0;
      string restaurantName = "";
      long restaurantPhoneNumber = 0;
      string restaurantAddress = "";
      int restaurantCuisineId = 0;
      string restaurantBestDish = "";

      while(rdr.Read())
      {
        restaurantId = rdr.GetInt32(0);
        restaurantName = rdr.GetString(1);
        restaurantPhoneNumber = rdr.GetInt64(2);
        restaurantAddress = rdr.GetString(3);
        restaurantCuisineId = rdr.GetInt32(4);
        restaurantBestDish = rdr.GetString(5);

      }

      Restaurant newRestaurant = new Restaurant(restaurantName, restaurantPhoneNumber, restaurantAddress, restaurantCuisineId, restaurantBestDish, restaurantId);

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return newRestaurant;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM restaurant;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
