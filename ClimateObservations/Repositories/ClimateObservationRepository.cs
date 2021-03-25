using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClimateObservations.Repositories
{
    public class ClimateObservationRepository
    {
        private static readonly string connectionString = "Server=localhost;Port=5432;Database=climate_observations;User ID=postgres; Password=+987;";

        #region Methods for Observers
        public Observer Testmetodattcopypasta(int id)
        {
            string stmt = "select * from observer where id = @id;";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            command.Parameters.AddWithValue("id", id);

            using var reader = command.ExecuteReader();

            Observer observer = null;
            while (reader.Read())
            {
                observer = new Observer
                {
                    ID = (int)reader["id"],
                    FirstName = (string)reader["firstname"],
                    LastName = (string)reader["lastname"]
                };
            }
            return observer;
        }

        public List<Observer> GetAllObservers()
        {
            string stmt = "select * from observer order by lastname;";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            using var reader = command.ExecuteReader();

            List<Observer> AllObservers = new List<Observer>();
            Observer observer = null;

            while (reader.Read())
            {
                observer = new Observer
                {
                    ID = (int)reader["id"],
                    FirstName = (string)reader["firstname"],
                    LastName = (string)reader["lastname"]
                };
                AllObservers.Add(observer);
            }
            return AllObservers;
        } //färdig

        public Observer RegisterNewObserver(string first, string last)
        {
            string stmt = "insert into observer (firstname,lastname) values (@first, @last);";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            command.Parameters.AddWithValue("first", first);
            command.Parameters.AddWithValue("last", last);

            using var reader = command.ExecuteReader();
            return GetObserver("select * from observer where id = (select max(id) from observer);");
        }

        public Observer GetSelectedObserver(int id)
        {
            return GetObserver($"select * from observer where id = {id}");
        }
        public Observer GetObserver (string stmt)
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            using var reader = command.ExecuteReader();

            Observer observer = null;
            while (reader.Read())
            {
                observer = new Observer
                {
                    ID = (int)reader["id"],
                    FirstName = (string)reader["firstname"],
                    LastName = (string)reader["lastname"]
                };
            }
            return observer;
        }
        /// <summary>
        /// Method to check if observer has made observations
        /// </summary>
        /// <param name="observerID"></param>
        /// <returns> Returns false if observations do not exist.</returns>
        private bool HasObservations(int observerID)
        {
            string stmt = "select observation.observer_id from observation where observation.observer_id = @id;";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            command.Parameters.AddWithValue("id", observerID);

            using var reader = command.ExecuteReader();

            int? observations = null;

            while (reader.Read())
            {
                observations = Convert.IsDBNull(reader["observer_id"]) ? null : (int?)reader["observer_id"];
            }
            if (observations == null)
            {
                return false;
            }   
            else
            {
                return true;
            }
        }

        public bool DeleteObserver (int observerID)
        {
            if (HasObservations(observerID) == false)
            {
                string stmt = "delete from observer where id=@id;";
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();

                using var command = new NpgsqlCommand(stmt, conn);

                command.Parameters.AddWithValue("id", observerID);

                using var reader = command.ExecuteReader();
                return true;
            }
            else 
            {
                return false;
            }
        }

        #endregion
        #region Methods for Observations
        public Observation RegisterNewObservation(int observer, int geolocation)
        {
            string stmt = "insert into observation (observer_id,geolocation_id) values (@observer,@geolocation)";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            command.Parameters.AddWithValue("observer", observer);
            command.Parameters.AddWithValue("geolocation", geolocation);

            using var reader = command.ExecuteReader();
            return GetObservation("select observation.id, observation.date, observation.observer_id, observer.firstname, observer.lastname, observation.geolocation_id from observation join observer on observation.observer_id=observer.id where observation.id = (select max(id) from observation);");
        }
        public Observation SelectObservation (int id)
        {
            return GetObservation($"select observation.id, observation.date, observation.observer_id, observation.geolocation_id from observation where observation.id = {id};");
        }
        public Observation GetObservation (string stmt)
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            using var reader = command.ExecuteReader();

            Observation observation = null;
            while (reader.Read())
            {
                observation = new Observation
                {
                    ID = (int)reader["id"],
                    Date = (DateTime)reader["date"],
                    ObserverID = (int) reader["observer_id"],
                    GeolocationID = (int)reader["geolocation_id"]
                };
            }
            return observation;
        }
        public List<Observation> GetAllObservations(int observerID)
        {
            string stmt = "select observation.id, observation.date, observation.observer_id, observation.geolocation_id from observation where observation.observer_id = @id;";

            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            command.Parameters.AddWithValue("id", observerID);

            using var reader = command.ExecuteReader();

            List<Observation> AllObservations = new List<Observation>();
            Observation observation = null;

            while (reader.Read())
            {
                observation = new Observation
                {
                    ID = (int)reader["id"],
                    Date = (DateTime)reader["date"],
                    ObserverID = (int)reader["observer_id"],
                    GeolocationID = (int)reader["geolocation_id"]
                };
                AllObservations.Add(observation);
            }
            return AllObservations;
        }
        #endregion
        #region Methods for Measurements
        public void AddMeasurement(double? value, int observationID, int categoryID)
        {
            string stmt = "insert into measurement (value, observation_id, category_id) values (@value,@observation,@category)";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            command.Parameters.AddWithValue("value", value);
            command.Parameters.AddWithValue("observation", observationID);
            command.Parameters.AddWithValue("category", categoryID);

            using var reader = command.ExecuteReader();
        }
        public List<Measurement> GetAllMeasurements (int observationID)
        {
            string stmt = "select measurement.id, measurement.value, unit.type, measurement.category_id, category.name from measurement join category on measurement.category_id = category.id join unit on category.unit_id = unit.id where measurement.observation_id = @id;";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            command.Parameters.AddWithValue("id", observationID);

            using var reader = command.ExecuteReader();

            List<Measurement> measurements = new List<Measurement>();
            Measurement measurement = null;
            while (reader.Read())
            {
                measurement = new Measurement
                {
                    ID = (int)reader["id"],
                    Value = Convert.IsDBNull(reader["value"]) ? null : (double?)reader["value"],
                    Unit = (string)reader["type"],
                    CategoryID = (int)reader["category_id"],
                    Category = (string)reader["name"],
                    BaseCategory = HasBasecategory((int)reader["category_id"]),
                };
                measurements.Add(measurement);
            }
            return measurements;
        }
        private string HasBasecategory (int CategoryID)
        {
            string stmt = "select category.name as basecat from category where category.id = (select basecategory_id from category where category.id = @id)";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            command.Parameters.AddWithValue("id", CategoryID);

            using var reader = command.ExecuteReader();

            string basecat = null;
            while (reader.Read())
            {
                basecat = (string)reader["basecat"];
            }
            return basecat;

        }
        public void AlterMeasurement(double? value, int measurementID)
        {
            string stmt = "UPDATE measurement SET value = @value WHERE id = @id;";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            command.Parameters.AddWithValue("value", value);
            command.Parameters.AddWithValue("id", measurementID);

            using var reader = command.ExecuteReader();
        }

        #endregion
        #region Methods for Geolocation
        public List<Geolocation> GetAllGeolocations()
        {
            string stmt = "select geolocation.id as id,area.name as area, country.name as country from geolocation join area on geolocation.area_id = area.id join country on area.country_id = country.id;";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            using var reader = command.ExecuteReader();

            List<Geolocation> AllGeolocations = new List<Geolocation>();
            Geolocation geolocation = null;

            while (reader.Read())
            {
                geolocation = new Geolocation
                {
                    ID = (int)reader["id"],
                    Area = (string)reader["area"],
                    Country = (string)reader["country"]
                };
                AllGeolocations.Add(geolocation);
            }
            return AllGeolocations;
        }
        
        #endregion

        #region Methods for Categories
        private string IsNull(int? id)
        {
            if (id == null)
            {
                return "select category.id, category.name, category.basecategory_id, unit.type as unit from category join unit on category.unit_id = unit.id where basecategory_id is null;";
            }
            else
                return $"select category.id, category.name, category.basecategory_id, unit.type as unit from category join unit on category.unit_id = unit.id where basecategory_id = {id};";

        }
        public List<Category> GetCategories(int? id)
        {
            string stmt = IsNull(id);
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            using var reader = command.ExecuteReader();

            List<Category> Categories = new List<Category>();
            Category category = null;

            while (reader.Read())
            {
                category = new Category
                {
                    ID = (int)reader["id"],
                    Name = (string)reader["name"],
                    BasecategoryID = Convert.IsDBNull(reader["basecategory_id"]) ? null : (int?)reader["basecategory_id"],
                    Unit = (string)reader["unit"]
                };
                Categories.Add(category);
            }
            return Categories;
        }
        #endregion
    }
}
