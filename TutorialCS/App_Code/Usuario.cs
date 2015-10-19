using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Usuario
/// </summary>
public class Usuario
{
    public string UserName { get; set; }
    public string Password { get; set; }

    public Usuario(string username, string password)
    {
        UserName = username;
        Password = password;
    }
}