using UnityEngine;

public static class UserDataManager {

    public static UserData UserData = new UserData();
    public static string email;
    public static string password;
 

    public static void CreateRandomValues() {
        email = $"email{Random.Range(0, 1000)}@email.main";
        password = $"00000{Random.Range(100, 1000)}";
    }
}
