﻿using MySql.Data.MySqlClient;

public class Database_Manager
{
    private MySqlConnection connection;

    public Database_Manager()
    {
        //Chorizo grande para conectarnos con la base de datos
        string connectionString = "Server=127.0.0.1;Port=3306;database=godduckgame_jcr;Uid=root;password=;SSL Mode=None;connect timeout=3600;default command timeout=3600;";
        //Establecemos conexion con la base de datos
        connection = new MySqlConnection(connectionString);

        try
        {
            //Intenamos conectarnos con la base de datos
            connection.Open();
            Console.WriteLine("Conexión a la base de datos establecida correctamente.");
        }
        catch (Exception ex)
        {
            //Si no podemos conectarnos con la base de datos saltara este error
            Console.WriteLine("Error al conectar con la base de datos: " + ex.Message);
        }
    }

    //Registramos nuevo usuario en la base de datos
    public bool Register(string nick, string password, string race)
    {
        //Comprobamos si el usuario ya existe en la base de datos
        string checkCommandString = "SELECT * FROM Users WHERE nick = @nick";
        MySqlCommand checkCommand = new MySqlCommand(checkCommandString, connection);
        checkCommand.Parameters.AddWithValue("@nick", nick);
        MySqlDataReader reader = checkCommand.ExecuteReader();
        bool userExists = reader.HasRows;
        reader.Close();

        if (userExists)
        {
            Console.WriteLine("El usuario ya existe.");
            return false;
        }

        //Insertamos nuevo usuario en la base de datos
        string commandString = "INSERT INTO Users (nick, password, race) VALUES (@nick, @password, @race)";
        MySqlCommand command = new MySqlCommand(commandString, connection);
        command.Parameters.AddWithValue("@nick", nick);
        command.Parameters.AddWithValue("@password", password);
        command.Parameters.AddWithValue("@race", race);
        try
        {
            //Ejecutamos comando de insercion de usuario en la base de datos
            command.ExecuteNonQuery();
            Console.WriteLine("Inserción exitosa.");
            return true;
        }
        catch (Exception ex)
        {
            //Si el comando falla salta error
            Console.WriteLine("Error al ejecutar comando: " + ex.Message);
            return false;
        }
    }

    //Iniciamos sesion de un usuario dentro de la base de datos
    public string Login(string nick, string password)
    {
        //Verificamos si el login es correcto mediante una query de SELECT
        string commandString = "SELECT * FROM Users WHERE nick = @nick AND password = @password";
        MySqlCommand command = new MySqlCommand(commandString, connection);
        command.Parameters.AddWithValue("@nick", nick);
        command.Parameters.AddWithValue("@password", password);
        MySqlDataReader reader = command.ExecuteReader();
        bool loginSuccessful = reader.HasRows;

        if (loginSuccessful)
        {
            //El usuario existe, así que obtenemos su raza
            reader.Read();
            string race = reader["race"].ToString();
            reader.Close();

            //Ahora obtenemos los valores de speed y jumpforce para esa raza
            string raceCommandString = "SELECT speed, jumpforce FROM Races WHERE race = @race";
            MySqlCommand raceCommand = new MySqlCommand(raceCommandString, connection);
            raceCommand.Parameters.AddWithValue("@race", race);
            MySqlDataReader raceReader = raceCommand.ExecuteReader();

            if (raceReader.HasRows)
            {
                //La raza existe, así que obtenemos los valores de speed y jumpforce
                raceReader.Read();
                float speed = float.Parse(raceReader["speed"].ToString());
                float jumpforce = float.Parse(raceReader["jumpforce"].ToString());
                raceReader.Close();

                //Devolvemos los valores de speed y jumpforce como parte de la respuesta
                return "true/" + speed.ToString() + "/" + jumpforce.ToString();
            }
            else
            {
                //La raza no existe, así que devolvemos un error
                raceReader.Close();
                return "false";
            }
        }
        else
        {
            //El usuario no existe, así que devolvemos un error
            reader.Close();
            return "false";
        }
    }

}
