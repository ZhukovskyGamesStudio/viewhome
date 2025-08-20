using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData {
    public string Email;
    public string Password;

    public List<Product> CartProducts = new();
}