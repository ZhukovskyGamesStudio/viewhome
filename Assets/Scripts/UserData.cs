using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class UserData {
    public string Email;
    public string Password;

    public List<Product> CartProducts => Room.Instance.ObjectsInRoom.Keys.ToList();
}