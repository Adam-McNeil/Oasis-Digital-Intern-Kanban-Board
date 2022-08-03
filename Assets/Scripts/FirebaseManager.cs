using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class FirebaseManager : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;    
    public FirebaseUser User;
    public DatabaseReference DBreference;
    private UIReferences uIReferences;
    static private int firebaseManagerCount = 0;

    public List<string> userList;

    void Awake()
    {
        if (firebaseManagerCount != 0)
        {
            Destroy(this.gameObject);
        }
        FindUIRefences();
        firebaseManagerCount++;
        DontDestroyOnLoad(this);
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    public void FindUIRefences()
    {
        uIReferences = GameObject.Find("UI Refence").GetComponent<UIReferences>();
    }

    public void SignOut()
    {
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void ClearLoginFeilds()
    {
        uIReferences.emailLoginField.text = "";
        uIReferences.passwordLoginField.text = "";
    }
    public void ClearRegisterFeilds()
    {
        uIReferences.usernameRegisterField.text = "";
        uIReferences.emailRegisterField.text = "";
        uIReferences.passwordRegisterField.text = "";
        uIReferences.passwordRegisterVerifyField.text = "";
    }

    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(uIReferences.emailLoginField.text, uIReferences.passwordLoginField.text));
    }
    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(uIReferences.emailRegisterField.text, uIReferences.passwordRegisterField.text, uIReferences.usernameRegisterField.text));
    }
    //Function for the sign out button
    public void SignOutButton()
    {
        auth.SignOut();
        UIManager.instance.LoginScreen();
        ClearRegisterFeilds();
        ClearLoginFeilds();
    }
    //Function for the save button
    public void SaveDataButton()
    {
        StartCoroutine(UpdateUsernameAuth(uIReferences.usernameField.text));
        StartCoroutine(UpdateUsernameDatabase(uIReferences.usernameField.text));
    }

    //function for when create button is pressed
    public void createButton()
    {
      StartCoroutine(createServerChild());
    }

    //function called on load button pressed
    public void loadButton()
    {
      StartCoroutine(LoadJSONString());
    }

    public void GetServerData() {
        StartCoroutine(FindExistingServer());
    }

    private IEnumerator FindExistingServer() {
        var serversDB = DBreference.Child("servers").Child(uIReferences.serverInput.text).GetValueAsync();
        yield return new WaitUntil(predicate: () => serversDB.IsCompleted);
        if (serversDB.Result.Value == null) {
            createButton();
        }
        else {
            loadButton();
        }
        yield return new WaitForSeconds(0.5f);
    }

    public void saveJSONCall()
    {
      StartCoroutine(saveJSON());
    }

    private IEnumerator LoadJSONString()
    {
        var DBTask = DBreference.Child("servers").Child(uIReferences.serverInput.text).Child("serverJSON").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            //No data exists yet
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;
        }
        Debug.Log(DBTask.Result.Value);
        ServerLoaderController.serverJSONString = (string) DBTask.Result.Value;
    }

    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            uIReferences.warningLoginText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            uIReferences.warningLoginText.text = "";
            uIReferences.confirmLoginText.text = "Logged In";
            StartCoroutine(LoadUserData());

            yield return new WaitForSeconds(0.5f);

            if(User.DisplayName.Equals(null) || User.DisplayName.Equals("")){
                uIReferences.usernameField.text = User.DisplayName;
                UIManager.instance.UserDataScreen(); // Change to user data UI
            }
            else
            {
                UIManager.instance.MainMenuScreen();
            }

            uIReferences.confirmLoginText.text = "";
            ClearLoginFeilds();
            ClearRegisterFeilds();
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            uIReferences.warningRegisterText.text = "Missing Username";
        }
        else if(uIReferences.passwordRegisterField.text != uIReferences.passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            uIReferences.warningRegisterText.text = "Password Does Not Match!";
        }
        else 
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                uIReferences.warningRegisterText.text = message;
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile{DisplayName = _username};

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        uIReferences.warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        UIManager.instance.LoginScreen();
                        uIReferences.warningRegisterText.text = "";
                        ClearRegisterFeilds();
                        ClearLoginFeilds();
                    }
                }
            }
        }
    }

    private IEnumerator UpdateUsernameAuth(string _username)
    {
        //Create a user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };

        //Call the Firebase auth update user profile function passing the profile with the username
        var ProfileTask = User.UpdateUserProfileAsync(profile);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
            //Auth username is now updated
        }        
    }

    private IEnumerator UpdateUsernameDatabase(string _username)
    {
        //Set the currently logged in user username in the database
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database username is now updated
        }
    }

    private IEnumerator LoadUserData()
    {
        //Get the currently logged in user data
        var DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            //No data exists yet
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;
        }
    }

    private IEnumerator createServerChild()
    {
      string serverName = uIReferences.serverInput.text;
      var DBTask = DBreference.Child("servers").Child(serverName).Child("serverJSON").SetValueAsync("");

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Deaths are now updated
        }
    }

    public void LoadServerJSON()
    {
      //FirebaseCarryOver.serverNameText;
    }

    private IEnumerator saveJSON()
    {
       var DBTask = DBreference.Child("servers").Child(FirebaseCarryOver.serverNameText).Child("serverJSON").SetValueAsync(ServerLoaderController.stringOutput);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database username is now updated
        }
    }
  
    public void addUserToServerCall()
    {
      StartCoroutine(addUserToServer());
    }
    
    private IEnumerator addUserToServer()
    {
        //get the username of the user currently logged in
        if (User != null)
        {
            var DBTaskUser = DBreference.Child("users").Child(User.UserId).Child("username").GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTaskUser.IsCompleted);

            if (DBTaskUser.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTaskUser.Exception}");
            }
            else
            {
                //Database username is now updated
            }

            //get the users that have logged into the server
            var DBTask = DBreference.Child("servers").Child(FirebaseCarryOver.serverNameText).Child("users").GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                //Database username is now updated
            }

            if (DBTask.Result.Value == null)
            {
                DBreference.Child("servers").Child(FirebaseCarryOver.serverNameText).Child("users").SetValueAsync(DBTaskUser.Result.Value);
                CheckUsers((string)DBTask.Result.Value);
            }

            //see if the current user has logged into that server
            CheckUsers((string)DBTask.Result.Value);
            if (!userList.Contains(DBTaskUser.Result.Value))
            {
                var DBTaskSetNewUser = DBreference.Child("servers").Child(FirebaseCarryOver.serverNameText).Child("users").SetValueAsync(DBTask.Result.Value + "," + DBTaskUser.Result.Value);
                CheckUsers((string)DBTask.Result.Value);
            }
            DBTask = DBreference.Child("servers").Child(FirebaseCarryOver.serverNameText).Child("users").GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                //Database username is now updated
            }
            CheckUsers((string)DBTask.Result.Value);
            GameObject.Find("DropdownAssigned").GetComponent<getCurrentUsers>().UpdateDropDown();
            GameObject.Find("DropdownPlayerUsers").GetComponent<getCurrentUsers>().UpdateDropDown();
        }
    }


    private void CheckUsers(string namesList)
    {
      userList = new List<string>();
      userList = namesList.Split(',').ToList();
      foreach (string x in userList)
      {
        //Debug.Log(x);
      }
    }
}