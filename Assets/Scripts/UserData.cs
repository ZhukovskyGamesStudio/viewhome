using System;
using System.Collections.Generic;

[Serializable]
public class UserData {
    public string Email;
    public string Password;

    public List<Product> CartProducts = new List<Product>();
}