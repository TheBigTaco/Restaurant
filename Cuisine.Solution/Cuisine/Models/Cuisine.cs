using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Cuisine.Models
{
  public class Types
  {
    public string Name {get;}
    public int Id {get;set;}

    public Types(string name, int id = 0)
    {
      Name = name;
      Id = id;
    }
    public override bool Equals(System.Object otherTypes)
    {
      if (!(otherTypes is Types))
      {
        return false;
      }

      else
      {
        Types newTypes = (Types) otherTypes;
        return this.Id.Equals(newTypes.Id);
      }
    }

    public override int GetHashCode()
    {
      return this.Id.GetHashCode();
    }


    public static List<Types> GetAll()
    {
      List<Types> allTypess = new List<Types> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cuisine;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int TypesId = rdr.GetInt32(0);
        string TypesName = rdr.GetString(1);
        Types newTypes = new Types(TypesName, TypesId);
        allTypess.Add(newTypes);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return allTypess;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO cuisine (name) VALUES (@name);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this.Name;
      cmd.Parameters.Add(name);

      cmd.ExecuteNonQuery();
      Id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public static Types Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `cuisine` WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int typeId = 0;
      string typeName = "";

      while(rdr.Read())
      {
          typeId = rdr.GetInt32(0);
          typeName = rdr.GetString(1);
      }

      Types foundTypes = new Types(typeName, typeId);
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return foundTypes;
    }

    public List<Restaurant> GetRestaurants()
    {
      List<Restaurant> allCuisineRestaurants = new List<Restaurant> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM restaurant WHERE cuisine_id = @cuisine_id ORDER BY name;";

      MySqlParameter typeId = new MySqlParameter();
      typeId.ParameterName = "@cuisine_id";
      typeId.Value = this.Id;
      cmd.Parameters.Add(typeId);
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
        allCuisineRestaurants.Add(newRestaurant);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return allCuisineRestaurants;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM cuisine;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
