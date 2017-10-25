using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Cuisine.Models
{
  public class Type
  {
    public string Name {get;}
    public int Id {get;set;}

    public Type(string name, int id = 0)
    {
      Name = name;
      Id = id;
    }
    public override bool Equals(System.Object otherType)
    {
      if (!(otherType is Type))
      {
        return false;
      }

      else
      {
        Type newType = (Type) otherType;
        return this.Id.Equals(newType.Id);
      }
    }

    public override int GetHashCode()
    {
      return this.Id.GetHashCode();
    }


    public static List<Type> GetAll()
    {
      List<Type> allTypes = new List<Type> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cuisine;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int TypeId = rdr.GetInt32(0);
        string TypeName = rdr.GetString(1);
        Type newType = new Type(TypeName, TypeId);
        allTypes.Add(newType);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return allTypes;
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

    public static Type Find(int id)
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

      Type foundType = new Type(typeName, typeId);
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return foundType;
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
